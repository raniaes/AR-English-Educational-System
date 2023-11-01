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
    public GameObject scrollViewContent; // ScrollView의 Content 오브젝트
    public GameObject buttonPrefab; // Button 항목의 프리팹
    public Text ID;

    private FirebaseFirestore db;

    private void Start()
    {
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);

            // Firestore에서 데이터 가져오기
            string teacherID = ID.text; // 선생님의 문서 ID를 여기에 입력
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

                            // students 리스트를 ScrollView에 표시하는 함수 호출
                            UpdateScrollViewWithStudents(students);
                        }
                        else
                        {
                            Debug.LogError("학생 목록이 데이터에 없습니다.");
                        }
                    }
                    else
                    {
                        Debug.LogError("해당 선생님 문서가 존재하지 않습니다.");
                    }
                }
                else if (Task.IsFaulted)
                {
                    Debug.LogError("Firestore에서 데이터 가져오기 실패: " + Task.Exception);
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
