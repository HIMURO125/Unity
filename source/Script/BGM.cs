using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] private AudioClip[] Audio;
    [SerializeField] private GameObject Panel;
    private AudioSource audioSource;
    private ShipController shipController;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        shipController=GameObject.FindWithTag("Player").GetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shipController.IsGameOver)
        {
            if (audioSource.clip != Audio[1])
            {
                audioSource.clip = Audio[1];
                audioSource.loop = false;
                audioSource.Play();
            }
        }
        else if (Panel.activeSelf)
        {
            if (audioSource.clip != Audio[2])
            {
                audioSource.clip = Audio[2];
                audioSource.loop = false;
                audioSource.Play();
            }
        }
        else if (audioSource.clip != Audio[0])
        {
            audioSource.clip = Audio[0];
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
