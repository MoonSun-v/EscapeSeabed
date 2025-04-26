using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FlyEyeFSM;

public class LizardFSM : MonoBehaviour
{
    private IState_Enemy currentState;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public Collider2D col;

    [Header("Movement")]
    public float moveSpeed = 0.6f;
    public float lastFlipXPos;

    [Header("Shooting")]
    public GameObject FireBallPrefab;
    public Transform fireSpawnPoint; 
    public float launchSpeed = 5.0f;


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

    public void ChangeState(IState_Enemy newState)
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

    public void Shoot()
    {
        GameObject fire = Instantiate(FireBallPrefab, fireSpawnPoint.position, fireSpawnPoint.rotation);
        Rigidbody2D rb = fire.GetComponent<Rigidbody2D>();

        if (transform.localScale.x < 0)
        {
            rb.AddForce(transform.right * launchSpeed, ForceMode2D.Impulse);
            fire.transform.localScale = new Vector3(-Mathf.Abs(fire.transform.localScale.x), fire.transform.localScale.y, fire.transform.localScale.z);
        }
        else
        {
            rb.AddForce(-transform.right * launchSpeed, ForceMode2D.Impulse);
            fire.transform.localScale = new Vector3(Mathf.Abs(fire.transform.localScale.x), fire.transform.localScale.y, fire.transform.localScale.z);
        }
    }
}
