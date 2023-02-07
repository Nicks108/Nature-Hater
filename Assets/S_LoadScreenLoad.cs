using UnityEngine;
using System.Collections;

public class S_LoadScreenLoad : MonoBehaviour {

    public AnimationCurve ScaleCurve;
    public float AnimationSpeed = 1;

    void Start()
    {
        Application.LoadLevel(S_LevelDataSingalton.Instance.LevelToBeLoaded);
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    float scale = NTTools.NTTween.Tween(0.5f, 1, Time.time * AnimationSpeed, ScaleCurve);
    //    this.transform.localScale = new Vector3(scale, scale, 1);
    //}
}
