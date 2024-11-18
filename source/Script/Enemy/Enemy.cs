using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject Timer;
    [SerializeField] private GameObject Effect;
    [SerializeField] private AudioClip Audio;
    private AudioSource audioSource;
    private float chargeTime = 3.0f;
    private float TimeCount;
    private Rigidbody rb;
    private int life = 2;
    private int speed = 3;
    private PauseManager pauseManager;
    private BreakAudio breakAudio;
    private TimeManager timeManager;
    private Vector3 moveDirection;
    private float x_Limit = 249.0f;
    private float z_Limit = 249.0f;
    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        breakAudio = GameObject.FindWithTag("GameController").GetComponent<BreakAudio>();
        timeManager = Timer.GetComponent<TimeManager>();
        audioSource = GetComponent<AudioSource>();
        SetRandomDirection();
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
            currentPos.x = Mathf.Clamp(currentPos.x, -x_Limit, x_Limit);
            currentPos.z = Mathf.Clamp(currentPos.z, -z_Limit, z_Limit);
            this.transform.position = currentPos;
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
            timeManager.Bonus();
            Destroy(this.gameObject);
        }
    }
    private void GenerateEffect(GameObject obj)
    {
        //�G�t�F�N�g�𐶐�����
        GameObject effect = Instantiate(Effect) as GameObject;
        //�G�t�F�N�g����������ꏊ�����肷��(�G�I�u�W�F�N�g�̏ꏊ)
        effect.transform.position = obj.transform.position;
    }
}
