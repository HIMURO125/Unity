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
            // Ball�I�u�W�F�N�g�̐���
            GameObject ball = Instantiate(Bullet, this.transform.position, Quaternion.identity);

            // �W�I�̍��W
            Vector3 targetPosition = TargetObject.transform.position;

            // �ˏo�p�x
            float angle = ThrowingAngle;

            // �ˏo���x���Z�o
            Vector3 velocity = CalculateVelocity(this.transform.position, targetPosition, angle);

            // �ˏo
            Rigidbody rid = ball.GetComponent<Rigidbody>();
            audioSource.PlayOneShot(Audio);
            rid.AddForce(velocity * rid.mass, ForceMode.Impulse);
            ball.name = Bullet.name;
            Destroy(ball, 5f);
        }
    }

    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // �ˏo�p�����W�A���ɕϊ�
        float rad = angle * Mathf.PI / 180;

        // ���������̋���x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // ���������̋���y
        float y = pointA.y - pointB.y;

        // �Ε����˂̌����������x�ɂ��ĉ���
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // �����𖞂����������Z�o�ł��Ȃ����Vector3.zero��Ԃ�
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
}
