using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_1 : MonoBehaviour
{
    public float speed;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    public GameObject[] grenades;
    public int hasGrenades;
    public GameObject grenadeObj;


    public Camera followCamera;


    public int ammo;
    public int hp;
    public int coin;
    public int heal;


    public int maxammo;
    public int maxhp;
    public int maxcoin;
    public int maxhasGrenades;
    public int maxheal;

    public GameManager gameManager; 


    float hAxis;
    //float vAxis;

    bool wDown;
    bool jDown;
    bool dodDown;
    bool eDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool fDown;
    bool rDown;
    bool gDown;
    bool enterDown;



    int jumpCount = 2;
    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isFireReady=true;
    bool isReload;
    bool isBorder;
    bool isDamage;
    bool isColl;
    
    Vector3 moveVec;
    Vector3 dodgeVec;

    Animator anim;
    Rigidbody rigid;
    MeshRenderer[] meshs;

    GameObject nearObject;
    public Weapon equipWeapon;
    public int equipWeaponIndex=-1;
    float fireDelay;

    CapsuleCollider cap;
    private void Awake()
    {
        cap = GetComponent<CapsuleCollider>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<MeshRenderer>();

      //  Debug.Log(PlayerPrefs.GetInt("MaxScore"));
        //PlayerPrefs.SetInt("MaxScore", 9999);
    }

    void FreezeRotaion() {
        rigid.angularVelocity = Vector3.zero;
    
    }
    void StopToWall() {

        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }



    private void FixedUpdate()
    {
        FreezeRotaion();
        StopToWall();
    }

    // Update is called once per frame
    void Update()
    {

        GetInput();
        Move();
        Turn();
        Jump();
        Attack();
        Grenade();
        Reload();
        Dodge();
        Heal();
        Swap();



    }


    void GetInput() {
        hAxis = Input.GetAxisRaw("Horizontal");
        //vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetKey(KeyCode.LeftControl);
        jDown = Input.GetButtonDown("Jump");
        dodDown = Input.GetKeyDown(KeyCode.LeftShift);
        eDown = Input.GetKeyDown(KeyCode.E);

        sDown1 = Input.GetKeyDown(KeyCode.Alpha1);
        sDown2 = Input.GetKeyDown(KeyCode.Alpha2);
        sDown3 = Input.GetKeyDown(KeyCode.Alpha3);

        fDown = Input.GetMouseButton(0);
        rDown = Input.GetKeyDown(KeyCode.R);
        gDown = Input.GetMouseButtonDown(1);
        enterDown = Input.GetKeyDown(KeyCode.Return);
    }
    void Move() {
        moveVec = new Vector3(hAxis, 0, 0).normalized;

        if (isDodge)
        {
            moveVec = dodgeVec;
        }

        if (isSwap)
            moveVec = Vector3.zero;
/*
        if (!isFireReady)
            moveVec = Vector3.zero;*/
        if(isReload)
            moveVec = Vector3.zero;

        if (!isBorder) {
            if (wDown)
                transform.position += moveVec * speed * 0.3f * Time.deltaTime;
            else
                transform.position += moveVec * speed * Time.deltaTime;
        }





        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }
    void Turn()
    {

        transform.LookAt(transform.position + moveVec);

/*        if (fDown) {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            if (Physics.Raycast(ray, out rayhit, Mathf.Infinity))
            {
                Vector3 nextvec = rayhit.point-transform.position ;
                nextvec.y = 0;
                transform.LookAt(transform.position+nextvec);


            }
        }*/


    }

    void Jump()
    {
        if (jDown && !isDodge && !isSwap    &&jumpCount > 0) {
            jumpCount--;
            isJump = true;
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
        }
    }

    void Attack() {

        if (equipWeapon == null) {
            return;
        }
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;
        if (fDown && isFireReady&&!isDodge&&!isSwap) {
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type==Weapon.Type.Melee?"doSwing":"doShot");
            fireDelay = 0f;
        
        }
    }
    void Grenade() {
        if (hasGrenades == 0)
            return;

        if (gDown && !isReload && !isSwap) {
            /*            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit rayhit;
                        if (Physics.Raycast(ray, out rayhit, Mathf.Infinity))
                        {
                            Vector3 nextvec = rayhit.point - transform.position;
                            nextvec.y = 10;*/
            Vector3 nextvec =  transform.forward*30+new Vector3(0,10,0);
            GameObject instantGrenade = Instantiate(grenadeObj, transform.position+new Vector3(0, 1, 0), transform.rotation);
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
                rigidGrenade.AddForce(nextvec, ForceMode.Impulse);
                rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);
                hasGrenades--;
           //     grenades[hasGrenades].SetActive(false);



           
        }
    
    }

    void Reload() {

        if (equipWeapon == null)
            return;
        if (equipWeapon.type == Weapon.Type.Melee)
            return;
        if (ammo == 0)
            return;
        if (rDown && !isJump && !isDodge && !isSwap && isFireReady &&!isReload) {
            anim.SetTrigger("doReload");
            isReload = true;
            Invoke("ReloadOut", 1f);
        }
    }
    void ReloadOut()
    {
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;

        isReload = false;


    }




    void Dodge()
    {
        if (dodDown &&!isDodge&&moveVec!=Vector3.zero && !isSwap)
        {
            dodgeVec = moveVec;
            dodDown = true;
            speed *= 2;
            cap.radius = cap.radius *0.1f;

            anim.SetTrigger("doDodge");
            isDodge = true;
            Invoke("DodgeOut", 0.5f);

        }
    }

    void DodgeOut() {
        speed *= 0.5f;
        isDodge = false;
        dodDown = false;

        cap.radius = cap.radius * 10f;

    }

    void Heal() {
        if (eDown &&!isJump&&!isDodge) {
            if (heal > 0&&hp!=maxhp)
            {
                heal--;
                hp = hp + 60 > maxhp ? maxhp : hp + 60;
            }
        
        }
    
    }

    void Swap() {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;



        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;
        if (sDown1 || sDown2 || sDown3&& !isJump&& !isDodge) {
            if (equipWeapon !=null)
            {
                equipWeapon.gameObject.SetActive(false);
            }
            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();    
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");
            isSwap = true;
            Invoke("SwapOut", 0.5f);
        
        }
    
    }
    void SwapOut()
    {
        isSwap = false;

    }




    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor") {
            isJump = false;
            jumpCount = 2;
            anim.SetBool("isJump", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item") {
            Item item = other.GetComponent<Item>();
            switch (item.type) {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo > maxammo) {
                        ammo = maxammo;
                    }

                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxcoin)
                    {
                        coin = maxcoin;
                    }

                    break;
                case Item.Type.Heart:

                    if (heal < maxheal)
                    {
                        heal++;
                    }
                    break;
                case Item.Type.Grenade:
                    grenades[hasGrenades].SetActive(true);
                    hasGrenades += item.value;
                    if (hasGrenades > maxhasGrenades)
                    {
                        hasGrenades = maxhasGrenades;
                    }

                    break;
            }
            Destroy(other.gameObject);

        }
        else if (other.tag == "EnemyBullet")
        {
            if (!isDamage) {

                Bullet enemybullet = other.GetComponent<Bullet>();
                hp -= enemybullet.damage;

                bool isBossMelee = other.name== "BossMeleeArea";
                StartCoroutine(OnDamage(isBossMelee));
                
            }
            if (other.GetComponent<Rigidbody>() != null)
            {
                Destroy(other.gameObject);
            }
        }

        
    }

    IEnumerator OnDamage(bool isBossMelee)
    {
        isDamage = true;

        if (isBossMelee) {
            rigid.AddForce(transform.forward * -25, ForceMode.Impulse);
        }
        foreach (MeshRenderer mesh in meshs) {
            mesh.material.color = Color.yellow;
        }
        yield return new WaitForSeconds(1f);
        
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
        if (isBossMelee)
        {
            rigid.velocity = Vector3.zero;
        }
        isDamage = false;
    }




    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon") {
            nearObject = other.gameObject;
        }

        if (other.tag == "Finish") {
            isColl = true;
            ExplanationOpen UIopen = other.gameObject.GetComponent<ExplanationOpen>();
            if (enterDown) {
                UIopen.Exit();
                gameManager.nextMap();
            }
        
        }

    

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = null;

        }


    }


}
