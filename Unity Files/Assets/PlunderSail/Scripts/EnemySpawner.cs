using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The distance from the spawner in metres that the spawner is aware of the player")]
    private float radius = 0;

    [SerializeField]
    [Tooltip("The number of enemy ships that can be active at the same time.")]
    private int enemyLimit = 0;

    [SerializeField]
    [Tooltip("The number of seconds after the player leaves the radius that the spawner stops.")]
    private float delayBeforeDeactivate = 0;

    private int activeShips = 0;

    [SerializeField]
    private List<SpawnPoint> spawnPoints = null;

    private bool towersActivated = false;

    [Header("Drag references into these")]
    [SerializeField]
    private Pier dock = null;

    [SerializeField]
    private GameObject islandCross = null;

    [SerializeField]
    private TowerBase[] towers = null;

    [SerializeField]
    private Transform[] enemyPrefabs = null;

    [SerializeField]
    private List<AIAgent> activeEnemies = null;

    [SerializeField]
    private bool unlocksTrident = false;

    [SerializeField]
    private bool unlocksDropBear = false;

    [SerializeField]
    private bool unlocksArmour = false;

    private Transform player = null;

    private GameManager gm;

    private bool cancelAttack = false;

    private bool attackingPlayer = false;

    private bool shouldHaveTowers = false;

    private int towerCount = 0;

    private bool isDefeated = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        shouldHaveTowers = (towers.Length > 0);

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        towerCount = towers.Length;

        if (shouldHaveTowers)
        {
            foreach (TowerBase tower in towers)
            {
                tower.OnDeath += EnemyDied;
            }
        }
    }

    private void Start()
    {
        activeEnemies = new List<AIAgent>(enemyLimit);

        StartCoroutine(CheckPlayerInRange());
    }

    private IEnumerator CheckPlayerInRange()
    {
        if (Vector3.Distance(player.position, transform.position) <= radius)
        {
            if (!attackingPlayer)
            {
                attackingPlayer = true;

                StartCoroutine(CheckEnemies());
            }

            if (!towersActivated)
            {
                ActivateTowers(true);
            }
        }
        else
        {
            attackingPlayer = false;
        }

        yield return new WaitForSeconds(1f);

        if (!isDefeated)
        {
            StartCoroutine(CheckPlayerInRange());
        }
    }

    // Checks if the number of active enemies is less than the limit, if so spawn an enemy.
    private IEnumerator CheckEnemies()
    {
        if (activeShips < enemyLimit && !GameState.BuildMode)
        {
            SpawnEnemy();
        }

        yield return new WaitForSeconds(1f);

        if (attackingPlayer)
        {
            StartCoroutine(CheckEnemies());
        }
    }

    private void ActivateTowers(bool _isActive)
    {
        for (int i = 0; i < towers.Length; i++)
        {
            if (towers[i] != null)
            {
                towers[i].gameObject.SetActive(_isActive);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0.7f, 0.6f, 1f);
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = new Color(1f, 0f, 0.5f, 1f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void SpawnEnemy()
    {
        // pick random spawn point until an open one is found.

        SpawnPoint point = null;

        if (spawnPoints.Count > 1)
        {
            point = spawnPoints[Random.Range(0, spawnPoints.Count)];

            if (!point.isOpen)
            {
                for (int i = 0; i < spawnPoints.Count; i++)
                {
                    if (spawnPoints[i].isOpen)
                    {
                        point = spawnPoints[i];
                    }
                }
            }
        }
        else
        {
            point = spawnPoints[0];
        }

        if (point != null)
        {
            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], point.transform.position, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)).gameObject;
            enemy.GetComponent<AIAgent>().OnDeath += shipDied;

            activeShips++;
         }
    }

    private void shipDied(LivingEntity _entity)
    {
        activeShips--;
    }

    private void EnemyDied(LivingEntity _entity)
    {
        Debug.Log("Tower Died");

        if (_entity.GetComponent<TowerBase>())
        {
            towerCount--;
        }

        if (towerCount == 0 && shouldHaveTowers && !isDefeated)
        {
            dock.isUnlocked = true;
            isDefeated = true;

            if (unlocksDropBear)
            {
                gm.UnlockDropBear();
            }

            if (unlocksTrident)
            {
                gm.UnlockTrident();
            }

            if (unlocksArmour)
            {
                gm.UnlockArmour();
            }

            if (islandCross != null)
            {
                gm.EncounterDefeated(islandCross);
            }
        }
    }

    Vector3 OutOfSightPos()
    {
        Vector3 spawnPoint = transform.position + GetPointOnUnitCircleCircumference();

        spawnPoint.y = 0f;

        return spawnPoint;
    }

    private Vector3 RandomPointOnUnitCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float z = Mathf.Cos(angle) * radius;

        return new Vector3(x, 0f, z);
    }

    private Vector3 GetPointOnUnitCircleCircumference()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        return new Vector3(Mathf.Sin(randomAngle), 0f, Mathf.Cos(randomAngle)) * radius;
    }
}