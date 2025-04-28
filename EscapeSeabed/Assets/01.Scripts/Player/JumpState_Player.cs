using UnityEngine;

public class JumpState_Player : IState_Player
{
    private PlayerFSM player;
    private float coyoteTime = 0.05f; // ���� �� �ش� �ð����� �ٴ� ���� ����
    private float elapsedTime;
    private bool canDoubleJump;  

    public JumpState_Player(PlayerFSM player)
    {
        this.player = player;
    }

    public void Enter()
    {
        // Debug.Log("JumpState");
        player.SetActiveState(PlayerFSM.PlayerState.Jumping);
        player.Jump();
        canDoubleJump = true;  
        elapsedTime = 0f;
    }

    public void HandleInput()
    {
        bool jumpPressed = Input.GetKey(KeyCode.UpArrow);
        bool jumpJustPressed = Input.GetKeyDown(KeyCode.UpArrow);

        // [ ���� ���� ]
        if (jumpJustPressed && !player.IsGrounded() && canDoubleJump)
        {
            player.Jump();
            canDoubleJump = false;
        }

        // [ ���� ���� ]
        if (jumpPressed && player.IsGrounded() && elapsedTime >= coyoteTime)
        {
            player.ChangeState(new JumpState_Player(player));
        }

        if (Input.GetKeyDown(KeyCode.Space) && (DataManager.instance.playerdata.AttackCount >= 1))
            player.ChangeState(new ShootState_Player(player));
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if ((player.IsGroundingLadder() && Input.GetKey(KeyCode.DownArrow)) || player.isAtLadderTop || player.IsTouchingLadder())
        {
            player.ChangeState(new ClimbingState_Player(player));
        }

        if (player.IsGrounded() && player.GetVelocity().y <= 0.01f && elapsedTime >= coyoteTime)
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
