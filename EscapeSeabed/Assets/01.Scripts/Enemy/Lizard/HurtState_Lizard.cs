using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState_Lizard : IState_Enemy
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
        // Debug.Log("Lizard_Hurt");
        lizard.SetActiveState(LizardFSM.LizardState.Hurt);
        if (lizard.col != null) lizard.col.enabled = false;
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > HurtTime)
        {
            lizard.Destroying();
        }
    }

    public void FixedUpdate()
    {

    }


    public void Exit()
    {
        if (lizard.col != null) lizard.col.enabled = true;
    }
}
