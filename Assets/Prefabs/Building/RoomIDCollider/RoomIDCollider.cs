using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomIDCollider : MonoBehaviour
{
    GameManager gameManager;

    //keep a counter in case we have multiple colliders on the same GO
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        //turn off the renderer (hide the ID collider) on startup
        GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        count++;
        //sets the UI text to the supplied string
        gameManager.SetRoomIDText(name);
    }

    private void OnTriggerExit(Collider other)
    {
        count--;

        if (count <= 0)  //should never be < 0!
        {
            //unset resets the UI text only if it matches the supplied string
            gameManager.UnSetRoomIDText(name);
        }
    }
}
