using System.Collections;
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
    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
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

    public float moveX = 0f; // 좌우 
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
        DataManager.instance.playerdata.AttackCount--;

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
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheckLeft.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckRight.position, groundCheckRadius);

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

        // 왼발과 오른발 위치에서 각각 땅 체크
        bool isLeftFootOnGround = Physics2D.OverlapCircle(groundCheckLeft.position, groundCheckRadius, groundLayer);
        bool isRightFootOnGround = Physics2D.OverlapCircle(groundCheckRight.position, groundCheckRadius, groundLayer);

        // 왼발이나 오른발이 땅에 닿으면 땅 위로 간주
        return isLeftFootOnGround || isRightFootOnGround;

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
        // 왼발과 오른발 위치에서 각각 땅 체크
        bool isLeftFootOnGround = Physics2D.OverlapCircle(groundCheckLeft.position, ladderCheckRadius, ladderLayer);
        bool isRightFootOnGround = Physics2D.OverlapCircle(groundCheckRight.position, ladderCheckRadius, ladderLayer);

        // 왼발이나 오른발이 땅에 닿으면 땅 위로 간주
        return isLeftFootOnGround || isRightFootOnGround;
    }

    // [ 충돌 체크 ]
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            ChangeState(new HurtState_Player(this));
        }

        if (collision.gameObject.CompareTag("Lava"))
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
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LadderTop"))
        {
            isAtLadderTop = false;
        }
    }

    

    public void CollisionOff()
    {
        if (col != null) col.enabled = false;
    }

}