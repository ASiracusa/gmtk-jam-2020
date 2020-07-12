using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private const float MIN_SPAWN_DISTANCE = 8;
    private const float MAX_SPAWN_DISTANCE = 12;
    private const float REMOVAL_DISTANCE = 14;

    public string[] enemies;

    private GameObject enemy;

    void Start()
    {
        StartCoroutine(DetectPlayer());
    }

    private IEnumerator DetectPlayer()
    {
        GameObject player = PlayerManager.current.gameObject;
        while (true)
        {
            if (enemy == null && Vector3.Distance(player.transform.position, transform.position) < MAX_SPAWN_DISTANCE && Vector3.Distance(player.transform.position, transform.position) > MIN_SPAWN_DISTANCE) {
                // spawn
                enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Creature"), transform.position, Quaternion.identity) as GameObject;
                enemy.GetComponent<Creature>().AssignCreatureType(enemies[(int)Random.Range(0, enemies.Length)], this);
            }
            else if (enemy != null && Vector3.Distance(player.transform.position, enemy.transform.position) > REMOVAL_DISTANCE)
            {
                Constants.CompletelyDestroy(enemy);
                enemy = null;
            }
            yield return null;
        }
    }

    public void RemoveEnemy()
    {
        enemy = null;
    }
}
