using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class IslandManager3 : MonoBehaviour
{
    [SerializeField] private GameObject[] Islands = new GameObject[4];
    [SerializeField] private GameObject[] Passnum = new GameObject[4];
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject Boss;
    [SerializeField] private GameObject TextPanel;
    [SerializeField] private TMPro.TMP_Text numText;
    public int[] unlocknum = new int[4];
    public int[] answernum = new int[4];
    private string[] text;
    private string[] rightText;
    private float textSpeed = 0.02f;
    private Coroutine showCoroutine;
    private bool showflg = false;
    private bool textflg = false;
    private bool failflg = false;
    private bool rightflg = false;
    private bool flg = false;
    private bool ans = true;
    private Pass1 Pass1;
    private Pass2 Pass2;
    private Pass3 Pass3;
    private Pass4 Pass4;
    private List<int> numbers = new List<int>();
    private int count = 4;
    private int i = 0;
    private int j = 0;
    private int k = 0;
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
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        for (int i = 0; i <= 9; i++)
        {
            numbers.Add(i);
        }
        Random.InitState(System.DateTime.Now.Millisecond);
        while (count-- > 0)
        {
            int index = Random.Range(0, numbers.Count);

            int ransu = numbers[index];
            unlocknum[k] = ransu;

            numbers.RemoveAt(index);
            k++;
        }
        num1 = unlocknum[0];
        num2 = unlocknum[1];
        num3 = unlocknum[2];
        num4 = unlocknum[3];
        Islands[0].GetComponent<Island1_3>().enabled = true;
        Islands[1].GetComponent<Island2_3>().enabled = true;
        Islands[2].GetComponent<Island3_3>().enabled = true;
        Islands[3].GetComponent<Island4_3>().enabled = true;
        Pass1 = Passnum[0].GetComponent<Pass1>();
        Pass2 = Passnum[1].GetComponent<Pass2>();
        Pass3 = Passnum[2].GetComponent<Pass3>();
        Pass4 = Passnum[3].GetComponent<Pass4>();
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        text = new string[4];
        text[0] = "大きめの小屋がある島だ\nここも人が住んでいる気配はない";
        text[1] = "………";
        text[2] = "小屋の２階に大きな宝箱を見つけた！\n前と同じで正しい４桁の番号の組み合わせで開きそうだ";
        text[3] = "数字をクリックで番号をかえてEnterキーで開けてみよう";
        rightText = new string[4];
        rightText[0] = "ガチャッと鍵が開いた音がした！\nどうやら正解だったようだ";
        rightText[1] = "やった！と思ったのも束の間\n中身は空っぽだった";
        rightText[2] = "どうやら一足先に中身を海賊に取られてしまったようだ";
        rightText[3] = "しかしさっきまでいた海賊たちはまだ探している様子だった\n新しい海賊船を探してみよう";
    }

    // Update is called once per frame
    void Update()
    {
        answernum[0] = Pass1.getpass;
        answernum[1] = Pass2.getpass;
        answernum[2] = Pass3.getpass;
        answernum[3] = Pass4.getpass;
        if (Input.GetKeyDown(KeyCode.Space) && flg && !rightflg && !showflg)
        {
            showflg = true;
            TextPanel.SetActive(true);
            pauseManager.Pause();
            Show();
        }
        if (Input.GetKeyDown(KeyCode.Return) && showflg && textflg && flg)
        {
            for (int i = 0; i < unlocknum.Length; i++)
            {
                if (answernum[i] != unlocknum[i])
                {
                    ans = false;
                }
            }
            if (ans == true)
            {
                Panel.SetActive(false);
                TextPanel.SetActive(true);
                textflg = false;
                showflg = false;
                RightShow();
            }
            else if (ans == false)
            {
                Panel.SetActive(false);
                Pass1.setpass();
                Pass2.setpass();
                Pass3.setpass();
                Pass4.setpass();
                ans = true;
                textflg = false;
                showflg = false;
                TextPanel.SetActive(true);
                FailShow();
            }
        }
        if (Input.GetKeyDown(KeyCode.Return) && showflg && !textflg && flg)
        {
            if (i < text.Length - 1 && !failflg)
            {
                i++;
                Show();
            }
            else if (j < rightText.Length - 1 && rightflg)
            {
                j++;
                RightShow();
            }
            else if (failflg || rightflg)
            {
                TextPanel.SetActive(false);
                showflg = false;
                failflg = false;
                pauseManager.Resume();
                if (rightflg)
                {
                    Boss.SetActive(true);
                }
            }
            else
            {
                TextPanel.SetActive(false);
                Panel.SetActive(true);
                textflg = true;
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
    public void FailShow()
    {
        numText.text = "不正解のようだ\nもう一度考えてみよう";
        // 前回の演出処理が走っていたら、停止
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        // １文字ずつ表示する演出のコルーチンを実行する
        showCoroutine = StartCoroutine(ShowCoroutine());
        failflg = true;
    }
    public void RightShow()
    {
        numText.text = rightText[j];
        // 前回の演出処理が走っていたら、停止
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        // １文字ずつ表示する演出のコルーチンを実行する
        showCoroutine = StartCoroutine(ShowCoroutine());
        rightflg = true;
    }
    // １文字ずつ表示する演出のコルーチン
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
        if (failflg || rightflg)
        {
            showflg = true;
        }
    }
}
