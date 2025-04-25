using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCtrl : MonoBehaviour
{
    [SerializeField] Transform StartingPlayerPosition;

    public Elevator Elevator1;
    public Elevator Elevator2;
    public Elevator Elevator3;

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        reset();
    }

    public void reset()
    {
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

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnXButton()
    {
        //ignore the X button if we are in the elevator
        if (InElevator()) return;

        gameManager.XButtonPressed();
    }

    public void OnYButton()
    {
        //ignore the Y button if we are in the elevator
        if (InElevator()) return;

        gameManager.YButtonPressed();
    }

    public void OnElevatorOpen()
    {
        Elevator1.ElevatorOpen();
        Elevator2.ElevatorOpen();
        Elevator3.ElevatorOpen();
    }

    public void OnElevatorClose()
    {
        Elevator1.ElevatorClose();
        Elevator2.ElevatorClose();
        Elevator3.ElevatorClose();
    }

    public void OnElevatorUp()
    {
        Elevator1.ElevatorUp();
        Elevator2.ElevatorUp();
        Elevator3.ElevatorUp();
    }

    public void OnElevatorDown()
    {
        Elevator1.ElevatorDown();
        Elevator2.ElevatorDown();
        Elevator3.ElevatorDown();
    }

    bool InElevator()
    {
        if (Elevator1.elevatorState == Elevator.ElevatorState.IN_ELEVATOR || Elevator1.elevatorState == Elevator.ElevatorState.AT_ENTRANCE) return true;
        if (Elevator2.elevatorState == Elevator.ElevatorState.IN_ELEVATOR || Elevator2.elevatorState == Elevator.ElevatorState.AT_ENTRANCE) return true;
        if (Elevator3.elevatorState == Elevator.ElevatorState.IN_ELEVATOR || Elevator3.elevatorState == Elevator.ElevatorState.AT_ENTRANCE) return true;
        return false;
    }


}
