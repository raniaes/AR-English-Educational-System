using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class imagerecog : MonoBehaviour
{
    public GameObject soundButton;
    public GameObject speechButton;

    void OnEnable()
    {
        // 이미지 트래킹 이벤트 리스너 등록
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        // 리스너 해제
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        soundButton.SetActive(true);
        speechButton.SetActive(true);
    }

    ARTrackedImageManager m_TrackedImageManager;
}
