using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_Lizard : IState_Lizard
{
    private LizardFSM lizard;

    private float IdleTime = 2.5f;
    private float elapsedTime = 0.0f;

    public IdleState_Lizard(LizardFSM lizard)
    {
        this.lizard = lizard;
    }

    public void Enter()
    {
        Debug.Log("Lizard_Idle");
        elapsedTime = 0.0f;
        lizard.SetActiveState(LizardFSM.LizardState.Idle);
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > IdleTime)
        {
            lizard.ChangeState(new ShootState_Lizard(lizard));
        }
    }

    public void Exit()
    {

    }
}
