using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundZone : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip sound;
    public PlayerController playerController;


    void Start()
    {
        if (playerController == null)
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        //sound = GameObject.Find("DM-CGS-03").GetComponent<AudioClip>();
        //Debug.Log(sound == null);
    }

    private void OnTriggerEnter(Collider collision){
        playerController.PlaySound(audioSource.clip);
        Destroy(gameObject);
        //audioSource.Play();
    }
}


