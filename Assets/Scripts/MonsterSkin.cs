using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkin : MonoBehaviour
{
    [SerializeField] MonsterType monsterType;
    public MonsterType MonsterType => monsterType;
    public bool isPooled = false;

    [SerializeField] Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void GetHitAnim()
    {
        animator.SetInteger("state", 2);
    }

    public void MoveAnim()
    {
        animator.SetInteger("state", 3);
    }

    public void DeathAnim()
    {
        animator.SetInteger("state", 1);
    }

    public float AnimStateLength()
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);
        return state.length;
    }

    private void ReturnToPool()
    {
        isPooled = true;
        MonsterSkinPool.instance.ReturnMonsterSkin(this);
    }

}
