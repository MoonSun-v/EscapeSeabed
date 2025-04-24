using UnityEngine;

public class RunShootState_Player : IState_Player
{
    private PlayerFSM player;
    private float elapsedTime;

    public RunShootState_Player(PlayerFSM player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("RunShootState");
        player.SetActiveState(PlayerFSM.PlayerState.RunShooting);
        player.Shoot();
    }

    public void HandleInput()
    {

    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > 0.5) player.ChangeState(new RunState_Player(player));
    }

    public void Exit()
    {

    }
}