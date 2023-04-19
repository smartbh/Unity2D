using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float currentPosition;
    private bool turnAround;
    public float direction = 0.1f;

    private void Start()
    {
        currentPosition = transform.position.y;
    }

    private void FixedUpdate()
    {
        currentPosition += Time.deltaTime * direction;

        if (currentPosition >= maxY)

        {

            direction *= -1;

            currentPosition = maxY;

        }

        //현재 위치(x)가 우로 이동가능한 (x)최대값보다 크거나 같다면

        //이동속도+방향에 -1을 곱해 반전을 해주고 현재위치를 우로 이동가능한 (x)최대값으로 설정

        else if (currentPosition <= minY)

        {

            direction *= -1;

            currentPosition = minY;

        }

        //현재 위치(x)가 좌로 이동가능한 (x)최대값보다 크거나 같다면

        //이동속도+방향에 -1을 곱해 반전을 해주고 현재위치를 좌로 이동가능한 (x)최대값으로 설정

        transform.position = new Vector3(maxX, currentPosition, 0);

        //"Stone"의 위치를 계산된 현재위치로 처리
    }
}
