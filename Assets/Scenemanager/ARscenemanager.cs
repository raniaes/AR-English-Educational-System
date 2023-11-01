using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;

public class ARscenemanager : MonoBehaviour
{
    public void gotostudentmain()
    {
        SceneManager.LoadScene("studentmain", LoadSceneMode.Single);
    }

    public void gotoScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName,LoadSceneMode.Single);
    }
}
