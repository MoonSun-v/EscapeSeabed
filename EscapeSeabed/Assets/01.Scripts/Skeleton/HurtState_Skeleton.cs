
using UnityEngine;

public class HurtState_Skeleton : IState_Enemy
{
    private SkeletonFSM skeleton;

    private float HurtTime = 2.0f;
    private float elapsedTime = 0.0f;

    public HurtState_Skeleton(SkeletonFSM skeleton)
    {
        this.skeleton = skeleton;
    }

    public void Enter()
    {
        Debug.Log("Skeleton_Hurt");
        skeleton.StopMoving();
        skeleton.SetActiveState(SkeletonFSM.SkeletonState.Hurt);
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > HurtTime)
        {
            skeleton.Destroying();
        }
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {

    }
}
