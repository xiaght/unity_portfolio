using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BOSS : Enemy
{
    public GameObject missile;
    public Transform missilePosA;
    public Transform missilePosB;
    public int ranAction;
    Vector3 lookVec;
    Vector3 tauntVec;
    bool isLook;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();

        anim = GetComponentInChildren<Animator>();

        isLook = true;

        StartCoroutine(Think());

    }
    IEnumerator Think() {
        yield return new WaitForSeconds(0.1f);
        ranAction = Random.Range(0, 5);
        switch (ranAction) {
            case 0:
            case 1:
                StartCoroutine(MissileShot());
                break;
            case 2:
            case 3:
                StartCoroutine(RockShot());
                break;
            case 4:
                StartCoroutine(Taunt());
                break;


        }
    }

    IEnumerator MissileShot() {
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instantMissileA = Instantiate(missile, missilePosA.position, missilePosA.rotation);
        BulletBOSS bossMissileA = instantMissileA.GetComponent<BulletBOSS>();
        Rigidbody rigidA = instantMissileA.GetComponent<Rigidbody>();
        rigidA.velocity = (transform.forward * 30);

        bossMissileA.targetBoss = target;

        yield return new WaitForSeconds(0.3f);
        GameObject instantMissileB = Instantiate(missile, missilePosB.position, missilePosB.rotation);
        BulletBOSS bossMissileB = instantMissileB.GetComponent<BulletBOSS>();
        Rigidbody rigidB = instantMissileB.GetComponent<Rigidbody>();
        rigidB.velocity = (transform.forward * 30);
        bossMissileB.targetBoss = target;


        yield return new WaitForSeconds(2f);
        StartCoroutine(Think());
    }

    IEnumerator RockShot()
    {
        isLook = false;
        GameObject instantRock = Instantiate(bullet, transform.position, transform.rotation);
        anim.SetTrigger("doBigShot");
        yield return new WaitForSeconds(3f);
        isLook = true;
        StartCoroutine(Think());
    }
    IEnumerator Taunt()
    {
        tauntVec = target.position ;

        isLook = false;
       // box.enabled = false;
        anim.SetTrigger("doTaunt");
        yield return new WaitForSeconds(1.5f);
        meleeAttack.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeAttack.enabled = false;
        yield return new WaitForSeconds(1f);
        isLook = true;
       // box.enabled = true;


        StartCoroutine(Think());


    }
    




    // Update is called once per frame
    void Update()
    {
        if (isDead) {
            StopAllCoroutines();
            return;
        }

        
    }
}
