using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.AI;
using TMPro;

public class Monster : MonoBehaviour
{
    public static UnityEvent<float> monsterDealsDamageToPlayer = new UnityEvent<float>();
    public static UnityEvent monsterInPortal = new UnityEvent();
    public static UnityEvent monsterKilled = new UnityEvent();
    public SOMonsterStats monsterStats;
    public MonsterSkin monsterSkin;

    [SerializeField] LayerMask mask;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float moveSpeed;
    [SerializeField] float damage;
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;
    [SerializeField] float bounceHitTime;
    [SerializeField] Color bounceHitColor;
    [SerializeField] Image healthFG;
    [SerializeField] RectTransform canvasRect;
    [SerializeField] Transform damageTextPos;
    [SerializeField] ParticleSystem getHitParticles;
    public ParticleSystem bounceHitParticles;

    public bool isDead = false;
    private bool isGetHit;
    private bool isCrit;
    private float getHitChance = 25;
    private Collider col;
    private SOPlayerStats playerStats;

    private void Start()
    {
        playerStats = Player.Instance.playerStats;
    }

    public void Reload()
    {
        isDead = false;
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        agent.radius = monsterStats.NavRadius;
        agent.avoidancePriority = monsterStats.NavPriority;

        damage = monsterStats.Damage;
        maxHealth = monsterStats.MaxHealth;
        moveSpeed = monsterStats.MoveSpeed;

        currentHealth = maxHealth;
        healthFG.fillAmount = (float)currentHealth / maxHealth;

        agent.speed = moveSpeed;

        col.enabled = true;

        canvasRect.forward = MainCam.Instance.transform.forward;
    }

    private void Update()
    {
        //if (monsterSkin.isPooled) ReturnToPool();
    }

    private IEnumerator WaitGetHitAnimation()
    {
        isGetHit = true;
        monsterSkin.GetHitAnim();
        agent.speed = moveSpeed * 0.2f;

        yield return new WaitForSeconds(monsterSkin.AnimStateLength());

        isGetHit = false;
        monsterSkin.MoveAnim();
        agent.speed = moveSpeed;
    }

    public void ReceiveDamage(float incomingDamage)
    {
        if (isDead) return;

        if (!isGetHit && gameObject.activeInHierarchy)
        {
            float getHitRoll = Random.Range(0f, 100f);
            if (getHitRoll <= getHitChance) StartCoroutine(WaitGetHitAnimation());
        }

        float finalDamage = incomingDamage;

        isCrit = false;
        float critRoll = Random.Range(0f, 100f);
        if (critRoll <= playerStats.CritChance)
        {
            isCrit = true;
            finalDamage = finalDamage * playerStats.CritDamage;
        }

        DamageText damageText = DamageTextPool.instance.GetDmgText();
        TextMeshProUGUI damageTextComponent = damageText.GetComponent<TextMeshProUGUI>();
        damageTextComponent.text = finalDamage.ToString();
        damageText.gameObject.SetActive(true);
        damageText.Launch(DamageTextPool.instance.ReturnDmgText, finalDamage, isCrit, damageTextPos);

        getHitParticles.Play();
        currentHealth -= finalDamage;
        if (currentHealth <= 0)
        {
            isDead = true;
            Death();
        }
        healthFG.fillAmount = (float)currentHealth / maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        monsterInPortal.Invoke();
        if (other.gameObject.tag == "Portal")
        {
            monsterDealsDamageToPlayer.Invoke(damage);
            ReturnToPool();
        }
    }

    private void Death()
    {
        if (!gameObject.activeInHierarchy) return;
        monsterKilled.Invoke();
        agent.speed = 0f;
        moveSpeed = 0f;
        monsterSkin.DeathAnim();
        col.enabled = false;
    }

    private void ReturnToPool()
    {
        MonsterSkinPool.instance.ReturnMonsterSkin(monsterSkin);
        monsterSkin = null;
        MonsterPool.instance.ReturnMonster(this);
    }

    public IEnumerator BounceGetHit()
    {
        StartCoroutine(WaitGetHitAnimation());

        monsterSkin.SetBounceColor(bounceHitColor);
        
        yield return new WaitForSeconds(bounceHitTime);

        monsterSkin.RestoreColors();
    }
}
