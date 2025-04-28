using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingState_Player : IState_Player
{
    private PlayerFSM player;

    public ClimbingState_Player(PlayerFSM player)
    {
        this.player = player;
    }

    public void Enter()
    {
        // 사다리 타기 시작할 때 (플레이어가 사다리 타기 상태로 전환될 때)
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("LadderTop"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), true);

        player.isClimbing = true;
        player.rb.gravityScale = 0; // 중력 제거
        player.rb.velocity = Vector2.zero; 
        player.SetActiveState(PlayerFSM.PlayerState.Climbing);

        Debug.Log("Climbing");
    }

    public void HandleInput()
    {
        // 아무것도 안해도 됨
    }

    public void Update()
    {
        if (!player.IsTouchingLadder())
        {
            if (player.isAtLadderTop)
            {
                // 사다리 꼭대기 도착했으면 땅 위에 서도록
                player.ChangeState(new IdleState_Player(player));
                player.rb.gravityScale = 5; // 다시 중력 적용
                player.rb.velocity = Vector2.zero; // 멈추기
            }
            else
            {
                // 사다리 벗어나면 그냥 Idle
                player.ChangeState(new IdleState_Player(player));
            }
        }
    }

    public void FixedUpdate()
    {
        float vInput = Input.GetAxisRaw("Vertical"); // 위아래 입력 받기

        player.rb.velocity = new Vector2(0, vInput * player.climbSpeed);

        if (Mathf.Abs(vInput) < 0.01f)
        {
            player.rb.velocity = Vector2.zero;
        }
    }

    public void Exit()
    {
        // 사다리 타기 끝날 때 (사다리 다 올라갔거나 내려왔을 때)
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("LadderTop"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), false);

        player.isClimbing = false;
        player.rb.gravityScale = 5; // 중력 다시 켜기

    }
}
