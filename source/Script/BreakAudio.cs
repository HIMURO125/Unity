using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakAudio : MonoBehaviour
{
    [SerializeField] private AudioClip Audio;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        audioSource.PlayOneShot(Audio);
    }
}
