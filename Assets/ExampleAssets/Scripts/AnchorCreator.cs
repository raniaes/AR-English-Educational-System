using distriqt.plugins.vibration;
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
    public GameObject descripbtn;
    public GameObject speechbtn;
    private float initialBatteryLevel;
    private AudioSource audioSource;

    [SerializeField] private AudioClip audioClip;

    public GameObject AnchorPrefab
    {
        get => m_AnchorPrefab;
        set => m_AnchorPrefab = value;
    }

    void Start()
    {
        initialBatteryLevel = UnityEngine.SystemInfo.batteryLevel;

        audioSource = GetComponent<AudioSource>();
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
                Vibration.Instance.Vibrate(100);

                if (!speechbtn.activeSelf)
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
                }

                float frameRate = 1.0f / Time.deltaTime;
                Debug.Log("Frame Rate: " + frameRate);

                float batteryUsage = UnityEngine.SystemInfo.batteryLevel - initialBatteryLevel;
                Debug.Log("Battery Usage: " + batteryUsage);

                if (soundbtn != null) 
                { 
                    soundbtn.SetActive(true);
                }
                if (descripbtn != null)
                {
                    descripbtn.SetActive(true);
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
    GameObject m_LastSpawnedObject;

    ARRaycastManager m_RaycastManager;

    ARAnchorManager m_AnchorManager;

    ARPlaneManager m_PlaneManager;
}
