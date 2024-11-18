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
        text[0] = "�Ȃ�Ƃ����Ă��c\n�ӊO�Ɖ��Ƃ��Ȃ�񂾂�";
        text[1] = "���āA�����D���𒲂ׂ����Ă��炨������\n�O�̂��ߌx���͂��Ă�����";
        text[2] = "�c�c�c";
        text[3] = "������������I���ǂ���͗��j�I���l��������݂̂�����\n����͑R��ׂ��@�ւ֓n���������ǂ�������";
        text[4] = "���͂Ƃ����ꂨ�����ɓ���邱�Ƃ��ł���\n���ꂩ����ǂ�ǂ񂨕�������Ă������I";
    }
    void Update()
    {
        bool isPaused = pauseManager.IsPaused;
        if (!isPaused)
        {
            TimeCount += Time.deltaTime;
            //�O�i
            this.transform.Translate(moveDirection * Time.deltaTime * speed, Space.World);
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
            //�w�莞�Ԍo��
            if (TimeCount > chargeTime)
            {
                SetRandomDirection();
                TimeCount = 0;
            }
            Vector3 currentPos = this.transform.position;
            //�}�b�v����
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
        // �����_���ȕ����x�N�g����XZ���ʏ�Ő���
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
    private void GenerateEffect(GameObject obj)
    {
        //�G�t�F�N�g�𐶐�����
        GameObject effect1 = Instantiate(Effect1) as GameObject;
        GameObject effect2 = Instantiate(Effect2) as GameObject;
        //�G�t�F�N�g����������ꏊ�����肷��(�G�I�u�W�F�N�g�̏ꏊ)
        effect1.transform.position = obj.transform.position;
        effect2.transform.position = obj.transform.position;
    }
}
