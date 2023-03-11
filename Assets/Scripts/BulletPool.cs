using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;
    public Bullet bulletPrefab;
    public int poolSize = 20;

    private Stack<Bullet> bulletsAll = new Stack<Bullet>();
    private Stack<Bullet> bulletsFree = new Stack<Bullet>();

    private GameObject bulletContainer;

    void Awake()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        bulletContainer = new GameObject("Bullet Container");
        bulletContainer.transform.SetParent(gameManager.transform);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < poolSize; i++)
        {
            CreateInstance();
        }
    }

    public Bullet GetBullet()
    {
        Bullet bullet = null;

        if (bulletsFree.Count == 0)
        {
            CreateInstance();
        }

        bullet = bulletsFree.Pop();
        bullet.GameObject.SetActive(true);

        return bullet;
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullet.GameObject.SetActive(false);
        bulletsFree.Push(bullet);
    }

    void CreateInstance()
    {
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.transform.SetParent(bulletContainer.transform);
        bullet.GameObject.SetActive(false);
        bulletsAll.Push(bullet);
        bulletsFree.Push(bullet);
    }
}
