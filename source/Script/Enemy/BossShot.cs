using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShot : MonoBehaviour
{
    [SerializeField] GameObject Bullet;
    [SerializeField] GameObject TargetObject;
    [SerializeField, Range(0F, 90F)] float ThrowingAngle;
    [SerializeField] private AudioClip Audio;
    private AudioSource audioSource;
    private float Distance;
    private float TimeCount = 0;
    private float chargeTime = 3.0f;
    private PauseManager pauseManager;

    private void Start()
    {
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        bool isPaused = pauseManager.IsPaused;
        if (!isPaused)
        {
            Distance = Vector3.Distance(this.transform.position, TargetObject.transform.position);
            TimeCount += Time.deltaTime;
            if (TimeCount > chargeTime && Distance < 40)
            {
                ThrowingBall();
                TimeCount = 0;
            }
        }
    }

    private void ThrowingBall()
    {
        if (Bullet != null && TargetObject != null)
        {
            // Ballオブジェクトの生成
            GameObject ball = Instantiate(Bullet, this.transform.position, Quaternion.identity);

            // 標的の座標
            Vector3 targetPosition = TargetObject.transform.position;

            // 射出角度
            float angle = ThrowingAngle;

            // 射出速度を算出
            Vector3 velocity = CalculateVelocity(this.transform.position, targetPosition, angle);

            // 射出
            Rigidbody rid = ball.GetComponent<Rigidbody>();
            audioSource.PlayOneShot(Audio);
            rid.AddForce(velocity * rid.mass, ForceMode.Impulse);
            ball.name = Bullet.name;
            Destroy(ball, 5f);
        }
    }

    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // 射出角をラジアンに変換
        float rad = angle * Mathf.PI / 180;

        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // 垂直方向の距離y
        float y = pointA.y - pointB.y;

        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
}
