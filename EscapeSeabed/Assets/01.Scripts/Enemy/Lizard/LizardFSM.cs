using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FlyEyeFSM;

public class LizardFSM : MonoBehaviour
{
    private IState_Lizard currentState;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public Collider2D col;

    [Header("Movement")]
    public float moveSpeed = 0.6f;
    public float lastFlipXPos;

    public enum LizardState { Idle, Shooting, Hurt }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        ChangeState(new IdleState_Lizard(this));
    }

    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(IState_Lizard newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void SetActiveState(LizardState activeParam)
    {
        foreach (var param in System.Enum.GetValues(typeof(LizardState)))
        {
            animator.SetBool(param.ToString(), param.Equals(activeParam));
        }
    }

    public void Destroying()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            ChangeState(new HurtState_Lizard(this));
        }
    }
}
