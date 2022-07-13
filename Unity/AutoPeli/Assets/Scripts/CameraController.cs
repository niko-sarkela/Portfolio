using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera[] cameras;
    private int activeCamera = 0;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Camera cam in cameras)
        {
            cam.enabled = false;
            cam.GetComponent<AudioListener>().enabled = false;
        }
        cameras[activeCamera].enabled = true;
        cameras[activeCamera].GetComponent<AudioListener>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameras[activeCamera].enabled = false;
            cameras[activeCamera].GetComponent<AudioListener>().enabled = false;
            if (activeCamera < cameras.Length - 1)
            {
                activeCamera++;
            }
            else
            {
                activeCamera = 0;
            }
            cameras[activeCamera].enabled = true;
            cameras[activeCamera].GetComponent<AudioListener>().enabled = true;
        }
    }
}
