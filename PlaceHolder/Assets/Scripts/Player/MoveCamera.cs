using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
    //public GameObject player1;
    //public PlayerController pc1;

    //void Start(){
    //    pc1 = player1.GetComponent<PlayerController>();
    //}
    // Update is called once per frame
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
