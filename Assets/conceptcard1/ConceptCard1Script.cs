﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Video;

public class ConceptCard1Script : MonoBehaviour
{
    private int play_once = 0;
    private string card_name = "";

    //Fetch the Animator
    public Animator m_Animator;
    public GameObject GUI_Interface;
    public VideoPlayer videoPlayer;
    public GameObject ring1;
    public GameObject ring2;
    private bool Object_Found = false;

    private ARTrackedImageManager _arTrackImageManager;

    private void Awake()
    {
        _arTrackImageManager = FindObjectOfType<ARTrackedImageManager>();

    }

    public void OnEnable()
    {
        _arTrackImageManager.trackedImagesChanged += OnImageChanged;
    }

    public void OnDisable()
    {
        _arTrackImageManager.trackedImagesChanged -= OnImageChanged;
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            Debug.Log(trackedImage.name);
            card_name = trackedImage.name;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool DoesTagExist(string aTag)
    {
        if(GameObject.FindWithTag("ConceptObject"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(DoesTagExist("ConceptObject") && Object_Found == false){
            Object_Found = true;
        }

        if(Object_Found){
            if(play_once == 0){
                if(m_Animator == null){
                    //Get the Animator from the GUI
                    GUI_Interface = GameObject.FindWithTag("ConceptObject");
                    m_Animator = GUI_Interface.gameObject.GetComponent<Animator>();
                    m_Animator.SetBool("start", false);
                }

                
                if(GUI_Interface != null){
                    // Get Video from GameObject in GUI_Interface
                    // play and pause video to set video frame to first frame of image rather then it being empty
                    videoPlayer = GUI_Interface.gameObject.transform.Find("Canvas").gameObject.transform.Find("Map").GetComponent<VideoPlayer>();
                    videoPlayer.Play();
                    videoPlayer.Pause();
                    ring1 = GameObject.Find("img2");
                    ring2 = GameObject.Find("img4");
                    play_once = 1;
                }
            }

            

            //if(Input.touchCount > 0 && play_once == 0 && System.String.Equals(card_name,"idcard"))
            if((Input.touchCount > 0 || Input.GetMouseButtonDown(0)) && play_once == 1)
            {
                m_Animator.SetBool("start", true);
                //Start the coroutine define below to play the video
                StartCoroutine(TimeCoroutine());
                play_once = 2;
            }

            
            if(play_once >= 1){
                ring1.transform.Rotate(0, 0, 2, Space.Self);
                ring2.transform.Rotate(0, 0, -2, Space.Self);
            }
        }


    }


    IEnumerator TimeCoroutine()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.5f);
        videoPlayer.Play();
    }
}
