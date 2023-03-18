using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager singleton;
    public List<Enemy> enemies;
    void Awake()
    {
        singleton = this;
        enemies = new List<Enemy>();
    }

}