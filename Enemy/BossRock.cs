using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : Bullet
{
    Rigidbody rigid;
    float angularPowor = 2;
    float scaleValue = 0.2f;
    bool isShoot;
    int hp = 4;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(GainPoworTimer());
        StartCoroutine(GainPowor());
        
    }
    IEnumerator GainPoworTimer() {
        yield return new WaitForSeconds(2.2f);
        isShoot = true;
    }
    IEnumerator GainPowor() {
        while (!isShoot) {
            angularPowor += 1f;
            scaleValue += 0.001f;
            transform.localScale=Vector3.one * scaleValue;
            rigid.AddTorque(transform.right * angularPowor, ForceMode.Acceleration);
            yield return null;

        
        }
    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            hp--;
            if (hp == 0)
            {
                Destroy(gameObject);
            }
        }

        if (!isMelee && other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }




}
