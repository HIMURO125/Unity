using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    private PauseManager pauseManager;
    void Start()
    {
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
    }
    public void OnClick()
    {
        pauseManager.Resume();
        Panel.SetActive(false);
    }
}
