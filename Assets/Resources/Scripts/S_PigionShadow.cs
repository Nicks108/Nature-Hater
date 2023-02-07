using UnityEngine;
using System.Collections;

public class S_PigionShadow : MonoBehaviour {
    public GameObject ParentPigionObject;
    void Awake()
    {
    }

    // Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
	    this.gameObject.SetActive(ParentPigionObject.activeSelf);
	}
}
