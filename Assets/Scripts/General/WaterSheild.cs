using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSheild : MonoBehaviour
{
    Vector3 vec;
    float eskiDeger = 0;
    // Start is called before the first frame update
    void OnEnable()
    {

        if (eskiDeger == 90 || eskiDeger == 270) GetComponentInChildren<Animator>().Play("suduvarYan");
        if (eskiDeger == 180 || eskiDeger == 360) GetComponentInChildren<Animator>().Play("suduvarUp");
    }

    void Update()
    {
        
            Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            vec = transform.eulerAngles;
            vec.z = Mathf.Round(vec.z / 90) * 90;
            transform.eulerAngles = new Vector3(vec.x,vec.y, eskiDeger);
            transform.GetChild(0).transform.localEulerAngles = new Vector3(transform.GetChild(0).transform.localEulerAngles.x, transform.GetChild(0).transform.localEulerAngles.y, -transform.localEulerAngles.z);


            if (eskiDeger == 90 || eskiDeger == 270) GetComponentInChildren<Animator>().Play("suduvarYan");
            if (eskiDeger == 180 || eskiDeger == 360) GetComponentInChildren<Animator>().Play("suduvarUp");
        
    }
}
