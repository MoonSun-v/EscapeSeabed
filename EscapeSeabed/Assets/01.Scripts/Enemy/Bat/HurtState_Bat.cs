using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState_Bat : IState_Enemy
{
    private BatFSM bat;

    private float HurtTime = 2.0f;
    private float elapsedTime = 0.0f;

    public HurtState_Bat(BatFSM bat)
    {
        this.bat = bat;
    }

    public void Enter()
    {
        // Debug.Log("Bat_Hurt");
        bat.StopMoving();
        bat.SetActiveState(BatFSM.BatState.Hurt);
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > HurtTime)
        {
            bat.Destroying();
        }
    }

    public void FixedUpdate()
    {
        
    }

    public void Exit()
    {

    }
}
