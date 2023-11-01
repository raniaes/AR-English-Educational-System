using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class showstudent : MonoBehaviour
{
    public GameObject scrollViewContent; // ScrollView�� Content ������Ʈ
    public GameObject buttonPrefab; // Button �׸��� ������
    public Text ID;

    private FirebaseFirestore db;

    private void Start()
    {
        // Firebase �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);

            // Firestore���� ������ ��������
            string teacherID = ID.text; // �������� ���� ID�� ���⿡ �Է�
            DocumentReference teacherRef = db.Collection("teachers").Document(teacherID);


            teacherRef.GetSnapshotAsync().ContinueWithOnMainThread(Task =>
            {
                if (Task.IsCompleted)
                {
                    DocumentSnapshot snapshot = Task.Result;
                    if (snapshot.Exists)
                    {
                        Dictionary<string, object> data = snapshot.ToDictionary();
                        if (data.ContainsKey("students"))
                        {
                            List<object> studentsList = data["students"] as List<object>;
                            List<string> students = studentsList.Cast<string>().ToList();

                            // students ����Ʈ�� ScrollView�� ǥ���ϴ� �Լ� ȣ��
                            UpdateScrollViewWithStudents(students);
                        }
                        else
                        {
                            Debug.LogError("�л� ����� �����Ϳ� �����ϴ�.");
                        }
                    }
                    else
                    {
                        Debug.LogError("�ش� ������ ������ �������� �ʽ��ϴ�.");
                    }
                }
                else if (Task.IsFaulted)
                {
                    Debug.LogError("Firestore���� ������ �������� ����: " + Task.Exception);
                }
            });
        });
    }

    void UpdateScrollViewWithStudents(List<string> students)
    {
        RectTransform contentRectTransform = scrollViewContent.GetComponent<RectTransform>();


        // students ����Ʈ�� �� �׸��� Button UI ��ҷ� �����
        foreach (string studentName in students)
        {

            string DBname = null;

            // Button ����
            GameObject buttonObject = Instantiate(buttonPrefab, contentRectTransform);

            // Button�� Text ���� (�л� �̸� ��)
            Text buttonText = buttonObject.GetComponentInChildren<Text>();

            // Button�� Ŭ�� �̺�Ʈ ó���� ���� �Լ� �Ҵ�
            buttonObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                // ��ư�� Ŭ���Ǿ��� �� ������ ���� �߰�
                Debug.Log("������ �л�: " + studentName);
                PlayerPrefs.SetString("studentID", studentName);
                PlayerPrefs.Save();
                SceneManager.LoadScene("studentachievement", LoadSceneMode.Single);
            });

            //get student name
            DocumentReference userRef = db.Collection("user").Document(studentName);

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
                    // DocumentSnapshot�� ��ųʸ��� ��ȯ
                    Dictionary<string, object> userData = snapshot.ToDictionary();

                    if (userData.TryGetValue("name", out object nameObj))
                    {
                        DBname = nameObj as string;
                        buttonText.text = DBname; // ��ư �ؽ�Ʈ ����
                    }
                    else
                    {
                        Debug.LogError("name field not found in the document.");
                    }
                }
                else
                {
                    Debug.LogError("User document does not exist.");
                }
            });
        }
    }
}