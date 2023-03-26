using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class SOPlayerStats : ScriptableObject
{
    [SerializeField] float health;
    [SerializeField] float mana;
    [SerializeField] float manaRegen;
    [SerializeField] float damage;
    [SerializeField] float critChance;
    [SerializeField] float critDamage;


    public float Health => health;
    public float Mana => mana;  
    public float ManaRegen => manaRegen;
    public float CritChance => critChance;
    public float CritDamage => critDamage;
    public float Damage => damage;
}
