using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState_Player : IState_Player
{
    private PlayerFSM player;
    private float hurtDuration = 1.0f; // 상태 유지 시간
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
        Debug.Log("튕기냐?");

        // 플레이어가 보고 있는 방향의 반대 방향 계산
        float knockbackDirX = -Mathf.Sign(player.transform.localScale.x);

        // y는 0 (위로 안 튀게)
        Vector2 force = new Vector2(knockbackDirX, 0f) * 50f; // 튕기는 세기 조절

        player.rb.velocity = Vector2.zero; // 기존 속도 제거 (중요!)
        player.rb.AddForce(force, ForceMode2D.Impulse);
    }
    */
}
