using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeTrap : MonoBehaviour
{     
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<BattleSystem>()?.OnDamage();
    }
}
