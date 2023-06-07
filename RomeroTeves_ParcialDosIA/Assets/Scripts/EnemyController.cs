using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    FiniteStateMachine _fsm;
    Vector3 _velocity;

    [HideInInspector]
    public int currentPatrolNode;

    [SerializeField] float _maxSpeed;
    [SerializeField] float _maxForce;

    public Transform[] patrolNodes;
    public LayerMask wallMask;
    public float nodeRadius;
    public Color enemyNextNodeColor;

    public Vector3 VELOCITY { get => _velocity; }

    void Start()
    {
        _fsm = new FiniteStateMachine();
        _fsm.AddState(EnemyStates.Patrol, new PatrolState(this));
        _fsm.AddState(EnemyStates.Persuit, new PersuitState());
        _fsm.AddState(EnemyStates.LookingFor, new LookingForState());
        _fsm.AddState(EnemyStates.Return, new ReturnState());
        _fsm.ChangeState(EnemyStates.Patrol);
    }

    private void Update()
    {
        _fsm.Update();

        //CheckerUpdate();
    }

    public Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desired = (targetPos - transform.position).normalized * _maxSpeed;
        Vector3 steering = Vector3.ClampMagnitude(desired - _velocity, _maxForce);
        return steering;
    }

    public void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }

    private void OnDrawGizmos()
    {
        Vector3 dir = patrolNodes[currentPatrolNode].position - transform.position;

        if (Physics.Raycast(transform.position, dir, out RaycastHit hitInfo, dir.magnitude, wallMask))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, hitInfo.point);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, patrolNodes[currentPatrolNode].position);
        }

        Gizmos.color = enemyNextNodeColor;

        foreach (var item in patrolNodes)
        {
            Gizmos.DrawWireSphere(patrolNodes[currentPatrolNode].position, nodeRadius);
        }

    }
}

public enum EnemyStates
{
    Patrol,
    Persuit,
    LookingFor,
    Return
}
