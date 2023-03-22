using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkin : MonoBehaviour
{
    [SerializeField] MonsterType monsterType;
    public MonsterType MonsterType => monsterType;
    public bool isPooled = false;

    [SerializeField] Animator animator;

    private MeshRenderer[] renderers;
    [SerializeField] List<Color> baseColors;

    private void Start()
    {
        animator = GetComponent<Animator>();

        renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        baseColors = new List<Color>();
        StoreColors();
    }

    private void StoreColors()
    {
        foreach (MeshRenderer r in renderers)
        {
            Color baseColor = r.material.color;
            baseColors.Add(baseColor);
        }
    }

    public void SetBounceColor(Color color)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = color;
        }
    }

    public void RestoreColors()
    {
        for (int i = 0; i < baseColors.Count; i++)
        {
            renderers[i].material.color = baseColors[i];
        }
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
