using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] List<Node> _neighborNode = new List<Node>();
    //posible modificacion
    int _cost = 1;
    public GraphConection _graph;
    Coordinates coordenadas;
    public int Cost { get { return _cost; } }
    //hasta aca
    public LayerMask wallMask;

    public void Initialize(GraphConection grid, int x, int y)
    {
        _graph = grid;
        coordenadas = new Coordinates(x, y);
        SetCost(1);
    }
    void SetCost(int cost)
    {
        _cost = cost < 1 ? 1 : cost;
        if (cost == 6) _cost = 5;
    }
    public List<Node> GetNeighbors()
    {
        if (_neighborNode.Count == 0)
        {
            _neighborNode = _graph.GetNeighborsAtPosition(coordenadas.x, coordenadas.y);
        }
        return _neighborNode;
    }

    private void OnDrawGizmos()
    {
        foreach (var item in _neighborNode)
        {
            Vector3 Dir = item.transform.position - transform.position;
            if (Physics.Raycast(transform.position, Dir, out RaycastHit hitInfo, Dir.magnitude, wallMask))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, hitInfo.point);
            }
            else
            {
                Gizmos.color = Color.white;

                Gizmos.DrawLine(transform.position, item.transform.position);
            }

        }
    }
}
struct Coordinates
{
    public Coordinates(int X, int Y)
    {
        x = X;
        y = Y;
    }
    public int x;
    public int y;

}
