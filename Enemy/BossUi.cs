using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUi : MonoBehaviour
{
    public RectTransform BossHpBar;
    public BOSS bosshp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (bosshp.curHp >= 0)
            BossHpBar.localScale = new Vector3(bosshp.curHp / (float)bosshp.maxHp, 1, 1);
    }
}
