using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void OnEnable()
    {
        audioSource?.Play();
    }
}