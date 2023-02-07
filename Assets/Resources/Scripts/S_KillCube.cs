using UnityEngine;
using System.Collections;

public class S_KillCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        collider.gameObject.SetActive(false);
    }
}
