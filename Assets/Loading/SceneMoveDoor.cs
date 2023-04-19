using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMoveDoor : MonoBehaviour
{
    [SerializeField] string moveSceneName;    
    LayerMask playerLayer = 1 << 6;

    private bool isOpen = false;

    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.E))
        {            
            LodingSceneMover.LoadScene(moveSceneName);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerLayer == (playerLayer | (1 << collision.gameObject.layer)))
        {
            isOpen = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerLayer == (playerLayer | (1 << collision.gameObject.layer)))
        {
            isOpen = false;
        }
    }

}
