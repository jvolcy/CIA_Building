using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] GameObject ElevatorControls;
    [SerializeField] GameObject ElevatorUpDownControls;
    ElevatorEntrance[] ElevatorEntrances;
    //[SerializeField] Transform[] ElevatorFloors;
    //[SerializeField] float YOffsets = 0f;

    bool InElevator = false;
    bool AtElevatorEntrance = false;
    bool DoorsOpen = false;
    int CurrentLevel = 0;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //find all of our elevator entrances
        ElevatorEntrances = GetComponentsInChildren<ElevatorEntrance>();

        //set the parent <Elevator> object for each elevator entrance
        foreach (ElevatorEntrance ee in ElevatorEntrances)
        {
            ee.elevator = this;
        }

        //At this point, we have a reference to all of our ElevatorEntrance
        //objects and each ElevatorEntrance has an <Elevator> reference to us.

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

    public void ElevatorEntranceEntered(ElevatorEntrance elevatorEntrance)
    {
        ElevatorControls.SetActive(true);
        AtElevatorEntrance = true;
        CurrentLevel = elevatorEntrance.floorIndex;
        Debug.Log(name + "CurrentLevel is " + CurrentLevel);
    }

    public void ElevatorEntranceExited(ElevatorEntrance elevatorEntrance)
    {
        if (InElevator == false)
        {
            ElevatorControls.SetActive(false);
            AtElevatorEntrance = false;
        }
        CloseDoors();
    }

    private void OnTriggerEnter(Collider other)
    {
        InElevator = true;
        ElevatorControls.SetActive(true);
        ElevatorUpDownControls.SetActive(true);
        DoorsOpen = true;
    }

    private void OnTriggerExit(Collider other)
    {
        InElevator = false;
        ElevatorUpDownControls.SetActive(false);
        ElevatorControls.SetActive(false);
    }

    public void ElevatorOpen()
    {
        Debug.Log(name + "ElevatorOpen");
        if (AtElevatorEntrance || InElevator) OpenDoors();
    }

    public void ElevatorClose()
    {
        Debug.Log(name + "ElevatorClose");
        if (AtElevatorEntrance || InElevator) CloseDoors();
    }

    public void ElevatorUp()
    {
        //Debug.Log("ElevatorUp");
        if (!InElevator)
        {
            Debug.Log(name + "Not in Elevator; [Up] instruction ignored.");
            return;
        }
        if (CurrentLevel == ElevatorEntrances.Length - 1)
        {
            Debug.Log(name + "Already on top floor; [Up] instruction ignored.");
            return;   //already on top floor
        }

        //need to check that the door is closed
        if (DoorsOpen)
        {
            Debug.Log(name + "Doors are open; [Up] instruction ignored.");
            return;
        }

        Debug.Log(name + "Current Y is " + player.transform.position.y + ". Target Y is " + ElevatorEntrances[CurrentLevel+1].transform.position.y);
        float yDelta = ElevatorEntrances[CurrentLevel+1].transform.position.y - player.transform.position.y;
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.Translate(0, yDelta, 0);
        player.GetComponent<CharacterController>().enabled = true;

        Debug.Log(name + "Translated player Y by " + yDelta);
        CurrentLevel++;
        Debug.Log(name + "CurrentLevel is " + CurrentLevel);

    }

    public void ElevatorDown()
    {
        //Debug.Log("ElevatorDown");
        if (!InElevator)
        {
            Debug.Log(name + "Not in Elevator; [Down] instruction ignored.");
            return;
        }
        if (CurrentLevel == 0)
        {
            Debug.Log(name + "Already on bottom floor; [Down] instruction ignored.");
            return;   //already on bottom floor
        }

        //need to check that the door is closed
        if(DoorsOpen)
        {
            Debug.Log(name + "Doors are open; [Down] instruction ignored.");
            return;
        }

        Debug.Log(name + "Current Y is " + player.transform.position.y + ". Target Y is " + ElevatorEntrances[CurrentLevel-1].transform.position.y);
        float yDelta = ElevatorEntrances[CurrentLevel-1].transform.position.y - player.transform.position.y;
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.Translate(0, yDelta, 0);
        player.GetComponent<CharacterController>().enabled = true;

        Debug.Log(name + "Translated player Y by " + yDelta);
        CurrentLevel--;
        Debug.Log(name + "CurrentLevel is " + CurrentLevel);
    }

    void OpenDoors()
    {
        foreach (ElevatorEntrance ee in ElevatorEntrances)
        {
            ee.OpenDoor();
        }
        DoorsOpen = true;
    }

    void CloseDoors()
    {
        foreach (ElevatorEntrance ee in ElevatorEntrances)
        {
            ee.CloseDoor();
        }
        DoorsOpen = false;
    }
}
