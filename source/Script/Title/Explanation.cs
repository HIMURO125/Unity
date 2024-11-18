using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explanation : MonoBehaviour
{
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject Explain;
    public void OnClick()
    {
        Title.SetActive(false);
        Explain.SetActive(true);
    }
}
