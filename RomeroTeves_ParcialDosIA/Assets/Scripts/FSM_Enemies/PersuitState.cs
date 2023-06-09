using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PersuitState : States
{
    EnemyController _enemy;
    Vector3 _lastPlayerPos;
    public PersuitState(EnemyController enemy)
    {
        _enemy = enemy;
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
            foreach (var item in _enemy.Friends)
            {
                item._alert = true;
            }
            //_enemy.MyForce(Evade());
            _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position, _enemy.player.transform.position, _enemy.MAXSPEED * Time.deltaTime);
            _enemy.transform.forward = _enemy.player.transform.position - _enemy.transform.position;
        }

        if(_enemy._alert == true)
        {
            //_enemy.MyForce(Evade());
            _enemy.transform.position = Vector3.MoveTowards(_enemy.transform.position, _enemy.player.transform.position, _enemy.MAXSPEED * Time.deltaTime);
            _enemy.transform.forward = _enemy.player.transform.position - _enemy.transform.position;
        }
    }

    //public Vector3 Evade()
    //{
    //    Vector3 Desired = Vector3.zero;
    //    foreach (var player in GameManager.Instance.Player)
    //    {
    //        Vector3 DistPl = player.transform.position - _enemy.transform.position;
    //        if (DistPl.magnitude <= _enemy.VIEWRANGE)
    //        {
    //            Vector3 ProxPos = player.transform.position + player.GetMySpeed() * Time.deltaTime;
    //            Desired = ProxPos - _enemy.transform.position;
    //        }
    //    }
    //    return _enemy.SteeringCalculate(Desired);
    //}

    public override void OnExit()
    { }
}
