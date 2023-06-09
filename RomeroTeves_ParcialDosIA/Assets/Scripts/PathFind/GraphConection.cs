using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphConection : MonoBehaviour
{
    List<Node> _grid;
    [SerializeField] int _sizeX;
    [SerializeField] int _sizeY;
    public List<Node> GetNeighborsAtPosition(int x, int y)
    {
        List<Node> neighbors = new List<Node>();
        if (x + 1 < _sizeX) neighbors.Add(_grid[x + y * _sizeX + 1]);
        if (y - 1 >= 0) neighbors.Add(_grid[x + (y - 1) * _sizeX]);
        if (x - 1 >= 0) neighbors.Add(_grid[x - 1 + y * _sizeX]);
        if (y + 1 < _sizeY) neighbors.Add(_grid[x + (y + 1) * _sizeX]);
        return neighbors;
    }


}
