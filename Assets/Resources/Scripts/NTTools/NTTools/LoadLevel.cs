using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour
{
    public string levelName;
    void OnMouseUp()
    {
        Application.LoadLevel(levelName);
    }
}
