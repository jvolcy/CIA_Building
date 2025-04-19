using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text TxtRoomID;
    [SerializeField] GameObject PanelResetPopUp;
    [SerializeField] Transform StartingPlayerPosition;
    GameObject player;


    enum ResetState { PLAYING, RESET_REQUESTED, RESET_CONFIRM }
    ResetState resetState;

    // Start is called before the first frame update
    void Start()
    {
        resetState = ResetState.PLAYING;

        //find the player
        var players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length != 1)
        {
            Debug.Log(name + " Warning: found " + players.Length + " objects tagged as 'Player'.");
        }
        player = null;
        if (players.Length == 1) Debug.Log(name + " Found 1 Player.");
        if (players.Length > 0) player = players[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoomIDText(string s)
    {
        TxtRoomID.text = s;
    }

    public void UnSetRoomIDText(string s)
    {
        if (TxtRoomID.text == s)
        {
            TxtRoomID.text = "";
        }
    }

    public void XButtonPressed()
    {
        //Debug.Log("X button");
        switch (resetState)
        {
            case ResetState.PLAYING:
                //user has requested a reset
                PanelResetPopUp.SetActive(true);
                resetState = ResetState.RESET_REQUESTED;
                break;
            case ResetState.RESET_REQUESTED:
                //user has denied to confirm the request
                PanelResetPopUp.SetActive(false);
                resetState = ResetState.PLAYING;
                break;
        }
    }

    public void YButtonPressed()
    {
        //Debug.Log("Y button");
        switch (resetState)
        {
            case ResetState.RESET_REQUESTED:
                //user has confirmed the reset
                PanelResetPopUp.SetActive(false);
                reset();
                resetState = ResetState.PLAYING;
                break;
        }
    }

    private void reset()
    {
        SceneManager.LoadScene(0);
        /*
        player.SetActive(false);
        player.transform.position = StartingPlayerPosition.position;
        player.transform.rotation = StartingPlayerPosition.rotation;
        player.SetActive(true);
        Debug.Log("Player position reset");
        */
        
    }
}
