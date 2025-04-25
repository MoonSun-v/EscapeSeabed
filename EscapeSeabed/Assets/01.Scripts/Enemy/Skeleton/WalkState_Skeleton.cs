
using UnityEngine;

public class WalkState_Skeleton : IState_Enemy
{
    private SkeletonFSM skeleton;
    public float moveDistanceThreshold = 4.0f; // 이동 거리 임계치  

    public WalkState_Skeleton(SkeletonFSM skeleton)
    {
        this.skeleton = skeleton;
    }

    public void Enter()
    {
        skeleton.lastFlipXPos = skeleton.transform.position.x;
        Debug.Log("Skeleton_Walk");
        skeleton.SetActiveState(SkeletonFSM.SkeletonState.Walking);
    }

    public void Update()
    {
        if (skeleton.DistanceMove() >= moveDistanceThreshold)
        {
            // 거리 도달했으면 idle로 전환 (이제 idle에서 방향 전환함!)
            skeleton.ChangeState(new IdleState_Skeleton(skeleton));
        }
    }

    public void FixedUpdate()
    {
        skeleton.Move();
    }


    public void Exit()
    {

    }
}