using UnityEngine;
using System.Collections;

public class S_PlayAgainNo : MonoBehaviour {

    public string LevelToBeLaoded = "Main Menu";
    void OnMouseDown()
    {
        Application.LoadLevel(LevelToBeLaoded);
    }
}
