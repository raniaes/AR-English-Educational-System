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
    private AudioSource addsound;

    public InputField teacherNameInput;
    public Text studentname;

    [SerializeField] private AudioClip[] audioClip;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);
        });

        addsound = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void AddStudentToTeacherOnClick()
    {
        string teacherName = teacherNameInput.text;

        if (string.IsNullOrEmpty(teacherName))
        {
            addsound.clip = audioClip[1];
            addsound.Play();
            return;
        }

        DocumentReference userRef = db.Collection("user").Document(teacherName);
        userRef.GetSnapshotAsync().ContinueWithOnMainThread(Task =>
        {
            if (Task.IsFaulted)
            {
                addsound.clip = audioClip[1];
                addsound.Play();
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
                                addsound.clip = audioClip[0];
                                addsound.Play();
                                Debug.Log($"student {studentName} add.");
                                SceneManager.LoadScene("studentmain", LoadSceneMode.Single);
                            }
                            else if (updateTask.IsFaulted)
                            {
                                addsound.clip = audioClip[1];
                                addsound.Play();
                                Debug.LogError($"Fault: {updateTask.Exception}");
                            }
                        });
                    }
                    else
                    {
                        addsound.clip = audioClip[1];
                        addsound.Play();
                        Debug.LogError("this is not teacher.");
                    }
                }
                else
                {
                    addsound.clip = audioClip[1];
                    addsound.Play();
                    Debug.LogError("UserType field not found in the document.");
                }
            }
            else
            {
                addsound.clip = audioClip[1];
                addsound.Play();
                Debug.LogError("User document does not exist.");
            }
        });
    }
}
