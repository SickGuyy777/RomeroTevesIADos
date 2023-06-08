using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersuitState : States
{
    EnemyController _enemy;

    public PersuitState(EnemyController enemy)
    {
        _enemy = enemy;
    }

    public override void OnEnter()
    {
        Debug.Log("Enemy persuit");
    }

    public override void Update()
    {
        if (!_enemy.InFOV(_enemy.player.transform.position))
            fsm.ChangeState(EnemyStates.Patrol);

        else
        {
            _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position, _enemy.player.transform.position, _enemy.MAXSPEED * Time.deltaTime);
            _enemy.transform.forward = _enemy.player.transform.position - _enemy.transform.position;
        }
    }

    public override void OnExit()
    { }
}
