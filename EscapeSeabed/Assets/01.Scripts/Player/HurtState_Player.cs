using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState_Player : IState_Player
{
    private PlayerFSM player;
    private float hurtDuration = 1.0f; // ���� ���� �ð�
    private float timer = 0f;
    // private bool knockbackApplied = false;

    public HurtState_Player(PlayerFSM player)
    {
        this.player = player;
    }


    public void Enter()
    {
        player.SetActiveState(PlayerFSM.PlayerState.Hurt);
        timer = 0f;
        // knockbackApplied = false;
    }

    public void HandleInput()
    {

    }

    public void Update()
    {
        timer += Time.deltaTime;

        /*
        if (!knockbackApplied)
        {
            ApplyKnockback();
            knockbackApplied = true;
        }
        */

        if (timer >= hurtDuration)
        {
            player.ChangeState(new IdleState_Player(player)); 
        }
    }

    public void Exit()
    {
        
    }

    /*
    private void ApplyKnockback()
    {
        Debug.Log("ƨ���?");

        // �÷��̾ ���� �ִ� ������ �ݴ� ���� ���
        float knockbackDirX = -Mathf.Sign(player.transform.localScale.x);

        // y�� 0 (���� �� Ƣ��)
        Vector2 force = new Vector2(knockbackDirX, 0f) * 50f; // ƨ��� ���� ����

        player.rb.velocity = Vector2.zero; // ���� �ӵ� ���� (�߿�!)
        player.rb.AddForce(force, ForceMode2D.Impulse);
    }
    */
}
