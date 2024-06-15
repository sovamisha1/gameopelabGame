using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundZone : MonoBehaviour
{
    //public AudioSource audioSource;
    public AudioClip sound;
    public PlayerController playerController;


    void Start()
    {
        if (playerController == null)
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        //audioSource = GetComponent<AudioSource>();
        if (sound == null)
            sound = GetComponent<AudioClip>();
        //sound = GameObject.Find("DM-CGS-03").GetComponent<AudioClip>();
        //Debug.Log(audioSource.clip == null);
    }

    private void OnTriggerEnter(Collider collision){
        playerController.PlaySound(sound);
        Destroy(gameObject);
        //audioSource.Play();
    }
}


