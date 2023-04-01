using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] TextMeshProUGUI playerHealthText;
    [SerializeField] Image playerHealthImage;

    [SerializeField] TextMeshProUGUI playerManaText;
    [SerializeField] Image playerManaImage;

    public SOPlayerStats playerStats;

    private float playerCurrentHealth;
    private float playerCurrentMana;

    [HideInInspector] public float PlayerCurrentHealth => playerCurrentHealth;
    [HideInInspector] public float PlayerCurrentMana => playerCurrentMana;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerCurrentHealth = playerStats.Health;
        playerHealthText.text = playerCurrentHealth.ToString();

        playerCurrentMana = playerStats.Mana;
        playerManaText.text = playerCurrentMana.ToString();

        Monster.monsterDealsDamageToPlayer.AddListener(PlayerTakeDamage);
        SkillDragDrop.skillUsed.AddListener(PlayerSpendMana);
    }

    private void Update()
    {
        CalculatePlayerMana();
    }



    private void CalculatePlayerHealth()
    {
        playerHealthImage.fillAmount = playerCurrentHealth / playerStats.Health;
        playerHealthText.text = Mathf.Floor(playerCurrentHealth).ToString();
    }

    private void PlayerTakeDamage(float damage)
    {
        playerCurrentHealth -= damage;
        CalculatePlayerHealth();
    }

    private void CalculatePlayerMana()
    {
        if (playerCurrentMana < playerStats.Mana)
        {
            playerCurrentMana += playerStats.ManaRegen * Time.deltaTime;
        }

        playerManaImage.fillAmount = playerCurrentMana / playerStats.Mana;
        playerManaText.text = Mathf.Floor(playerCurrentMana).ToString();
    }

    private void PlayerSpendMana(float mana)
    {
        playerCurrentMana -= mana;
        CalculatePlayerMana();
    }
}
