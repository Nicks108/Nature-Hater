using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NTTools;

public class S_BonusTextScript : MonoBehaviour, IObjectPool  {
    public List<string> Text = new List<string>();

    private bool _isActive;
    public bool IsActive
    {
        get { return this.gameObject.activeSelf; }
        set { this.gameObject.SetActive(value); }
    }

    public bool bFadeOut = false;
    public bool bFadeIn = false;
    public float FadeOutTime = 0.5f;
    public float FadeInTime = 0.5f;
    public bool isFading
    {
        get 
        { 
            bool temp = false;
            if (bFadeIn || bFadeOut)
            {
                temp = true;
            }
            return temp;
        }
    }
    //public Vector3 ScaleToSizeRelative;
    //private Vector3 OldScaleSize;
    //private Vector3 NewScaleSize;
    //public bool isScaleing
    //{
    //    get 
    //    {
    //        bool temp = false;
    //        if (this.transform.localScale)
    //        {
    //        }
    //    }
    //}

    public enum MoveDirections
    {
        None =0,
        Up,
        Down,
        Left,
        Right
    };
    public MoveDirections MoveDirection;

    private Vector3 GetVectorFromEnum(MoveDirections Direction)
    {
        Vector3 value = Vector3.zero;
        switch(Direction)
        {
            case MoveDirections.None:
                value = Vector3.zero;
                break;
            case MoveDirections.Up:
                value = Vector3.up;
                break;
            case MoveDirections.Down:
                value = Vector3.down;
                break;
            case MoveDirections.Left:
                value = Vector3.left;
                break;
            case MoveDirections.Right:
                value = Vector3.right;
                break;
        }
        return value;
    }


    private Vector3 v3MoveDirection = Vector3.zero; //why the fuck does this not work??
    public float MoveSpeed;
	// Use this for initialization
	void Start () {
        GenerateText();
	}
	
	// Update is called once per frame
	void Update () {
        v3MoveDirection = GetVectorFromEnum(MoveDirection);
        if(bFadeOut)
            FadeOut();
        if (bFadeIn)
            fadeIn();
        //Debug.Log(" Modirecction nono" + GetVectorFromEnum(MoveDirections.None));
        if (v3MoveDirection != GetVectorFromEnum(MoveDirections.None))
        {
            this.transform.Translate(v3MoveDirection * (Time.deltaTime * MoveSpeed));
        }
	}

    private void FadeOut()
    {
        Color temp = renderer.material.color;;
        if (temp.a >= 0)
        {
            temp.a -= Time.deltaTime / FadeOutTime;
            renderer.material.color = temp;
            //Debug.Log("temp" + temp);
        }
        else
        {
            bFadeOut = false;
        }
    }
    private void fadeIn()
    {
        Color temp = renderer.material.color;
        if (temp.a <= 1)
        {
            temp.a += Time.deltaTime * FadeInTime;
            renderer.material.color = temp;
        }
        else
        {
            bFadeIn = false;
        }
    }

    public void GenerateText()
    {
        if (Text.Count >0)
        {
            StringBuilder SB = new StringBuilder();
            foreach (string s in Text)
            {
                SB.Append(s + " ");
            }
            this.GetComponent<TextMesh>().text = SB.ToString().Trim();
        }
    }

    public void ShowAtPositnion(Rect pos)
    {
        ShowAtPositnion(pos.x, pos.y);
    }
    public void ShowAtPositnion(float Xpos, float Ypos)
    {
        Show(true);
        this.transform.position = new Vector3(Xpos, Ypos, this.transform.position.z);
    }
    public void Show(bool show)
    {
        this.GetComponent<Renderer>().enabled = show;
    }
    //public void ShowForTimeAtPositon(Vector3 pos, float time)
    //{

    //}

    public void Set(Vector3 location, string text, bool FadeIn = false, bool FadeOut = false, float FadeInTime = 0, float FadeOutTime = 0)
    {
        this.transform.position = location;
        this.GetComponent<TextMesh>().text = text;
        this.bFadeIn = FadeIn;
        this.bFadeOut = FadeOut;
        this.FadeInTime = FadeInTime;
        this.FadeOutTime = FadeOutTime;
    }
}
