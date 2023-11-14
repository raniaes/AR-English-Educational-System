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

        userRef.SetAsync(updates, SetOptions.MergeAll).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"complete");
            }
            else if (task.IsFaulted)
            {
                Debug.LogError($"Fault: " + task.Exception);
            }
        });

        SceneManager.LoadScene("wordmain", LoadSceneMode.Single);
    }
}
