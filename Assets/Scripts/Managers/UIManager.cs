using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Skill system UI")]
    public Text skillName;

    void Awake()
    {
        if (!instance)
        {
            instance = this;

        }
        else
            Destroy(gameObject);

    }
}
