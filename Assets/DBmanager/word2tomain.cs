using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;

public class word2tomain : MonoBehaviour
{
    public Text IDtext;

    private FirebaseFirestore db;

    void Start()
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

    public void gotostudentmain()
    {
        string id = IDtext.text;
        int word2check = PlayerPrefs.GetInt("word2check");

        DocumentReference userRef = db.Collection("achievement").Document(id);

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "word2", word2check == 1 ? true : false }
        };

        // SetAsync 메서드를 사용하여 필드를 추가하거나 업데이트합니다.
        userRef.SetAsync(updates, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"word2 필드 추가 또는 업데이트 완료.");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError($"word2 필드 추가 또는 업데이트 중 오류 발생: " + task.Exception);
            }
        });

        SceneManager.LoadScene("wordmain", LoadSceneMode.Single);
    }
}
