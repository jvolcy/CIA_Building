using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] GameObject ElevatorControls;
    [SerializeField] GameObject ElevatorUpDownControls;
    [SerializeField] GameObject ResetControls;

    ElevatorEntrance[] ElevatorEntrances;

    bool DoorsOpen = false;
    int CurrentLevel = 0;

    public enum ElevatorState { IDLE, IDLE_2_ENTRANCE, AT_ENTRANCE,
                                ENTRANCE_2_ELEVATOR, IN_ELEVATOR,
                                ELEVATOR_2_ENTRANCE, ENTRANCE_2_IDLE }
    /// <summary>
    /// There are 3 resting states (IDLE, AT_ENTRANCE and IN_ELEVATOR) and 4
    /// transient states (IDLE_2_ENTRANCE, ENTRANCE_2_ELEVATOR,
    /// ELEVATOR_2_ENTRANCE and ENTRACE_2_IDLE).  The transient states do not
    /// survive an update cycle.  That is, at the start and end of each update
    /// frame, the value of the ElevatorState variable must be one of the
    /// resting states.  Always call the UpdateState() function after setting
    /// the ElevatorState variable to one of the transient states.
    /// Assumptions: [1] you cannot enter the IN_ELEVATOR state without first
    /// going through the AT_ENTRANCE state. [2] if you leave the IN_ELEVATOR
    /// state you will be in the AT_ENTRANCE state. [3] you cannot "reset" to
    /// the start position while in the elevator. [4] the entrance and elevator
    /// colliders intersect.  If you exit the entrance collider, you are either
    /// in the elevator (in which case you are collided with the elevator
    /// trigger) or you are walking away from the the elevator (in which case
    /// you are not collided with the elevator trigger).
    /// </summary>
    public ElevatorState elevatorState = ElevatorState.IDLE;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //find all of our elevator entrances
        ElevatorEntrances = GetComponentsInChildren<ElevatorEntrance>();

        //set the parent <Elevator> object for each elevator entrance
        foreach (ElevatorEntrance ee in ElevatorEntrances)
        {
            //ee.elevator = this;
            ee.ElevatorEntranceStateChange.AddListener(ElevatorEntranceStateChange);
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
    /*
    void Update()
    {
        
    }
    */


    void UpdateState(ElevatorState state)
    {
        elevatorState = state;

        switch (elevatorState)
        {
            case ElevatorState.IDLE:
                break;
            case ElevatorState.IDLE_2_ENTRANCE:
                ElevatorControls.SetActive(true);
                ResetControls.SetActive(false);
                Debug.Log(name + "CurrentLevel is " + CurrentLevel);
                elevatorState = ElevatorState.AT_ENTRANCE;
                break;
            case ElevatorState.AT_ENTRANCE:
                break;
            case ElevatorState.ENTRANCE_2_ELEVATOR:
                //we are entering the elevator
                //enable up/down controls here
                ElevatorUpDownControls.SetActive(true);
                DoorsOpen = true;
                elevatorState = ElevatorState.IN_ELEVATOR;
                break;
            case ElevatorState.IN_ELEVATOR:
                break;
            case ElevatorState.ELEVATOR_2_ENTRANCE:
                //we are exiting the elevator
                //disable up/down controls here
                ElevatorUpDownControls.SetActive(false);
                elevatorState = ElevatorState.AT_ENTRANCE;
                break;
            case ElevatorState.ENTRANCE_2_IDLE:
                //turn off elevator controls
                ElevatorControls.SetActive(false);
                ResetControls.SetActive(true);
                //close the elevator doors
                CloseDoors();
                elevatorState = ElevatorState.IDLE;
                break;
        }
    }


    public void ElevatorEntranceStateChange(ElevatorEntrance elevatorEntrance, ElevatorEntrance.ElevatorEntranceState elevatorEntranceState)
    {
        if (elevatorEntranceState == ElevatorEntrance.ElevatorEntranceState.Entered)

        { 
            switch (elevatorState)
            {
                case ElevatorState.IDLE:
                    CurrentLevel = elevatorEntrance.floorIndex;
                    UpdateState(ElevatorState.IDLE_2_ENTRANCE);
                    break;
                case ElevatorState.AT_ENTRANCE:
                    Debug.Log(name + "***WARNING*** Re-entering the AT_ENTRANCE state: this shouldn't happen!!");
                    break;
                case ElevatorState.IN_ELEVATOR:
                    //we are exiting the elevator
                    UpdateState(ElevatorState.ELEVATOR_2_ENTRANCE);
                    break;
            }
        }

        if (elevatorEntranceState == ElevatorEntrance.ElevatorEntranceState.Exited)
        {

            switch (elevatorState)
            {
                case ElevatorState.IDLE:
                    Debug.Log(name + "***WARNING*** Re-entering the IDLE state: this shouldn't happen!!");
                    break;
                case ElevatorState.AT_ENTRANCE:
                    UpdateState(ElevatorState.ENTRANCE_2_IDLE);
                    break;
                case ElevatorState.IN_ELEVATOR:
                    //we are entering the elevator
                    UpdateState(ElevatorState.ENTRANCE_2_ELEVATOR);
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (elevatorState)
        {
            case ElevatorState.IDLE:
                //we should only be entering the elevator from the AT_ENTRANCE state.
                Debug.Log(name + "***WARNING*** Entering elevator from the IDLE state: this shouldn't happen!!");
                break;
            case ElevatorState.AT_ENTRANCE:
                UpdateState(ElevatorState.ENTRANCE_2_ELEVATOR);
                break;
            case ElevatorState.IN_ELEVATOR:
                //we should only be entering the elevator from the AT_ENTRANCE state.
                Debug.Log(name + "***WARNING*** Entering elevator from the ELEVATOR state: this shouldn't happen!!");
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (elevatorState)
        {
            case ElevatorState.IDLE:
                //we should only be exiting the elevator from the IN_ELEVATOR state.
                Debug.Log(name + "***WARNING*** Exiting elevator from the IDLE state: this shouldn't happen!!");
                break;
            case ElevatorState.AT_ENTRANCE:
                //we should only be exiting the elevator from the IN_ELEVATOR state.
                Debug.Log(name + "***WARNING*** Exiting elevator from the AT_ENTRANCE state: this shouldn't happen!!");
                break;
            case ElevatorState.IN_ELEVATOR:
                UpdateState(ElevatorState.ELEVATOR_2_ENTRANCE);
                break;
        }
    }

    public void ElevatorOpen()
    {
        Debug.Log(name + "ElevatorOpen");
        if (elevatorState == ElevatorState.AT_ENTRANCE || elevatorState == ElevatorState.IN_ELEVATOR) OpenDoors();
    }

    public void ElevatorClose()
    {
        Debug.Log(name + "ElevatorClose");
        if (elevatorState == ElevatorState.AT_ENTRANCE || elevatorState == ElevatorState.IN_ELEVATOR) CloseDoors();
    }

    public void ElevatorUp()
    {
        //Debug.Log("ElevatorUp");
        if (elevatorState != ElevatorState.IN_ELEVATOR)
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
        if (elevatorState != ElevatorState.IN_ELEVATOR)
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

