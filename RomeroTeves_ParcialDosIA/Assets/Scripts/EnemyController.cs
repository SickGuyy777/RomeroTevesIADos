using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    FiniteStateMachine _fsm;

    void Start()
    {
        _fsm = new FiniteStateMachine();
        _fsm.AddState(EnemyStates.Patrol, new PatrolState());
        _fsm.AddState(EnemyStates.Persuit, new PersuitState());
        _fsm.AddState(EnemyStates.LookingFor, new LookingForState());
        _fsm.AddState(EnemyStates.Return, new ReturnState());
        _fsm.ChangeState(EnemyStates.Patrol);
    }

    private void Update()
    {
        _fsm.Update();
    }
}

public enum EnemyStates
{
    Patrol,
    Persuit,
    LookingFor,
    Return
}
