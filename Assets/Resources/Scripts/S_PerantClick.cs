using UnityEngine;
using System.Collections;

public class S_PerantClick : MonoBehaviour {

    public string GUItext
    {
        get { return this.GetComponent<GUIText>().text; }
        set {
            string temp =value;
            if (temp == "-1")//if inf num of projectiles, print inf symbole
                temp = "\u221E";
            this.GetComponent<GUIText>().text = temp;
        }
    }
    public Texture GUITexture
    {
        get { return this.GetComponent<GUITexture>().texture; }
        set { this.GetComponent<GUITexture>().texture = value; }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        this.transform.parent.GetComponent<S_WeaponMenu>().MoveMenu();
    }
}
