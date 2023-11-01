using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class FirestoreExample : MonoBehaviour
{
    // 사용자 입력 필드 및 토글 버튼에 대한 참조
    public InputField idInputField;
    public InputField passwordInputField;
    public InputField nameInputField;
    public InputField emailInputField;
    public Toggle teacherToggle;

    // Firestore 데이터베이스에 대한 참조
    private FirebaseFirestore db;

    private void Start()
    {
        // Firebase 초기화 및 Firestore 인스턴스 가져오기
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

    // 사용자 데이터 Firestore에 추가
    public void AddUserToFirestore()
    {
        // 입력 필드에서 사용자 정보 가져오기
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
                // 사용자에게 알림을 표시하거나 회원가입을 거부할 수 있습니다.
            }
            else
            {
                // ID가 중복되지 않았을 때 회원가입을 진행합니다.
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