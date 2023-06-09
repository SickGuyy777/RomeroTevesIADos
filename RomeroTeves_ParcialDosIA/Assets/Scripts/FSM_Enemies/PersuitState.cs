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
        //Debug.Log("Se vio al jugador por ultima vez en: " + _lastPlayerPos);
    }

    public override void Update()
    {
        if (!_enemy.InFOV(_enemy.player.transform.position))
        {
            _enemy._alert = false;
            fsm.ChangeState(EnemyStates.Patrol);
        }
        else if(_enemy.InFOV(_enemy.player.transform.position))
        {
            _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position, _enemy.player.transform.position, _enemy.MAXSPEED * Time.deltaTime);
            _enemy.transform.forward = _enemy.player.transform.position - _enemy.transform.position;
            for (int i = 0; i < _enemy.Friends.Count; i++)
            {
                _enemy.Friends[i]._alert = true;
                _enemy._fsm.ChangeState(EnemyStates.Persuit);
            }
        }
        else
        {
            fsm.ChangeState(EnemyStates.Patrol);
        }
    }



    public override void OnExit()
    { }
}
