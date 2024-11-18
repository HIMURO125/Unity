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
        text[0] = "�傫�߂̏��������铇��\n�������l���Z��ł���C�z�͂Ȃ�";
        text[1] = "�c�c�c";
        text[2] = "�����̂Q�K�ɑ傫�ȕ󔠂��������I\n�O�Ɠ����Ő������S���̔ԍ��̑g�ݍ��킹�ŊJ��������";
        text[3] = "�������N���b�N�Ŕԍ���������Enter�L�[�ŊJ���Ă݂悤";
        rightText = new string[4];
        rightText[0] = "�K�`���b�ƌ����J�������������I\n�ǂ���琳���������悤��";
        rightText[1] = "������I�Ǝv�����̂����̊�\n���g�͋���ۂ�����";
        rightText[2] = "�ǂ����ꑫ��ɒ��g���C���Ɏ���Ă��܂����悤��";
        rightText[3] = "�������������܂ł����C�������͂܂��T���Ă���l�q������\n�V�����C���D��T���Ă݂悤";
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
        // �O��̉��o�����������Ă�����A��~
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        // �P�������\�����鉉�o�̃R���[�`�������s����
        showCoroutine = StartCoroutine(ShowCoroutine());
    }
    public void FailShow()
    {
        numText.text = "�s�����̂悤��\n������x�l���Ă݂悤";
        // �O��̉��o�����������Ă�����A��~
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        // �P�������\�����鉉�o�̃R���[�`�������s����
        showCoroutine = StartCoroutine(ShowCoroutine());
        failflg = true;
    }
    public void RightShow()
    {
        numText.text = rightText[j];
        // �O��̉��o�����������Ă�����A��~
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        // �P�������\�����鉉�o�̃R���[�`�������s����
        showCoroutine = StartCoroutine(ShowCoroutine());
        rightflg = true;
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
        if (failflg || rightflg)
        {
            showflg = true;
        }
    }
}
