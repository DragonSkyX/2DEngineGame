using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {



    GameObject Player;
    Vector3 offset;
    Camera Cam;
    float CameraH;
    Vector3 cameraposition;
    Vector3 initialcamera;



    // Use this for initialization

    void Awake()
    {

       Player = GameObject.FindGameObjectWithTag("Player");
       Cam = Camera.main;
       cameraposition = transform.position;
    }


    void Start()
    {

        
        cameraposition = transform.position;
        initialcamera = transform.position;
        offset = cameraposition - Player.transform.position;



    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.


        
        
            cameraposition = Player.transform.position + offset;
            transform.position = cameraposition;
        
   


    }


}

