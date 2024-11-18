using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager2 : MonoBehaviour
{
    public static bool Level2Clear = false;
    [SerializeField] private GameObject[] Islands = new GameObject[4];
    [SerializeField] private GameObject ScorePanel;
    [SerializeField] private GameObject TextPanel;
    [SerializeField] private TMPro.TMP_Text numText;
    private string[] text;
    private int[,] Numbers = new int[3, 4] { { -4, 2, -20, 4 }, { -2, 1, 18, 3 }, { 15, 1, -12, 4 } };
    private Vector3[] Position = new Vector3[3] { new Vector3(-42, 0, -204), new Vector3(-21, 0, 183), new Vector3(151, 0, -124) };
    private Coroutine showCoroutine;
    private float textSpeed = 0.02f;
    private bool showflg = false;
    private bool flg = false;
    private int i = 0;
    private int num1;
    public int Num1
    {
        get { return num1; }
    }
    private int num2;
    public int Num2
    {
        get { return num2; }
    }
    private int num3;
    public int Num3
    {
        get { return num3; }
    }
    private int num4;
    public int Num4
    {
        get { return num4; }
    }
    private PauseManager pauseManager;
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int index = Random.Range(0, 3);
        gameObject.transform.position = Position[index];
        num1 = Numbers[index, 0];
        num2 = Numbers[index, 1];
        num3 = Numbers[index, 2];
        num4 = Numbers[index, 3];
        Islands[0].GetComponent<Island1_2>().enabled = true;
        Islands[1].GetComponent<Island2_2>().enabled = true;
        Islands[2].GetComponent<Island3_2>().enabled = true;
        Islands[3].GetComponent<Island4_2>().enabled = true;
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        text = new string[4];
        text[0] = "うーん…\nここにお宝があると思うんだけど…";
        text[1] = "もしかして海の底に？\n少し潜って調べてみよう";
        text[2] = "………";
        text[3] = "ガチガチに固まった宝箱があった！\n早速持ち帰って開けてみよう！";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && flg && !showflg)
        {
            TextPanel.SetActive(true);
            pauseManager.Pause();
            Show();
            showflg = true;
        }
        if (Input.GetKeyDown(KeyCode.Return) && flg && showflg)
        {
            if (i < text.Length - 1)
            {
                i++;
                Show();
            }
            else
            {
                TextPanel.SetActive(false);
                ScorePanel.SetActive(true);
                Level2Clear = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            flg = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            flg = false;
        }
    }
    public void Show()
    {
        numText.text = text[i];
        // 前回の演出処理が走っていたら、停止
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        // １文字ずつ表示する演出のコルーチンを実行する
        showCoroutine = StartCoroutine(ShowCoroutine());
    }
    private IEnumerator ShowCoroutine()
    {
        // 待機用コルーチン
        // GC Allocを最小化するためキャッシュしておく
        var delay = new WaitForSeconds(textSpeed);

        // テキスト全体の長さ
        var length = numText.text.Length;

        // １文字ずつ表示する演出
        for (var i = 0; i < length; i++)
        {
            // 徐々に表示文字数を増やしていく
            numText.maxVisibleCharacters = i;

            // 一定時間待機
            yield return delay;
        }

        // 演出が終わったら全ての文字を表示する
        numText.maxVisibleCharacters = length;

        showCoroutine = null;
    }
}
