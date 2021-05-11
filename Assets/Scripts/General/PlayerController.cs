using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    Camera cam;

    Rigidbody2D rigid;
    public Skills[] skills;//player skills
    Skills selectedSkill;//Selected Skill

    ActionController actionController;
    int currentSkill;

    public Transform animationParent;

    private float lastClickTime = 0;
    float catchTime = .25f;

    Vector2 dir = Vector2.down;

    public GameObject waterSheild;
    public GameObject dashDir;

    bool dash = false;
    bool sheild = false;
    bool isFootStepPlaying;


    [Header("SFX")]
    [SerializeField] AudioClip fireballSfx;
    [SerializeField] AudioClip toprakCikarma;
    [SerializeField] AudioClip iceWall;
    [SerializeField] AudioClip footStep;
    [SerializeField] AudioClip dashClip;
    [SerializeField] AudioClip olmeClip;

    AudioSource audio;
    [SerializeField]AudioSource audiofoot;
    //Properties
    public Skills SelectedSkill { get { return selectedSkill; } set { selectedSkill = value; UpdateSkillUI(); } }
    public bool IsWater { get { return selectedSkill.type == SkillTypes.Types.Water; } }
    public bool IsTerra { get { return selectedSkill.type == SkillTypes.Types.Terra; } }
    public bool IsFire { get { return selectedSkill.type == SkillTypes.Types.Fire; } }
    public bool IsAir { get { return selectedSkill.type == SkillTypes.Types.Wind; } }
    public bool Dash => dash;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        actionController = GetComponent<ActionController>();
        audio = GetComponent<AudioSource>();
        SelectedSkill = skills[0];
    }

    void Update()
    {
        if (GameManager.instance.cutscenePanel.activeSelf) return;
        ProcessScrollWheel();
        DoubleClick();
        if (Input.GetMouseButtonDown(0)) {
            ScreenMouseRay();
        }


    }
    public void OlmeSFXCal() {
        audio.clip = olmeClip;
        audio.Play();
    
    }
    private void FixedUpdate()
    {
        if (GameManager.instance.cutscenePanel.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }


        //Player Movement
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);

        GetComponentInChildren<Animator>().SetBool("Saga", false);
        GetComponentInChildren<Animator>().SetBool("Asagi", false);
        GetComponentInChildren<Animator>().SetBool("Yukari", false);




        if (movement.x != 0 || movement.y != 0)
        {
            if (!isFootStepPlaying) StartCoroutine(footStpepPlay());


            if (Input.GetAxis("Horizontal") > 0)
            {
                SolAnim();
            }
            if (Input.GetAxis("Horizontal") < 0)
            {

                SagAnim();
            }
            if (Input.GetAxis("Vertical") > 0)
            {

                YukariAnim();
            }
            if (Input.GetAxis("Vertical") < 0)
            {

                AsagiAnim();
            }
            rigid.MovePosition(transform.position + movement * Time.fixedDeltaTime * moveSpeed);

        }
        


    }


    void UpdateSkillUI() {
        UIManager.instance.skillName.text = selectedSkill.type.ToString();
        if (!IsWater) waterSheild.SetActive(false);
    }
    IEnumerator footStpepPlay() {

        isFootStepPlaying = true;
        yield return new WaitForSeconds(0.2f);
        audiofoot.Play();
        isFootStepPlaying = false;
    }
    private void DoubleClick()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if ((Time.time - lastClickTime < catchTime) && !dash && IsAir)
            {
                //dash
                Vector3 _dir = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
                dashDir.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                Vector3 vec = dashDir.transform.eulerAngles;
                vec.z = Mathf.Round(vec.z / 90) * 90;
                dashDir.transform.eulerAngles = vec;
                dash = true;
                rigid.DOMove(transform.position + dashDir.transform.right * 3, 0.5f).SetEase(Ease.InOutCubic).OnComplete(() => { dash = false; });

                audio.clip = dashClip;
                audio.Play();

                Invoke("DashCancel", 0.5f);
                if (vec.z == 180) { SagAnim(); }
                if (vec.z == 360) { SolAnim(); }
                if (vec.z == 90) { YukariAnim(); }
                if (vec.z == 270) { AsagiAnim(); }

                /*
                Sequence mySequence = DOTween.Sequence();
                mySequence.Append(rigid.DOMove(transform.position + dashDir.transform.right * 3, 1).SetEase(Ease.InOutCubic))
                  .Append(transform.DOScale(new Vector3(1.3f, 1, 1),1))
                  .PrependInterval(1)
                  .Insert(0, transform.DOScale(new Vector3(1, 1, 1), mySequence.Duration()));
            */

            }

            lastClickTime = Time.time;
        }
    }

    void SagAnim() {

        animationParent.transform.localScale = new Vector3(-1, this.transform.localScale.y, this.transform.localScale.z);
        dir = Vector2.left;
        animationParent.GetComponentInChildren<Animator>().SetBool("Saga", true);
        animationParent.GetComponentInChildren<Animator>().SetBool("Asagi", false);
        animationParent.GetComponentInChildren<Animator>().SetBool("Yukari", false);

    }
    void SolAnim()
    {

        animationParent.transform.localScale = new Vector3(1, this.transform.localScale.y, this.transform.localScale.z);
        animationParent.GetComponentInChildren<Animator>().SetBool("Saga", true);
        animationParent.GetComponentInChildren<Animator>().SetBool("Asagi", false);
        animationParent.GetComponentInChildren<Animator>().SetBool("Yukari", false);
        dir = Vector2.right;
    }
    void YukariAnim()
    {
        animationParent.transform.localScale = new Vector3(1, this.transform.localScale.y, this.transform.localScale.z);
        animationParent.GetComponentInChildren<Animator>().SetBool("Saga", false);
        animationParent.GetComponentInChildren<Animator>().SetBool("Asagi", false);
        animationParent.GetComponentInChildren<Animator>().SetBool("Yukari", true);
        dir = Vector2.up;

    }
    void AsagiAnim()
    {
        animationParent.transform.localScale = new Vector3(1, this.transform.localScale.y, this.transform.localScale.z);
        animationParent.GetComponentInChildren<Animator>().SetBool("Saga", false);
        animationParent.GetComponentInChildren<Animator>().SetBool("Asagi", true);
        animationParent.GetComponentInChildren<Animator>().SetBool("Yukari", false);
        dir = Vector2.down;

    }
    void DashCancel() {

        dash = false;


    }
    void closeSheild() {
        sheild = false;
        waterSheild.SetActive(false);
    
    }
    private void ProcessScrollWheel()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentSkill >= skills.Length - 1)
            {
                currentSkill = 0;
            }
            else
            {
                currentSkill++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (currentSkill <= 0)
            {
                currentSkill = skills.Length - 1;
            }
            else
            {
                currentSkill--;
            }
        }
        SelectedSkill = skills[currentSkill];
        GetComponentInChildren<SkillSwitcher>().currentSkill = currentSkill;
        GetComponentInChildren<SkillSwitcher>().SetSkillActive();

    }
    /// <summary>
    /// Cast a ray from the mouse to the target object
    /// Then sets the target position of the ability to that object.
    /// </summary>
    public void ScreenMouseRay()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 2f;

        Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);

        Collider2D[] col = Physics2D.OverlapPointAll(v);



        if (col.Length > 0)
        {
            foreach (Collider2D c in col)
            {
                if (col[0].gameObject.GetComponent<Interactable>() != null)
                {
                    Interactable interactable = col[0].gameObject.GetComponent<Interactable>();
                    if (Vector2.Distance(transform.position, interactable.transform.position) < interactable.radius)
                    {
                        interactable.OnFocused(transform);
                        interactable.Interact();
                        // targetPos = c.collider2D.gameObject.transform.position;
                    }
                }
            }
        }
        else if(actionController.playerPointer.GetComponent<SpriteRenderer>().enabled){
            if (IsTerra) {
                audio.clip = toprakCikarma;
                audio.Play();
                Instantiate(GameManager.instance.terraPrefab,
                    actionController.playerPointer.transform.position,Quaternion.identity);
                
                
            
            }
          

        }
        else if (IsWater)
        {
            if (!sheild)
            {
                /*
                Instantiate(GameManager.instance.terraPrefab,
                        actionController.playerPointer.transform.position, Quaternion.identity);

                */
                waterSheild.SetActive(true);

                //waterSheild.GetComponent<WaterSheild>

                Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(waterSheild.transform.position);
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                waterSheild.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                Vector3 vec = waterSheild.transform.eulerAngles;
                vec.z = Mathf.Round(vec.z / 90) * 90;
                print(vec);
                waterSheild.transform.eulerAngles = vec;
                waterSheild.transform.GetChild(0).transform.localEulerAngles = new Vector3(waterSheild.transform.GetChild(0).transform.localEulerAngles.x,
                waterSheild.transform.GetChild(0).transform.localEulerAngles.y, -waterSheild.transform.localEulerAngles.z);

                if (vec.z == 90 || vec.z == 270) waterSheild.GetComponentInChildren<Animator>().Play("suduvarYan");
                if (vec.z == 180 || vec.z == 360 || vec.z==0) waterSheild.GetComponentInChildren<Animator>().Play("suduvarUp");

                audio.clip = iceWall;
                audio.Play();

                GetComponentInChildren<Animator>().SetTrigger("Buyu");
                sheild = true;
                Invoke("closeSheild", 1);
            }
        }

        if (IsFire)
        {
            audio.clip = fireballSfx;
            audio.Play();
           GameObject fire= Instantiate(GameManager.instance.fireThrow,
                transform.position, Quaternion.identity);
            fire.GetComponent<FireThrown>().dir = dir;
            GetComponentInChildren<Animator>().SetTrigger("Buyu");

        }
        

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Elements>()) {
            dash = false;
        
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Elements>())
        {
            if (collision.gameObject.GetComponent<Elements>().IsFire)
            {
                GameManager.instance.GameFinish();

            }
        }
    }
    

}
