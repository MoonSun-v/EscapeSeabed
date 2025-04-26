using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatFSM : MonoBehaviour
{
    private IState_Enemy currentState;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public Collider2D col;

    [Header("Movement")]
    public float moveSpeed = 0.4f;
    public float lastFlipYPos;

    public enum BatState { Fly, Hurt }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        lastFlipYPos = transform.position.y;

        ChangeState(new FlyState_Bat(this));
    }

    void Update()
    {
        currentState?.Update();
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void ChangeState(IState_Enemy newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void SetActiveState(BatState activeParam)
    {
        foreach (var param in System.Enum.GetValues(typeof(BatState)))
        {
            animator.SetBool(param.ToString(), param.Equals(activeParam));
        }
    }

    public float DistanceMove()
    {
        return Mathf.Abs(transform.position.y - lastFlipYPos);
    }

    public void Move()
    {
        rb.velocity = new Vector2(rb.velocity.x, moveSpeed);
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
            ChangeState(new HurtState_Bat(this));
        }
    }
}
