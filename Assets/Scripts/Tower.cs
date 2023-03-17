using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float fireReload;
    [SerializeField] private float speed;
    [SerializeField] private Transform bulletSpawnPosition;
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private float damage;
    [SerializeField] private float critChance;
    [SerializeField] private float critDamage;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private float timer;
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color selectedColor;

    private Color unselectedColor;
    private bool isCrit;
    private float rayCastDistance = 500f;
    private Animator animator;
    private AudioSource audioSource;



    private void Start()
    {
        unselectedColor = renderers[0].material.color;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        timer = fireReload;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireReload)
        {
            GetReadyToFire();
        }
    }

    private void GetReadyToFire()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Monster");

        Ray ray = new Ray(bulletSpawnPosition.transform.position, bulletSpawnPosition.transform.forward);

        if (Physics.Raycast(ray, rayCastDistance, layerMask))
        {
            timer = 0;
            FireBullet();
        }

    }

    private void FireBullet()
    {
        Bullet bullet = BulletPool.instance.GetBullet();

        bullet.Transform.position = bulletSpawnPosition.position;
        bullet.Transform.rotation = bulletSpawnPosition.rotation;

        bullet.Rigidbody.velocity = Vector3.zero;
        bullet.Rigidbody.AddForce(Vector3.forward * speed, ForceMode.VelocityChange);

        bullet.Launch(BulletPool.instance.ReturnBullet, CalculateDamage(damage, critChance, critDamage), isCrit);

        animator.SetInteger("state", 2);
        animator.SetInteger("state", 1);
        fireParticles.Play();
        audioSource.PlayOneShot(fireSound);
    }

    private void ToIdle()
    {
        animator.SetInteger("state", 2);
    }

    private float CalculateDamage(float damage, float critChance, float critDamage)
    {
        isCrit = false;
        float finalDamage = damage;

        float roll = Random.Range(0f, 100f);
        if (roll <= critChance)
        {
            isCrit = true;
            finalDamage += finalDamage * critDamage / 100;
        }

        return finalDamage;
    }

    void OnMouseEnter()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = selectedColor;
        }
    }

    private void OnMouseExit()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = unselectedColor;
        }
    }

    private void OnMouseDown()
    {
        timer = Mathf.Infinity;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Fire"))
        {
            animator.SetInteger("state", 3);
        }
    }
}