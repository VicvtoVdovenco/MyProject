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

    private void SetAnimatorState(int state)
    {
        if (animator.GetInteger("state") == 1 || state == 1)
        {
            animator.SetInteger("state", 1);
        }
        else
        {
            animator.SetInteger("state", state);
        }
    }

    public void DeathAnim()
    {
        SetAnimatorState(1);
    }

    public void GetHitAnim()
    {
        SetAnimatorState(2);
    }

    public void MoveAnim()
    {
        SetAnimatorState(3);
    }

    public float AnimStateLength()
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);
        return state.length;
    }

    private void ReturnSkinToPool()
    {
        isPooled = true;
        MonsterSkinPool.instance.ReturnMonsterSkin(this);
    }

}
