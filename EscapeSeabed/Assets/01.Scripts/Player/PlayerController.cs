using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    private float moveSpeed = 4f;
    private float jumpForce = 15f;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer; // 바닥 판정용 레이어

    [SerializeField] private Transform groundCheck;

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator animator;
    private SpriteRenderer sr;

    private Vector2 movement;

    private float minX, maxX; // 카메라 경계 저장용

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // [ 카메라 경계 계산 ]
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)); // 왼쪽 아래
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)); // 오른쪽 위 
        minX = min.x + col.bounds.extents.x;
        maxX = max.x - col.bounds.extents.x;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // ←, →

        // [ 점프 가능 여부 디버그용 ]
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius, isGrounded ? Color.green : Color.red);

        // [ 좌우 반전 ]
        if (movement.x != 0) sr.flipX = movement.x < 0;

        // [ 점프 ]
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            animator.SetTrigger("JumpTrigger");
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }

        
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);  // 좌우 이동 

        // 경계 체크 (카메라 밖으로 나가지 못하게)
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.position = clampedPosition;
    }
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
