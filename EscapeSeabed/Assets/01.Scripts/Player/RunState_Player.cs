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
        // Debug.Log("RunState");
        player.SetActiveState(PlayerFSM.PlayerState.Running);
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            player.ChangeState(new RunShootState_Player(player));

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 0)
            player.ChangeState(new IdleState_Player(player));

        if (Input.GetKeyDown(KeyCode.UpArrow) && player.IsGrounded())
            player.ChangeState(new JumpState_Player(player));
    }

    public void Update() 
    {
        if ((player.IsGroundingLadder() && Input.GetKey(KeyCode.DownArrow)) || player.isAtLadderTop)
        {
            player.ChangeState(new ClimbingState_Player(player));
        }
    }

    public void FixedUpdate()
    {

    }

    public void Exit() 
    { 

    }
}