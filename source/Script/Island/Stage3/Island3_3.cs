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
        string[] question = { "���|�񁁂Q�@���|�����P�@���|�Z���H", "�O���R����@�O���W�����@�O���O���H",
                              "�Q�X�Q�O���W�@�P�W�Q�T���T�@�R�U�T���P�@�V�R�O���H", "�S�T�~�Q�V���S�@�P�Q�R�~�S�T�U���T�@�X�T�~�P�O���H",
                              "�q���P�@�\���X�@�K���H�@�\��x", "A���P�@J���P�O�@E���H", "���|�����R�@���{�����P�T�@���|�����H�@�A�i���O���v",
                              "�P�Q�R�U�X���@�d�b", "�Ђ�����Ԃ��Ă��ς��Ȃ����|���ƉʂĂ��Ȃ��傫���Ȃ�", "�W���P�O�@�S���V�@�U���H�@�����Q�悷��" };
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        text = new string[5];
        text[0] = "���Ȃ�傫���X�R��\n���ׂ�̂��Ȃ��ς����c";
        text[1] = "�c�c�c";
        text[2] = "���������܂��Ă�������\n����Ď��o���Ă݂悤";
        text[3] = "����Əo�Ă����c\n�肪�₽���c";
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
