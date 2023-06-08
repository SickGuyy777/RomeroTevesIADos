using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] List<Node> _neighborNode = new List<Node>();

    public float distanceMagnitude;
    public LayerMask nodeMask;
    public LayerMask wallMask;

    public List<Node> GetNeighbors()
    {
        return _neighborNode;
    }

    private void OnDrawGizmos()
    {
        //if (Physics.Raycast(transform.position, transform.position.x, distanceMagnitude, nodeMask))
        //{
        //    Gizmos.color = Color.white;
        //    Gizmos.DrawLine
        //}
    }
}
