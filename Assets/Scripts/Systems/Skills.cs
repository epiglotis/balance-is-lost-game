using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skills
{
    public SkillTypes.Types type;
    public Sprite icon;
    public int coolDown;

}

[System.Serializable]
public class SkillTypes {

    public enum Types { Terra, Wind, Fire, Water }


}