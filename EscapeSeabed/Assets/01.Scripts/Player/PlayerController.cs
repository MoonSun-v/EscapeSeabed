using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    private float moveSpeed = 4f;
    private float jumpForce = 15f;
    public LayerMask groundLayer; // 바닥 판정용 레이어

    private Rigidbody2D rb;
    private Collider2D col;

    Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>(); 
    }

    void Update()
    {
        // 방향키 입력
        movement.x = Input.GetAxisRaw("Horizontal"); // ←, →

        // 바닥 레이어와 충돌 중일 때만 점프
        if (Input.GetKeyDown(KeyCode.Space) && col.IsTouchingLayers(groundLayer))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // 좌우 이동 
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }

}
