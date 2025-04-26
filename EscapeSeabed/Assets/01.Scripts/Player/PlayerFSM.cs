using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    private IState_Player currentState;

    [Header("Components")]
    public Rigidbody2D rb;
    private Animator animator;
    private Collider2D col;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float fallForce = 8f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("Shooting")]
    public GameObject BulletPrefab;
    public float launchSpeed = 15.0f;

    private float moveX = 0f; // �¿� 
    private float minX, maxX; // ī�޶� ���
    private float wall_distance = 0.4f;

    public enum PlayerState { Idle, Running, Jumping, Shooting, RunShooting, Hurt }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();

        ChangeState(new IdleState_Player(this));

        // [ ī�޶� ��� ��� ]
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)); // ���� �Ʒ�
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)); // ������ �� 
        minX = min.x + col.bounds.extents.x;
        maxX = max.x - col.bounds.extents.x;
    }

    void Update()
    {
        currentState?.HandleInput();
        currentState?.Update();

        moveX = Input.GetAxisRaw("Horizontal"); // -1.0 ~ 1.0
        
        // [ �¿� ���� ]
        if (moveX != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveX);
            transform.localScale = scale;
        }
    }

    void FixedUpdate()
    {
        if (IsTouchingWall() && !IsGrounded()) { rb.velocity = new Vector2(0, rb.velocity.y); } // [ ���߿��� �� �浹 ]
        else { rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y); }// [ �¿� �̵� ]

            
        // [ ȭ�� ��� ]
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

    public void SetActiveState(PlayerState activeParam)
    {
        foreach (var param in System.Enum.GetValues(typeof(PlayerState)))
        {
            animator.SetBool(param.ToString(), param.Equals(activeParam));
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void Fall()
    {
        rb.velocity = new Vector2(rb.velocity.x, -fallForce);
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

    public Rigidbody2D GetRigidbody()
    {
        return rb;
    }

    public Vector2 LastHitPosition { get; set; }

    // [ �浹 �ð�ȭ ] 
    void OnDrawGizmos()
    {
        // �� 
        if (groundCheck != null)
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        // ��
        if (Application.isPlaying)
        {
            Vector2 direction = Vector2.right * Mathf.Sign(transform.localScale.x);

            Vector2 originLower = new Vector2(transform.position.x, transform.position.y - 0.57f);
            Vector2 originUpper = new Vector2(transform.position.x, transform.position.y + 0.4f);

            Gizmos.color = IsTouchingWall() ? Color.cyan : Color.yellow;

            Gizmos.DrawLine(originLower, originLower + direction * wall_distance);
            Gizmos.DrawLine(originUpper, originUpper + direction * wall_distance);
        }
    }

    // [ �� �ε��� üũ ]
    public bool IsTouchingWall()
    {
        Vector2 direction = Vector2.right * Mathf.Sign(transform.localScale.x);

        // ���� ���� ��ġ�� (���� ��/�Ʒ�)
        Vector2 originLower = new Vector2(transform.position.x, transform.position.y - 0.57f);
        Vector2 originUpper = new Vector2(transform.position.x, transform.position.y + 0.4f);

        // �� �� �ϳ��� ���� ������ true
        bool hitLower = Physics2D.Raycast(originLower, direction, wall_distance, groundLayer);
        bool hitUpper = Physics2D.Raycast(originUpper, direction, wall_distance, groundLayer);

        return hitLower || hitUpper;
    }

    // [ �浹 üũ ]
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            ChangeState(new HurtState_Player(this));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FireBall"))
        {
            ChangeState(new HurtState_Player(this));
        }
    }
}