using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState_Lizard : IState_Lizard
{
    private LizardFSM lizard;

    private float ShootTime = 1.5f;
    private float elapsedTime = 0.0f;

    public ShootState_Lizard(LizardFSM lizard)
    {
        this.lizard = lizard;
    }

    public void Enter()
    {
        Debug.Log("Lizard_Shoot");

        lizard.SetActiveState(LizardFSM.LizardState.Shooting);
    }

    public void Update()
    {
        
        elapsedTime += Time.deltaTime;

        if (elapsedTime > ShootTime)
        {
            lizard.ChangeState(new IdleState_Lizard(lizard));
        }
        
    }

    public void Exit()
    {

    }
}
