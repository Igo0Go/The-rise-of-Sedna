using UnityEngine;
using System.Collections.Generic;

public class EnemyGrouoActor : Actor
{
    [SerializeField]
    private List<EnemyBase> enemies;

    private void Awake()
    {
        foreach (EnemyBase enemy in enemies)
        {
            enemy.deadEvent.AddListener(OnEnemyDead);
        }
    }

    private void OnEnemyDead(EnemyBase enemy)
    {
        enemy.deadEvent.RemoveListener(OnEnemyDead);
        enemies.Remove(enemy);
        if(enemies.Count == 0)
        {
            Activate();
        }
    }
}
