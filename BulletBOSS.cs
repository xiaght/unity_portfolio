using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BulletBOSS : Bullet
{
    public Transform targetBoss;
    int hp=2;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet") {
            hp--;
            if (hp == 0) {
                Destroy(gameObject);
            }
        }
        if (!isMelee && other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame

}
