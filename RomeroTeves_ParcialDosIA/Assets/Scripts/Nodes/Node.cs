using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> _neighborNode = new List<Node>();
    //posible modificacion
    int _cost = 1;
    public int Cost { get { return _cost; } }
    //hasta aca
    public LayerMask wallMask;
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
