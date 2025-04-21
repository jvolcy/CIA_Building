using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorEntrance : MonoBehaviour
{

    bool ElevatorOpen = false;
    Animator animator;
    public Elevator elevator;   //automatically set by the parent <Elevator> object
    public int floorIndex = 0;
    public float YOffset = 0.1f;

    public enum ElevatorEntranceState { Entered, Exited }
    public UnityEvent<ElevatorEntrance, ElevatorEntranceState> ElevatorEntranceStateChange;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        if (ElevatorOpen == false)
        {
            animator.Play("ElevatorDoorOpen");
        }
        ElevatorOpen = true;
    }

    public void CloseDoor()
    {
        if (ElevatorOpen == true)
        {
            animator.Play("ElevatorDoorClose");
        }
        ElevatorOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //elevator.ElevatorEntranceEntered(this);
        ElevatorEntranceStateChange.Invoke(this, ElevatorEntranceState.Entered);
    }

    private void OnTriggerExit(Collider other)
    {
        //elevator.ElevatorEntranceExited(this);
        ElevatorEntranceStateChange.Invoke(this, ElevatorEntranceState.Exited);
    }

}
