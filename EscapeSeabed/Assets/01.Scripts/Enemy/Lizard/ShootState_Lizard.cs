using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState_Lizard : IState_Enemy
{
    private LizardFSM lizard;

    private float FireBallSpawnTime = 0.6f;
    private bool hasShot = false;

    private float ShootTime = 1.5f;
    private float elapsedTime = 0.0f;

    public ShootState_Lizard(LizardFSM lizard)
    {
        this.lizard = lizard;
    }

    public void Enter()
    {
        // Debug.Log("Lizard_Shoot");

        lizard.SetActiveState(LizardFSM.LizardState.Shooting);
        
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > ShootTime)
        {
            lizard.ChangeState(new IdleState_Lizard(lizard));
        }
        else if (elapsedTime > FireBallSpawnTime && !hasShot)
        {
            lizard.Shoot();
            hasShot = true; 
        }
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }
}
