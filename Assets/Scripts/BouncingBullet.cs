using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : MonoBehaviour
{
    //public float speed = 10f;
    //public float radius = 5f;
    //public int maxBounces = 3;
    //public float damage = 10f;

    //private int bounceCount = 0;
    //private List<Transform> targets = new List<Transform>();

    //private void Start()
    //{
    //    // Populate the list of targets with the transforms of all enemies
    //    // in the game world
    //    foreach (GameObject enemy in EnemyPool.instance.enemyList)
    //    {
    //        targets.Add(enemy.transform);
    //    }
    //}

    //private void Update()
    //{
    //    if (targets.Count == 0 || bounceCount == 0)
    //    {
    //        // No more targets or bounce count is zero, destroy the projectile
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        // Move towards the closest target within the radius
    //        Transform closestTarget = null;
    //        float closestDistance = Mathf.Infinity;
    //        foreach (Transform target in targets)
    //        {
    //            float distance = Vector3.Distance(transform.position, target.position);
    //            if (distance < closestDistance && distance <= radius)
    //            {
    //                closestTarget = target;
    //                closestDistance = distance;
    //            }
    //        }
    //        if (closestTarget != null)
    //        {
    //            transform.position = Vector3.MoveTowards(transform.position, closestTarget.position, speed * Time.deltaTime);
    //            if (Vector3.Distance(transform.position, closestTarget.position) < 0.1f)
    //            {
    //                // Target is hit, do damage and reduce bounce count
    //                closestTarget.GetComponent<Enemy>().TakeDamage(damage);
    //                bounceCount--;
    //                // Remove the target from the list
    //                targets.Remove(closestTarget);
    //            }
    //        }
    //        else
    //        {
    //            // No target within the radius, destroy the projectile
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    //public void SetBounceCount(int count)
    //{
    //    bounceCount = count;
    //}

}
