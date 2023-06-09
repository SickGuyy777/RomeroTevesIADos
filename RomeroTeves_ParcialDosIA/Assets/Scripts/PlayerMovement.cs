using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _movementSpeed;

    Vector3 _movedirection;
    private void Start()
    {
        GameManager.Instance.AddHuntter(this);
    }
    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        _movedirection = new Vector3(h, 0, v);
        transform.position += _movedirection * _movementSpeed * Time.deltaTime;
    }

    public Vector3 GetMySpeed()//para poder utilizar el vector del agente dentro del chasestate que es uno de los estados del Hunter
    {
        return _movedirection;
    }
}
