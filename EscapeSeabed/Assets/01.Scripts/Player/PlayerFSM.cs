using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    private IState_Player currentState;

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Shooting")]
    public GameObject BulletPrefab;
    public float launchSpeed = 10.0f;

    [Header("StateSetting")] // Enum으로 변경 할말 ? 
    private string[] stateParams = { "isIdle", "isRunning", "isJumping", "isShooting" };

    private float moveX = 0f; // 좌우 
    private float minX, maxX; // 카메라 경계

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        ChangeState(new IdleState_Player(this));

        // [ 카메라 경계 계산 ]
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)); // 왼쪽 아래
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)); // 오른쪽 위 
        minX = min.x + col.bounds.extents.x;
        maxX = max.x - col.bounds.extents.x;
    }

    void Update()
    {
        currentState?.HandleInput();
        currentState?.Update();

        moveX = Input.GetAxisRaw("Horizontal"); // -1.0 ~ 1.0
        
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

        // [ 화면 경계 ]
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.position = clampedPosition;
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

    public void Shoot()
    {
        GameObject bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (transform.localScale.x < 0) 
        {
            rb.AddForce(-transform.right * launchSpeed, ForceMode2D.Impulse);
            bullet.transform.localScale = new Vector3(-Mathf.Abs(bullet.transform.localScale.x), bullet.transform.localScale.y, bullet.transform.localScale.z);
        }
        else
        {
            rb.AddForce(transform.right * launchSpeed, ForceMode2D.Impulse);
            bullet.transform.localScale = new Vector3(Mathf.Abs(bullet.transform.localScale.x), bullet.transform.localScale.y, bullet.transform.localScale.z);
        }
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