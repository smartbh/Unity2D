using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExitDoor : MonoBehaviour
{
    private bool isGameEnd;
    public GameObject boss;
    // Start is called before the first frame update
    private void Awake()
    {
        isGameEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!boss.GetComponent<BossMove>().OnLive())
        {
            isGameEnd = true;
        }

        if(isGameEnd && GameManager.instance.gameClearScreen.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                //UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && isGameEnd)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                GameManager.instance.gameClearScreen.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
}
