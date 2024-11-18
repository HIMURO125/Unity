using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island3 : MonoBehaviour
{
    [SerializeField] private GameObject Island;
    [SerializeField] private GameObject Panel;
    [SerializeField] private TMPro.TMP_Text numText;
    private string[] text;
    private float textSpeed = 0.02f;
    private IslandManager IslandManager;
    private Coroutine showCoroutine;
    public int num;
    private bool showflg = false;
    private bool flg = false;
    private PauseManager pauseManager;
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        IslandManager = GameObject.FindWithTag("T_Island").GetComponent<IslandManager>();
        num = IslandManager.Num3;
        string[] question = { "こえぜたはろ　１３５２４６", "キーボード　！　ぬ", "５＝３１　９＝３０　？＝２８または２９",
                              "１２＝０　１５＝？", "さ＝３　じ＝１０　き－ろ＋い＝？", "１０　１６　４　２６　１０゛　五十音順",
                              "VIII－IV＋II＝？", "丁＝４　癸＝１０　庚＝？　十干", "すてきへへと　＋３", "こえきうゅはた　左右交互" };
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        text = new string[5];
        text[0] = "島というより岩礁だ\nこんなところに手がかりがあるだろうか…";
        text[1] = "………";
        text[2] = "岩の隙間に小箱があった！\n調べてみるものだね";
        text[3] = "残念ながら中身はお宝ではなく一枚の紙だった\n内容は…";
        for (int i = 0; i < 10; i++)
        {
            if (num == i)
            {
                text[4] = $"３つ目のカギ\n{question[i]}";
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
