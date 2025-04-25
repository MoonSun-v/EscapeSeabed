using UnityEngine;

public class IdleState_Skeleton : IState_Enemy
{
    private SkeletonFSM skeleton;

    private float IdleTime = 3.0f;
    private float elapsedTime = 0.0f;

    public IdleState_Skeleton(SkeletonFSM skeleton)
    {
        this.skeleton = skeleton;
    }

    public void Enter()
    {
        Debug.Log("Skeleton_Idle");
        elapsedTime = 0f;
        skeleton.StopMoving();
        skeleton.SetActiveState(SkeletonFSM.SkeletonState.Idle);
        
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > IdleTime)
        {
            // 방향 전환
            skeleton.moveSpeed *= -1;

            Vector3 localScale = skeleton.transform.localScale;
            localScale.x *= -1;
            skeleton.transform.localScale = localScale;

            // 새로운 기준점 저장
            skeleton.lastFlipXPos = skeleton.transform.position.x;

            // Walk 상태로 전환
            skeleton.ChangeState(new WalkState_Skeleton(skeleton));
        }
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }
}
