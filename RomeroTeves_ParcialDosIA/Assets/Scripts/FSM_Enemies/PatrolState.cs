using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : States
{
    public override void OnEnter()
    {
    }

    public override void Update()
    {
        Debug.Log("Enemy patrol");
        //hacer funcion de perseguir al enemigo
    }

    public override void OnExit()
    {

    }
}
