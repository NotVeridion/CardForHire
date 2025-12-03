using UnityEngine;

public class BossStateMachine
{
    public BossState CurrentState { get; private set; }

    public void Initialize(BossState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(BossState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState.Tick();
    }

}