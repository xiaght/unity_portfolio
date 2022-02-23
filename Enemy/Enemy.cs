using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public enum Tyep { A,B,C,D};
    public Tyep enemyTyep;


    public int maxHp;
    public int curHp;

    public Transform target;
    public bool isChase;
    public bool isAttack;
    public bool isDead;
    public BoxCollider meleeAttack;
    public GameObject bullet;

    

    public Rigidbody rigid;
    public BoxCollider box;
    public MeshRenderer[] meshs;
    public Animator anim;


    float moveRan;
    public float hAxis;
    Vector3 moveVec;
    bool isBorder;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();

        anim = GetComponentInChildren<Animator>();
        if(enemyTyep!=Tyep.D)
        moveRan = Random.Range(0, 2);
        Invoke("Think", moveRan);



    }

    
    private void Update()
    {
        Move();
        Iscorner();

    }
    void Iscorner() {
        Vector3 frontVec = new Vector3(rigid.position.x + hAxis * 3, rigid.position.y, rigid.position.z);
        Debug.DrawRay(frontVec, Vector3.down * 1, Color.green);


        RaycastHit hitInfo;
        if (!Physics.Raycast(frontVec, Vector3.down, out hitInfo, 1))
        {
            hAxis *= -1;
            CancelInvoke();
            Invoke("Think", 2);
        }
    }

    void Move() {

        if (hAxis == 0)
            anim.SetBool("isWalk", false);
        if (curHp <= 0)
            hAxis=0;
        moveVec = new Vector3(hAxis, 0, 0).normalized;
        transform.position += moveVec * 10 * 0.3f * Time.deltaTime;
        transform.LookAt(transform.position + moveVec);
    }
    void Think()
    {
        anim.SetBool("isWalk", true);
        moveRan = Random.Range(2, 4);
        hAxis = Random.Range(-1, 2);
        Invoke("Think", moveRan);
    }


    private void FixedUpdate()
    {
        Freezevelocity();
        Targerting();

    }







    void Freezevelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }






    void Targerting() {
        if (enemyTyep != Tyep.D&& !isDead)
        {

            float targetRadius = 0;
            float targetRange = 0;

            switch (enemyTyep)
            {
                case Tyep.A:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Tyep.B:

                    targetRadius = 1f;
                    targetRange = 10f;
                    break;
                case Tyep.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;


                    break;
            }




            RaycastHit[] rayhits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayhits.Length > 0 && !isAttack)
            {

                StartCoroutine(Attack());
            }
        }
    
    }
    IEnumerator Attack() {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);
        moveVec = Vector3.zero;

        switch (enemyTyep)
        {
            case Tyep.A:
                yield return new WaitForSeconds(0.2f);
                meleeAttack.enabled = true;

                yield return new WaitForSeconds(1f);

                meleeAttack.enabled = false;
                yield return new WaitForSeconds(1f);
                break;
            case Tyep.B:

                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward*50, ForceMode.Impulse);
                meleeAttack.enabled = true;
                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeAttack.enabled = false;
                yield return new WaitForSeconds(2f);

                break;
            case Tyep.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity=(transform.forward * 30);



                yield return new WaitForSeconds(2f);


                break;

        }

       

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false); 
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee") {
            Weapon weapon = other.GetComponent<Weapon>();
            curHp -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;


            StartCoroutine(OnDamage(reactVec,false));

        }
        if (other.tag == "Bullet")
        {
            Debug.Log("bullet");
            Bullet bullet = other.GetComponent<Bullet>();
            curHp -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec,false));
        }
    }

    public void HitByGrenade(Vector3 explosionPos) {
        Debug.Log("Enemy");
        curHp -= 100;
        Vector3 reactVex = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVex,true));

    
    }
    IEnumerator OnDamage(Vector3 reactVec,bool isGrenade) {

        foreach (MeshRenderer one in meshs) {

            one.material.color = Color.red;
        }
        
        yield return new WaitForSeconds(0.1f);
        if (curHp > 0)
        {

            foreach (MeshRenderer one in meshs)
            {

                one.material.color = Color.white;
            }
        }
        else {
            anim.SetTrigger("doDie");
            isChase = false;
            isDead = true;
            moveVec = Vector3.zero;
            foreach (MeshRenderer one in meshs)
            {

                one.material.color = Color.gray;
            }
            gameObject.layer = 12;
            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up*3;
                rigid.freezeRotation = false;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec* 15, ForceMode.Impulse);

            }
            else {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }

            if (enemyTyep != Tyep.D)
                Destroy(gameObject, 2);
        }
    
    }


}
