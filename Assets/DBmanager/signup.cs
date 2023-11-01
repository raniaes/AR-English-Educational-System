using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class FirestoreExample : MonoBehaviour
{
    // ����� �Է� �ʵ� �� ��� ��ư�� ���� ����
    public InputField idInputField;
    public InputField passwordInputField;
    public InputField nameInputField;
    public InputField emailInputField;
    public Toggle teacherToggle;

    // Firestore �����ͺ��̽��� ���� ����
    private FirebaseFirestore db;

    private void Start()
    {
        // Firebase �ʱ�ȭ �� Firestore �ν��Ͻ� ��������
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

    // ����� ������ Firestore�� �߰�
    public void AddUserToFirestore()
    {
        // �Է� �ʵ忡�� ����� ���� ��������
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
                // ����ڿ��� �˸��� ǥ���ϰų� ȸ�������� �ź��� �� �ֽ��ϴ�.
            }
            else
            {
                // ID�� �ߺ����� �ʾ��� �� ȸ�������� �����մϴ�.
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