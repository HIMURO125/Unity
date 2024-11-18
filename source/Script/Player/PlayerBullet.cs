using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private GameObject ShotPoint;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private AudioClip Audio;
    private AudioSource audioSource;
    private float ShotSpeed = 15.5f;
    private Vector3 instantiatePosition;
    public Vector3 InstantiatePosition
    {
        get { return instantiatePosition; }
    }
    private Vector3 shootVelocity;
    public Vector3 ShootVelocity
    {
        get { return shootVelocity; }
    }
    private Vector3 Angle;
    private float MaxAngle = 75f;
    private float MinAngle = 45f;
    private float Input_X;
    private float Input_Y;
    private float TimeCount = 2;
    private PauseManager pauseManager;
    private ShipController shipController;
    // Start is called before the first frame update
    void Start()
    {
        //èâä˙äpìx
        ShotPoint.transform.rotation = Quaternion.Euler(45, 0, 0);
        Angle = ShotPoint.transform.eulerAngles;
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        shipController = GameObject.FindWithTag("Player").GetComponent<ShipController>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isPaused = pauseManager.IsPaused;
        bool GameOver = shipController.IsGameOver;
        if (!isPaused && !GameOver)
        {
            shootVelocity = ShotPoint.transform.up * ShotSpeed;
            instantiatePosition = ShotPoint.transform.position;
            TimeCount += Time.deltaTime;
            if (Input.GetMouseButtonDown(1))
            {
                if (TimeCount > 2)
                {
                    Vector3 BulletPosition = instantiatePosition;
                    GameObject newBullet = Instantiate(Bullet, BulletPosition, ShotPoint.transform.rotation);
                    Vector3 Direction = newBullet.transform.up;
                    audioSource.PlayOneShot(Audio);
                    newBullet.GetComponent<Rigidbody>().AddForce(Direction * ShotSpeed, ForceMode.Impulse);
                    newBullet.name = Bullet.name;
                    Destroy(newBullet, 5f);
                    TimeCount = 0;
                }
            }
            //éÀèoäpìxïœçX
            Input_X = Input.GetAxis("Arrow Horizontal");
            Input_Y = Input.GetAxis("Arrow Vertical");
            Angle.x += Input_X * 0.5f;
            Angle.y += Input_Y * 0.5f;
            //äpìxêßå¿
            if (Angle.x > MaxAngle)
            {
                Angle.x = MaxAngle;
            }
            if (Angle.x < MinAngle)
            {
                Angle.x = MinAngle;
            }
            ShotPoint.transform.eulerAngles = new Vector3(Angle.x, Angle.y, 0);
        } 
    }
}
