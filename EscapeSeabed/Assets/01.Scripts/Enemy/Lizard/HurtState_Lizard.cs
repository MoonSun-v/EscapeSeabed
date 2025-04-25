using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState_Lizard : IState_Lizard
{
    private LizardFSM lizard;

    private float HurtTime = 2.0f;
    private float elapsedTime = 0.0f;

    public HurtState_Lizard(LizardFSM lizard)
    {
        this.lizard = lizard;
    }

    public void Enter()
    {
        Debug.Log("Lizard_Hurt");
        lizard.SetActiveState(LizardFSM.LizardState.Hurt);
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > HurtTime)
        {
            lizard.Destroying();
        }
    }

    public void Exit()
    {

    }
}
