using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject player;
    private Vector2 rotationspeed;
    private float basedistance;
    private float distance;
    private Camera mainCamera;
    private Vector2 lastMousePosition;
    private Vector3 lastPlayerPosition;
    private Vector3 baseCameraPosition;
    private float maxAngle = 35.0f;
    private Vector3 totalAngle = Vector3.zero;
    private Vector3 direction;
    private Vector3 Position;
    private LayerMask layer;
    private RaycastHit hit;
    private PauseManager pauseManager;
    private ShipController shipController;
    void Start()
    {
        //メインカメラの取得
        mainCamera = Camera.main;
        rotationspeed.x = 1;
        rotationspeed.y = 1;
        lastPlayerPosition = player.transform.position;
        baseCameraPosition = player.transform.position;
        baseCameraPosition.y += 8.0f;
        baseCameraPosition.z += -7.0f;
        //カメラの初期位置設定
        mainCamera.transform.position = baseCameraPosition;
        //カメラの向き設定
        direction = mainCamera.transform.position - player.transform.position;
        mainCamera.transform.rotation = Quaternion.LookRotation(-direction);
        layer = LayerMask.GetMask("Object");
        //基準距離
        basedistance = Vector3.Distance(player.transform.position, baseCameraPosition);
        pauseManager = GameObject.FindWithTag("GameController").GetComponent<PauseManager>();
        shipController = GameObject.FindWithTag("Player").GetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isPaused = pauseManager.IsPaused;
        bool GameOver = shipController.IsGameOver;
        if(!isPaused)
        {
            if (!GameOver)
            {
                //カメラ追従
                mainCamera.transform.position += player.transform.position - lastPlayerPosition;
                lastPlayerPosition = player.transform.position;
                //クリック時視点移動
                if (Input.GetMouseButtonDown(0))
                {
                    lastMousePosition = Input.mousePosition;
                }
                else if (Input.GetMouseButton(0))
                {
                    var x = (Input.mousePosition.x - lastMousePosition.x);
                    var y = (lastMousePosition.y - Input.mousePosition.y);

                    var newAngle = Vector3.zero;
                    newAngle.x = x * rotationspeed.x;
                    newAngle.y = y * rotationspeed.y;
                    if (maxAngle < totalAngle.y + newAngle.y)
                    {
                        newAngle.y = maxAngle - totalAngle.y;
                    }
                    if (-maxAngle > totalAngle.y + newAngle.y)
                    {
                        newAngle.y = -maxAngle - totalAngle.y;
                    }
                    totalAngle.y += newAngle.y;

                    mainCamera.transform.RotateAround(player.transform.position, Vector3.up, newAngle.x);
                    mainCamera.transform.RotateAround(player.transform.position, transform.right, newAngle.y);
                    lastMousePosition = Input.mousePosition;
                }
                //カメラのめり込み回避
                if (Physics.Raycast(player.transform.position, mainCamera.transform.position - player.transform.position, out hit, basedistance, layer, QueryTriggerInteraction.Ignore))
                {
                    mainCamera.transform.position = hit.point;
                }
                else
                {
                    var NowCamera = mainCamera.transform.position;
                    direction = NowCamera - player.transform.position;
                    direction = direction.normalized;
                    distance = Vector3.Distance(player.transform.position, NowCamera);
                    Mathf.Abs(distance);
                    if (distance < basedistance)
                    {
                        var step = basedistance - distance;
                        Vector3 targetPos = Vector3.MoveTowards(NowCamera, NowCamera + direction * step, 5.0f * Time.deltaTime);
                        mainCamera.transform.position = targetPos;
                    }
                }
            }
        }
    }
}
