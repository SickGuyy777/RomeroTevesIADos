using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public List<Vector3> BreadthFirstSearch(Node start, Node goal)
    {
        Queue<Node> frontier = new Queue<Node>();
        frontier.Enqueue(start);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Node current = default;

        while(frontier.Count != 0)
        {
            current = frontier.Dequeue();

            if (current == goal) break;

            foreach (var next in current.GetNeighbors())
            {
                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);
                }
            }
        }

        List<Vector3> path = new List<Vector3>();
        if (current != goal) return path;

        while(current != null)
        {
            path.Add(current.transform.position);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }
}
