using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    [SerializeField] GameObject Target;
    [SerializeField] TMPro.TMP_Text PosText;
    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.FindWithTag("Player");
        PosText = GameObject.FindWithTag("Position").GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 world = Target.transform.position;
        string positionString = $"ç¿ïW [{(int)world.x} : {(int)world.z}]";
        PosText.text = positionString;
    }
}
