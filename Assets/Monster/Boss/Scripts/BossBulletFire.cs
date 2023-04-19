using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletFire : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bulletPrefab; //ÃÑ¾Ë ÇÁ¸®ÆÕ
    public float fireRate = 1f; // ¹ß»ç ¼Óµµ
    public Transform firePoint;
    public BossMove aiScript;
    

    public void Fire()
    {
        
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Vector2 direction = (Vector2)aiScript.player.transform.position - (Vector2)transform.position;
        direction.Normalize();
        bullet.GetComponent<BulletScript>().direction = direction;
    }
}
