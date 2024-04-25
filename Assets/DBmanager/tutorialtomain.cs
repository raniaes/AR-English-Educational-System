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
    private AudioSource audioSource;

    [SerializeField] private AudioClip audioClip;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);
        });

        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
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
                Dictionary<string, object> userData = snapshot.ToDictionary();

                if (userData.TryGetValue("usertype", out object usertypeObj))
                {
                    bool usertype = (bool)usertypeObj;
                    if (!usertype)
                    {
                        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
                        foreach (AudioSource audioSource in audioSources)
                        {
                            if (audioSource.isPlaying)
                            {
                                audioSource.Stop();
                            }
                        }

                        audioSource.clip = audioClip;
                        audioSource.Play();

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
