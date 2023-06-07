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
        if (InLOS(_enemy.transform.position, _enemy.patrolNodes[_enemy.currentPatrolNode].position))
        {
            WaypointsMove();
        }
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

    bool InLOS(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, _enemy.wallMask);
    }

    public override void OnExit()
    {
    }
}
