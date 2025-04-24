using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    private IState_Player currentState;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("AnimationSetting")]
    private string[] stateParams = { "isIdle", "isRunning", "isJumping" };

    private float moveX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        ChangeState(new IdleState_Player(this));
    }

    void Update()
    {
        currentState?.Update();

        moveX = Input.GetAxisRaw("Horizontal");
        
        // [ 좌우 반전 ]
        if (moveX != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveX);
            transform.localScale = scale;
        }

    }

    void FixedUpdate()
    {
        // [ 좌우 이동 ]
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
    }

    public void ChangeState(IState_Player newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void SetActiveState(string activeParam)
    {
        foreach (var param in stateParams)
        {
            animator.SetBool(param, param == activeParam);
        }
    }

    public void SetTrigger(string name)
    {
        animator.SetTrigger(name);
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }

    // [ 땅 충돌 체크 시각화 ] 
    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            if(IsGrounded()) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}