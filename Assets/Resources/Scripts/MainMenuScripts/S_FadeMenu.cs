using UnityEngine;
using System.Collections;

public class S_FadeMenu : MonoBehaviour
{
    public float TimeUntillFade=10f;
    public float Speed=1;

    private Color ToCol;
    public Material mat1;
    // Use this for initialization
	void Start () {
        //FadeTo.color = new Color(255,255,255,0);

	    //MR = this.gameObject.GetComponent<MeshRenderer>();
        //renderer.material = Mat1;
        ToCol = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, renderer.material.color.a);
	    ToCol.a = 0;
	    mat1 = renderer.material;
	}
    public float duration = 2.0F;
	// Update is called once per frame
	void Update ()
	{
	    FadeChangeTexureOverTime();
	}

    private void FadeChangeTexureOverTime()
    {
        //Color endResult0 = new Color(MR.materials[0].color.r, MR.materials[0].color.g, MR.materials[0].color.b, MR.materials[0].color.a);
        //endResult0.a = 255;
        //Color endResult1 = new Color(MR.materials[1].color.r, MR.materials[1].color.g, MR.materials[1].color.b, MR.materials[1].color.a);
        //endResult1.a = 0;
        if(TimeUntillFade < 0)
        {
            //Debug.Log("Color: " + mat1.color);
            mat1.color = Color.Lerp(renderer.material.color, ToCol, Time.deltaTime * Speed);
            renderer.material.color = mat1.color;
            
        }
        else
        {
            TimeUntillFade -= Time.deltaTime;
        }
    }
}
