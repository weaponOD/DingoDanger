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
                CallToArms();
            }
        }
        else
        {
            if (!cancelAttack)
            {
                cancelAttack = true;

                Invoke("CancelArms", delayBeforeDeactivate);
            }
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
        if (activeEnemies.Count < enemyLimit && !GameState.BuildMode)
        {
            SpawnEnemy();
        }

        yield return new WaitForSeconds(0.5f);

        if (attackingPlayer)
        {
            StartCoroutine(CheckEnemies());
        }
    }

    private void CallToArms()
    {
        cancelAttack = false;

        AudioManager.instance.PlaySound("BattleTheme");

        if(!AudioManager.instance.isBattleMusicPlaying)
        {
            AudioManager.instance.PlaySound("BattleLoop");
            AudioManager.instance.isBattleMusicPlaying = true;
        }

        foreach (TowerBase tower in towers)
        {
            tower.enabled = true;
        }

        StartCoroutine(CheckEnemies());
    }

    private void CancelArms()
    {
        if (Vector3.Distance(player.position, transform.position) > radius || isDefeated)
        {
            attackingPlayer = false;

            foreach (TowerBase tower in towers)
            {
                if (tower != null)
                {
                    tower.enabled = false;
                }
            }

            foreach (AIAgent ship in activeEnemies)
            {
                if (ship != null)
                {
                    ship.gameObject.SetActive(false);
                }
            }

            AudioManager.instance.FadeOut("BattleLoop", 1f);
            AudioManager.instance.isBattleMusicPlaying = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0.7f, 0.6f, 1f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void SpawnEnemy()
    {
        Vector3 spawnPos = OutOfSightPos();

        if ((Physics2D.OverlapCircle(spawnPos, 100f)) == null && Vector3.Distance(player.transform.position, spawnPos) > 5)
        {
            activeEnemies.Add(Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPos, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)).GetComponent<AIAgent>());
        }
        else
        {
            SpawnEnemy();
        }
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

            if (islandCross != null)
            {
                gm.EncounterDefeated(islandCross);
                //islandCross.SetActive(true);
            }

            CancelArms();
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