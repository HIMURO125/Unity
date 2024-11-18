using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island3_3 : MonoBehaviour
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
        num = IslandManager.Num3;
        string[] question = { " |“ñ‚Q@¢|”ª‚P@ |˜ZH", "‚O‚ª‚Rç@‚O‚ª‚W‰­@‚O‚ª‚OH",
                              "‚Q‚X‚Q‚O‚W@‚P‚W‚Q‚T‚T@‚R‚U‚T‚P@‚V‚R‚OH", "‚S‚T~‚Q‚V‚S@‚P‚Q‚R~‚S‚T‚U‚T@‚X‚T~‚P‚OH",
                              "q‚P@\‚X@‰KH@\“ñx", "A‚P@J‚P‚O@EH", "«|¨‚R@ª{¨‚P‚T@©|¨H@ƒAƒiƒƒOŒv",
                              "‚P‚Q‚R‚U‚X”@“d˜b", "‚Ğ‚Á‚­‚è•Ô‚µ‚Ä‚à•Ï‚í‚ç‚È‚¢‚ª“|‚ê‚é‚Æ‰Ê‚Ä‚µ‚È‚­‘å‚«‚­‚È‚é", "‚W¨‚P‚O@‚S¨‚V@‚U¨H@¶‚ğ‚Qæ‚·‚é" };
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        text = new string[5];
        text[0] = "‚©‚È‚è‘å‚«‚¢•XR‚¾\n’²‚×‚é‚Ì‚©‚È‚è‘å•Ï‚»‚¤c";
        text[1] = "ccc";
        text[2] = "¬” ‚ª–„‚Ü‚Á‚Ä‚¢‚»‚¤‚¾\ní‚Á‚Äæ‚èo‚µ‚Ä‚İ‚æ‚¤";
        text[3] = "‚â‚Á‚Æo‚Ä‚«‚½c\nè‚ª—â‚½‚¢c";
        for (int i = 0; i < 10; i++)
        {
            if (num == i)
            {
                text[4] = $"‚R‚Â–Ú‚ÌƒJƒM\n{question[i]}";
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
        // ‘O‰ñ‚Ì‰‰oˆ—‚ª‘–‚Á‚Ä‚¢‚½‚çA’â~
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        // ‚P•¶š‚¸‚Â•\¦‚·‚é‰‰o‚ÌƒRƒ‹[ƒ`ƒ“‚ğÀs‚·‚é
        showCoroutine = StartCoroutine(ShowCoroutine());
    }
    // ‚P•¶š‚¸‚Â•\¦‚·‚é‰‰o‚ÌƒRƒ‹[ƒ`ƒ“
    private IEnumerator ShowCoroutine()
    {
        // ‘Ò‹@—pƒRƒ‹[ƒ`ƒ“
        // GC Alloc‚ğÅ¬‰»‚·‚é‚½‚ßƒLƒƒƒbƒVƒ…‚µ‚Ä‚¨‚­
        var delay = new WaitForSeconds(textSpeed);

        // ƒeƒLƒXƒg‘S‘Ì‚Ì’·‚³
        var length = numText.text.Length;

        // ‚P•¶š‚¸‚Â•\¦‚·‚é‰‰o
        for (var i = 0; i < length; i++)
        {
            // ™X‚É•\¦•¶š”‚ğ‘‚â‚µ‚Ä‚¢‚­
            numText.maxVisibleCharacters = i;

            // ˆê’èŠÔ‘Ò‹@
            yield return delay;
        }

        // ‰‰o‚ªI‚í‚Á‚½‚ç‘S‚Ä‚Ì•¶š‚ğ•\¦‚·‚é
        numText.maxVisibleCharacters = length;

        showCoroutine = null;
    }
}
