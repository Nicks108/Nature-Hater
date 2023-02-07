using UnityEngine;
using System.Collections;

public class S_ShowFullCreditsButton : MonoBehaviour
{
    public  Material FadeTo;
    public Material Matt1;
    public Material Matt2;
    public MeshRenderer MR;
    public float Speed=1;
    public bool ShouldFade = false;
	// Use this for initialization
	void Start () {
	    //FadeTo.color = new Color(255,255,255,0);
	}
	
	// Update is called once per frame
	void Update () {
            //FadeChangeTexureOverTime();
	}
    void OnMouseDown()
    {
        ShouldFade = !ShouldFade;

        if(!ShouldFade)
            MR.material = Matt1;
        else
            MR.material = Matt2;
    }
   
}
