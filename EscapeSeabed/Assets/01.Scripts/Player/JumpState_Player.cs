using UnityEngine;

public class JumpState_Player : IState_Player
{
    private PlayerFSM player;
    private float coyoteTime = 0.02f; // ���� �� �ش� �ð����� �ٴ� ���� ����
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
            player.Fall(); // �Ʒ��� ������ �б�
        }

        // bool movePressed = (Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetKeyDown(KeyCode.RightArrow));

        // ���� �ð� �̻� ��� GetVelocity().y �� ��ȭ�� ���ٸ� �Ʒ��� �� �༭ ����߸��� 
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
