using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public List<EnemyController> Friends= new List<EnemyController>();
    public FiniteStateMachine _fsm;
    Vector3 _velocity;
    Pathfinding _pf = new Pathfinding();
    List<Vector3> _path = new List<Vector3>();

    [SerializeField] List<Node> _Path = new List<Node>();
    List<Vector3> pathfd=new List<Vector3>();
    public bool _persuit;
    public bool _alert;
    [HideInInspector]
    public int currentPatrolNode;

    [SerializeField] float _maxSpeed;
    [SerializeField] float _maxForce;
    [SerializeField] float _viewAngle;
    [SerializeField] float _viewRange;

    public Node[] patrolNodes;
    public LayerMask wallMask;
    public float nodeRadius;
    public Color enemyNextNodeColor;
    public PlayerMovement player;

    public Vector3 VELOCITY { get => _velocity; }
    public float VIEWANGLE { get => _viewAngle; }
    public float VIEWRANGE { get => _viewRange; }
    public float MAXSPEED { get => _maxSpeed; }
    public float MAXForce { get => _maxForce; }


    public string nameenemy;
    public string vdgfr;
    void Start()
    {
        _fsm = new FiniteStateMachine();
        _fsm.AddState(EnemyStates.Patrol, new PatrolState(this, _path));
        _fsm.AddState(EnemyStates.Persuit, new PersuitState(this, _path));
        _fsm.AddState(EnemyStates.LookingFor, new LookingForState());
        _fsm.ChangeState(EnemyStates.Patrol);
    }

    private void Update()
    {

        //Debug.Log("Soy " + nameenemy + " Mi nodo mas cercano es " + StartNode());
        //Debug.Log("El nodo Mas cercano a player es " + GoalNodePlayer());
        if (!_alert)
        {
            _fsm.Update();
        }
        else
        {
            if (pathfd.Count > 0)
            {
                TravelPath(pathfd);
            }
            else
            {
                pathfd = GetPathBasedOnPFTypePlayer();
                pathfd.Reverse();
                
            }
        }
        
    }

    //Posible Modificacion
    public List<Vector3> GetPathBasedOnPFTypePlayer()//esto hace el A* pero desde el pathfinding
    {
        return _pf.AStar(StartNode(), GoalNodePlayer());
    }
    public List<Vector3> GetPathBasedOnPFTypePatrol()
    {
        return _pf.AStar(StartNode(), patrolNodes[currentPatrolNode]);
    }
    public Node StartNode()
    {
        Node initialNode = null;
        float shortestDistance = float.MaxValue;
        foreach (Node node in _Path)
        {
            float distance = Vector3.Distance(transform.position, node.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                initialNode = node;
            }
        }

        return initialNode;
    }
    public Node GoalNodePlayer()
    {
        Node GoalNode = null;
        float shortestDistance = float.MaxValue;
        foreach (Node node in _Path)
        {
            float distance = Vector3.Distance(player.transform.position, node.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                GoalNode = node;
            }
        }

        return GoalNode;
    }
    public void TravelPath(List<Vector3> _path)
    {
            Vector3 target = _path[0] - Vector3.forward;
            Vector3 dir = target - transform.position;
            transform.forward = dir;
            transform.position += dir.normalized * MAXSPEED * Time.deltaTime;

            if (Vector3.Distance(target, transform.position) <= 0.1f) _path.RemoveAt(0);

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

        Vector3 dir = patrolNodes[currentPatrolNode].transform.position - transform.position;

        if (Physics.Raycast(transform.position, dir, out RaycastHit hitInfo, dir.magnitude, wallMask))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, hitInfo.point);
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
