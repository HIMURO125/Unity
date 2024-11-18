using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pass2 : MonoBehaviour
{
    private int Pass = 0;
    private TMPro.TMP_Text Text;
    public int getpass
    {
        get { return Pass; }
    }
    public void setpass()
    {
        Pass = 0;
        Text.SetText(Pass.ToString());
    }
    void Start()
    {
        Text = GetComponent<TMPro.TMP_Text>();
        Text.SetText(Pass.ToString());
    }
    public void Onclick()
    {
        Pass++;
        if (Pass > 9)
        {
            Pass = 0;
        }
        Text.SetText(Pass.ToString());
    }
}
