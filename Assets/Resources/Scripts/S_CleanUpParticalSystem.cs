using UnityEngine;
using System.Collections;

public class S_CleanUpParticalSystem : MonoBehaviour {

    ParticleSystem PS;

    void Awake()
    {
        PS = this.GetComponent<ParticleSystem>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (PS.isStopped)
            Destroy(this.gameObject);
	}
}
