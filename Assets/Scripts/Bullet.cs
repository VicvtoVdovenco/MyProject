using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifetime = 3f;
    private float timer = 0f;
    private Tower tower;
    private SOPlayerStats playerStats;

    //private bool isCrit = false;
    private bool isBouncing = false;

    [SerializeField] GameObject _gameObject;
    [SerializeField] Transform _transform;
    [SerializeField] Rigidbody _rigibody;
    [SerializeField] BouncingBullet bouncingBulletPrefab;
    [SerializeField] GameObject baseSkin;
    [SerializeField] GameObject bouncingSkin;

    System.Action<Bullet> callback;

    public GameObject GameObject => _gameObject;
    public Transform Transform => _transform;
    public Rigidbody Rigidbody => _rigibody;

    private void Start()
    {
        playerStats = Player.Instance.playerStats;
    }

    public void Launch(System.Action<Bullet> callback, float damage, bool isBouncing)
    {
        this.callback = callback;
        //this.isCrit = isCrit;
        this.isBouncing = isBouncing;
        baseSkin.SetActive(!isBouncing);
        bouncingSkin.SetActive(isBouncing);
        timer = 0f;
    }


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
            callback?.Invoke(this);


            if (isBouncing)
            {
                monster.ReceiveDamage(playerStats.Damage * bouncingBulletPrefab.damageMultiplier);
                BouncingBullet bouncingBullet = Instantiate(bouncingBulletPrefab, monster.transform.position + new Vector3(0, 1f, 0), Quaternion.identity);
                bouncingBullet.Initiate(monster.transform);
                monster.StartCoroutine(monster.BounceGetHit());
                monster.bounceHitParticles.Play();
                return;
            }
            monster.ReceiveDamage(playerStats.Damage);
        }
    }
}
