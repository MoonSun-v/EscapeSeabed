using UnityEngine;

public class JumpState_Player : IState_Player
{
    private PlayerFSM player;
    private float coyoteTime = 0.02f; // 점프 후 해당 시간동안 바닥 감지 무시
    private float fallTime = 0.5f;
    private float elapsedTime;
    private bool canDoubleJump;  

    public JumpState_Player(PlayerFSM player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("JumpState");
        player.SetActiveState("isJumping");
        player.Jump();
        canDoubleJump = true;  
        elapsedTime = 0f;
    }

    public void HandleInput()
    {
        bool jumpPressed = Input.GetKey(KeyCode.UpArrow);
        bool jumpJustPressed = Input.GetKeyDown(KeyCode.UpArrow);

        // [ 더블 점프 ]
        if (jumpJustPressed && !player.IsGrounded() && canDoubleJump)
        {
            player.Jump();
            canDoubleJump = false;
        }

        // [ 연속 점프 ]
        if (jumpPressed && player.IsGrounded() && elapsedTime >= coyoteTime)
        {
            player.ChangeState(new JumpState_Player(player));
        }

        if (Input.GetKeyDown(KeyCode.Space))
            player.ChangeState(new ShootState_Player(player));
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (player.IsGrounded() && player.GetVelocity().y <= 0.01f && elapsedTime >= coyoteTime)
        {
            player.ChangeState(new IdleState_Player(player));
        }

        if (player.IsTouchingWall() && elapsedTime > 0.3f)
        {
            player.Fall(); // 아래로 강제로 밀기
        }

        // bool movePressed = (Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetKeyDown(KeyCode.RightArrow));

        // 일정 시간 이상 계속 GetVelocity().y 의 변화가 없다면 아래로 힘 줘서 떨어뜨리기 
        /*
        if (!(player.IsGrounded()) && player.GetVelocity().y <= 0.01f && elapsedTime >= fallTime)
        {
            player.Fall();
        }
        */
    }


    public void Exit() 
    {
        
    }
}
