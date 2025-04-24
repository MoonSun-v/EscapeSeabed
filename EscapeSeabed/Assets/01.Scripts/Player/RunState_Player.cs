using UnityEngine;

public class RunState_Player : IState_Player
{
    private PlayerFSM player;

    public RunState_Player(PlayerFSM player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("RunState");
        player.SetActiveState("isRunning");
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            player.ChangeState(new ShootState_Player(player));

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 0)
            player.ChangeState(new IdleState_Player(player));

        if (Input.GetKeyDown(KeyCode.UpArrow) && player.IsGrounded())
            player.ChangeState(new JumpState_Player(player));
    }

    public void Update() 
    { 

    }

    public void Exit() 
    { 

    }
}