using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Start : MonoBehaviour
{
    [SerializeField] private GameObject Panel;
    [SerializeField] private TMPro.TMP_Text numText;
    private string[] text;
    private float textSpeed = 0.02f;
    private bool showflg = false;
    private Coroutine showCoroutine;
    private PauseManager pauseManager;
    public int i = 0;
    void Start()
    {
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        text = new string[4];
        text[0] = "�O��͂ƂĂ��ǂ����n������\n�������ǂ����������������";
        text[1] = "�O��Ɠ����悤�ɂ��̊C��ɂ��󂪂���͂�\n�C���ƔR���ɒ��ӂ��ĒT�����悤";
        text[2] = "����ƁA�O����C���̐��������Ă���݂�����\n�C��t���Ȃ���";
        text[3] = "���āA�ǂ�Ȃ��󂪂��邩�ȁH";
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
