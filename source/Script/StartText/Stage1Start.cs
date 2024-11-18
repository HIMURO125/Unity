using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Start : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private TMPro.TMP_Text numText;
    private string[] text;
    private float textSpeed = 0.02f;
    private bool showflg = false;
    private Coroutine showCoroutine;
    private PauseManager pauseManager;
    private int i = 0;
    void Start()
    {
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        text = new string[6];
        text[0] = "僕は海のトレジャーハンター\n今日もお宝を探しに来た";
        text[1] = "この海域にお宝があるのは確かなはず\nとにかく手がかりを探そう";
        text[2] = "お宝のうわさを聞きつけて海賊もいるみたいだ\n燃料の残りにも注意しないと";
        text[3] = "でも海賊を倒せれば燃料を少し補充できるかも…\nまああまり無茶はしないでおこう";
        text[4] = "それと、ここから一定以上離れるのは海流の影響でできないようだ\nお宝探しに集中しよう";
        text[5] = "さあ、お宝探しの始まりだ！";
        showflg = true;
        Panel.SetActive(true);
        pauseManager.Pause();
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && showflg)
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
