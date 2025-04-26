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
        // 사다리에서 벗어나거나 맨 꼭대기/아래 도달하면
        if (!player.IsTouchingLadder())
        {
            player.ChangeState(new IdleState_Player(player));
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
        player.isClimbing = false;
        player.rb.gravityScale = 5; // 중력 다시 켜기
        player.SetActiveState(PlayerFSM.PlayerState.Idle); 
    }
}
