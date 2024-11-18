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
        string[] question = { "���������͂�@�P�R�T�Q�S�U", "�L�[�{�[�h�@�I�@��", "�T���R�P�@�X���R�O�@�H���Q�W�܂��͂Q�X",
                              "�P�Q���O�@�P�T���H", "�����R�@�����P�O�@���|��{�����H", "�P�O�@�P�U�@�S�@�Q�U�@�P�O�J�@�܏\����",
                              "VIII�|IV�{II���H", "�����S�@ᡁ��P�O�@�M���H�@�\��", "���Ă��ււƁ@�{�R", "����������͂��@���E����" };
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        text = new string[5];
        text[0] = "���Ƃ�������ʂ�\n����ȂƂ���Ɏ肪���肪���邾�낤���c";
        text[1] = "�c�c�c";
        text[2] = "��̌��Ԃɏ������������I\n���ׂĂ݂���̂���";
        text[3] = "�c�O�Ȃ��璆�g�͂���ł͂Ȃ��ꖇ�̎�������\n���e�́c";
        for (int i = 0; i < 10; i++)
        {
            if (num == i)
            {
                text[4] = $"�R�ڂ̃J�M\n{question[i]}";
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
