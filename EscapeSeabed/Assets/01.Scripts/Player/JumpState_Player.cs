using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class JumpState_Player : IState_Player
{
    private PlayerFSM player;
    private float coyoteTime = 0.01f; // ���� �� �� �ð����� �ٴ� ���� ����
    private float elapsedTime;

    public JumpState_Player(PlayerFSM player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("JumpState");
        player.SetActiveState("isJumping");
        player.Jump();
        elapsedTime = 0f;
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            player.ChangeState(new ShootState_Player(player));
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        // ��Ÿ�� ���� �ٴڿ� ��� �ϰ� ���� ���� Idle�� ��ȯ
        if (elapsedTime >= coyoteTime && player.IsGrounded() && player.GetVelocity().y <= 0.01f)
        {
            player.ChangeState(new IdleState_Player(player));
        }

    }

    public void Exit() 
    {
        
    }
}
