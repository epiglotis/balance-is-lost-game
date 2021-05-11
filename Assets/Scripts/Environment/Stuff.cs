using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuff : Interactable
{
    public override void Interact()
    {
        print("Interact with "+name);
        base.Interact();
    }
}
