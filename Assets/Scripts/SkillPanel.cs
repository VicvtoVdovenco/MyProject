using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanel : MonoBehaviour
{
    private GridLayout gridLayout;
    [SerializeField] GameObject skill;
    [SerializeField] int numberOfSkills;
    private bool isCreatingSkill;

    private void Start()
    {
        gridLayout = GetComponent<GridLayout>();

        for (int i = 0; i < numberOfSkills; i++)
        {
            CreateSkill();
        }
    }

    private void Update()
    {
        if (transform.childCount < numberOfSkills && !isCreatingSkill)
        {
            Invoke("CreateSkill", 0.3f);
            isCreatingSkill = true;
        }
    }

    private void CreateSkill()
    {
        GameObject instance = Instantiate(skill, gameObject.transform);
        isCreatingSkill = false;
    }
}
