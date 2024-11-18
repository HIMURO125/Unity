using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public static bool Level3Clear = false;
    [SerializeField] private GameObject Score;
    [SerializeField] private GameObject Panel;
    [SerializeField] private TMPro.TMP_Text numText;
    [SerializeField] private GameObject Effect1;
    [SerializeField] private GameObject Effect2;
    [SerializeField] private AudioClip Audio;
    private AudioSource audioSource;
    private string[] text;
    private float textSpeed = 0.02f;
    private bool showflg = false;
    private Coroutine showCoroutine;
    private int i = 0;
    private float chargeTime = 4f;
    private float TimeCount;
    private Rigidbody rb;
    private int life = 5;
    private int speed = 4;
    private PauseManager pauseManager;
    private BreakAudio breakAudio;
    private Vector3 moveDirection;
    private float Limit = 249.0f;
    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        breakAudio = GameObject.FindWithTag("GameController").GetComponent<BreakAudio>();
        audioSource = GetComponent<AudioSource>();
        SetRandomDirection();
        text = new string[5];
        text[0] = "なんとか勝てた…\n意外と何とかなるんだね";
        text[1] = "さて、少し船内を調べさせてもらおうかな\n念のため警戒はしておこう";
        text[2] = "………";
        text[3] = "お宝を見つけた！けどこれは歴史的価値があるものみたいだ\nこれは然るべき機関へ渡した方が良さそうだ";
        text[4] = "何はともあれお宝を手に入れることができた\nこれからもどんどんお宝を見つけていこう！";
    }
    void Update()
    {
        bool isPaused = pauseManager.IsPaused;
        if (!isPaused)
        {
            TimeCount += Time.deltaTime;
            //前進
            this.transform.Translate(moveDirection * Time.deltaTime * speed, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            //指定時間経過
            if (TimeCount > chargeTime)
            {
                SetRandomDirection();
                TimeCount = 0;
            }
            Vector3 currentPos = this.transform.position;
            //マップ制限
            currentPos.x = Mathf.Clamp(currentPos.x, -Limit, Limit);
            currentPos.z = Mathf.Clamp(currentPos.z, -Limit, Limit);
            this.transform.position = currentPos;
        }
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
                Score.SetActive(true);
                showflg = false;
                Level3Clear = true;
            }
        }
    }
    void SetRandomDirection()
    {
        // ランダムな方向ベクトルをXZ平面上で生成
        float randomAngle = Random.Range(0f, 360f);
        moveDirection = new Vector3(Mathf.Sin(randomAngle * Mathf.Deg2Rad), 0, Mathf.Cos(randomAngle * Mathf.Deg2Rad)).normalized;
    }
    public void Life()
    {
        life--;
        if(life > 0)
        {
            audioSource.PlayOneShot(Audio);
        }
        else if (life <= 0)
        {
            breakAudio.PlayAudio();
            GenerateEffect(gameObject);
            showflg = true;
            Panel.SetActive(true);
            pauseManager.Pause();
            Show();
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
    private void GenerateEffect(GameObject obj)
    {
        //エフェクトを生成する
        GameObject effect1 = Instantiate(Effect1) as GameObject;
        GameObject effect2 = Instantiate(Effect2) as GameObject;
        //エフェクトが発生する場所を決定する(敵オブジェクトの場所)
        effect1.transform.position = obj.transform.position;
        effect2.transform.position = obj.transform.position;
    }
}
