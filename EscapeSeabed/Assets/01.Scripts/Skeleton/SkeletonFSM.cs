using System.Collections;
using UnityEngine;

public class SkeletonFSM : MonoBehaviour
{
    private IState_Enemy currentState;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public Collider2D col;

    [Header("Movement")]
    public float moveSpeed = 0.5f;
    public float lastFlipXPos;

    public enum SkeletonState { Idle, Walking }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        lastFlipXPos = transform.position.x;

        ChangeState(new IdleState_Skeleton(this));
    }

    
    void Update()
    {
        currentState?.Update();
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    // [ 이동한 거리 계산 ] 
    public float DistanceMove()
    {
        return Mathf.Abs(transform.position.x - lastFlipXPos);
    }

    
    public void Move()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }
    

    public void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    public void ChangeState(IState_Enemy newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void SetActiveState(SkeletonState activeParam)
    {
        foreach (var param in System.Enum.GetValues(typeof(SkeletonState)))
        {
            animator.SetBool(param.ToString(), param.Equals(activeParam));
        }
    }
}
