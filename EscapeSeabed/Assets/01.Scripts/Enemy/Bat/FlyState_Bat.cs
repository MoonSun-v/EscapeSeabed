using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyState_Bat : IState_Enemy
{
    private BatFSM bat;

    public float moveDistanceThreshold = 3.0f; 
                                                
    public FlyState_Bat(BatFSM bat)
    {
        this.bat = bat;
    }

    public void Enter()
    {
        Debug.Log("Bat_Fly");
        bat.SetActiveState(BatFSM.BatState.Fly);
    }

    public void Update()
    {
        if (bat.DistanceMove() >= moveDistanceThreshold)
        {
            bat.moveSpeed *= -1;

            bat.lastFlipYPos = bat.transform.position.y;
        }
    }

    public void FixedUpdate()
    {
        bat.Move();
    }

    public void Exit()
    {

    }
}
