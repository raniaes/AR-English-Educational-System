using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class scenerestart : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;

    public void RestartScene()
    {
        trackedImageManager.enabled = false;

        ARTrackedImage[] trackedImages = FindObjectsOfType<ARTrackedImage>();
        foreach (var trackedImage in trackedImages)
        {
            trackedImage.gameObject.SetActive(false);
        }

        //int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        //UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);

        trackedImageManager.enabled = true;
    }
}
