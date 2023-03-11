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
    public enum MonsterType
    {
        CapsuleMonster,
        SphereMonster,
        CubeMonster
    }

    [Serializable]
    public class SpawnWave
    {
        public MonsterType monsterType;
        public int numberOfMonsters;
        public float spawnOffset;
        public float initialOffset;
    }

    [SerializeField] public SpawnWave[] spawnWaves;
    [SerializeField] private GameObject[] monsterPrefabs;
    [SerializeField] private Transform[] spawnLocation;
    [SerializeField] Transform[] _destinations;

    private int _monsterCount = 0;
    public int MonsterCount => _monsterCount;

    public UnityEvent levelStarted = new UnityEvent();

    private void Awake()
    {
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
            GameObject monsterPrefab = monsterPrefabs[(int)spawnWaves[i].monsterType];
            GameObject monster = Instantiate(monsterPrefab, spawnLocation[spawnIndex].position, Quaternion.Euler(0, 180f, 0));
            NavAgentController agentScript = monster.GetComponent<NavAgentController>();
            agentScript.agentDestination = _destinations[spawnIndex].transform;
            yield return new WaitForSeconds(spawnWaves[i].spawnOffset);
        }
    }

}



