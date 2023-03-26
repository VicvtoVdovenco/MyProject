using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : MonoBehaviour
{
    public float manaCost;
    public float damageMultiplier;
    //public static float DamageMultiplier;

    [SerializeField] float speed = 10f;
    [SerializeField] float radius = 5f;
    [SerializeField] int maxBounces = 3;
    [SerializeField] float bounceDelay = 0.5f;

    private int bounceCount;
    private List<Transform> targets = new List<Transform>();
    private SOPlayerStats playerStats;

    //private void Awake()
    //{
    //    DamageMultiplier = damageMultiplier;
    //}

    private void Start()
    {
        playerStats = Player.Instance.playerStats;
    }

    //public float GetDamageMultiplier()
    //{
    //    return damageMultiplier;
    //}

    public void Initiate(Transform spawnTransform)
    {
        bounceCount = maxBounces;
        foreach (Monster monster in MonsterPool.instance.GetActiveMonsters())
        {
            if (!monster.isDead) targets.Add(monster.transform);
        }
        targets.Remove(spawnTransform);

        StartCoroutine(BounceToNext());
    }

    private IEnumerator BounceToNext()
    {
        while (targets.Count > 0 && bounceCount > 0)
        {
            Transform target = FindNextTarget();

            if (target == null)
            {
                Destroy(this.gameObject);
                yield break;
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(target.position - transform.position);

                while (Vector3.Distance(transform.position, target.position) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                    yield return null;
                }

                Monster monster = target.GetComponent<Monster>();
                if (monster.gameObject.activeInHierarchy)
                {
                    monster.ReceiveDamage(playerStats.Damage * damageMultiplier);
                    monster.StartCoroutine(monster.BounceGetHit());
                    monster.bounceHitParticles.Play();
                }

                bounceCount--;
                targets.Remove(target);
            }

            yield return new WaitForSeconds(bounceDelay);
        }

        Destroy(this.gameObject);
    }

    private Transform FindNextTarget()
    {
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.gameObject.transform.position);
            if (distance < closestDistance && distance <= radius)
            {
                closestDistance = distance;
                closestTarget = target.transform;
            }
        }

        return closestTarget;
    }
}
