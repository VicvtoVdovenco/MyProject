using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkinPool : MonoBehaviour
{
    public static MonsterSkinPool instance;

    private Stack<MonsterSkin> monsterSkinsAll = new Stack<MonsterSkin>();
    private Stack<MonsterSkin> monsterSkinsFree = new Stack<MonsterSkin>();

    private GameObject monsterSkinsContainer;    

    private Dictionary<MonsterType, MonsterSkin> monsterSkinsDict = new Dictionary<MonsterType, MonsterSkin>();

    void Awake()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        monsterSkinsContainer = new GameObject("MonsterSkin Container");
        monsterSkinsContainer.transform.SetParent(gameManager.transform);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //for (int i = 0; i < poolSize; i++)
        //{
        //    CreateInstance();
        //}

        MonsterSkin[] allMonsterSkins = Resources.LoadAll<MonsterSkin>("MonsterSkins");

        foreach (MonsterSkin monsterSkin in allMonsterSkins)
        {
            monsterSkinsDict.Add(monsterSkin.MonsterType, monsterSkin);
        }

    }

    public MonsterSkin GetMonsterSkin(MonsterType monsterType)
    {
        MonsterSkin monsterSkin = null;

        if (monsterSkinsDict.TryGetValue(monsterType, out MonsterSkin skin))
        {
            monsterSkin = skin;
        }


        //if (monsterSkinsFree.Count == 0)
        //{
        //    CreateInstance();
        //}

        //monsterSkin = monsterSkinsFree.Pop();
        monsterSkin.gameObject.SetActive(true);

        return monsterSkin;
    }

    public void ReturnMonsterSkin(MonsterSkin monsterSkin)
    {
        monsterSkin.gameObject.SetActive(false);
        //monsterSkinsFree.Push(monsterSkin);
    }

    //void CreateInstance()
    //{
    //    MonsterSkin skin = Instantiate(monsterSkin);
    //    skin.transform.SetParent(monsterContainer.transform);
    //    skin.gameObject.SetActive(false);
    //    monstersAll.Push(skin);
    //    monstersFree.Push(skin);
    //}
}
