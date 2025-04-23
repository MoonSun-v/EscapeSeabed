using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    private float moveSpeed = 4f;
    private float jumpForce = 15f;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer; // �ٴ� ������ ���̾�

    [SerializeField] private Transform groundCheck;

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator animator;
    private SpriteRenderer sr;

    private Vector2 movement;

    private float minX, maxX; // ī�޶� ��� �����

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // [ ī�޶� ��� ��� ]
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)); // ���� �Ʒ�
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0)); // ������ �� 
        minX = min.x + col.bounds.extents.x;
        maxX = max.x - col.bounds.extents.x;
    }


    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal"); // ��, ��

        // [ ���� ���� ���� ����׿� ] : ���� ���� ���� 
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius, isGrounded ? Color.green : Color.red);

        // [ �¿� ���� ]
        if (movement.x != 0) sr.flipX = movement.x < 0;

        // [ ���� ]
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }

        // [ �ִϸ��̼� �Ķ���� ]
        bool isJumping = !isGrounded;
        bool isRunning = Mathf.Abs(movement.x) > 0.01f && isGrounded;
        bool isIdle = isGrounded && Mathf.Abs(movement.x) <= 0.01f;

        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isIdle", isIdle); 
    }


    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);  // �¿� �̵� 

        // [ ȭ�� ��� ]
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        transform.position = clampedPosition;
    }

}
