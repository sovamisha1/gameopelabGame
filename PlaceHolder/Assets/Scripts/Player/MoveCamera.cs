using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    //public Transform cameraPosition;
    public Transform cameraPos;
    
    private void Start(){
    if (cameraPos == null)
            cameraPos = GameObject.Find("CameraPos").GetComponent<Transform>();
    }

    private void Update(){
        transform.position = cameraPos.position;
    }

}
