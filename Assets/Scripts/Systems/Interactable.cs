using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3.0f;
    bool hasInteracted = false;
    bool isFocus = false;
    [HideInInspector] public Transform player;


    public virtual void Interact() { 
        
    
    
    }
    public void OnFocused(Transform playerTransform) {
        isFocus = true;
        player= playerTransform;
    }
    public void DeFocused()
    {
        isFocus = true;
        player = null;
        hasInteracted = false;
    }


}
