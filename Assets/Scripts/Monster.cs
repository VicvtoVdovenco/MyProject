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

    [SerializeField] LayerMask mask;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] SOMonsterStats monsterStats;
    [SerializeField] float moveSpeed;
    [SerializeField] float damage;
    [SerializeField] float maxHealth;
    private float currentHealth;
    private Animator animator;
    private bool isGetHit;
    private float getHitChance = 25;
    Collider col;

    [SerializeField] Image healthFG;
    [SerializeField] RectTransform canvasRect;
    [SerializeField] Transform damageTextPos;    
    [SerializeField] ParticleSystem getHitParticles;


    void Start()
    {
        moveSpeed = monsterStats.MoveSpeed;
        damage = monsterStats.Damage;
        maxHealth = monsterStats.MaxHealth;

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();
        agent.speed = moveSpeed;

        currentHealth = maxHealth;

        canvasRect.forward = MainCam.Instance.transform.forward;
    }

    private IEnumerator WaitGetHitAnimation()
    {
        isGetHit = true;
        animator.SetInteger("state", 2);
        var state = animator.GetCurrentAnimatorStateInfo(0);
        float length = state.length;
        agent.speed = moveSpeed * 0.2f;

        yield return new WaitForSeconds(length);

        isGetHit = false;
        animator.SetInteger("state", 3);
        agent.speed = moveSpeed;

    }

    public void ReceiveDamage(float towerDamage, bool isCrit)
    {
        if (!isGetHit)
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
        if (currentHealth <= 0) Death();
        healthFG.fillAmount = (float)currentHealth / maxHealth;
    }


    private void OnTriggerEnter(Collider other)
    {
        monsterInPortal.Invoke();
        if (other.gameObject.tag == "Portal")
        {
            monsterDealsDamageToPlayer.Invoke(damage);
            Destruction();
        }
    }

    private void Death()
    {
        monsterKilled.Invoke();
        agent.speed = 0f;
        moveSpeed = 0f;
        animator.SetInteger("state", 1);
        col.enabled = false;
    }

    private void Destruction()
    {
        Destroy(gameObject);
    }


}

























