using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class AnchorCreator : MonoBehaviour
{
    [SerializeField]
    GameObject m_AnchorPrefab;

    public GameObject soundbtn;
    public GameObject speechbtn;
    private float initialBatteryLevel;

    public GameObject AnchorPrefab
    {
        get => m_AnchorPrefab;
        set => m_AnchorPrefab = value;
    }

    void Start()
    {
        // 앱이 시작될 때 현재 배터리 레벨을 저장
        initialBatteryLevel = UnityEngine.SystemInfo.batteryLevel;
    }

    public void RemoveAllAnchors()
    {
        foreach (var anchor in m_AnchorPoints)
        {
            Destroy(anchor);
        }
        m_AnchorPoints.Clear();
    }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AnchorManager = GetComponent<ARAnchorManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_AnchorPoints = new List<ARAnchor>();
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
            return;

        if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            var hitTrackableId = s_Hits[0].trackableId;
            var hitPlane = m_PlaneManager.GetPlane(hitTrackableId);

            var anchor = m_AnchorManager.AttachAnchor(hitPlane, hitPose);

            // 삭제하기 전에 이전 오브젝트 삭제
            if (m_LastSpawnedObject != null)
            {
                Destroy(m_LastSpawnedObject);
            }

            m_LastSpawnedObject = Instantiate(m_AnchorPrefab, anchor.transform);

            if (anchor == null)
            {
                Debug.Log("Error creating anchor.");
            }
            else
            {
                m_AnchorPoints.Add(anchor);

                float frameRate = 1.0f / Time.deltaTime;
                Debug.Log("Frame Rate: " + frameRate);

                float batteryUsage = UnityEngine.SystemInfo.batteryLevel - initialBatteryLevel;
                Debug.Log("Battery Usage: " + batteryUsage);

                // AR 오브젝트가 생성될 때 버튼 활성화
                if (soundbtn != null) 
                { 
                    soundbtn.SetActive(true);
                }
                if (speechbtn != null) 
                { 
                    speechbtn.SetActive(true); 
                }
            }
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    List<ARAnchor> m_AnchorPoints;
    GameObject m_LastSpawnedObject; // 이전에 생성된 오브젝트를 추적하기 위한 변수

    ARRaycastManager m_RaycastManager;

    ARAnchorManager m_AnchorManager;

    ARPlaneManager m_PlaneManager;
}
