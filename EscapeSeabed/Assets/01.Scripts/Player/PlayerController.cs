using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    private float moveSpeed = 4f;
    private float jumpForce = 15f;
    public LayerMask groundLayer; // �ٴ� ������ ���̾�

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
        // ����Ű �Է�
        movement.x = Input.GetAxisRaw("Horizontal"); // ��, ��

        // �ٴ� ���̾�� �浹 ���� ���� ����
        if (Input.GetKeyDown(KeyCode.Space) && col.IsTouchingLayers(groundLayer))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // �¿� �̵� 
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);
    }

}
