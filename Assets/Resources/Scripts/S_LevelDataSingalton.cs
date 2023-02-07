using UnityEngine;
using System.Collections;

public class S_LevelDataSingalton : Singleton<S_LevelDataSingalton>
{
    protected S_LevelDataSingalton() { } // guarantee this will be always a singleton only - can't use the constructor!

    public string LevelToBeLoaded;
    public void LoadLevel(string LevelName)
    {
        LevelToBeLoaded = LevelName;
        Application.LoadLevel("LoadScreen");
    }

}
