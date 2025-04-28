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
        // ��ٸ� Ÿ�� ������ �� (�÷��̾ ��ٸ� Ÿ�� ���·� ��ȯ�� ��)
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("LadderTop"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), true);

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
        if (!player.IsTouchingLadder())
        {
            if (player.isAtLadderTop)
            {
                // ��ٸ� ����� ���������� �� ���� ������
                player.ChangeState(new IdleState_Player(player));
                player.rb.gravityScale = 5; // �ٽ� �߷� ����
                player.rb.velocity = Vector2.zero; // ���߱�
            }
            else
            {
                // ��ٸ� ����� �׳� Idle
                player.ChangeState(new IdleState_Player(player));
            }
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
        // ��ٸ� Ÿ�� ���� �� (��ٸ� �� �ö󰬰ų� �������� ��)
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("LadderTop"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), false);

        player.isClimbing = false;
        player.rb.gravityScale = 5; // �߷� �ٽ� �ѱ�

    }
}
