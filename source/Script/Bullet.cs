using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject Effect;
    private GameObject Enemy1;
    private GameObject Enemy2;
    private GameObject Enemy3;
    private GameObject Enemy4;
    private GameObject Enemy5;
    private GameObject BossShip;
    private GameObject Player;
    private Enemy enemy1;
    private Enemy enemy2;
    private Enemy enemy3;
    private Enemy enemy4;
    private Enemy enemy5;
    private Boss boss;
    private ShipController shipController;
    private PauseManager pauseManager;
    private Vector3 velocity;
    private Vector3 angularVelocity;
    private Rigidbody rb;
    private bool flg = false;
    private void Start()
    {
        Enemy1 = GameObject.FindWithTag("Enemy1");
        Enemy2 = GameObject.FindWithTag("Enemy2");
        Enemy3 = GameObject.FindWithTag("Enemy3");
        Enemy4 = GameObject.FindWithTag("Enemy4");
        Enemy5 = GameObject.FindWithTag("Enemy5");
        BossShip = GameObject.FindWithTag("Boss");
        Player = GameObject.FindWithTag("Player");
        if(Enemy1 != null)
        {
            enemy1 = Enemy1.GetComponent<Enemy>();
        }
        if (Enemy2 != null)
        {
            enemy2 = Enemy2.GetComponent<Enemy>();
        }
        if (Enemy3 != null)
        {
            enemy3 = Enemy3.GetComponent<Enemy>();
        }
        if (Enemy4 != null)
        {
            enemy4 = Enemy4.GetComponent<Enemy>();
        }
        if (Enemy5 != null)
        {
            enemy5 = Enemy5.GetComponent<Enemy>();
        }
        if(BossShip != null)
        {
            boss = BossShip.GetComponent<Boss>();
        }
        shipController = Player.GetComponent<ShipController>();
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        rb = this.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        bool isPaused = pauseManager.IsPaused;
        if(isPaused)
        {
            if (flg == false)
            {
                velocity = rb.velocity; // 速度を保存
                angularVelocity = rb.angularVelocity; // 角速度を保存
                rb.isKinematic = true;
                flg = true;
            }
        }else if (!isPaused)
        {
            if(flg == true)
            {
                rb.isKinematic = false;
                rb.velocity = velocity; // 一時停止直前の速度に戻す
                rb.angularVelocity = angularVelocity; // 一時停止直前の角速度に戻す
                flg = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sea"))
        {
            Destroy(gameObject);
        }
        if(other.gameObject.CompareTag("Enemy1"))
        {
            enemy1.Life();
            Destroy(gameObject);
            GenerateEffect(Enemy1);
        }
        if (other.gameObject.CompareTag("Enemy2"))
        {
            enemy2.Life();
            Destroy(gameObject);
            GenerateEffect(Enemy2);
        }
        if (other.gameObject.CompareTag("Enemy3"))
        {
            enemy3.Life();
            Destroy(gameObject);
            GenerateEffect(Enemy3);
        }
        if (other.gameObject.CompareTag("Enemy4"))
        {
            enemy4.Life();
            Destroy(gameObject);
            GenerateEffect(Enemy4);
        }
        if (other.gameObject.CompareTag("Enemy5"))
        {
            enemy5.Life();
            Destroy(gameObject);
            GenerateEffect(Enemy5);
        }
        if (other.gameObject.CompareTag("Boss"))
        {
            boss.Life();
            Destroy(gameObject);
            GenerateEffect(BossShip);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            shipController.LifeController();
            Destroy(gameObject);
            GenerateEffect(Player);
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