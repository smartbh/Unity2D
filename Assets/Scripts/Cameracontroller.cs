using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontroller : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    public Vector3 minPosition;
    public Vector3 maxPosition;
    public float smoothTime = 0.3f;

    private Vector3 velocity;


    private void LateUpdate()
    {
        // ī�޶��� ���ο� ��ġ ���
        Vector3 targetPosition = player.transform.position + offset;
        targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

        // ī�޶� �̵�
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
