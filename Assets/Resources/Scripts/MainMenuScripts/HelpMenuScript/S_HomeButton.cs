﻿using UnityEngine;
using System.Collections;

public class S_HomeButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown()
    {
        Application.LoadLevel("Main Menu");
	}
}
