using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;

public class delstudent : MonoBehaviour
{
    public Text IDText;
    public Text IDstudent;
    public GameObject confirmationPopup;

    private FirebaseFirestore db;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);
        });
    }

    public void ShowConfirmationPopup()
    {
        confirmationPopup.SetActive(true);
    }

    public void CloseConfirmationPopup()
    {
        confirmationPopup.SetActive(false);
    }

    public void ConfirmDelete()
    {
        CloseConfirmationPopup();

        delstudentclick();
    }

    public void CancelDelete()
    {
        CloseConfirmationPopup();
    }

    public void delstudentclick()
    {
        string ID = IDText.text;
        string studentID = IDstudent.text;

        DocumentReference userRef = db.Collection("teachers").Document(ID);

        userRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to get teacher document: " + task.Exception);
                return;
            }

            DocumentSnapshot snapshot = task.Result;

            if (snapshot.Exists)
            {
                Dictionary<string, object> teacherData = snapshot.ToDictionary();

                if (teacherData.TryGetValue("students", out object studentsObj))
                {
                    List<object> studentsList = studentsObj as List<object>;

                    studentsList.Remove(studentID);

                    Dictionary<string, object> updates = new Dictionary<string, object>
                {
                    { "students", studentsList }
                };

                    userRef.UpdateAsync(updates).ContinueWithOnMainThread(updateTask =>
                    {
                        if (updateTask.IsFaulted)
                        {
                            Debug.LogError("Failed to update teacher document: " + updateTask.Exception);
                        }
                        else
                        {
                            Debug.Log("Student removed successfully.");
                            SceneManager.LoadScene("teachermain", LoadSceneMode.Single);
                        }
                    });
                }
                else
                {
                    Debug.LogError("students field not found in the teacher document.");
                }
            }
            else
            {
                Debug.LogError("Teacher document does not exist.");
            }
        });

    }
}
