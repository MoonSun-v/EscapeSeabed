using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState_FlyEye : IState_Enemy
{
    private FlyEyeFSM flyeye;

    private float HurtTime = 2.0f;
    private float elapsedTime = 0.0f;

    public HurtState_FlyEye(FlyEyeFSM flyeye)
    {
        this.flyeye = flyeye;
    }

    public void Enter()
    {
        // Debug.Log("FlyEye_Hurt");
        flyeye.StopMoving();
        flyeye.SetActiveState(FlyEyeFSM.FlyEyeState.Hurt);
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > HurtTime)
        {
            flyeye.Destroying();
        }
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }
}
