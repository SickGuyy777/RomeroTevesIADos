using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] List<Node> _neighborNode = new List<Node>();
}
