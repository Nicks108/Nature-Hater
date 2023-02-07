//Author Nicholas David Thomas
//Email ndt597@googlemail.com
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using NTTools;

public class S_Player : S_2dSpriteBase{
    
    NTDebug Debug;

    private Vector3 LastMouseEventPosition;

    public S_AnimationFrameSet WalkAnimation = new S_AnimationFrameSet();
    public S_AnimationFrameSet ThrowAnimation = new S_AnimationFrameSet();
    public S_AnimationFrameSet StoodAnimation = new S_AnimationFrameSet();
    public S_AnimationFrameSet SquatAnimation = new S_AnimationFrameSet();
    public S_AnimationFrameSet JumpAnimation = new S_AnimationFrameSet();
    public S_AnimationFrameSet HurtAnimation = new S_AnimationFrameSet();
    public S_AnimationFrameSet BuildAniamtion = new S_AnimationFrameSet();
    public S_AnimationFrameSet DeadAniamtion = new S_AnimationFrameSet();

    public List<Texture> WalkFrames;
    public List<Texture> ThrowFrames;
    public List<Texture> StoodFrames; 
    public List<Texture> SquatFrames; 
    public List<Texture> JumpFrames;
    public List<Texture> HurtFrames; 
    public List<Texture> BuildFrames;
    public List<Texture> DeadFrames;

    public float Scalefactor= 10;


    public LineRenderer AimLine;

    public Vector3 ShotDirectionVector;
    private System.Collections.Generic.List<Vector3> positions;

    //public float ProjectileResistance = 1;

    public S_GUI_Weapon weaponGUIScript;
    public GameObject Projectile;
    //public float ProjectileDamage;
    //public Texture ProjectileTexture;
    //public float ColiderRadius;
    //public float Bouncibility;

    public GameObject ArrowHead;
    public GameObject ArrowLine;

    public bool DebugLine =  true;
    public bool DebugAimArrow = true;

    public GameObject AimCursor;
    public float AimCursorRotationOffset = 0;

    public S_PlayRandomAudioClip RandomHurtAudio;
    public S_PlayRandomAudioClip RandomThrowAudio;
    public S_PlayRandomAudioClip RandomOutOfAmmoClickAudio;

    public bool DeadAnimationPLayed = false;
    public float TimeInDyingPose = 1;

    public bool WalkOffScreen = false;
    public enum WalkDirections
    {
        Left = 0,
        Right = 1
    }
    public WalkDirections WalkDirection = WalkDirections.Right;

	// Use this for initialization
	void Start () {
       Debug = new NTDebug(this.gameObject);


        //chech for Line Renderer
        if (AimLine == null)
        {
            //create new game object
            GameObject newLinerendere = new GameObject("PlayerAimLine");
            //add line renderer componant
            newLinerendere.AddComponent<LineRenderer>();
            //assigne new game object as Aim line 
            AimLine = newLinerendere.GetComponent<LineRenderer>();
            AimLine.SetWidth(0.03f, 0.03f);
        }

        //set up first animation
        this.CurrentAnimationSet = StoodAnimation;

        //get a weapon (fake a selection) so player has something to throw straight away and there are no Null pointer errors
        GameObject.Find("GUI WEAPNON EGG").GetComponent<S_GUI_Weapon>().setCurrentWeapon();

	    RandomHurtAudio = GameObject.Find("PlayerHurtAudio").GetComponent<S_PlayRandomAudioClip>();
        RandomThrowAudio = GameObject.Find("PlayerThrowAudio").GetComponent<S_PlayRandomAudioClip>();
        RandomOutOfAmmoClickAudio = GameObject.Find("PlayerOutOfAmmoClickAudio").GetComponent<S_PlayRandomAudioClip>();
	}


    new void Awake()
    {
        base.Awake();

    //    WalkAnimation
        //Debug.Log("Load Files");
        //WalkAnimation.FrameSet = S_2dSpriteBase.GetAllFramesForAnimation(Application.streamingAssetsPath + "\\Main Charicter\\walk", "*.png");
        //ThrowAnimation.FrameSet = S_2dSpriteBase.GetAllFramesForAnimation(Application.streamingAssetsPath + "\\Main Charicter\\throw", "*.png");
        //StoodAnimation.FrameSet = S_2dSpriteBase.GetAllFramesForAnimation(Application.streamingAssetsPath + "\\Main Charicter\\stood", "*.png");
        //SquatAnimation.FrameSet = S_2dSpriteBase.GetAllFramesForAnimation(Application.streamingAssetsPath + "\\Main Charicter\\squat", "*.png");
        //JumpAnimation.FrameSet = S_2dSpriteBase.GetAllFramesForAnimation(Application.streamingAssetsPath + "\\Main Charicter\\jump", "*.png");
        //HurtAnimation.FrameSet = S_2dSpriteBase.GetAllFramesForAnimation(Application.streamingAssetsPath + "\\Main Charicter\\hurt", "*.png");
        //BuildAniamtion.FrameSet = S_2dSpriteBase.GetAllFramesForAnimation(Application.streamingAssetsPath + "\\Main Charicter\\build", "*.png");


        WalkAnimation.FrameSet = WalkFrames;
        ThrowAnimation.FrameSet = ThrowFrames;
        StoodAnimation.FrameSet = StoodFrames;
        SquatAnimation.FrameSet = SquatFrames;
        JumpAnimation.FrameSet = JumpFrames;
        HurtAnimation.FrameSet = HurtFrames;
        BuildAniamtion.FrameSet = BuildFrames;
        DeadAniamtion.FrameSet = DeadFrames;
    }

    
	// Update is called once per frame
    void Update()
    {
        //UnityEngine.Debug.Log("MYDEBUGMESSAGE:in player update");
        if (GameManager.PlayerIsDead)
        {
            if(DeadAnimationPLayed == false)
                StartCoroutine(PlayDeadAnimation());
        }
        else
        {
            PlayCurrentAnimation();    
        }
        
        
        if(WalkOffScreen)
        {
            CurrentAnimationSet = WalkAnimation;
            FramesPerSecond = 10; //TODO fix this!!! should be done in inspector, not in code!!!
            if(WalkDirection == WalkDirections.Right)
            {
                this.Move(Vector3.left);
                ChangeWalkDirection();
            }
            else
            {
                this.Move(Vector3.right);
                ChangeWalkDirection();
            }
        }

//#if UNITY_ANDROID
//        foreach (Touch t in Input.touches)
//        {
//            //UnityEngine.Debug.Log("MYDEBUGMESSAGE:this is unity android");
//            if(t.phase == TouchPhase.Began)
//            {
//                UnityEngine.Debug.Log("MYDEBUGMESSAGE:touch began");
//                //this.OnMouseDown();
//            }
//            if(t.phase == TouchPhase.Ended)
//            {
//                UnityEngine.Debug.Log("MYDEBUGMESSAGE:touch ended");
//                //this.OnMouseUp();
//            }
//        }

//#endif
    }

    private bool HasShiftedForDeadPose = false;
    IEnumerator PlayDeadAnimation()
    {
        
        this.renderer.material.mainTexture = DeadAniamtion.GetFrame(0);
        yield return new WaitForSeconds(TimeInDyingPose);

        if(!HasShiftedForDeadPose) 
        {
            this.transform.position = new Vector3(this.transform.position.x + -0.059309f, this.transform.position.y + -0.0165729f, this.transform.position.z);
            HasShiftedForDeadPose = true;
        } 
        this.renderer.material.mainTexture = DeadAniamtion.GetFrame(1);
        DeadAnimationPLayed = true;
    }


    void OnMouseUp()
    {
        if(GameManager.PlayerIsDead)return;
        if (!GameManager.GameCanPlay) return;
        if (GameManager.GameFinished)return;

        Debug.Log("setting throw animation");
        CurrentAnimationSet = ThrowAnimation.PlayOnceAndMoveToAnimiation(StoodAnimation);
        if (DebugAimArrow)
        {
            AimLine.enabled = false;
            AimLine.SetVertexCount(1);
            AimLine.SetPosition(0, this.transform.position);
        }
        AimCursor.SetActive(false);
        //spawn new butllet

        //GameObject NewProjectile = (GameObject)Instantiate(Projectile);
        //NewProjectile.GetComponent<S_ProjectileBase>().SetUp(ProjectileDamage, ProjectileTexture, ColiderRadius);

        //NewProjectile.transform.position = this.transform.position;
        //NewProjectile.tag = "Projectile";

        //S_ProjectileBase NewProjectileScript = (S_ProjectileBase) NewProjectile.GetComponent(typeof(S_ProjectileBase));
        //NewProjectileScript.TrajectoryPath = positions;

        if (weaponGUIScript.AmmoCount > 0 || weaponGUIScript.AmmoCount == -1)
        {
            RandomThrowAudio.PlayRandomClip();
            GameObject NewProjectile = (GameObject)Instantiate(Projectile);
            Rigidbody rigbody = NewProjectile.GetComponent<Rigidbody>();
            rigbody.velocity = ShotDirectionVector;
            rigbody.useGravity = weaponGUIScript.UseGravity;
            NewProjectile.GetComponent<S_ProjectileBase>().SetUp(weaponGUIScript.ProjectileDamage, weaponGUIScript.ProjectileTexture,
                weaponGUIScript.ColiderRadius, weaponGUIScript.Bouncibility, weaponGUIScript.isKillOneEnimyOnly, weaponGUIScript.TimeToKill);
            NewProjectile.transform.position = this.transform.position;
            NewProjectile.tag = "Projectile";
            GameManager.SubTractPointPoints(weaponGUIScript.PointsDeductionPerShoot);
            if (weaponGUIScript.AmmoCount != -1)
                weaponGUIScript.AmmoCount--;

            //GameObject.Find("SelectedWeaponGUITexture").GetComponent<S_PerantClick>().GUItext = weaponGUIScript.AmmoCount.ToString();
        }
        else
        {
            RandomOutOfAmmoClickAudio.PlayRandomClip();
        }

        if (DebugAimArrow)
        {
            ArrowHead.SetActive(false);
            ArrowLine.SetActive(false);
        }

        AimCursor.SetActive(false);
    }

    void OnMouseDrag()
    {
        if (GameManager.PlayerIsDead) return;
        if (!GameManager.GameCanPlay) return;
        if (GameManager.GameFinished) return;

        float DistanceFromPlayerToMainCam = Vector3.Distance(this.transform.position, Camera.main.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 MousePositionInWorldSpace = ray.origin + (ray.direction * DistanceFromPlayerToMainCam);

        Ray oldMousePosRay = Camera.main.ScreenPointToRay(LastMouseEventPosition);
        Vector3 LastMouseDownPosToWorldSpace = oldMousePosRay.origin + (oldMousePosRay.direction * DistanceFromPlayerToMainCam);

        float ArrowDistFromPlayer = Vector3.Distance(MousePositionInWorldSpace, LastMouseDownPosToWorldSpace);
        Color ColorStrengthOfArrow = new Color(0 + ArrowDistFromPlayer, 1 - ArrowDistFromPlayer, 0f, 1f);
        //UnityEngine.Debug.Log(" ArrowDistFromPlayer: " + ArrowDistFromPlayer);

        AimCursor.GetComponent<S_AimArrowTipScript>().rotateArrowTipToMatchOppositFingurePositionsFromPlayer(MousePositionInWorldSpace, AimCursorRotationOffset);
        //AimCursor.GetComponent<S_AimArrowTipScript>().positionArrowTipAtOppositSideOfPlayerRelativeToMousePoint(Input.mousePosition, this.transform.position);
        AimCursor.renderer.material.color = ColorStrengthOfArrow;

        if (AimCursor.activeSelf == false)
            AimCursor.SetActive(true);

        Vector3 ShotDirection = GetShotDirectionVector(Input.mousePosition, LastMouseEventPosition).normalized;


        Debug.Log("shot direction normalized: " + ShotDirection);
        Debug.Log("get shotdirection vecotr: " + GetShotDirectionVector(Input.mousePosition, LastMouseEventPosition));
        if (DebugAimArrow)
        {
            //set up arrow
            ArrowHead.GetComponent<S_AimArrowTipScript>().rotateArrowTipToMatchOppositFingurePositionsFromPlayer(this.transform.position);
            ArrowHead.GetComponent<S_AimArrowTipScript>().positionArrowTipAtOppositSideOfPlayerRelativeToMousePoint(Input.mousePosition, this.transform.position);
            ArrowLine.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
            ArrowLine.GetComponent<LineRenderer>().SetPosition(1, ArrowHead.transform.position);

            //get arrow head distance

            ArrowHead.renderer.material.color = ColorStrengthOfArrow;
            ArrowLine.GetComponent<LineRenderer>().renderer.material.color = ColorStrengthOfArrow;

            if (ArrowHead.activeSelf == false)
            {
                ArrowHead.SetActive(true);
                ArrowLine.SetActive(true);
            }
        }


        //end arrow setup


        //Debug.Log(Vector3.Distance(Input.mousePosition, LastMouseEventPosition));
        float ShotStrength = ((LastMouseEventPosition - Input.mousePosition).magnitude / weaponGUIScript.ProjectileResistance);
        if (weaponGUIScript.MaxShootStrength != -1)
        {
            if (ShotStrength > weaponGUIScript.MaxShootStrength)
            {
                ShotStrength = weaponGUIScript.MaxShootStrength;
            }
        }
        if (weaponGUIScript.MinShootStrength != -1)
        {
            if (ShotStrength < weaponGUIScript.MinShootStrength)
            {
                ShotStrength = weaponGUIScript.MinShootStrength;
            }
        }

        ShotDirectionVector = ShotDirection * ShotStrength;
        ShotDirectionVector.y *= -1;
        ShotDirectionVector.x *= 1f;
        ShotDirectionVector *= 1f;


        if (DebugLine)
        {
            //time of flight calculations
            float TimeOfFlight;
            TimeOfFlight = (ShotDirectionVector.y * -1) - Mathf.Sqrt((ShotDirectionVector.y * ShotDirectionVector.y) - 2 * (this.transform.position.y * Physics.gravity.y));
            if (weaponGUIScript.UseGravity)
                TimeOfFlight = TimeOfFlight / Physics.gravity.y;

            //Debug.Log("position differnce "+PositionDiffernce);
            //TimeOfFlight = TimeOfFlight * (PositionDiffernce);
            //Debug.Log("time of flight " + TimeOfFlight);

            // Number of vertices to calculate - more gives a smoother line
            int NumVertices = 120;
            //int NumVertices = 30 / (Convert.ToInt32(Vector3.Distance(Input.mousePosition, LastMouseEventPosition))/100);
            positions = new System.Collections.Generic.List<Vector3>();

            // The first line point is wherever the player's cannon, etc is
            positions.Add(this.transform.position);

            //float segTime = TimeOfFlight / NumVertices;
            Vector3 gravity = Physics.gravity;
            if (!weaponGUIScript.UseGravity)
                gravity = Vector3.zero;

            for (int i = 1; i < NumVertices; i++)
            {
                float segTime = (TimeOfFlight / (NumVertices - 1)) * (i);
                positions.Add(positions[0] + (ShotDirectionVector * segTime) + (0.5f * gravity * (segTime * segTime)));
            }

            AimLine.SetVertexCount(NumVertices);
            for (int j = 0; j < NumVertices; j++)
            {
                AimLine.SetPosition(j, positions[j]);
            }
        }
    }

    void OnMouseDown()
    {
        if (GameManager.PlayerIsDead) return;
        if(!GameManager.GameCanPlay)return;
        if (GameManager.GameFinished) return;

        AimCursor.SetActive(true);
        CurrentAnimationSet = BuildAniamtion;
        //Debug.Log("setting build animation");
        LastMouseEventPosition = Input.mousePosition;
        if (DebugAimArrow)
        {
            AimLine.enabled = true;
        }
    }


    public void PLayWalkAnimation()
    {
        WalkOffScreen = true;
    }

    public bool WalkDirectionChanged = true;
    private void ChangeWalkDirection()
    {
        if(!WalkDirectionChanged) return;
        if (WalkDirection == WalkDirections.Right)
            this.transform.Rotate(Vector3.forward, 0f);
        else
            this.transform.Rotate(Vector3.forward, 180f);
        WalkDirectionChanged = false;
    }
    

    Vector3 GetShotDirectionVector(Vector3 a, Vector3 b)
    {
        Vector3 c = a - b;
        return c;
    }

    S_AnimationFrameSet FramesetBeforHurt;
    public S_Health HealthBar;
    public void TakeDamage(int damage)
    {
        Debug.Log("player taking damage");
        
        if(Health >0)
        {
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
                Handheld.Vibrate();
#endif
            RandomHurtAudio.PlayRandomClip();
            if (CurrentAnimationSet != HurtAnimation)
                FramesetBeforHurt = CurrentAnimationSet;
            CurrentAnimationSet = HurtAnimation.PlayOnceAndMoveToAnimiation(FramesetBeforHurt);
        }
        Health -= damage;
        
        if (Health <= 0)
        {
            Health = 0;
            GameManager.PlayerIsDead = true;
            //play death sound?
        }
        HealthBar.HealthChange(Health);

        
        
        

        
    }
    //private IEnumerator WaitAndChangeAnimation(float WaitTime, S_AnimationFrameSet NewSet)
    //{
    //    S_AnimationFrameSet originalSet = CurrentAnimationSet;
    //    CurrentAnimationSet = NewSet;

    //}

    //void OnTriggerEnter(Collider collider)
    //{
    //    string ColliderName; // need this as cant not change Collider.name. Well, i shouldnt atleased
    //    if (collider.name.Contains("ProjectileBase"))
    //        ColliderName = "ProjectileBase";
    //    else
    //        ColliderName = collider.name;

    //    Debug.Log("Collider Name: " + ColliderName);
    //    Debug.Break();
    //    //if(ColliderName.Contains(""))
    //    //{
            
    //    //    Debug.Log("pigion hit player");
            
    //    //}
    //    if (collider.tag == "Enemey")
    //    {
    //        Debug.Log("pigion hit player");
    //        Debug.Break();
    //    }
    //}
}
