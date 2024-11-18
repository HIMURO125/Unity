using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : MonoBehaviour
{
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject Select;
    public void OnClick()
    {
        Title.SetActive(true);
        Select.SetActive(false);
    }
}
