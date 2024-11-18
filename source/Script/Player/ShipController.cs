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
        //メインカメラの取得
        mainCamera = Camera.main;
        //Rigidbodyの取得
        rb = GetComponent<Rigidbody>();
        //X軸とZ軸の回転を止める
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
            //キー入力時回転を止める
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                rb.angularVelocity = Vector3.zero;
            }
            //速度取得
            Speed = rb.velocity.magnitude;
            //WASD操作
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");
            // ベクトルの正規化、増加
            Vector3 targetVelocity = new Vector3(moveX, 0, moveZ).normalized * Force;

            // カメラの回転を考慮した移動ベクトル
            targetVelocity = Quaternion.Euler(0, mainCamera.transform.rotation.eulerAngles.y, 0) * targetVelocity;

            // スムーズなベクトル補間 (0.1f は補間速度、必要に応じて調整)
            MoveVelocity = Vector3.Lerp(MoveVelocity, targetVelocity, 0.1f);
        }
    }
    private void FixedUpdate()
    {
        if (!isPaused && !GameOver)
        {// 船の向き調整
            if (MoveVelocity.magnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(MoveVelocity);
                rb.MoveRotation(targetRotation);
            }
            //速度制限
            if (Speed < MaxSpeed)
            {
                //力を加える
                rb.AddForce(MoveVelocity);
            }
            Vector3 currentPos = this.transform.position;
            //マップ制限
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
        //エフェクトを生成する
        GameObject effect = Instantiate(Effect) as GameObject;
        //エフェクトが発生する場所を決定する(敵オブジェクトの場所)
        effect.transform.position = obj.transform.position;
    }
}
