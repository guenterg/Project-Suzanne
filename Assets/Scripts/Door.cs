﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("door collided");
        if (other.gameObject.CompareTag("Player") && KeyDoorManager.instance.keyPickedUp)
        {
            Cursor.lockState = UnityEngine.CursorLockMode.Confined;
            Cursor.visible = true;
            SceneManager.LoadScene("Death Screen", LoadSceneMode.Single);

        }
    }
}
