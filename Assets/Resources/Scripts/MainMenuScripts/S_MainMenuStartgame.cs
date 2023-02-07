using UnityEngine;
using System.Collections;

public class S_MainMenuStartgame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        //Debug.Log("loading level");
        Application.LoadLevel("Level Select");
        //S_LevelDataSingalton.Instance.LoadLevel("Level Select");
    }
}
