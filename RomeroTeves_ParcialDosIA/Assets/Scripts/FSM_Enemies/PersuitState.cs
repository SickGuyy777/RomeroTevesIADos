using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PersuitState : States
{
    EnemyController _enemy;
    Vector3 _lastPlayerPos;
    List<Vector3> _path = new List<Vector3>();
    public PersuitState(EnemyController enemy,List<Vector3> waypath)
    {
        _enemy = enemy;
        _path = waypath;
    }

    public override void OnEnter()
    {
        Debug.Log("Enemy persuit");

        _lastPlayerPos = _enemy.player.transform.position;
        Debug.Log("Se vio al jugador por ultima vez en: " + _lastPlayerPos);
    }

    public override void Update()
    {
        if (!_enemy.InFOV(_enemy.player.transform.position))
        { 
            fsm.ChangeState(EnemyStates.Patrol);
        }
        else if(_enemy.InFOV(_enemy.player.transform.position))
        {
            _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position, _enemy.player.transform.position, _enemy.MAXSPEED * Time.deltaTime);
            _enemy.transform.forward = _enemy.player.transform.position - _enemy.transform.position;
            foreach (var item in _enemy.Friends)
            {
                item._alert = true;
            }
            _path = _enemy.GetPathBasedOnPFTypePlayer();
            if (_path?.Count > 0) _path.Reverse();
            _enemy.GetPathBasedOnPFTypePlayer();
            if (_path.Count > 0)
            {
                TravelPath();
            }
        }

        if(_enemy._alert == true)
        {
            _path = _enemy.GetPathBasedOnPFTypePlayer();
            if (_path?.Count > 0) _path.Reverse();
            _enemy.GetPathBasedOnPFTypePlayer();
            if (_path.Count > 0)
            {
                TravelPath();
                if(!_enemy.InFOV(_enemy.player.transform.position))
                {
                    fsm.ChangeState(EnemyStates.Patrol);
                }
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



    public override void OnExit()
    { }
}
