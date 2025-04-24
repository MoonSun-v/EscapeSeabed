using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState_Player : IState_Player
{
    private PlayerFSM player;
    private float elapsedTime;

    public ShootState_Player(PlayerFSM player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("ShootState");
        player.SetActiveState(PlayerFSM.PlayerState.Shooting);
        player.Shoot();
    }

    public void HandleInput()
    {

    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.25) player.ChangeState(new IdleState_Player(player));

    }

    public void Exit()
    {

    }
}
