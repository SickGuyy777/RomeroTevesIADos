using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : States
{
    EnemyController _enemy;

    public PatrolState (EnemyController enemy)
    {
        _enemy = enemy;
    }

    public override void OnEnter()
    {
        Debug.Log("Entro a Patrullar");
    }

    public override void Update()
    {
        if (_enemy.InFOV(_enemy.player.transform.position))
        {
            _enemy._persuit = true;
            fsm.ChangeState(EnemyStates.Persuit);
        }

        else if (_enemy.InLOS(_enemy.transform.position, _enemy.patrolNodes[_enemy.currentPatrolNode].position))
            WaypointsMove();

        else
        {
            Debug.Log("No se encontro un Nodo"); //hacer A*
        }
    }

    void WaypointsMove()
    {
        _enemy.AddForce(_enemy.Seek(_enemy.patrolNodes[_enemy.currentPatrolNode].position));
        if (Vector3.Distance(_enemy.patrolNodes[_enemy.currentPatrolNode].position, _enemy.transform.position) <= _enemy.nodeRadius)
            _enemy.currentPatrolNode++;

        if (_enemy.currentPatrolNode >= _enemy.patrolNodes.Length)
            _enemy.currentPatrolNode = 0;

        _enemy.transform.position += _enemy.VELOCITY * Time.deltaTime;
        _enemy.transform.forward = _enemy.VELOCITY;
    }

    public override void OnExit()
    { }
}
