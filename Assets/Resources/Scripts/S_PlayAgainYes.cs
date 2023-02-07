using UnityEngine;
using System.Collections;

public class S_PlayAgainYes : MonoBehaviour {

    public string LevelToBeLaoded = "1-1";
    void OnMouseDown()
    {
        Application.LoadLevel(LevelToBeLaoded);
    }
}
