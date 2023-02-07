using UnityEngine;
using System.Collections;

public class S_LoadLevel : MonoBehaviour
{
    public string LevelName= "1-1";
    void OnMouseDown()
    {
        S_LoadLevel.LoadLevel(LevelName);
    }
    public static void LoadLevel(string LevelName)
    {
        S_LevelDataSingalton.Instance.LoadLevel(LevelName);
    }
}
