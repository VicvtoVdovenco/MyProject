using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameCanvas : MonoBehaviour
{
    public static GameCanvas instance;

    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] Level level;
    private int monsterCount; 

    public AnimationCurve dmgPosCurve;
    public AnimationCurve dmgScaleCurve;
    public AnimationCurve dmgAlphaCurve;
    public AnimationCurve critScaleCurve;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        counterText.text = null;        
        Monster.monsterKilled.AddListener(MonsterCount);
        Monster.monsterInPortal.AddListener(MonsterCount);
        monsterCount = level.MonsterCount;
        level.levelStarted.AddListener(StartMonsterCounter);

    }

    private void Update()
    {

    }

    private void MonsterCount()
    {
        monsterCount--;
        counterText.text = "Enemies left: " + monsterCount.ToString();   
    }

    private void StartMonsterCounter()
    {
        counterText.text = "Enemies left: " + monsterCount.ToString();
    }
}
