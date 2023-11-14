using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FirestoreAuth : MonoBehaviour
{
    public InputField IDInput;
    public InputField passwordInput;

    private FirebaseFirestore db;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);
        });
    }

    public void Login()
    {
        string ID = IDInput.text;
        string password = passwordInput.text;
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
                Dictionary<string, object> userData = snapshot.ToDictionary();

                if (userData.TryGetValue("password", out object passwordObj))
                {
                    string DBpassword = passwordObj as string;

                    if (userData.TryGetValue("usertype", out object usertypeObj))
                    {
                        bool usertype = (bool)usertypeObj;
                        if (password == DBpassword)
                        {
                            if (!usertype)
                            {
                                PlayerPrefs.SetString("PlayerID", ID);
                                PlayerPrefs.Save();
                                Debug.Log("Learner Login Success.");
                                SceneManager.LoadScene("studentmain", LoadSceneMode.Single);
                            }
                            else
                            {
                                PlayerPrefs.SetString("PlayerID", ID);
                                PlayerPrefs.Save();
                                Debug.Log("Educator Login Success.");
                                SceneManager.LoadScene("teachermain", LoadSceneMode.Single);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        Debug.LogError("UserType field not found in the document.");
                    }
                }
                else
                {
                    Debug.LogError("Password field not found in the document.");
                }
            }
            else
            {
                Debug.LogError("User document does not exist.");
            }
        });
    }
}
