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

    private Vector2 movement;

    private float minX, maxX; // 카메라 경계 저장용

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        // [ 카메라 경계 계산 ]
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)); // 왼쪽 아래
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)); // 오른쪽 위 
        minX = min.x + col.bounds.extents.x;
        maxX = max.x - col.bounds.extents.x;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // ←, →

        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 디버그용 표시 
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius, isGrounded ? Color.green : Color.red);

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
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

}
