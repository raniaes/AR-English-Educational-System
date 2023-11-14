using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Addteacher : MonoBehaviour
{
    private FirebaseFirestore db;

    public InputField teacherNameInput;
    public Text studentname;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);
        });
    }

    public void AddStudentToTeacherOnClick()
    {
        string teacherName = teacherNameInput.text;

        DocumentReference userRef = db.Collection("user").Document(teacherName);
        userRef.GetSnapshotAsync().ContinueWithOnMainThread(Task =>
        {
            if (Task.IsFaulted)
            {
                Debug.LogError("Failed to get user document: " + Task.Exception);
                return;
            }

            DocumentSnapshot snapshot = Task.Result;

            if (snapshot.Exists)
            {
                Dictionary<string, object> userData = snapshot.ToDictionary();

                if (userData.TryGetValue("usertype", out object usertypeObj))
                {
                    bool usertype = (bool)usertypeObj;

                    if (usertype)
                    {
                        DocumentReference teacherRef = db.Collection("teachers").Document(teacherName);

                        string studentName = studentname.text; 

                        Dictionary<string, object> updates = new Dictionary<string, object>
                        {
                            { "students", FieldValue.ArrayUnion(studentName) }
                        };

                        teacherRef.SetAsync(updates, SetOptions.MergeAll).ContinueWithOnMainThread(updateTask =>
                        {
                            if (updateTask.IsCompleted)
                            {
                                Debug.Log($"student {studentName} add.");
                                SceneManager.LoadScene("studentmain", LoadSceneMode.Single);
                            }
                            else if (updateTask.IsFaulted)
                            {
                                Debug.LogError($"Fault: {updateTask.Exception}");
                            }
                        });
                    }
                    else
                    {
                        Debug.LogError("this is not teacher.");
                    }
                }
                else
                {
                    Debug.LogError("UserType field not found in the document.");
                }
            }
            else
            {
                Debug.LogError("User document does not exist.");
            }
        });
    }
}
