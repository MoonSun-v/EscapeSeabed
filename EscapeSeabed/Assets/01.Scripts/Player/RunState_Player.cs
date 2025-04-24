
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
        player.SetActiveState("isRunning");
    }

    public void Update()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 0)
            player.ChangeState(new IdleState_Player(player));

        if (Input.GetKeyDown(KeyCode.UpArrow) && player.IsGrounded())
            player.ChangeState(new JumpState_Player(player));
    }

    public void Exit()
    {
        
    }
}