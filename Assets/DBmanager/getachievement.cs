using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Linq;

public class getachievement : MonoBehaviour
{
    public Text ID;
    public Text quizscore;
    public Text puzzlescore;
    public Text wordstate;
    public Text word2state;
    public Text sentencestate;
    public Text senariostate;

    private bool[] wordach;
    private bool[] sentenceach;
    private bool[] senarioach;
    private bool[] quizach;
    private bool[] puzzleach;

    private FirebaseFirestore db;

    void Start()
    {
        wordach = new bool[2];
        sentenceach = new bool[1];
        senarioach = new bool[1];
        quizach = new bool[1];
        puzzleach = new bool[1];

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            db = FirebaseFirestore.GetInstance(app);

            string studentID = ID.text;
            DocumentReference studentRef = db.Collection("achievement").Document(studentID);


            studentRef.GetSnapshotAsync().ContinueWithOnMainThread(Task =>
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

                    //word1 data get
                    if (userData.TryGetValue("word1", out object wordObj))
                    {
                        if (wordObj is bool)
                        {
                            wordach[0] = (bool)wordObj;
                        }
                        else
                        {
                            wordach[0] = false;
                        }
                    }
                    else
                    {
                        Debug.LogError("word field not found in the document.");
                    }

                    //word2 data get
                    if (userData.TryGetValue("word2", out object word2Obj))
                    {
                        if (word2Obj is bool)
                        {
                            wordach[1] = (bool)word2Obj;
                        }
                        else
                        {
                            wordach[1] = false;
                        }
                    }
                    else
                    {
                        Debug.LogError("word field not found in the document.");
                    }

                    //sentence data get
                    if (userData.TryGetValue("sentence", out object sentenceObj))
                    {
                        if (sentenceObj is bool)
                        {
                            sentenceach[0] = (bool)sentenceObj;
                        }
                        else
                        {
                            sentenceach[0] = false;
                        }
                    }
                    else
                    {
                        Debug.LogError("sentence field not found in the document.");
                    }

                    //senario data get
                    if (userData.TryGetValue("senario", out object senarioObj))
                    {
                        if (senarioObj is bool)
                        {
                            senarioach[0] = (bool)senarioObj;
                        }
                        else
                        {
                            senarioach[0] = false;
                        }
                    }
                    else
                    {
                        Debug.LogError("senario field not found in the document.");
                    }

                    //quiz data get
                    if (userData.TryGetValue("quiz", out object quizoObj))
                    {
                        if (quizoObj is bool)
                        {
                            quizach[0] = (bool)quizoObj;
                        }
                        else
                        {
                            quizach[0] = false;
                        }
                    }
                    else
                    {
                        Debug.LogError("quiz field not found in the document.");
                    }

                    //puzzle data get
                    if (userData.TryGetValue("puzzle", out object puzzleObj))
                    {
                        if (puzzleObj is bool)
                        {
                            puzzleach[0] = (bool)puzzleObj;
                        }
                        else
                        {
                            puzzleach[0] = false;
                        }
                    }
                    else
                    {
                        Debug.LogError("puzzle field not found in the document.");
                    }
                }
                else
                {
                    Debug.LogError("User document does not exist.");
                }

                showdata();
            });
        });
    }

    public void showdata()
    {
        //quiz
        int quiztrue = quizach.Count(x => x);

        quizscore.text = quiztrue + "/" + quizach.Length;

        //puzzle
        int puzzletrue = puzzleach.Count(y => y);

        puzzlescore.text = puzzletrue + "/" + puzzleach.Length;

        //word1
        wordstate.text = wordach[0] ? "Complete" : "Incomplete";

        //word2
        word2state.text = wordach[1] ? "Complete" : "Incomplete";

        //sentence
        sentencestate.text = sentenceach[0] ? "Complete" : "Incomplete";

        //senario
        senariostate.text = senarioach[0] ? "Complete" : "Incomplete";
    }
}
