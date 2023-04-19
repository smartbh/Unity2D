using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGameStart : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey)
        {
            LodingSceneMover.LoadScene("Map(1-1~2)");
        }
    }
}
