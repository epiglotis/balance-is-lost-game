using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public GameObject playerPointer;
    Grid grid;
    PlayerController playerController;
    bool isEnabled = false;
    SpriteRenderer sprite;
    Camera cam;
    Collider[] colliders;
    public Collider[] Colliders { get { return colliders;  } set { colliders = value; } }


    public bool IsEnabled =>isEnabled;
    // Start is called before the first frame update
    void Start()
    {
        grid = playerPointer.GetComponentInParent<Grid>();
        playerController = GetComponent<PlayerController>();
        sprite = playerPointer.GetComponent<SpriteRenderer>();
        cam = GameManager.instance.mainCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerPointer.transform.position) < 3)
        {
            if(playerController.SelectedSkill.type==SkillTypes.Types.Terra)
            isEnabled = true;
            else
                isEnabled = false;
        }
        else
        {
            isEnabled = false;
        }
        sprite.enabled = isEnabled;


        Vector3Int cellPosition = grid.WorldToCell(new Vector3(
            cam.ScreenToWorldPoint(Input.mousePosition).x,
            cam.ScreenToWorldPoint(Input.mousePosition).y,
            0));

        playerPointer.transform.position = grid.GetCellCenterWorld(cellPosition);


        /*
        if (Input.GetMouseButtonDown(0)) {
            if (colliders.Length > 0) {
                for (int i = 0; i < colliders.Length; i++) {
                    print(colliders[i].name);
                }
            
            }

        }*/
        

    }
}
