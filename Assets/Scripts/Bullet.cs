using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float lifetime = 3f;
    float timer = 0f;
    float damage = 0;

    bool isMonsterHit = false;
    bool isCrit = false;

    [SerializeField] GameObject _gameObject;
    [SerializeField] Transform _transform;
    [SerializeField] Rigidbody _rigibody;

    System.Action<Bullet> callback;

    public GameObject GameObject => _gameObject;
    public Transform Transform => _transform;
    public Rigidbody Rigidbody => _rigibody;

    private Tower tower;

    public void Launch(System.Action<Bullet> callback, float damage, bool isCrit)
    {
        this.callback = callback;
        this.damage = damage;
        this.isCrit = isCrit;
        timer = 0f;
    }

    //tempstuff

    private void Awake()
    {
        _gameObject = gameObject;
        _transform = transform;
        _rigibody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if ((timer > lifetime && timer <= lifetime + Time.deltaTime))
        {
            callback?.Invoke(this);
        }

        timer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();
            monster.ReceiveDamage(damage, isCrit);
            callback?.Invoke(this);
        }
    }
}
