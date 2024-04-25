using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using distriqt.plugins.vibration;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]

public class ARtutorial : MonoBehaviour
{
    [SerializeField]
    GameObject m_AnchorPrefab;

    public GameObject circle1;
    public GameObject circle2;
    public GameObject circle3;
    public GameObject circle4;
    public Text infotext;

    private AudioSource audioSource;

    [SerializeField] private AudioClip audioClip;

    public GameObject AnchorPrefab
    {
        get => m_AnchorPrefab;
        set => m_AnchorPrefab = value;
    }

    public void RemoveAllAnchors()
    {
        foreach (var anchor in m_AnchorPoints)
        {
            Destroy(anchor);
        }
        m_AnchorPoints.Clear();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

                if (circle1 != null)
                {
                    circle1.SetActive(true);
                }
                if (circle2 != null)
                {
                    circle2.SetActive(true);
                }
                if (circle3 != null)
                {
                    circle3.SetActive(true);
                }
                if (circle4 != null)
                {
                    circle4.SetActive(true);
                }

                if (infotext != null)
                {
                    infotext.text = "buttons will be created in the red circles, buttons can be used to interact with the system.\nClick circle.";
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
