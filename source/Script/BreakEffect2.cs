using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEffect2 : MonoBehaviour
{
    void Start()
    {
        Invoke("EffectBreak", 3.0f);

    }
    void EffectBreak()
    {
        Destroy(gameObject);
    }
}
