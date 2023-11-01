using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class tutorialtomain : MonoBehaviour
{
    public Text IDtext;
    private FirebaseFirestore db;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);
        });
    }

    public void movescene()
    {
        string ID = IDtext.text;
        bool usertype;

        DocumentReference userRef = db.Collection("user").Document(ID);
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

                if (userData.TryGetValue("usertype", out object usertypeObj))
                {
                    bool usertype = (bool)usertypeObj;
                    // password 비교 및 로그인 화면 전환
                    if (!usertype)
                    {
                        SceneManager.LoadScene("studentmain", LoadSceneMode.Single);
                    }
                    else
                    {
                        SceneManager.LoadScene("teachermain", LoadSceneMode.Single);
                    }
                }
                else
                {
                    Debug.LogError("UserType field not found in the document.");
                }
            }
            else
            {
                Debug.LogError("User document does not exist.");
            }
        });
    }
}
