using UnityEngine;

public class MoveDoor : MonoBehaviour
{
    [SerializeField] GameObject moveDoor;
    LayerMask playerLayer=1<<6;

    private bool isOpen = false;

    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.E))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = moveDoor.transform.position;
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