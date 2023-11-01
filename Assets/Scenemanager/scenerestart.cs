using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class scenerestart : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;  // ARTrackedImageManager 참조

    public void RestartScene()
    {
        trackedImageManager.enabled = false;

        // 모든 이미지 게임 오브젝트 비활성화 또는 제거
        ARTrackedImage[] trackedImages = FindObjectsOfType<ARTrackedImage>();
        foreach (var trackedImage in trackedImages)
        {
            trackedImage.gameObject.SetActive(false);
        }

        // 현재 씬 다시로드
        //int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        //UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);

        // 이미지 추적 다시 시작
        trackedImageManager.enabled = true;
    }
}
