using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
//using SRandom = System.Random;
using URandom = UnityEngine.Random;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    [Serializable]
    public class SpawnWave
    {
        public MonsterType monsterType;
        public int numberOfMonsters;
        public float spawnOffset;
        public float initialOffset;
    }

    private Dictionary<MonsterType, SOMonsterStats> monsterStatsDict = new Dictionary<MonsterType, SOMonsterStats>();

    [SerializeField] public SpawnWave[] spawnWaves;
    [SerializeField] private GameObject[] monsterPrefabs;
    [SerializeField] private Transform[] spawnLocation;
    [SerializeField] Transform[] _destinations;

    private int _monsterCount = 0;
    public int MonsterCount => _monsterCount;

    public UnityEvent levelStarted = new UnityEvent();


    private void Awake()
    {
        SOMonsterStats[] allMonsterStats = Resources.LoadAll<SOMonsterStats>("SO/MonsterStats");

        foreach (SOMonsterStats monsterStats in allMonsterStats)
        {
            monsterStatsDict[monsterStats.MonsterType] = monsterStats;
        }

        for (int i = 0; i < spawnWaves.Length; i++)
        {
            _monsterCount += spawnWaves[i].numberOfMonsters;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (spawnWaves.Length > 0)
            {
                levelStarted.Invoke();
                for (int i = 0; i < spawnWaves.Length; i++)
                {
                    StartCoroutine(SpawnWaves(i));
                }
            }
        }
    }

    IEnumerator SpawnWaves(int i)
    {
        yield return new WaitForSeconds(spawnWaves[i].initialOffset);

        for (int j = 0; j < spawnWaves[i].numberOfMonsters; j++)
        {
            int spawnIndex = URandom.Range(0, spawnLocation.Length);

            Monster monster = MonsterPool.instance.GetMonster();

            if (monsterStatsDict.TryGetValue(spawnWaves[i].monsterType, out SOMonsterStats stats))
            {
                monster.monsterStats = stats;
            }

            monster.Reload();

            monster.gameObject.transform.position = spawnLocation[spawnIndex].position;
            monster.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);

            NavMeshAgent agent = monster.GetComponent<NavMeshAgent>();
            agent.enabled = true;

            NavAgentController agentCTRL = monster.GetComponent<NavAgentController>();
            agentCTRL.SetNavDestination(_destinations[spawnIndex].transform.position);            

            MonsterSkin monsterSkin = MonsterSkinPool.instance.GetMonsterSkin(monster.monsterStats.MonsterType);
            monsterSkin.transform.SetParent(monster.gameObject.transform);

            yield return new WaitForSeconds(spawnWaves[i].spawnOffset);
        }
    }
}



