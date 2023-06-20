using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public WaitForSeconds time = new WaitForSeconds(0.01f);
    //hay que ver q hacemos con el A* en el foreach
    #region A* 
    //Posible Modificacion
    public List<Vector3> AStar(Node start, Node goal)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(start, 0);

        Node current = default;

        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            if (current == goal) break;

            foreach (var next in current._neighborNode)
            {
                int newCost = costSoFar[current] + next.Cost;

                if (!costSoFar.ContainsKey(next))
                {
                    costSoFar.Add(next, newCost);
                    frontier.Enqueue(next, newCost + Heuristic(next.transform.position, goal.transform.position));
                    cameFrom.Add(next, current);
                }
                else if (newCost < costSoFar[current])
                {
                    frontier.Enqueue(next, newCost + Heuristic(next.transform.position, goal.transform.position));
                    costSoFar[next] = newCost;
                    cameFrom[next] = current;
                }
            }
        }
        List<Vector3> path = new List<Vector3>();
        if (current != goal) return path;

        while (current != null  /*antes era start*/)
        {
            path.Add(current.transform.position);
            current = cameFrom[current];
        }
        path.Add(start.transform.position);
        return path;
    }
    #endregion
    float Heuristic(Vector3 start, Vector3 end)
    {
        return Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y);
    }
    //hasta aca
}
