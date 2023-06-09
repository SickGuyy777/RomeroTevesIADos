using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public List<EnemyController> Friends= new List<EnemyController>();
    FiniteStateMachine _fsm;
    Vector3 _velocity;
    Pathfinding _pf = new Pathfinding();
    List<Vector3> _path = new List<Vector3>();
    Node _start;
    Node _goal;

    public bool _persuit;
    public bool _alert;
    [HideInInspector]
    public int currentPatrolNode;

    [SerializeField] float _maxSpeed;
    [SerializeField] float _maxForce;
    [SerializeField] float _viewAngle;
    [SerializeField] float _viewRange;

    public Transform[] patrolNodes;
    public LayerMask wallMask;
    public float nodeRadius;
    public Color enemyNextNodeColor;
    public PlayerMovement player;

    public Vector3 VELOCITY { get => _velocity; }
    public float VIEWANGLE { get => _viewAngle; }
    public float VIEWRANGE { get => _viewRange; }
    public float MAXSPEED { get => _maxSpeed; }

    void Start()
    {
        _fsm = new FiniteStateMachine();
        _fsm.AddState(EnemyStates.Patrol, new PatrolState(this));
        _fsm.AddState(EnemyStates.Persuit, new PersuitState(this));
        _fsm.AddState(EnemyStates.LookingFor, new LookingForState());
        _fsm.AddState(EnemyStates.Return, new ReturnState());
        _fsm.ChangeState(EnemyStates.Patrol);
    }

    private void Update()
    {
        _fsm.Update();
    }

    //esto lo saque de mi code del parcial anterior
    //public Vector3 SteeringCalculate(Vector3 Desired)
    //{
    //    return Vector3.ClampMagnitude(Desired.normalized * MAXSPEED - _velocity, MAXSPEED);
    //}
    //public void MyForce(Vector3 force)
    //{
    //    _velocity += force;
    //    if (_velocity.magnitude >= MAXSPEED)
    //    {
    //        _velocity = _velocity.normalized * MAXSPEED;
    //    }
    //}
    //esto lo estoy haciendo en base a lo del profe
    List<Vector3> GetPathBasedOnPFType()//esto hace el A* pero desde el pathfinding
    {
        return _pf.AStar(_start, _goal);
    }
    //hasta aca de ultima se borra
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

    public bool InLOS(Vector3 start, Vector3 end)
    {
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, wallMask);
    }

    public bool InFOV(Vector3 endPos)
    {
        Vector3 dir = endPos - transform.position;
        if (dir.magnitude > _viewRange) return false;
        if (Vector3.Angle(transform.forward, dir) > _viewAngle / 2) return false;
        if (!InLOS(transform.position, endPos)) return false;

        return true;
    }

    //Intento de A* en el #region

    #region
    void CheckerUpdate()
    {
        _path = _pf.BreadthFirstSearch(_start, _goal);

        if(_path.Count > 0)
        {
            TravelTo();
        }
    }

    void TravelTo()
    {
        Vector3 dir = _path[0] - transform.position;
        transform.position += dir.normalized * _maxSpeed * Time.deltaTime;
    }
    #endregion

    Vector3 GetAngleFromDir(float angleInDegree)
    {
        return new Vector3(Mathf.Sin(angleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegree * Mathf.Deg2Rad));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = enemyNextNodeColor;
        Gizmos.DrawWireSphere(transform.position, _viewRange);

        Vector3 dirA = GetAngleFromDir(_viewAngle / 2 + transform.eulerAngles.y);
        Vector3 dirB = GetAngleFromDir(-_viewAngle / 2 + transform.eulerAngles.y);

        Gizmos.DrawLine(transform.position, transform.position + dirA.normalized * _viewRange);
        Gizmos.DrawLine(transform.position, transform.position + dirB.normalized * _viewRange);

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

    //comentario de mi suposicion de lo que nos falta por hacer:
    //-tenemos que tener un startingNode y GoalNode dentro del enemycontroler y que estos se seten en base a lo que necesitemos en este caso starting node seria el nodo mas
    //cercano al enemy y el goal node seria el nodo al que queremos llegar (no se aun como se podria hacer aun)
    //-una vez tengamos eso lo que tenemos que hacer es cual es el starting node
}

public enum EnemyStates
{
    Patrol,
    Persuit,
    LookingFor,
    Return
}
