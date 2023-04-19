using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Range(0, 5)] public int myLife = 5;

    public Transform lifeParent;

    public GameObject lifeImagePrefab;
    public GameObject[] myLifeObj = new GameObject[5];

    public AudioClip[] bgmList;
    public AudioSource bgmPlayer;

    public string nowSceneName;

    public GameObject gameOverScreen;
    public GameObject gameClearScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameOverScreen.SetActive(false);
    }
    /// <summary>
    /// 생명생성함수 게임시작씬으로 넘어갈때 발동
    /// </summary>
    public void StartGame()
    {
        for (int i = 0; i < myLifeObj.Length; i++)
        {
            if (myLifeObj[i] == null)
            {
                myLifeObj[i] = Instantiate(lifeImagePrefab, lifeParent);
            }
        }
        foreach (GameObject obj in myLifeObj)
        {
            obj.SetActive(true);
            myLife = 5;
        }
        gameOverScreen.SetActive(false);
    }
    /// <summary>
    /// 배경음악변경
    /// 0: ToVictory 1:Forward 2:Boss Battle
    /// </summary>
    /// <param name="bgm"></param>
    public void PlayBGM(int bgm)
    {
        bgmPlayer.clip = bgmList[bgm];
        bgmPlayer.loop = true;
        bgmPlayer.Play();
    }
    /// <summary>
    /// 생명증가
    /// </summary>
    public void AddLife()
    {
        myLifeObj[myLife++ - 1].SetActive(true);
    }
    /// <summary>
    /// 생명감소
    /// </summary>
    public void RemoveLife()
    {
        myLifeObj[myLife-- - 1].SetActive(false);   
    }
    /// <summary>
    /// 생명상황
    /// </summary>
    /// <returns></returns>
    public int GetLife()
    {
        return myLife;
    }
}