using Enemy.StateMachine;

public class EnemyBaseState
{
    public string name;
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(string name, EnemyStateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() {}
    
}
