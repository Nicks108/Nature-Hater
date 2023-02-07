using UnityEngine;
using System.Collections;

public class S_TestStuff : MonoBehaviour
{
    private MeshRenderer meshRendere;
    public float fadeAmount = 0;


    void Start()
    {
        meshRendere = this.gameObject.GetComponent<MeshRenderer>();
    }
    
    void OnGUI()
    {
        fadeAmount = GUI.HorizontalSlider(new Rect(10,10,500,100),fadeAmount, 0f, 1f);

        Color col0 = meshRendere.materials[0].color;
        Color col1 = meshRendere.materials[1].color;

        col1.a = 255f - (255f * fadeAmount);
        col0.a = 255f * fadeAmount;

        meshRendere.materials[0].color = col0;
        meshRendere.materials[1].color = col1;
    }
    
  
}
