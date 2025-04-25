using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyState_FlyEye : IState_FlyEye
{
    private FlyEyeFSM flyeye;

    public float moveDistanceThreshold = 4.0f; // 이동 거리 임계치  

    public FlyState_FlyEye(FlyEyeFSM flyeye)
    {
        this.flyeye = flyeye;
    }

    public void Enter()
    {
        Debug.Log("FlyEye_Fly");
        flyeye.SetActiveState(FlyEyeFSM.FlyEyeState.Fly);
    }

    public void Update()
    {
        if (flyeye.DistanceMove() >= moveDistanceThreshold)
        {
            flyeye.moveSpeed *= -1;

            Vector3 localScale = flyeye.transform.localScale;
            localScale.x *= -1;
            flyeye.transform.localScale = localScale;

            flyeye.lastFlipXPos = flyeye.transform.position.x;
        }
    }

    public void FixedUpdate()
    {
        flyeye.Move();
    }

    public void Exit()
    {

    }

}
