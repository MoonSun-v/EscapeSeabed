using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEyeFSM : MonoBehaviour
{
    private IState_FlyEye currentState;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public Collider2D col;

    [Header("Movement")]
    public float moveSpeed = 0.6f;
    public float lastFlipXPos;

    public enum FlyEyeState { Fly, Hurt }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        lastFlipXPos = transform.position.x;

        ChangeState(new FlyState_FlyEye(this));
    }

    void Update()
    {
        currentState?.Update();
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void ChangeState(IState_FlyEye newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void SetActiveState(FlyEyeState activeParam)
    {
        foreach (var param in System.Enum.GetValues(typeof(FlyEyeState)))
        {
            animator.SetBool(param.ToString(), param.Equals(activeParam));
        }
    }

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

    public void Destroying()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            ChangeState(new HurtState_FlyEye(this));
        }
    }
}
