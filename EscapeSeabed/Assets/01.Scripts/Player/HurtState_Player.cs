using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState_Player : IState_Player
{
    private PlayerFSM player;
    private float hurtDuration = 1.0f; // ���� ���� �ð�
    private float timer = 0f;

    public HurtState_Player(PlayerFSM player)
    {
        this.player = player;
    }


    public void Enter()
    {
        player.SetActiveState(PlayerFSM.PlayerState.Hurt);
        DataManager.instance.playerdata.HeartCount--;
        Debug.Log("HeartCount = " + DataManager.instance.playerdata.HeartCount);
        timer = 0f;

        // ���� ���� (���´� �� �������� ������ ��ӵ�)
        player.StartInvincibility();
    }

    public void HandleInput()
    {

    }

    public void Update()
    {
        timer += Time.deltaTime;

        if(DataManager.instance.playerdata.HeartCount<=0 )
        {
            SceneChange.instance.LoadOverScene();
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
