using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island4_3 : MonoBehaviour
{
    [SerializeField] private GameObject Island;
    [SerializeField] private GameObject Panel;
    [SerializeField] private TMPro.TMP_Text numText;
    private string[] text;
    private IslandManager3 IslandManager;
    private Coroutine showCoroutine;
    private float textSpeed = 0.02f;
    public int num;
    private bool showflg = false;
    private bool flg = false;
    private PauseManager pauseManager;
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        IslandManager = GameObject.FindWithTag("T_Island").GetComponent<IslandManager3>();
        num = IslandManager.Num4;
        string[] question = { "□−二＝２　△−八＝１　□−六＝？", "０が３＝千　０が８＝億　０が０＝？",
                              "２９２０＝８　１８２５＝５　３６５＝１　７３０＝？", "４５×２７＝４　１２３×４５６＝５　９５×１０＝？",
                              "子＝１　申＝９　卯＝？　十二支", "A＝１　J＝１０　E＝？", "↓−→＝３　↑＋→＝１５　←−→＝？　アナログ時計",
                              "１２３６９＃　電話", "ひっくり返しても変わらないが倒れると果てしなく大きくなる", "８→１０　４→７　６→？　左を２乗する" };
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        text = new string[4];
        text[0] = "雪で覆われた島だ\n一見何もなさそうだけど調べてみよう";
        text[1] = "………";
        text[2] = "木の上になにかあるみたい\n頑張って登ってみるか";
        for (int i = 0; i < 10; i++)
        {
            if (num == i)
            {
                text[3] = $"４つ目のカギ\n{question[i]}";
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !showflg && flg)
        {
            showflg = true;
            Panel.SetActive(true);
            pauseManager.Pause();
            Show();
        }
        if (Input.GetKeyDown(KeyCode.Return) && showflg && flg)
        {
            if (i < text.Length - 1)
            {
                i++;
                Show();
            }
            else
            {
                Panel.SetActive(false);
                pauseManager.Resume();
                showflg = false;
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
    }
}
