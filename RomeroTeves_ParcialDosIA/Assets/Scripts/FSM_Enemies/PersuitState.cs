using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
