using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    public bool isPaused = false;
    public bool IsPaused
    {
        get { return isPaused; }
    }
    private ShipController shipController;
    private void Start()
    {
        shipController = GameObject.FindWithTag("Player").GetComponent<ShipController>();
    }
    private void Update()
    {
        bool GameOver = shipController.IsGameOver;
        if (!isPaused && !GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
                Panel.SetActive(true);
            }
        }
    }
    //�|�[�Y���
    public void Pause()
    {
        isPaused = true;
    }
    //��|�[�Y���
    public void Resume()
    {
        isPaused = false;
    }
}
