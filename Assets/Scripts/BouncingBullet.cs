using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float radius = 5f;
    [SerializeField] int maxBounces = 3;
    [SerializeField] float damage = 10f;
    [SerializeField] float bounceDelay = 0.5f;
    

    private int bounceCount;
    private bool isReadyToBounce = false;
    private List<Transform> targets = new List<Transform>();

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

            if (target != null)
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
                    monster.ReceiveDamage(damage, false);
                    monster.StartCoroutine(monster.BounceGetHit());
                }

                bounceCount--;
                targets.Remove(target);
                isReadyToBounce = true;
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
