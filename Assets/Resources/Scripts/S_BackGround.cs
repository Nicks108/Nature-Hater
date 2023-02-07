using UnityEngine;
using System.Collections;

public class S_BackGround : S_2dSpriteBase {

    new void Awake()
    {
        Texture texture = S_2dSpriteBase.GetSingleImage(Application.streamingAssetsPath + "\\BackGrounds\\pigeon-edition-001", ".jpg");
        this.renderer.material.mainTexture = texture;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
