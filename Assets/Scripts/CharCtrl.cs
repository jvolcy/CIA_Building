using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCtrl : MonoBehaviour
{
    [SerializeField] Transform StartingPlayerPosition;

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        //Set the player's position
        SetPosition(StartingPlayerPosition);
    }

    void SetPosition(Transform t)
    {
        gameObject.SetActive(false);
        transform.position = t.position;
        transform.rotation = t.rotation;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnXButton()
    {
        gameManager.XButtonPressed();
    }

    public void OnYButton()
    {
        gameManager.YButtonPressed();
    }

}
