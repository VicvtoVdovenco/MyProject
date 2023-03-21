using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterPool : MonoBehaviour
{
    public static MonsterPool instance;
    public Monster monsterPrefab;

    private int poolSize = 20;
    private Stack<Monster> monstersAll = new Stack<Monster>();
    private Stack<Monster> monstersFree = new Stack<Monster>();

    private GameObject monsterContainer;

    void Awake()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        monsterContainer = new GameObject("Monster Container");
        monsterContainer.transform.SetParent(gameManager.transform);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            CreateInstance();
        }
    }

    public Monster GetMonster()
    {
        Monster monster = null;

        if (monstersFree.Count == 0)
        {
            CreateInstance();
        }

        monster = monstersFree.Pop();
        monster.gameObject.SetActive(true);

        return monster;
    }

    public void ReturnMonster(Monster monster)
    {

        monster.gameObject.SetActive(false);
        monstersFree.Push(monster);
    }

    void CreateInstance()
    {
        Monster monster = Instantiate(monsterPrefab);
        monster.transform.SetParent(monsterContainer.transform);
        monster.transform.position = Vector3.zero;
        monster.gameObject.SetActive(false);        
        monstersAll.Push(monster);
        monstersFree.Push(monster);
    }

    public List<Monster> GetActiveMonsters()
    {
        List<Monster> activeMonsters = new List<Monster>();

        foreach (Monster monster in monstersAll)
        {
            if (monster.gameObject.activeInHierarchy)
            {
                activeMonsters.Add(monster);
            }
        }

        return activeMonsters;
    }
}
