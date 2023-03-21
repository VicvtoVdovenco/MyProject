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

    private MeshRenderer[] renderers;
    [SerializeField] List<Color> baseColors;

    private bool isGetHit;
    public bool isDead = false;
    private float getHitChance = 25;
    private Collider col;

    private void Start()
    {
        renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        baseColors = new List<Color>();
        foreach (MeshRenderer r in renderers)
        {
            Color baseColor = r.material.color;
            baseColors.Add(baseColor);
        }
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
        if (monsterSkin.isPooled) ReturnToPool();
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

    public void ReceiveDamage(float towerDamage, bool isCrit)
    {
        if (isDead) return;

        if (!isGetHit && gameObject.activeInHierarchy)
        {
            float roll = Random.Range(0f, 100f);
            if (roll <= getHitChance) StartCoroutine(WaitGetHitAnimation());
        }

        DamageText damageText = DamageTextPool.instance.GetDmgText();
        TextMeshProUGUI damageTextComponent = damageText.GetComponent<TextMeshProUGUI>();
        damageTextComponent.text = towerDamage.ToString();
        damageText.gameObject.SetActive(true);
        damageText.Launch(DamageTextPool.instance.ReturnDmgText, towerDamage, isCrit, damageTextPos);

        getHitParticles.Play();
        currentHealth -= towerDamage;
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

        foreach (MeshRenderer r in renderers)
        {
            r.material.color = bounceHitColor;
        }

        yield return new WaitForSeconds(bounceHitTime);

        for (int i = 0; i < baseColors.Count; i++)
        {
            renderers[i].material.color = baseColors[i];
        }
    }
}

























