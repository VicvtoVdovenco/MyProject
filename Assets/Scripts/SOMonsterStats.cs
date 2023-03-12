using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterStats", menuName = "ScriptableObjects/MonsterStats", order = 1)]
public class SOMonsterStats : ScriptableObject
{
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _damage = 10f;
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] float navRadius = 0f;
    [SerializeField] int navPriority = 0;



    public float MoveSpeed => _moveSpeed;
    public float Damage => _damage;
    public float MaxHealth => _maxHealth;
    public float NavRadius => navRadius;
    public int NavPriority => navPriority;




}
