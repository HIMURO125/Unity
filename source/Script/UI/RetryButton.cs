using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    public void OnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Panel.SetActive(false);
    }
}
