public interface IState_Player
{
    void Enter();
    void Update();
    void FixedUpdate();
    void HandleInput(); // 입력 처리 
    void Exit();
}