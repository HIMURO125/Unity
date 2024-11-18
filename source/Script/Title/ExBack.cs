using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExBack : MonoBehaviour
{
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject Explain;
    public void OnClick()
    {
        Title.SetActive(true);
        Explain.SetActive(false);
    }
}
