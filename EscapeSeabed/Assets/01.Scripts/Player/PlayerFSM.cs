using UnityEngine;
using UnityEngine.EventSystems;

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
    public LayerMask aeriagroundLayer;
    public bool isMoveable = true;

    [Header("Shooting")]
    public GameObject BulletPrefab;
    public float launchSpeed = 15.0f;

    [Header("Climbing")]
    public bool isClimbing = false;
    public float climbSpeed = 3f;
    public Transform ladderCheck;
    public float ladderCheckRadius = 0.1f;
    public LayerMask ladderLayer;
    public bool isAtLadderTop = false;

    public bool isJumping;

    private float moveX = 0f; // 좌우 
    private float minX, maxX; // 카메라 경계
    private float wall_distance = 0.4f;

    public enum PlayerState { Idle, Running, Jumping, Shooting, RunShooting, Hurt, Climbing }

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

    void Update() // 키 입력 
    {
        currentState?.HandleInput();
        currentState?.Update();

        Move();
    }

    void FixedUpdate() // 리지드바디 연산 
    {
        currentState?.FixedUpdate();

        if (IsTouchingWall() && !IsGrounded()) { rb.velocity = new Vector2(0, rb.velocity.y); } // [ 공중에서 벽 충돌 ]
        else { rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y); }// [ 좌우 이동 ]

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

    public void SetActiveState(PlayerState activeParam)
    {
        foreach (var param in System.Enum.GetValues(typeof(PlayerState)))
        {
            animator.SetBool(param.ToString(), param.Equals(activeParam));
        }
    }

    public void Move()
    {
        if (!isMoveable) return;
        moveX = Input.GetAxisRaw("Horizontal"); // -1.0 ~ 1.0

        // [ 좌우 반전 ]
        if (moveX != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveX);
            transform.localScale = scale;
        }
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

    // [ 충돌 시각화 ] 
    void OnDrawGizmos()
    {
        // 땅 
        if (groundCheck != null)
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        // 벽
        if (Application.isPlaying)
        {
            Vector2 direction = Vector2.right * Mathf.Sign(transform.localScale.x);

            Vector2 originLower = new Vector2(transform.position.x, transform.position.y - 0.57f);
            Vector2 originUpper = new Vector2(transform.position.x, transform.position.y + 0.4f);

            Gizmos.color = IsTouchingWall() ? Color.cyan : Color.yellow;

            Gizmos.DrawLine(originLower, originLower + direction * wall_distance);
            Gizmos.DrawLine(originUpper, originUpper + direction * wall_distance);
        }

        // 사다리 
        Gizmos.color = IsTouchingLadder() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(ladderCheck.position, ladderCheckRadius);
    }

    // [ 땅 체크 ]
    public bool IsGrounded()
    {
        if (IsTouchingLadder() && IsGroundingLadder()) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    // [ 공중 땅 체크 ]
    public bool IsAeriaGrounded()
    {
        if (IsTouchingLadder() && IsGroundingLadder()) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, aeriagroundLayer);
    }

    // [ 벽 부딪힘 체크 ]
    public bool IsTouchingWall()
    {
        if(IsTouchingLadder()&& IsGroundingLadder()) return false;

        Vector2 direction = Vector2.right * Mathf.Sign(transform.localScale.x);

        // 레이 시작 위치들 (몸통 위/아래)
        Vector2 originLower = new Vector2(transform.position.x, transform.position.y - 0.57f);
        Vector2 originUpper = new Vector2(transform.position.x, transform.position.y + 0.4f);

        // 둘 중 하나라도 벽에 닿으면 true
        bool hitLower = Physics2D.Raycast(originLower, direction, wall_distance, groundLayer);
        bool hitUpper = Physics2D.Raycast(originUpper, direction, wall_distance, groundLayer);

        return hitLower || hitUpper;
    }

    // [ 사다리 체크 ]
    public bool IsTouchingLadder()
    {
        return Physics2D.OverlapCircle(ladderCheck.position, ladderCheckRadius, ladderLayer);
    }
    public bool IsGroundingLadder()
    {
        return Physics2D.OverlapCircle(groundCheck.position, ladderCheckRadius, ladderLayer);
    }
    
    // [ 충돌 체크 ]
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            ChangeState(new HurtState_Player(this));
        }

        if (collision.gameObject.CompareTag("AeriaGround") /*|| !IsAeriaGrounded()*/)
        {
            // GetComponent<BoxCollider2D>().isTrigger = true;
            
            Collider2D otherCol = collision.collider;

            // 충돌 지점(contact point) 가져오기
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // 접촉한 점이 플레이어보다 '아래'에 있다면 (즉 발로 밟은 경우)
                if (contact.point.y < transform.position.y)
                {
                    Debug.Log("발로 AeriaGround 착지 -> 충돌 허용");
                    Physics2D.IgnoreCollision(col, otherCol, false);
                    return;
                }
            }

            // 만약 모두 위/옆 충돌이면
            Debug.Log("몸통/머리로 AeriaGround 충돌 -> 충돌 무시");
            Physics2D.IgnoreCollision(col, otherCol, true);
            
        }
    }

    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("AeriaGround"))
        {
            Collider2D otherCol = collision.collider;

            Physics2D.IgnoreCollision(col, otherCol, false);
            Debug.Log("충돌 허용 복구합니다.");
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FireBall"))
        {
            ChangeState(new HurtState_Player(this));
        }

        if (other.CompareTag("Ladder"))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                ChangeState(new ClimbingState_Player(this));
            }
        }

        if (other.CompareTag("LadderTop"))
        {
            isAtLadderTop = true;
        }

        if (other.CompareTag("StartTrigger"))
        {
            HandleStartTrigger();
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LadderTop"))
        {
            isAtLadderTop = false;
        }

    }

    void HandleStartTrigger()
    {
        isMoveable = false;
        moveX = 0;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            Vector2 knockbackDir = new Vector2(-1, 1).normalized; 
            float knockbackForce = 20f;
            rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        }

        CollisionOff();
    }

    void CollisionOff()
    {
        if (col != null) col.enabled = false;
    }

}