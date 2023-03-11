using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerHealthText;
    [SerializeField] Image playerHealthImage;

    [SerializeField] float playerMaxHealth;
    private float playerCurrentHealth;
    [HideInInspector] public float PlayerCurrentHealth => playerCurrentHealth;
    


    private void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        playerHealthText.text = playerCurrentHealth.ToString();
        Monster.monsterDealsDamageToPlayer.AddListener(PlayerTakeDamage);
    }

    private void Update()
    {

    }

    private void CalculatePlayerHealth()
    {
        playerHealthImage.fillAmount = playerCurrentHealth / playerMaxHealth;
        playerHealthText.text = (playerCurrentHealth / playerMaxHealth * 100).ToString();
    }

    private void PlayerTakeDamage(float damage)
    {
        playerCurrentHealth -= damage;
        CalculatePlayerHealth();
    }

}
