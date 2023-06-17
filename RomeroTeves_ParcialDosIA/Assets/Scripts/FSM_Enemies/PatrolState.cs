using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : States
{
    EnemyController _enemy;
    List<Vector3> _path = new List<Vector3>();
    public PatrolState (EnemyController enemy,List<Vector3> Path)
    {
        _enemy = enemy;
        _path = Path;
    }

    public override void OnEnter()
    {
        Debug.Log("Entro a Patrullar");
    }

    public override void Update()
    {
        if (_enemy.InFOV(_enemy.player.transform.position)||_enemy._alert==true)
        {
            _enemy._persuit = true;
            fsm.ChangeState(EnemyStates.Persuit);
        }
        else if (_enemy.InLOS(_enemy.transform.position, _enemy.patrolNodes[_enemy.currentPatrolNode].position))
        {
            WaypointsMove();
        }
        else
        {
            _path = _enemy.GetPathBasedOnPFTypePatrol();
            if (_path?.Count > 0) _path.Reverse();
            _enemy.GetPathBasedOnPFTypePatrol();
            if (_path.Count > 0)
            {
                TravelPath();
            }
        }
    }
    void TravelPath()
    {
        Vector3 target = _path[0] - Vector3.right;
        Vector3 dir = target - _enemy.transform.position;
        _enemy.transform.position += dir.normalized * _enemy.MAXSPEED * Time.deltaTime;

        if (Vector3.Distance(target, _enemy.transform.position) <= 0.1f) _path.RemoveAt(0);
    }

    void WaypointsMove()
    {
        foreach (var item in _enemy.Friends)
        {
            item._alert = false;
        }
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
