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
        player.rb.gravityScale = 0; // �߷� ����
        player.rb.velocity = Vector2.zero; 
        player.SetActiveState(PlayerFSM.PlayerState.Climbing);

        Debug.Log("Climbing");
    }

    public void HandleInput()
    {
        // �ƹ��͵� ���ص� ��
    }

    public void Update()
    {
        // ��ٸ����� ����ų� �� �����/�Ʒ� �����ϸ�
        if (!player.IsTouchingLadder())
        {
            player.ChangeState(new IdleState_Player(player));
        }
    }

    public void FixedUpdate()
    {
        float vInput = Input.GetAxisRaw("Vertical"); // ���Ʒ� �Է� �ޱ�

        player.rb.velocity = new Vector2(0, vInput * player.climbSpeed);

        if (Mathf.Abs(vInput) < 0.01f)
        {
            player.rb.velocity = Vector2.zero;
        }
    }

    public void Exit()
    {
        player.isClimbing = false;
        player.rb.gravityScale = 5; // �߷� �ٽ� �ѱ�
        player.SetActiveState(PlayerFSM.PlayerState.Idle); 
    }
}
