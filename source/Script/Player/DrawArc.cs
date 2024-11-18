using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArc : MonoBehaviour
{
    private bool draw = true;
    private int segmentCount = 60;
    private float predictionTime = 6.0f;
    [SerializeField] private Material ArcMaterial;
    private float ArcWidth = 0.1f;
    private LineRenderer[] LineRenderers;
    private PlayerBullet PlayerBullet;
    private Vector3 initialVelocity;
    private Vector3 arcStartPosition;
    // Start is called before the first frame update
    void Start()
    {
        CreateLineRendererObjects();
        PlayerBullet = GetComponent<PlayerBullet>();
    }

    // Update is called once per frame
    void Update()
    {
        initialVelocity = PlayerBullet.ShootVelocity;
        arcStartPosition = PlayerBullet.InstantiatePosition;
        if (draw)
        {
            float timeStep = predictionTime / segmentCount;
            bool draw = false;
            float hitTime = float.MaxValue;
            for (int i = 0; i < segmentCount; i++)
            {
                // ���̍��W���X�V
                float startTime = timeStep * i;
                float endTime = startTime + timeStep;
                SetLineRendererPosition(i, startTime, endTime, !draw);

                // �Փ˔���
                if (!draw)
                {
                    hitTime = GetArcHitTime(startTime, endTime);
                    if (hitTime != float.MaxValue)
                    {
                        draw = true; // �Փ˂����炻�̐�̕������͕\�����Ȃ�
                    }
                }
            }
        }
        else
        {
            // �������ƃ}�[�J�[��\�����Ȃ�
            for (int i = 0; i < LineRenderers.Length; i++)
            {
                LineRenderers[i].enabled = false;
            }
        }
    }

    private Vector3 GetArcPositionAtTime(float time)
    {
        return (arcStartPosition + ((initialVelocity * time) + (0.5f * time * time) * Physics.gravity));
    }

    private void SetLineRendererPosition(int index, float startTime, float endTime, bool draw = true)
    {
        LineRenderers[index].SetPosition(0, GetArcPositionAtTime(startTime));
        LineRenderers[index].SetPosition(1, GetArcPositionAtTime(endTime));
        LineRenderers[index].enabled = draw;
    }

    void CreateLineRendererObjects()
    {
        GameObject arcObjectsParent = new GameObject("ArcObject");

        LineRenderers = new LineRenderer[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            GameObject newObject = new GameObject("LineRenderer_" + i);
            newObject.transform.SetParent(arcObjectsParent.transform);
            LineRenderers[i] = newObject.AddComponent<LineRenderer>();

            // �����֘A���g�p���Ȃ�
            LineRenderers[i].receiveShadows = false;
            LineRenderers[i].reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            LineRenderers[i].lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            LineRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            // ���̕��ƃ}�e���A��
            LineRenderers[i].material = ArcMaterial;
            LineRenderers[i].startWidth = ArcWidth;
            LineRenderers[i].endWidth = ArcWidth;
            LineRenderers[i].numCapVertices = 5;
            LineRenderers[i].enabled = false;
        }
    }
    private float GetArcHitTime(float startTime, float endTime)
    {
        // Linecast��������̎n�I�_�̍��W
        Vector3 startPosition = GetArcPositionAtTime(startTime);
        Vector3 endPosition = GetArcPositionAtTime(endTime);

        // �Փ˔���
        RaycastHit hitInfo;
        if (Physics.Linecast(startPosition, endPosition, out hitInfo))
        {
            // �Փ˂���Collider�܂ł̋���������ۂ̏Փˎ��Ԃ��Z�o
            float distance = Vector3.Distance(startPosition, endPosition);
            return startTime + (endTime - startTime) * (hitInfo.distance / distance);
        }
        return float.MaxValue;
    }
}
