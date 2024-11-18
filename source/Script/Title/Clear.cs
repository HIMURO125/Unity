using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    [SerializeField] private GameObject Special;

    // Update is called once per frame
    void Update()
    {
        if (Boss.Level3Clear)
        {
            Special.SetActive(true);
        }
    }
}
