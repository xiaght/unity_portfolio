using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public RectTransform title;
    public GameObject scaffolding;

    public void OnClickStartButton()
    {
       // Destroy(title);
        Destroy(scaffolding);
        title.gameObject.SetActive(false);
        /*
                Debug.Log("start");*/

    }


    public void OnClickExitButton()
    {

#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }



}
