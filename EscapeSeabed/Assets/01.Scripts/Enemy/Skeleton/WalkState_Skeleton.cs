
using UnityEngine;

public class WalkState_Skeleton : IState_Enemy
{
    private SkeletonFSM skeleton;
    

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
        if (skeleton.DistanceMove() >= skeleton.moveDistanceThreshold)
        {
            // �Ÿ� ���������� idle�� ��ȯ
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