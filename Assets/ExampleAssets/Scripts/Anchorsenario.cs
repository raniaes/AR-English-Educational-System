/*using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class Anchorsenario : MonoBehaviour
{
    [SerializeField]
    GameObject m_AnchorPrefab1;

    [SerializeField]
    GameObject m_AnchorPrefab2;

    public GameObject AnchorPrefab1
    {
        get => m_AnchorPrefab1;
        set => m_AnchorPrefab1 = value;
    }

    public GameObject AnchorPrefab2
    {
        get => m_AnchorPrefab2;
        set => m_AnchorPrefab2 = value;
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

            var anchor1 = m_AnchorManager.AttachAnchor(hitPlane, hitPose);
            var anchor2 = m_AnchorManager.AttachAnchor(hitPlane, hitPose);

            if (m_LastSpawnedObject1 != null)
            {
                Destroy(m_LastSpawnedObject1);
            }

            if (m_LastSpawnedObject2 != null)
            {
                Destroy(m_LastSpawnedObject2);
            }

            m_LastSpawnedObject1 = Instantiate(m_AnchorPrefab1, anchor1.transform);
            m_LastSpawnedObject2 = Instantiate(m_AnchorPrefab2, anchor2.transform);

            if (anchor1 == null || anchor2 == null)
            {
                Debug.Log("Error creating anchors.");
            }
            else
            {
                m_AnchorPoints.Add(anchor1);
                m_AnchorPoints.Add(anchor2);
            }
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    List<ARAnchor> m_AnchorPoints;
    GameObject m_LastSpawnedObject1;
    GameObject m_LastSpawnedObject2;

    ARRaycastManager m_RaycastManager;

    ARAnchorManager m_AnchorManager;

    ARPlaneManager m_PlaneManager;
}
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class Anchorsenario : MonoBehaviour
{
    [SerializeField]
    GameObject m_AnchorPrefab1;

    [SerializeField]
    GameObject m_AnchorPrefab2;

    public GameObject startbtn;

    public GameObject AnchorPrefab1
    {
        get => m_AnchorPrefab1;
        set => m_AnchorPrefab1 = value;
    }

    public GameObject AnchorPrefab2
    {
        get => m_AnchorPrefab2;
        set => m_AnchorPrefab2 = value;
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
        if (m_RaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            var hitTrackableId = s_Hits[0].trackableId;
            var hitPlane = m_PlaneManager.GetPlane(hitTrackableId);

            var anchor1 = m_AnchorManager.AttachAnchor(hitPlane, hitPose);
            var anchor2 = m_AnchorManager.AttachAnchor(hitPlane, hitPose);

            if (m_LastSpawnedObject1 != null)
            {
                Destroy(m_LastSpawnedObject1);
            }

            if (m_LastSpawnedObject2 != null)
            {
                Destroy(m_LastSpawnedObject2);
            }

            m_LastSpawnedObject1 = Instantiate(m_AnchorPrefab1, anchor1.transform);
            m_LastSpawnedObject2 = Instantiate(m_AnchorPrefab2, anchor2.transform);

            if (anchor1 == null || anchor2 == null)
            {
                Debug.Log("Error creating anchors.");
            }
            else
            {
                m_AnchorPoints.Add(anchor1);
                m_AnchorPoints.Add(anchor2);
                startbtn.SetActive(true);
            }
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    List<ARAnchor> m_AnchorPoints;
    GameObject m_LastSpawnedObject1;
    GameObject m_LastSpawnedObject2;

    ARRaycastManager m_RaycastManager;

    ARAnchorManager m_AnchorManager;

    ARPlaneManager m_PlaneManager;
}
