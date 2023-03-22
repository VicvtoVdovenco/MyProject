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

    public SOPlayerStats playerStats;

    private float playerCurrentHealth;
    [HideInInspector] public float PlayerCurrentHealth => playerCurrentHealth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
        Monster.monsterDealsDamageToPlayer.AddListener(PlayerTakeDamage);
    }

    private void CalculatePlayerHealth()
    {
        playerHealthImage.fillAmount = playerCurrentHealth / playerStats.Health;
        playerHealthText.text = (playerCurrentHealth / playerStats.Health * 100).ToString();
    }

    private void PlayerTakeDamage(float damage)
    {
        playerCurrentHealth -= damage;
        CalculatePlayerHealth();
    }

}
