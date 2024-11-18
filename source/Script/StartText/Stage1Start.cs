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
        text[0] = "�l�͊C�̃g���W���[�n���^�[\n�����������T���ɗ���";
        text[1] = "���̊C��ɂ��󂪂���̂͊m���Ȃ͂�\n�Ƃɂ����肪�����T����";
        text[2] = "����̂��킳�𕷂����ĊC��������݂�����\n�R���̎c��ɂ����ӂ��Ȃ���";
        text[3] = "�ł��C����|����ΔR����������[�ł��邩���c\n�܂����܂薳���͂��Ȃ��ł�����";
        text[4] = "����ƁA����������ȏ㗣���̂͊C���̉e���łł��Ȃ��悤��\n����T���ɏW�����悤";
        text[5] = "�����A����T���̎n�܂肾�I";
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
