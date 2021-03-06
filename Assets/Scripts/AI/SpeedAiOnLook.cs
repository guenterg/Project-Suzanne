﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpeedAiOnLook : MonoBehaviour
{

    public float speed = 5;
    public float speedChangeDelay;
    public float maxDistance = 200000f;
    public string aiObjectName; //IMPORTANT!! This name will be used to search ai object and set speed.
    private static GameObject aiObject;
    private static MoveTo aiMoveTo;
    private static int visibleObjects = 0;
    private bool isVisible = false, changedVisibleCounter = false;


    // Start is called before the first frame update
    void Start()
    {
        if (aiObjectName != null && aiObjectName.Length > 0)
        {
            aiObject = GameObject.Find(aiObjectName);
            aiMoveTo = aiObject.GetComponent<MoveTo>();
        }
        else
        {
            Debug.LogError("Please set aiObjectName for " + this.gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(visibleObjects);
        if(isVisible)
        {
            Vector3 cameraPosition = Camera.main.transform.position;
            RaycastHit hit;
            int layermask = 1 << 3; //Set up to ignore IgnoreRaycast layer. Change the shifting if layers change!!
            layermask = ~layermask;
            Ray ray = new Ray(cameraPosition, transform.position - cameraPosition);
            if (Physics.Raycast(ray, out hit, maxDistance, layermask))
            {
                if ((hit.collider.gameObject == this.gameObject))
                {   //Visible
                    aiMoveTo.SetAiSpeed(Mathf.Max(speed, aiMoveTo.GetAiSpeed()));   //This won't decrease the speed, just send speed to decrease it.
                    if (changedVisibleCounter == false)
                    {
                        visibleObjects++;
                        changedVisibleCounter = true;
                    }
                }
                else
                {   //Obstructed
                    if(changedVisibleCounter == true)
                    {
                        if (visibleObjects > 0)
                        {
                            visibleObjects--;
                            changedVisibleCounter = false;
                        }
                        if (visibleObjects == 0)
                            aiMoveTo.ResetAiSpeed();
                    } 
                }
            }
            else
            {   //Obstructed
                if (changedVisibleCounter == true)
                {
                    if (visibleObjects > 0)
                    {
                        visibleObjects--;
                        changedVisibleCounter = false;
                    }
                    if (visibleObjects == 0)
                        aiMoveTo.ResetAiSpeed();
                }
            }
        }
        else 
        {
            if (aiObject != null && aiMoveTo != null)
            {
                //Off screen
                if (changedVisibleCounter == true)
                {
                    if (visibleObjects > 0)
                    {
                        visibleObjects--;
                        changedVisibleCounter = false;
                    }
                    if (visibleObjects == 0)
                        aiMoveTo.ResetAiSpeed();
                }
            }
        }
    }

    void OnBecameInvisible()
    {
        isVisible = false;       
    }

    void OnBecameVisible()
    {
        isVisible = true;
    }
}