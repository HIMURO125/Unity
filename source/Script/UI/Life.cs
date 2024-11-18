using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    [SerializeField] RawImage Life1;
    [SerializeField] RawImage Life2;
    [SerializeField] RawImage Life3;
    [SerializeField] Texture2D RedHeart;
    [SerializeField] Texture2D BlackHeart;
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        i = transform.childCount - 1;
        Life1.texture = RedHeart;
        Life2.texture = RedHeart;
        Life3.texture = RedHeart;
    }
    public void Damage()
    {
        transform.GetChild(i).GetComponent<RawImage>().texture = BlackHeart;
        if (i > 0)
        {
            i--;
        }
    }
}
