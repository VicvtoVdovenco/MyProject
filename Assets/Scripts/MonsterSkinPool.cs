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



        MonsterSkin[] allMonsterSkins = Resources.LoadAll<MonsterSkin>("MonsterSkins");

        foreach (MonsterSkin monsterSkin in allMonsterSkins)
        {
            MonsterSkin monsterSkinGO = Instantiate(monsterSkin, monsterSkinsContainer.transform);
            monsterSkinGO.gameObject.SetActive(false);
            monsterSkinGO.gameObject.transform.SetParent(monsterSkinsContainer.transform);
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
        else
        {
            MonsterSkin prefab = Resources.Load<MonsterSkin>("MonsterSkins/" + monsterType.ToString());
            if (prefab != null)
            {
                monsterSkin = Instantiate(prefab, monsterSkinsContainer.transform);
                monsterSkin.gameObject.SetActive(false);
                monsterSkinsDict.Add(monsterType, monsterSkin);
            }
        }
        //monsterSkin = monsterSkinsFree.Pop();
        monsterSkin.gameObject.SetActive(true);

        return monsterSkin;
    }

    public void ReturnMonsterSkin(MonsterSkin monsterSkin)
    {
        monsterSkin.gameObject.transform.parent = monsterSkinsContainer.transform;
        monsterSkin.gameObject.SetActive(false);
        //monsterSkinsFree.Push(monsterSkin);
    }
}
