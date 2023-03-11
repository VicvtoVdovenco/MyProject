using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Playing, Victory, Defeat }

    private GameState currentState = GameState.Playing;

    Player player;

    [SerializeField] GameObject canvas;
    [SerializeField] GameObject victoryPrefab;
    [SerializeField] GameObject defeatPrefab;
    [SerializeField] Level level;

    private bool isGameEnded = false;
    private int monsterCount;
    private float victoryOffset = 2f;

    private void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        player = playerObject.GetComponent<Player>();
        monsterCount = level.MonsterCount;
        Monster.monsterKilled.AddListener(TrackVictory);
        Monster.monsterInPortal.AddListener(TrackVictory);
    }

    private void TrackVictory()
    {
        monsterCount--;
        if (monsterCount <= 0)
        {
            currentState = GameState.Victory;
        }
    }

    private void Update()
    {
        if (currentState == GameState.Playing)
        {
            Playing();
        }

        if (currentState == GameState.Victory)
        {
            Invoke(nameof(Victory), victoryOffset);
        }

        if (currentState == GameState.Defeat)
        {
            Defeat();
        }
    }

    private void Playing()
    {
        if (player.PlayerCurrentHealth <= 0)
        {
            currentState = GameState.Defeat;
        }
    }

    private void Victory()
    {
        if (!isGameEnded)
        {
            isGameEnded = true;
            GameObject victoryObject = Instantiate(victoryPrefab, Vector3.zero, Quaternion.identity);
            victoryObject.transform.localScale = Vector3.one;
            victoryObject.transform.SetParent(canvas.transform, false);
            Time.timeScale = 0;
        }

    }

    private void Defeat()
    {
        if (!isGameEnded)
        {
            isGameEnded = true;
            GameObject defeatObject = Instantiate(defeatPrefab, Vector3.zero, Quaternion.identity);
            defeatObject.transform.localScale = Vector3.one;
            defeatObject.transform.SetParent(canvas.transform, false);
            Time.timeScale = 0;
        }

    }

}

