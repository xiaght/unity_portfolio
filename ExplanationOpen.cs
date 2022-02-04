using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplanationOpen : MonoBehaviour
{
    //    public RectTransform UiExplanation;
    public RectTransform UiExplanation;
    public 
    Player_1 enterplayer;

    public void Enter() {
       // GameObject myInstance = Instantiate(UiExplanation);

        //UiExplanation.gameObject.SetActive(true);
       // myInstance.transform.position =   new Vector3(0, 1.0f, 0);

      //   UiExplanation.transform.position = Vector3.up * 1500;
        UiExplanation.transform.position = new Vector3(350, 200, 0);
        Debug.Log("1");
    
    }

    public void Exit() {
     //    UiExplanation.anchoredPosition = Vector3.down * 1500;
        UiExplanation.transform.position = new Vector3(0,-1500,0);
      //  UiExplanation.anch

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Enter();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Exit();

        }

    }




}
