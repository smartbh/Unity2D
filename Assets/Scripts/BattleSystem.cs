using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleSystem : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] int maxHp;
    BoxCollider2D monCollider;

    private void Awake()
    {
        monCollider = GetComponent<BoxCollider2D>();
    }

    public int HP
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);
        }
    }
    Coroutine colorChange;
    public virtual void OnDamage()
    {
        HP--;
        if (HP > 0)
        {
            colorChange = StartCoroutine(ChangeColor());
        }
        else isDeath();
    }
    IEnumerator ChangeColor()
    {
        GetComponent<SpriteRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().material.color = Color.white;
    }

    public bool OnLive()
    {
        return HP > 0;
    }

    public void isDeath()
    {
        GetComponent<Animator>().SetTrigger("Death");
        monCollider.enabled = false;
        Debug.Log(monCollider.gameObject.name);
    }
}
