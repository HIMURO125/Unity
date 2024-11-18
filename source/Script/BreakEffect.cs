using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEffect : MonoBehaviour
{
    void Start()
    {
        Invoke("EffectBreak", 2.0f);

    }
    void EffectBreak()
    {
        Destroy(gameObject);
    }
}
