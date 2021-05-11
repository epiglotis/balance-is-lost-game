using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In game elemets Ex: Water, Lava vs.
/// </summary>
public class Elements : Stuff
{

    public SkillTypes.Types type;
    PlayerController playerController;
    Collider2D collider;
    public bool IsWater { get { return type == SkillTypes.Types.Water; } }
    public bool IsTerra { get { return type == SkillTypes.Types.Terra; } }
    public bool IsFire { get { return type == SkillTypes.Types.Fire; } }
    public bool IsAir { get { return type == SkillTypes.Types.Wind; } }
    public AudioClip toprakKirilmaSFX;
    public AudioClip atesSondurmeSFX;
    public AudioClip toprakItekleme;
    public AudioClip suyaToprak;


    private void Start()
    {
        collider = GetComponent<Collider2D>();

        Vector3Int cellPosition = GameManager.instance.gridObject.WorldToCell(new Vector3(
            transform.position.x,
            transform.position.y,
            0));

        transform.position = GameManager.instance.gridObject.GetCellCenterWorld(cellPosition);
    }
    public override void Interact()
    {
        print(player.name +" "+type );
        playerController = player.GetComponent<PlayerController>();
        if (playerController) {

            if (playerController.IsTerra && IsWater) {
                collider.enabled = false;
                GetComponent<SpriteRenderer>().sprite = GameManager.instance.terraPrefab.GetComponent<SpriteRenderer>().sprite;
                GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);



                GetComponent<AudioSource>().clip = suyaToprak;
                GetComponent<AudioSource>().Play();
            }

            if (playerController.IsWater && IsFire)
            {
                Instantiate(GameManager.instance.terraPrefab,transform.position,Quaternion.identity);
                GetComponent<AudioSource>().clip = atesSondurmeSFX;
                GetComponent<AudioSource>().Play();
                Destroy(gameObject,1);
            }


        }


        base.Interact();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsTerra)
        {
            if (collision.tag == "FireThrown")
            {
                if (!collision.gameObject.GetComponent<FireThrown>().close)
                {
                    GetComponent<AudioSource>().clip = toprakKirilmaSFX;
                    GetComponent<AudioSource>().Play();
                    collision.gameObject.GetComponent<FireThrown>().close = true;

                    Destroy(gameObject,1f);
                    Destroy(collision.gameObject);


                }

            }
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                if (collision.gameObject.GetComponent<PlayerController>().Dash)
                {
                    GetComponent<Rigidbody2D>().isKinematic = false;
                    GetComponent<AudioSource>().clip = toprakItekleme;
                    GetComponent<AudioSource>().Play();
                }
            }


        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsTerra)
        {
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                if (collision.gameObject.GetComponent<PlayerController>().Dash)
                {
                    GetComponent<Rigidbody2D>().isKinematic = false;

                }
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsTerra)
        {
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                GetComponent<Rigidbody2D>().isKinematic = true;

            }
        }
    }

}
