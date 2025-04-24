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
        Debug.Log("IdleÀÇ Enter");
        player.SetActiveState("isIdle");
    }

    public void Update()
    {
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0)
            player.ChangeState(new RunState_Player(player));

        if (Input.GetKeyDown(KeyCode.UpArrow) && player.IsGrounded())
            player.ChangeState(new JumpState_Player(player));
    }

    public void Exit() 
    {

    }
}