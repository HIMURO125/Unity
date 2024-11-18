using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island3_2 : MonoBehaviour
{
    [SerializeField] private GameObject Island;
    [SerializeField] private GameObject Panel;
    [SerializeField] private TMPro.TMP_Text numText;
    private string[] text;
    private IslandManager2 IslandManager2;
    private Coroutine showCoroutine;
    private float textSpeed = 0.02f;
    public int num;
    private bool showflg = false;
    private bool flg = false;
    private PauseManager pauseManager;
    private int i = 0;
    void Start()
    {
        IslandManager2 = GameObject.FindWithTag("T_Island").GetComponent<IslandManager2>();
        num = IslandManager2.Num3;
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        text = new string[4];
        text[0] = "�������ΎR�����铇��\n�����������Ă��Ȃ������������ɂ������ӂ��悤";
        text[1] = "�c�c�c";
        text[2] = "�Ό��ɐΔł��������I\n����ȂƂ���ɒu������Ă��Ƃ͓������玀�ΎR�������̂��낤";
        if (num == -20)
        {
            text[3] = "�R�ڂ̃J�M\nA���P�@J���P�O�@�i�|D�j�~E���H";
        }
        else if (num == 18)
        {
            text[3] = "�R�ڂ̃J�M\n�P���P�@A���P�O�@D���P�R�@�P�Q���H�@�P�U�i�ސ�";
        }
        else if (num == -12)
        {
            text[3] = "�R�ڂ̃J�M\n2d 31 32�@Shift-JIS";
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
        // �O��̉��o�����������Ă�����A��~
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        // �P�������\�����鉉�o�̃R���[�`�������s����
        showCoroutine = StartCoroutine(ShowCoroutine());
    }
    // �P�������\�����鉉�o�̃R���[�`��
    private IEnumerator ShowCoroutine()
    {
        // �ҋ@�p�R���[�`��
        // GC Alloc���ŏ������邽�߃L���b�V�����Ă���
        var delay = new WaitForSeconds(textSpeed);

        // �e�L�X�g�S�̂̒���
        var length = numText.text.Length;

        // �P�������\�����鉉�o
        for (var i = 0; i < length; i++)
        {
            // ���X�ɕ\���������𑝂₵�Ă���
            numText.maxVisibleCharacters = i;

            // ��莞�ԑҋ@
            yield return delay;
        }

        // ���o���I�������S�Ă̕�����\������
        numText.maxVisibleCharacters = length;

        showCoroutine = null;
    }
}
