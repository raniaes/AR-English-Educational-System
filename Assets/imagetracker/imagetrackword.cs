using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(ARTrackedImageManager))]
public class imagetrackword : MonoBehaviour
{
    //public float _timer;
    public Text updatecheck;
    public ARTrackedImageManager trackedImageManger;
    public List<GameObject> _objectList = new List<GameObject>();
    private Dictionary<string, GameObject> _prefabDic = new Dictionary<string, GameObject>();
    private List<ARTrackedImage> _trackedImg = new List<ARTrackedImage>();
    //private List<float> _trackedtimer = new List<float>();


    void Start()
    {
        PlayerPrefs.SetInt("word2check", 0);
        PlayerPrefs.Save();
    }

    void Awake()
    {
        foreach(GameObject obj in _objectList)
        {
            string tName = obj.name;
            _prefabDic.Add(tName, obj);
        }
    }

    void Update()
    {
        if (_trackedImg.Count > 0)
        {
            List <ARTrackedImage> tNumList = new List<ARTrackedImage> ();
            for (var i = 0; i < _trackedImg.Count; i++)
            {
                if (_trackedImg[i].trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited)
                {
                    //if (_trackedtimer[i] > _timer)
                    //{
                        string name = _trackedImg[i].referenceImage.name;
                        GameObject tObj = _prefabDic[name];
                        tObj.SetActive(false);  
                        tNumList.Add(_trackedImg[i]);
                    //}
                    /*else
                    {
                        _trackedtimer[i] += 0.01f;
                        updatecheck.text = "" + _trackedtimer[0];
                    }*/
                }
            }

            if ( tNumList.Count > 0)
            {
                for (var i = 0; i < tNumList.Count ; i++)
                {
                    int num = _trackedImg.IndexOf(tNumList[i]);
                    _trackedImg.Remove(_trackedImg[num]);
                    //_trackedtimer.Remove(_trackedtimer[num]);
                }
            }
        }
    }

    void OnEnable()
    {
        trackedImageManger.trackedImagesChanged += ImageChanged;
    }

    void OnDisable()
    {
        trackedImageManger.trackedImagesChanged -= ImageChanged;
    }


    void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            if (!_trackedImg.Contains(trackedImage))
            {
                Debug.Log("recognetion");
                _trackedImg.Add(trackedImage);
                //_trackedtimer.Add(0);
                
            }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            if (!_trackedImg.Contains(trackedImage))
            {
                Debug.Log("recognetion");
                _trackedImg.Add(trackedImage);
                //_trackedtimer.Add(0);
            }
            else
            {
                int num = _trackedImg.IndexOf(trackedImage);
                //_trackedtimer[num] = 0;
            }
            UpdateImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            _prefabDic[trackedImage.name].SetActive(false);
        }

        if (eventArgs.updated.Count > 2)
        {
            PlayerPrefs.SetInt("word2check", 1);
            PlayerPrefs.Save();
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;

        GameObject tObj = _prefabDic[name];
        tObj.transform.position = trackedImage.transform.position;
        tObj.transform.rotation = trackedImage.transform.rotation;
        tObj.SetActive(true);
        Debug.Log("creation object");
    }
}
