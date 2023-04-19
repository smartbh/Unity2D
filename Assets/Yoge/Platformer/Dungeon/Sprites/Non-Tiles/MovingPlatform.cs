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

        //���� ��ġ(x)�� ��� �̵������� (x)�ִ밪���� ũ�ų� ���ٸ�

        //�̵��ӵ�+���⿡ -1�� ���� ������ ���ְ� ������ġ�� ��� �̵������� (x)�ִ밪���� ����

        else if (currentPosition <= minY)

        {

            direction *= -1;

            currentPosition = minY;

        }

        //���� ��ġ(x)�� �·� �̵������� (x)�ִ밪���� ũ�ų� ���ٸ�

        //�̵��ӵ�+���⿡ -1�� ���� ������ ���ְ� ������ġ�� �·� �̵������� (x)�ִ밪���� ����

        transform.position = new Vector3(maxX, currentPosition, 0);

        //"Stone"�� ��ġ�� ���� ������ġ�� ó��
    }
}
