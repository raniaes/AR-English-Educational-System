using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;

public class wordtomain : MonoBehaviour
{

    public Text IDtext;
    public Text speechtext;

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

    public void gotostudentmain(string content)
    {
        string id = IDtext.text;

        DocumentReference userRef = db.Collection("achievement").Document(id);

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { content, speechtext.color == Color.green }
        };

        // SetAsync 메서드를 사용하여 필드를 추가하거나 업데이트합니다.
        userRef.SetAsync(updates, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"{content} filed add or update complete.");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError($"{content} filed add or update fail: " + task.Exception);
            }
        });

        if (content == "word1" || content == "word2")
        {
            SceneManager.LoadScene("wordmain", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("studentmain", LoadSceneMode.Single);
        }
    }
}
