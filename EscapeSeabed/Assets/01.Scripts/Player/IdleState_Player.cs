using UnityEngine;

public class IdleState_Player : IState_Player
{
    private PlayerFSM player;

    public IdleState_Player(PlayerFSM player)
    {
        this.player = player;
    }

    public void Enter()
    {
        // Debug.Log("IdleState");
        player.SetActiveState(PlayerFSM.PlayerState.Idle);
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (DataManager.instance.playerdata.AttackCount >= 1))
            player.ChangeState(new ShootState_Player(player));

        if (Input.GetKeyDown(KeyCode.Space) && (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0))
            player.ChangeState(new RunShootState_Player(player));

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
            player.ChangeState(new RunState_Player(player));

        if (Input.GetKeyDown(KeyCode.UpArrow) && player.IsGrounded() )
            player.ChangeState(new JumpState_Player(player));
    }

    public void Update()
    {
        if ((player.IsGroundingLadder()&& Input.GetKey(KeyCode.DownArrow)) || player.isAtLadderTop)
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