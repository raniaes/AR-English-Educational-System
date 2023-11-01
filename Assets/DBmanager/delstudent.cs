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
        // 팝업 창을 활성화합니다.
        confirmationPopup.SetActive(true);
    }

    public void CloseConfirmationPopup()
    {
        // 팝업 창을 닫습니다.
        confirmationPopup.SetActive(false);
    }

    public void ConfirmDelete()
    {
        // Yes 버튼을 누르면 실행되는 함수
        CloseConfirmationPopup(); // 팝업 창을 닫습니다.

        // 삭제 작업 수행
        delstudentclick();
    }

    public void CancelDelete()
    {
        // No 버튼을 누르면 실행되는 함수
        CloseConfirmationPopup(); // 팝업 창을 닫습니다.
    }

    public void delstudentclick()
    {
        string ID = IDText.text;
        string studentID = IDstudent.text;

        DocumentReference userRef = db.Collection("teachers").Document(ID);

        // 해당 문서를 가져와서 students 배열을 업데이트합니다.
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
                // DocumentSnapshot을 딕셔너리로 변환
                Dictionary<string, object> teacherData = snapshot.ToDictionary();

                if (teacherData.TryGetValue("students", out object studentsObj))
                {
                    List<object> studentsList = studentsObj as List<object>;

                    // 학생을 삭제할 조건을 설정하세요.
                    // 예를 들어, studentID를 삭제하려면 다음과 같이 학생을 제거할 수 있습니다.
                    studentsList.Remove(studentID);

                    // students 배열을 업데이트합니다.
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
