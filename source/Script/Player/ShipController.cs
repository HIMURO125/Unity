using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private Canvas lifegage;
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject Life;
    [SerializeField] private GameObject Timer;
    [SerializeField] private GameObject Pos;
    [SerializeField] private GameObject Effect;
    [SerializeField] private AudioClip Audio1;
    [SerializeField] private AudioClip Audio2;
    private AudioSource audioSource;
    private Vector3 MoveVelocity;
    private float Speed;
    private float moveX;
    private float moveZ;
    private float Force = 7.0f;
    private float MaxSpeed = 10.0f;
    private float x_Limit = 249.0f;
    private float z_Limit = 249.0f;
    private Camera mainCamera;
    private Rigidbody rb;
    private PauseManager pauseManager;
    private int life = 3;
    private Life lifes;
    private bool isPaused;
    private bool GameOver = false;
    public bool IsGameOver
    {
        get { return GameOver; }
    }
    void Start()
    {
        //���C���J�����̎擾
        mainCamera = Camera.main;
        //Rigidbody�̎擾
        rb = GetComponent<Rigidbody>();
        //X����Z���̉�]���~�߂�
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        lifes = lifegage.GetComponent<Life>();
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        isPaused = pauseManager.IsPaused;
        if (!isPaused && !GameOver)
        {
            //�L�[���͎���]���~�߂�
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                rb.angularVelocity = Vector3.zero;
            }
            //���x�擾
            Speed = rb.velocity.magnitude;
            //WASD����
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");
            // �x�N�g���̐��K���A����
            Vector3 targetVelocity = new Vector3(moveX, 0, moveZ).normalized * Force;

            // �J�����̉�]���l�������ړ��x�N�g��
            targetVelocity = Quaternion.Euler(0, mainCamera.transform.rotation.eulerAngles.y, 0) * targetVelocity;

            // �X���[�Y�ȃx�N�g����� (0.1f �͕�ԑ��x�A�K�v�ɉ����Ē���)
            MoveVelocity = Vector3.Lerp(MoveVelocity, targetVelocity, 0.1f);
        }
    }
    private void FixedUpdate()
    {
        if (!isPaused && !GameOver)
        {// �D�̌�������
            if (MoveVelocity.magnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(MoveVelocity);
                rb.MoveRotation(targetRotation);
            }
            //���x����
            if (Speed < MaxSpeed)
            {
                //�͂�������
                rb.AddForce(MoveVelocity);
            }
            Vector3 currentPos = this.transform.position;
            //�}�b�v����
            currentPos.x = Mathf.Clamp(currentPos.x, -x_Limit, x_Limit);
            currentPos.z = Mathf.Clamp(currentPos.z, -z_Limit, z_Limit);
            this.transform.position = currentPos;
        }
    }
    public void LifeController()
    {
        life--;
        lifes.Damage();
        if(life > 0)
        {
            audioSource.PlayOneShot(Audio1);
        }
        else if (life <= 0)
        {
            audioSource.PlayOneShot(Audio2);
            GenerateEffect(gameObject);
            Panel.SetActive(true);
            GameOver = true;
            Life.SetActive(false);
            Timer.SetActive(false);
            Pos.SetActive(false);
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
