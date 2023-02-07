using UnityEngine;
using System.Collections;

public class S_DontDestroyOnLoad : Singleton<S_DontDestroyOnLoad>
{
    private static bool _created = false;
    protected S_DontDestroyOnLoad()
    {
    }

    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("MainMenuMusic").Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
            _created = true;
            //Debug.Log("created: "+_created+", Instance: null");
            this.audio.Play();
        }
        else
        {
            Destroy(this.gameObject, 0.1f);
        }
    }
    
}
