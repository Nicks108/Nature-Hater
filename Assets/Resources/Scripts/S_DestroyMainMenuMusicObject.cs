using UnityEngine;
using System.Collections;

public class S_DestroyMainMenuMusicObject : MonoBehaviour {


    void Awake ()
    {
        GameObject menuMusicObj = GameObject.Find("MenuMusicSourse");
        if(menuMusicObj != null)
            Destroy(menuMusicObj);
    }
}
