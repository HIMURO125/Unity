using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject Select;
    [SerializeField] private GameObject Level2;
    [SerializeField] private GameObject Level3;
    public void Onclick()
    {
        Title.SetActive(false);
        Select.SetActive(true);
        if (IslandManager.Level1Clear)
        {
            Level2.SetActive(true);
            Level3.SetActive(false);
        }
        if (IslandManager2.Level2Clear)
        {
            Level2.SetActive(true);
            Level3.SetActive(true);
        }
    }
}
