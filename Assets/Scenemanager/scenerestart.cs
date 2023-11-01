using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class scenerestart : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;  // ARTrackedImageManager ����

    public void RestartScene()
    {
        trackedImageManager.enabled = false;

        // ��� �̹��� ���� ������Ʈ ��Ȱ��ȭ �Ǵ� ����
        ARTrackedImage[] trackedImages = FindObjectsOfType<ARTrackedImage>();
        foreach (var trackedImage in trackedImages)
        {
            trackedImage.gameObject.SetActive(false);
        }

        // ���� �� �ٽ÷ε�
        //int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        //UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);

        // �̹��� ���� �ٽ� ����
        trackedImageManager.enabled = true;
    }
}
