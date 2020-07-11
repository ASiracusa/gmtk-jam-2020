using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private const float SPAWN_DISTANCE = 10;
    private const float REMOVAL_DISTANCE = 15;

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
            if (enemy == null && Vector3.Distance(player.transform.position, transform.position) < SPAWN_DISTANCE) {
                // spawn
                GameObject creature = Instantiate(Resources.Load<GameObject>("Prefabs/Creature")) as GameObject;
                CreatureData cd = Resources.Load<CreatureData>("Data/Creatures/" + enemies[(int)Random.Range(0, enemies.Length)]);
            }
            else if (enemy != null && Vector3.Distance(player.transform.position, transform.position) < REMOVAL_DISTANCE && Vector3.Distance(player.transform.position, enemy.transform.position) < SPAWN_DISTANCE)
            {
                Destroy(enemy);
                enemy = null;
            }
            yield return null;
        }
    }
}
