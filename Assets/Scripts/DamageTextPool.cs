using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextPool : MonoBehaviour
{
    public static DamageTextPool instance;
    public DamageText damageText;
    public int poolSize = 10;

    private Stack<DamageText> damageTextAll = new Stack<DamageText>();
    private Stack<DamageText> damageTextFree = new Stack<DamageText>();

    private GameObject damageTextContainer;


    void Awake()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        damageTextContainer = new GameObject("DmgText Container");
        damageTextContainer.transform.SetParent(gameManager.transform);

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

    public DamageText GetDmgText()
    {
        DamageText dmgText = null;

        if (damageTextFree.Count == 0)
        {
            CreateInstance();
        }

        dmgText = damageTextFree.Pop();
        dmgText.gameObject.SetActive(true);

        return dmgText;
    }

    public void ReturnDmgText(DamageText dmgText)
    {
        dmgText.gameObject.SetActive(false);
        damageTextFree.Push(dmgText);
    }

    void CreateInstance()
    {
        DamageText dmgText = Instantiate(damageText);
        dmgText.transform.SetParent(damageTextContainer.transform);
        dmgText.gameObject.SetActive(false);
        damageTextAll.Push(dmgText);
        damageTextFree.Push(dmgText);
    }
}
