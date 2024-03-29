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
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private float timer;
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Color selectedColor;

    private Color unselectedColor;
    private float rayCastDistance = 500f;
    private Animator animator;
    private AudioSource audioSource;
    private bool isBouncing = false;
    private SOPlayerStats playerStats;

    private void Start()
    {
        playerStats = Player.Instance.playerStats;
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

        bullet.Launch(BulletPool.instance.ReturnBullet, playerStats.Damage, isBouncing);

        animator.SetInteger("state", 2);
        animator.SetInteger("state", 1);
        fireParticles.Play();
        audioSource.PlayOneShot(fireSound);
        isBouncing = false;
    }

    private void ToIdle()
    {
        animator.SetInteger("state", 2);
    }

    public void TargetTower()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = selectedColor;
        }
    }

    public void UntargetTower()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = unselectedColor;
        }
    }

    public void BounceShoot()
    {
        isBouncing = true;
        timer = Mathf.Infinity;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Fire"))
        {
            animator.SetInteger("state", 3);
        }
    }
}