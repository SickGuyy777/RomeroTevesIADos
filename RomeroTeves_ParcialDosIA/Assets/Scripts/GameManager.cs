using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<EnemyController> Hunters = new List<EnemyController>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddHuntter(EnemyController Hunt)
    {
        if (!Hunters.Contains(Hunt))
        {
            Hunters.Add(Hunt);
        }
    }
}
