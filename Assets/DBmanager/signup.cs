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
    private AudioSource signupsound;

    private FirebaseFirestore db;

    [SerializeField] private AudioClip[] audioClip;

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

        signupsound = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void AddUserToFirestore()
    {
        string id = idInputField.text;
        string password = passwordInputField.text;
        string name = nameInputField.text;
        string email = emailInputField.text;
        bool isTeacher = teacherToggle.isOn;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
        {
            signupsound.clip = audioClip[1];
            signupsound.Play();
            return;
        }

        DocumentReference userRef = db.Collection("user").Document(id);

        userRef.GetSnapshotAsync().ContinueWithOnMainThread(Task =>
        {
            DocumentSnapshot snapshot = Task.Result;

            if (snapshot.Exists)
            {
                Debug.LogError("User with the same ID already exists.");
                signupsound.clip = audioClip[1];
                signupsound.Play();
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
                        signupsound.clip = audioClip[0];
                        signupsound.Play();
                        Debug.Log("User data added to Firestore successfully.");
                        SceneManager.LoadScene("Login", LoadSceneMode.Single);
                    }
                    else if (task.IsFaulted)
                    {
                        signupsound.clip = audioClip[1];
                        signupsound.Play();
                        Debug.LogError("Failed to add user data to Firestore: " + task.Exception);
                    }
                });
            }
        });
    }
}