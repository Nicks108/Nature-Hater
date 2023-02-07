using UnityEngine;
using System.Collections;
using NTTools;
using System.Collections.Generic;

public class S_WeaponMenu : MonoBehaviour {
	// Use this for initialization

    NTTween tween = new NTTween();
    public float TimeToMove = 10f;
    float TweenTime = 0f;
    Vector2 to = new Vector2();

    public GameObject[] WeaponArray;

	void Start () {
        MainWindow.Item1 = "Weapon Select";
        float MenuHeight = 90;
        float MenuWidth = 100;
        //float menuStartPosition = Screen.width + MenuWidth;
        MainWindow.Item2 = new Rect(Screen.width, (Screen.height/2)-(MenuHeight/2), MenuWidth, MenuHeight);
        //(416, 400, 100, 90);
	}
	
	// Update is called once per frame
	void Update () {
	}

    
    void OnMouseDown()
    {
        //MoveMenu();
    }

    private bool MoveLeft = true;
    private bool MoveRight
    {
        get { return !MoveLeft; }
        set { MoveLeft = !MoveLeft;}
    }
    public void MoveMenu()
    {
        
        bool MoveOut = this.gameObject.GetComponent<S_WeaponNewWeaponMenu>().MoveMenuOut;
        if (MoveOut == false)
        {
            this.gameObject.GetComponent<S_WeaponNewWeaponMenu>().ClickTime = Time.time;
            this.gameObject.GetComponent<S_WeaponNewWeaponMenu>().MoveMenuOut = !MoveOut;
        }
        //Debug.Log("move out" + MoveOut);
        //Debug.Break();
        //this.gameObject.GetComponent<S_WeaponNewWeaponMenu>().DrawButtons(b);
        ////float DistanceToMove = MainWindow.Item2.height;
        //Rect testTO = MainWindow.Item2;
        ////Debug.Log("MoveUp : " + MoveUp);
        //if (MoveLeft)
        //{
        //    //Debug.Log("Weapon Menu Moveing Left");
        //    testTO.x -= (MainWindow.Item2.width - (Screen.width - MainWindow.Item2.x));
        //    to = new Vector2(testTO.x, testTO.y);
        //    TweenTime = TimeToMove - TweenTime;
        //    //Debug.Log("Left toTemp vec: " + to);
        //    ////tween.SetTween2D(to, TimeToMove, Time.deltaTime, MainWindow.Item2);
        //    //Debug.Log("Set tween done");
        //}
        //else
        //{
        //    //Debug.Log("Weapon Menu Moveing Right");
        //    //Rect testTO = MainWindow.Item2;
        //    testTO.x += ((Screen.width - MainWindow.Item2.x));
        //    to = new Vector2(testTO.x, testTO.y);
        //    TweenTime = TimeToMove - TweenTime;
        //    //Debug.Log("Right toTemp vec: " + to);
        //    //tween.SetTween2D(to, TimeToMove, Time.deltaTime, MainWindow.Item2);
        //    //Debug.Log("Set tween done");
        //}
        //MoveLeft = !MoveLeft;
    }

    public NTObjectPair<string, Rect> MainWindow = new NTObjectPair<string, Rect>();

    void OnGUI()
    {
        // Make a background box
        GUI.Box(MainWindow.Item2, MainWindow.Item1);

        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed

        for (int i = 0; i < WeaponArray.Length; i ++)
        {
            Texture ProjectileTexture = WeaponArray[i].GetComponent<GUITexture>().texture;
            int iAmmoCount = WeaponArray[i].GetComponent<S_GUI_Weapon>().AmmoCount;
            string sAmmoCount = iAmmoCount.ToString();
            if (iAmmoCount == -1)//if inf num of projectiles, print inf symbole
                sAmmoCount = "\u221E";
            if (GUI.Button(new Rect(MainWindow.Item2.x + 10, MainWindow.Item2.y + (30 * i), 80, 20), new GUIContent(sAmmoCount, ProjectileTexture)))
            {
                //Debug.Log(WeaponArray[i].name);
                WeaponArray[i].GetComponent<S_GUI_Weapon>().setCurrentWeapon();

                if(MoveRight)
                    MoveMenu();
            }
        }

        RectPositionSet(ref MainWindow.Item2, tween.Tween2D(MainWindow.Item2, to, Time.deltaTime, ref TweenTime));
    }
    
    void RectPositionSet(ref Rect rect, Vector2 value)
    {
        rect.x += value.x;
        rect.y += value.y;
    }
}
