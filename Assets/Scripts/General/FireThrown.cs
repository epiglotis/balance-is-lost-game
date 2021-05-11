using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireThrown : MonoBehaviour
{
    public Vector2 dir;
    float speed = 10;
    public bool close = false;
    void Start()
    {
        
    }

    
    void Update()
    {
        transform.position += new Vector3(dir.x,dir.y,0) * Time.deltaTime * speed;

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
