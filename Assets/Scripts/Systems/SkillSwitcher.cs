using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSwitcher : MonoBehaviour
{
    
    public int currentSkill = 0;
    void Start()
    {
        SetSkillActive();
    }



   

    public void SetSkillActive()                   
    {
        int skillIndex = 0;

        foreach (Transform skill in transform)
        {
            if (skillIndex == currentSkill)
            {
                skill.gameObject.SetActive(true);
            }
            else
            {
                skill.gameObject.SetActive(false);
            }
            skillIndex++;
        }
    }



}


