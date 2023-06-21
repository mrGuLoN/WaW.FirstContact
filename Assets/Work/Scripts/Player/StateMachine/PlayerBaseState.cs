using Player.StateMachine;

public class PlayerBaseState
{
    public string name;
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(string name, PlayerStateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() {}
    
}
