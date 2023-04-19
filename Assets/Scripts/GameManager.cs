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
    /// ��������Լ� ���ӽ��۾����� �Ѿ�� �ߵ�
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
    /// ������Ǻ���
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
    /// ��������
    /// </summary>
    public void AddLife()
    {
        myLifeObj[myLife++ - 1].SetActive(true);
    }
    /// <summary>
    /// ������
    /// </summary>
    public void RemoveLife()
    {
        myLifeObj[myLife-- - 1].SetActive(false);   
    }
    /// <summary>
    /// �����Ȳ
    /// </summary>
    /// <returns></returns>
    public int GetLife()
    {
        return myLife;
    }
}