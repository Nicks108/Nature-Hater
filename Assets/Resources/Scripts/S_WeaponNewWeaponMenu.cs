using UnityEngine;
using System.Collections;
using NTTools;

public class S_WeaponNewWeaponMenu : MonoBehaviour
{
    public S_GUI_Weapon Weapon1;
    public S_GUI_Weapon Weapon2;
    public S_GUI_Weapon Weapon3;
    public S_GUI_Weapon Weapon4;

    private S_GameManager gameManager;

    public bool OverRidePlayerPos;
    public Vector2 OraginPosInScreenSpace;
    public bool UseScreenPercentage;
    public Vector2 ScreenPositionPercentage;

    public Texture Weapon1BackGround;
    public Texture Weapon2BackGround;
    public Texture Weapon3BackGround;
    public Texture Weapon4BackGround;

    private Texture Weapon1Texture;
    private Texture Weapon2Texture;
    private Texture Weapon3Texture;
    private Texture Weapon4Texture;

    public float Weapon1Size = 30;
    public float Weapon1BackGroundSize = 30;

    public float Weapon2Size = 30;
    public float Weapon2BackGroundSize = 30;

    public float Weapon3Size = 30;
    public float Weapon3BackGroundSize = 30;

    public float Weapon4Size = 30;
    public float Weapon4BackGroundSize = 30;

    public Vector2 ButtonStartPos;
    public Vector2 Button2Offset;
    public bool OverRideWeaponDisapearAnimation = false;
    private Vector2 Button2EndPos
    {
        get { return ButtonStartPos + Button2Offset; }
    }
    public Vector2 Button3Offset;
    private Vector2 Button3EndPos
    {
        get { return ButtonStartPos + Button3Offset; }
    }public Vector2 Button4Offset;
    private Vector2 Button4EndPos
    {
        get { return ButtonStartPos + Button4Offset; }
    }

    public S_PlayRandomAudioClip WeaponMenuChingSounds;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<S_GameManager>();

        Weapon1Texture = Weapon1.ProjectileTexture;
        Weapon2Texture = Weapon2.ProjectileTexture;
        Weapon3Texture = Weapon3.ProjectileTexture;
        Weapon4Texture = Weapon4.ProjectileTexture;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(!OverRidePlayerPos)
            OraginPosInScreenSpace = Camera.main.WorldToScreenPoint(gameManager.PlayerObjecttRef.transform.position);
        else if(UseScreenPercentage)
            OraginPosInScreenSpace = new Vector2(NTUtils.Percentage(ScreenPositionPercentage.x, Screen.width), NTUtils.Percentage(ScreenPositionPercentage.y, Screen.height));

        ButtonStartPos = new Vector2(OraginPosInScreenSpace.x, OraginPosInScreenSpace.y);
    }


    void OnMouseDown()
    {
        if (gameManager.PlayerIsDead) return;
        WeaponMenuChingSounds.PlayRandomClip();
        if (MoveMenuOut == false)
        {
            ClickTime = Time.time;
            MoveMenuOut = !MoveMenuOut;
        }
    }

    public GUIStyle ButtonStyle;
    public AnimationCurve OscillationCurve;
    public AnimationCurve WeaponMoveIn;
    public AnimationCurve WeaponMoveOut;
    public AnimationCurve WeaponSizeIn;
    public AnimationCurve WeaponSizeOut;
    private void OnGUI()
    {
        //if(SizeRatio>0)
        DrawButtons();
    }

    public float ClickTime=-1;
    public bool moveMenuOut = false;
    public bool MoveMenuOut
    {
        get { return moveMenuOut; }
        set
        {
            moveMenuOut = value;
            if(value == true)
                SizeRatio = 0.0001f;
        }
    }
    private float SizeRatio = 0;


    public S_PlayRandomAudioClip SelectionSounds;
    //public void DrawButtons(bool b)
    //{
    //    moveMenuOut = b;
    //    DrawButtons();
    //}
    public void DrawButtons()
    {
        bool Button1Press = false;
        bool Button2Press = false;
        bool Button3Press = false;
        bool Button4Press = false;
        //if (SizeRatio == 1)
        //    return;
        AnimationCurve currentAnimation;
        AnimationCurve currentSizeCurve;
        if (moveMenuOut)
        {
            currentAnimation = WeaponMoveIn;
            currentSizeCurve = WeaponSizeIn;
            //Debug.Log("in");

        }
        else
        {
            currentAnimation = WeaponMoveOut;
            currentSizeCurve = WeaponSizeOut;
           // Debug.Log("out");
        }

        SizeRatio = NTTween.Tween(0, 1, Time.time - ClickTime, currentSizeCurve);


        float WeaponTempSize = SizeRatio*Weapon1BackGroundSize;
        if (GUI.Button(new Rect(ButtonStartPos.x - (WeaponTempSize / 2),
                            (Camera.main.pixelHeight - ButtonStartPos.y) - (WeaponTempSize / 2),
                            WeaponTempSize, WeaponTempSize), Weapon1BackGround, ButtonStyle))
            Button1Press = true;
        //Debug.Log("OscillationCurve.Evaluate(Time.time): " + OscillationCurve.Evaluate((Weapon1Size-15)/(30-15)));
        //Weapon1Size = NTTween.Tween(15, 30, Time.time, OscillationCurve);
        WeaponTempSize = SizeRatio * Weapon1Size;
        if (GUI.Button(new Rect(ButtonStartPos.x - (WeaponTempSize / 2),
                            (Camera.main.pixelHeight - ButtonStartPos.y) - (WeaponTempSize / 2),
                            WeaponTempSize, WeaponTempSize), Weapon1Texture, ButtonStyle))
            Button1Press = true;


        float buttonXPos = NTTween.Tween(ButtonStartPos.x ,
            Button2EndPos.x, Time.time - ClickTime, currentAnimation);
        float buttonYPos = NTTween.Tween(ButtonStartPos.y ,
            Button2EndPos.y, Time.time - ClickTime, currentAnimation);
        WeaponTempSize = SizeRatio * Weapon2BackGroundSize;
        Rect buttonBackGroundPos = new Rect(buttonXPos - (WeaponTempSize / 2),
                                   (Camera.main.pixelHeight - buttonYPos) - (WeaponTempSize / 2),
                                   WeaponTempSize, WeaponTempSize);
        WeaponTempSize = SizeRatio * Weapon2Size;
        Rect buttonForeGroundPos = new Rect(buttonXPos - (WeaponTempSize / 2),
                                   (Camera.main.pixelHeight - buttonYPos) - (WeaponTempSize / 2),
                                   WeaponTempSize, WeaponTempSize);
        if (GUI.Button(buttonBackGroundPos, Weapon2BackGround, ButtonStyle))
            Button2Press = true;
        if (GUI.Button(buttonForeGroundPos, Weapon2Texture, ButtonStyle))
            Button2Press = true;



        buttonXPos = NTTween.Tween(ButtonStartPos.x,
            Button3EndPos.x, Time.time - ClickTime, currentAnimation);
        buttonYPos = NTTween.Tween(ButtonStartPos.y,
            Button3EndPos.y, Time.time - ClickTime, currentAnimation);
        WeaponTempSize = SizeRatio * Weapon3BackGroundSize;
        buttonBackGroundPos = new Rect(buttonXPos - (WeaponTempSize / 2),
                                   (Camera.main.pixelHeight - buttonYPos) - (WeaponTempSize / 2),
                                   WeaponTempSize, WeaponTempSize);
        WeaponTempSize = SizeRatio * Weapon3Size;
        buttonForeGroundPos = new Rect(buttonXPos - (WeaponTempSize / 2),
                                   (Camera.main.pixelHeight - buttonYPos) - (WeaponTempSize / 2),
                                   WeaponTempSize, WeaponTempSize);
        if (GUI.Button(buttonBackGroundPos, Weapon3BackGround, ButtonStyle))
            Button3Press = true;
        if (GUI.Button(buttonForeGroundPos, Weapon3Texture, ButtonStyle))
            Button3Press = true;


        buttonXPos = NTTween.Tween(ButtonStartPos.x,
            Button4EndPos.x, Time.time - ClickTime, currentAnimation);
        buttonYPos = NTTween.Tween(ButtonStartPos.y,
            Button4EndPos.y, Time.time - ClickTime, currentAnimation);
        WeaponTempSize = SizeRatio * Weapon4BackGroundSize;
        buttonBackGroundPos = new Rect(buttonXPos - (WeaponTempSize / 2),
                                   (Camera.main.pixelHeight - buttonYPos) - (WeaponTempSize / 2),
                                   WeaponTempSize, WeaponTempSize);
        WeaponTempSize = SizeRatio * Weapon4Size;
        buttonForeGroundPos = new Rect(buttonXPos - (WeaponTempSize / 2),
                                   (Camera.main.pixelHeight - buttonYPos) - (WeaponTempSize / 2),
                                   WeaponTempSize, WeaponTempSize);
        if (GUI.Button(buttonBackGroundPos, Weapon4BackGround, ButtonStyle))
            Button4Press = true;
        if (GUI.Button(buttonForeGroundPos, Weapon4Texture, ButtonStyle))
            Button4Press = true;

        
        if (Button1Press || Button2Press || Button3Press || Button4Press)
        {
            SelectionSounds.PlayRandomClip();
        }
        if(Button1Press)
        {
            Weapon1.setCurrentWeapon();
            if (!OverRideWeaponDisapearAnimation)
            {
                moveMenuOut = false;
                ClickTime = Time.time;
            }
        }
        if(Button2Press)
        {
            Weapon2.setCurrentWeapon();
            if (!OverRideWeaponDisapearAnimation)
            {
                moveMenuOut = false;
                ClickTime = Time.time;
            }
        }
        if(Button3Press)
        {
            Weapon3.setCurrentWeapon();
            if (!OverRideWeaponDisapearAnimation)
            {
                moveMenuOut = false;
                ClickTime = Time.time;
            }
        }
        if(Button4Press)
        {
            Weapon4.setCurrentWeapon();
            if (!OverRideWeaponDisapearAnimation)
            {
                moveMenuOut = false;
                ClickTime = Time.time;
            }
        }
    }

}
