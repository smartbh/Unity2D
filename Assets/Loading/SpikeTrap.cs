using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(TrapDamage(collision.GetComponent<BattleSystem>()));
    }

    IEnumerator TrapDamage(BattleSystem player)
    {
        while (true)
        {
            player.OnDamage();
            if (!player.OnLive())
            {
                yield break;
            }
            yield return new WaitForSeconds(1.0f);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StopAllCoroutines();
    }

}