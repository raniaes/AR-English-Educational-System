using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using distriqt.plugins.vibration;
using UnityEngine.EventSystems;

public class showstudent : MonoBehaviour
{
    public GameObject scrollViewContent;
    public GameObject buttonPrefab;
    public Text ID;

    private FirebaseFirestore db;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);

            string teacherID = ID.text;
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

                            UpdateScrollViewWithStudents(students);
                        }
                        else
                        {
                            Debug.LogError("Don't exist student list");
                        }
                    }
                    else
                    {
                        Debug.LogError("Don't exist teacher document");
                    }
                }
                else if (Task.IsFaulted)
                {
                    Debug.LogError("Falut: " + Task.Exception);
                }
            });
        });
    }

    void UpdateScrollViewWithStudents(List<string> students)
    {
        RectTransform contentRectTransform = scrollViewContent.GetComponent<RectTransform>();


        // students 리스트의 각 항목을 Button UI 요소로 만들기
        foreach (string studentName in students)
        {

            string DBname = null;

            // Button 생성
            GameObject buttonObject = Instantiate(buttonPrefab, contentRectTransform);

            // Button의 Text 설정 (학생 이름 등)
            Text buttonText = buttonObject.GetComponentInChildren<Text>();

            // Button의 클릭 이벤트 처리를 위한 함수 할당
            buttonObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                // 버튼이 클릭되었을 때 실행할 동작 추가
                Debug.Log("선택한 학생: " + studentName);
                PlayerPrefs.SetString("studentID", studentName);
                PlayerPrefs.Save();
                SceneManager.LoadScene("studentachievement", LoadSceneMode.Single);
            });

            // Button에 진동 이벤트 처리를 추가
            EventTrigger trigger = buttonObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = buttonObject.AddComponent<EventTrigger>();
            }

            // PointerDown 이벤트에 대한 이벤트 핸들러 추가
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { Vibration.Instance.Vibrate(100); });
            trigger.triggers.Add(entry);

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
                    // DocumentSnapshot을 딕셔너리로 변환
                    Dictionary<string, object> userData = snapshot.ToDictionary();

                    if (userData.TryGetValue("name", out object nameObj))
                    {
                        DBname = nameObj as string;
                        buttonText.text = DBname; // 버튼 텍스트 설정
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
