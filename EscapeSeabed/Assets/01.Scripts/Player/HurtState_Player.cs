using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState_Player : IState_Player
{
    private PlayerFSM player;
    private float hurtDuration = 1.0f; // 상태 유지 시간
    private float timer = 0f;

    public HurtState_Player(PlayerFSM player)
    {
        this.player = player;
    }


    public void Enter()
    {
        player.SetActiveState(PlayerFSM.PlayerState.Hurt);
        DataManager.instance.playerdata.HeartCount--;
        timer = 0f;
    }

    public void HandleInput()
    {

    }

    public void Update()
    {
        timer += Time.deltaTime;

        if(DataManager.instance.playerdata.HeartCount<=0 )
        {
            Debug.Log("게임 오버");
        }

        if (timer >= hurtDuration)
        {
            player.ChangeState(new IdleState_Player(player)); 
        }
    }

    public void FixedUpdate()
    {

    }

    public void Exit()
    {
        
    }

}
