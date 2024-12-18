using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island2 : MonoBehaviour
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
    void Start()
    {
        IslandManager = GameObject.FindWithTag("T_Island").GetComponent<IslandManager>();
        num = IslandManager.Num2;
        string[] question = { "±¦º½Íë@PRTQSU", "L[{[h@I@Ê", "TRP@XRO@HQWÜ½ÍQX",
                              "PQO@PTH", "³R@¶PO@«|ë{¢H", "PO@PU@S@QU@POJ@Ü\¹",
                              "VIII|IV{IIH", "S@á¡PO@MH@\±", "·Ä«ÖÖÆ@{R", "±¦«¤ãÍ½@¶EðÝ" };
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        text = new string[4];
        text[0] = "NÌµ¢âR¾\noéÌÍåÏ»¤¾¯Ç²×ÄÝæ¤";
        text[1] = "ccc";
        text[2] = "yÌêÉÎÅªÜÁÄ¢½I\n½Æ©Çß»¤¾";
        for (int i = 0; i < 10; i++)
        {
            if (num == i)
            {
                text[3] = $"QÂÚÌJM\n{question[i]}";
                break;
            }
        }
    }

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
        // OñÌoªÁÄ¢½çAâ~
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        // P¶¸Â\¦·éoÌR[`ðÀs·é
        showCoroutine = StartCoroutine(ShowCoroutine());
    }
    // P¶¸Â\¦·éoÌR[`
    private IEnumerator ShowCoroutine()
    {
        // Ò@pR[`
        // GC AllocðÅ¬»·é½ßLbVµÄ¨­
        var delay = new WaitForSeconds(textSpeed);

        // eLXgSÌÌ·³
        var length = numText.text.Length;

        // P¶¸Â\¦·éo
        for (var i = 0; i < length; i++)
        {
            // XÉ\¦¶ðâµÄ¢­
            numText.maxVisibleCharacters = i;

            // êèÔÒ@
            yield return delay;
        }

        // oªIíÁ½çSÄÌ¶ð\¦·é
        numText.maxVisibleCharacters = length;

        showCoroutine = null;
    }
}
