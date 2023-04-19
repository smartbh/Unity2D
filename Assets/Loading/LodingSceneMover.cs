using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LodingSceneMover : MonoBehaviour
{
    static string nextScene;
    public Slider loadingBar;


    public static void LoadScene(string scenmename)
    {
        nextScene = scenmename;
        GameManager.instance.nowSceneName = nextScene;
        SceneManager.LoadScene("LoadingScene");
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }
    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float time = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                loadingBar.value = op.progress;
            }
            else
            {
                time += Time.unscaledDeltaTime;
                loadingBar.value = Mathf.Lerp(0.9f, 1.0f, time);
                if (loadingBar.value >= 1f)
                {
                    switch (nextScene)
                    {
                        case "MainMenu":
                            GameManager.instance.PlayBGM(0);
                            break;
                        case "Map(1-1~2)":
                            GameManager.instance.StartGame();
                            GameManager.instance.PlayBGM(1);
                            break;
                        case "BossRoom":
                            GameManager.instance.PlayBGM(2);
                            break;
                    }
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}