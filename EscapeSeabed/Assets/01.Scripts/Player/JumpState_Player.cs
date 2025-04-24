using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class JumpState_Player : IState_Player
{
    private PlayerFSM player;
    private float coyoteTime = 0.01f; // 점프 후 이 시간동안 바닥 감지 무시
    private float elapsedTime;

    public JumpState_Player(PlayerFSM player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.SetActiveState("isJumping");
        player.Jump();
        elapsedTime = 0f;
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        // 쿨타임 이후 바닥에 닿고 하강 중일 때만 Idle로 전환
        if (elapsedTime >= coyoteTime && player.IsGrounded() && player.GetVelocity().y <= 0.01f)
        {
            player.ChangeState(new IdleState_Player(player));
        }
    }

    public void Exit() 
    {
        
    }
}
