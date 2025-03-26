using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<GameObject> activeEnemies = new List<GameObject>();

    public void AddEnemy(GameObject enemy)
    {
        activeEnemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
            activeEnemies.Remove(enemy);
    }

    public bool AreAllEnemiesDead()
    {
        return activeEnemies.Count == 0;
    }
}
