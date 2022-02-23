using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] Map;
    public int MapIndex;
    public Player_1 player;
    public Transform parent;

    public Material skyBoxMat;
    public Material skyBoxMatVillage;

    public Light li;

    GameObject myInstance;

    private void Awake()
    {

 //       Map[0].SetActive(true);
 //       myInstance = Instantiate(Map[0],parent);

    }
    public void nextMap() {

        Map[MapIndex].SetActive(false);
        //Destroy(myInstance.gameObject);
        if (MapIndex == Map.Length-1)
        {
            GoHome();
        }
        RenderSettings.skybox = skyBoxMat;
        li.color = new Color(0, 0, 0, 1);
        MapIndex++;
        //GameObject myInstance = Instantiate(Map[MapIndex], parent);
        Map[MapIndex].SetActive(true);
        player.gameObject.transform.position = Vector3.zero;
    }

    void GoHome() {
        MapIndex = -1;
        Debug.Log("1");
        RenderSettings.skybox = skyBoxMatVillage;
        Debug.Log("2");
        li.color = new Color(0.4803311f, 0.5310479f, 0.990566f, 1);
        Debug.Log("3");
    }

}
