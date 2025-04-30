using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 3.0f);
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Debug.Log("FireBall이 플레이어에게 타격을 입혔습니다!");

            Stop();
            animator.SetTrigger("FireBallBombTrigger");

            Destroy(gameObject, 0.5f);
        }
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
    }
}
