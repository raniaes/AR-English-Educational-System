using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class FirestoreExample : MonoBehaviour
{
    public InputField idInputField;
    public InputField passwordInputField;
    public InputField nameInputField;
    public InputField emailInputField;
    public Toggle teacherToggle;

    private FirebaseFirestore db;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);

            if (db != null)
            {
                Debug.Log("Firebase Firestore initialized.");
            }
            else
            {
                Debug.LogError("Failed to initialize Firebase Firestore.");
            }
        });
    }

    public void AddUserToFirestore()
    {
        string id = idInputField.text;
        string password = passwordInputField.text;
        string name = nameInputField.text;
        string email = emailInputField.text;
        bool isTeacher = teacherToggle.isOn;

        DocumentReference userRef = db.Collection("user").Document(id);

        userRef.GetSnapshotAsync().ContinueWithOnMainThread(Task =>
        {
            DocumentSnapshot snapshot = Task.Result;

            if (snapshot.Exists)
            {
                Debug.LogError("User with the same ID already exists.");
            }
            else
            {
                var userData = new
                {
                    password = password,
                    name = name,
                    email = email,
                    usertype = isTeacher
                };

                userRef.SetAsync(userData).ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompleted)
                    {
                        Debug.Log("User data added to Firestore successfully.");
                        SceneManager.LoadScene("Login", LoadSceneMode.Single);
                    }
                    else if (task.IsFaulted)
                    {
                        Debug.LogError("Failed to add user data to Firestore: " + task.Exception);
                    }
                });
            }
        });
    }
}