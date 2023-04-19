using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroBulletFire : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bulletPrefab; //�Ѿ� ������
    public float fireRate = 1f; // �߻� �ӵ�
    public Transform firePoint;
    

    public void Fire()
    {
        Vector2 firepos =  firePoint.position;
        firepos.y += 10f;
        GameObject bullet = Instantiate(bulletPrefab, firepos, Quaternion.identity);
        bullet.GetComponent<BulletScript>().direction = Vector2.down;
    }
}
