using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HudManager_1 : MonoBehaviour
{
    public Camera gameCam;

    public Player_1 player;
    public BOSS boss;
    public bool isBattle;


    public GameObject gamePanel;


    public Text PlayerHpText;
    public RectTransform PlayerHpBar;
    public Text PlayerCoinText;
    public Text PlayerAmmoText;
    public Text PlayerGreande;



    public Image[] weaponImage;
    public Image[] PlayerHeal;

    public Image weaponrImage;
    


    int curindex;
    private void Awake()
    {
      

    }


    private void LateUpdate()
    {

        PlayerHpText.text = player.hp + "/" + player.maxhp;
        PlayerCoinText.text = string.Format("{0:n0}", player.coin);
        if (player.equipWeapon == null) {
            PlayerAmmoText.text = "-/" + player.ammo;
        }else if(player.equipWeapon.type==Weapon.Type.Melee)
            PlayerAmmoText.text = "-/" + player.ammo;
        else
            PlayerAmmoText.text = player.equipWeapon.curAmmo+"/" + player.ammo;

        PlayerGreande.text = player.hasGrenades + "/" + player.maxhasGrenades;

        if (player.equipWeaponIndex == -1&&curindex!=player.equipWeaponIndex)
        {
            weaponImage[0].color = new Color(1, 1, 1, 0.4f);
            weaponImage[1].color = new Color(1, 1, 1, 0.4f);
            weaponImage[2].color = new Color(1, 1, 1, 0.4f);    
        }
        else if(player.equipWeaponIndex==0){
            weaponImage[0].color = new Color(1, 1, 1, 1);
            weaponImage[1].color = new Color(1, 1, 1, 0.4f);
            weaponImage[2].color = new Color(1, 1, 1, 0.4f);

        }
        else if (player.equipWeaponIndex == 1)
        {
            weaponImage[0].color = new Color(1, 1, 1, 0.4f);
            weaponImage[1].color = new Color(1, 1, 1, 1);
            weaponImage[2].color = new Color(1, 1, 1, 0.4f);

        }
        else if (player.equipWeaponIndex == 2)
        {
            weaponImage[0].color = new Color(1, 1, 1, 0.4f);
            weaponImage[1].color = new Color(1, 1, 1, 0.4f);
            weaponImage[2].color = new Color(1, 1, 1, 1);
        }
        weaponrImage.color = new Color(1, 1, 1, player.hasGrenades > 0?1:0.4f);



        if (player.heal==4)
        {
            PlayerHeal[0].color = new Color(1, 1, 1, 0);
            PlayerHeal[1].color = new Color(1, 1, 1, 0);
            PlayerHeal[2].color = new Color(1, 1, 1, 0);
            PlayerHeal[3].color = new Color(1, 1, 1, 0);
            PlayerHeal[4].color = new Color(1, 1, 1, 1);
        }
        if (player.heal == 3)
        {
            PlayerHeal[0].color = new Color(1, 1, 1, 0);
            PlayerHeal[1].color = new Color(1, 1, 1, 0);
            PlayerHeal[2].color = new Color(1, 1, 1, 0);
            PlayerHeal[3].color = new Color(1, 1, 1, 1);
            PlayerHeal[4].color = new Color(1, 1, 1, 0);
        }
        if (player.heal == 2)
        {
            PlayerHeal[0].color = new Color(1, 1, 1, 0);
            PlayerHeal[1].color = new Color(1, 1, 1, 0);
            PlayerHeal[2].color = new Color(1, 1, 1, 1);
            PlayerHeal[3].color = new Color(1, 1, 1, 0);
            PlayerHeal[4].color = new Color(1, 1, 1, 0);
        }
        if (player.heal == 1)
        {
            PlayerHeal[0].color = new Color(1, 1, 1, 0);
            PlayerHeal[1].color = new Color(1, 1, 1, 1);
            PlayerHeal[2].color = new Color(1, 1, 1, 0);
            PlayerHeal[3].color = new Color(1, 1, 1, 0);
            PlayerHeal[4].color = new Color(1, 1, 1, 0);
        }
        if (player.heal == 0)
        {
            PlayerHeal[0].color = new Color(1, 1, 1, 1);
            PlayerHeal[1].color = new Color(1, 1, 1, 0);
            PlayerHeal[2].color = new Color(1, 1, 1, 0);
            PlayerHeal[3].color = new Color(1, 1, 1, 0);
            PlayerHeal[4].color = new Color(1, 1, 1, 0);
        }

        if(player.hp>0)
        PlayerHpBar.localScale = new Vector3(player.hp / (float)player.maxhp, 1,1) ;




        /*
                {
                    if (player.equipWeapon != null)
                    {
                        player.equipWeapon.gameObject.SetActive(false);
                    }
                    player.equipWeaponIndex = weaponIndex;
                    player.equipWeapon = player.weapons[weaponIndex].GetComponent<Weapon>();
                    player.equipWeapon.gameObject.SetActive(true);



                }*/






    }

}
