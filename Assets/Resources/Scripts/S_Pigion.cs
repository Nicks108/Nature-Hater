//Author Nicholas David Thomas
//Email ndt597@googlemail.com
using System;
using System.Collections;
using System.Collections.Generic;
using NTTools;
using UnityEngine;

public class S_Pigion : S_EnemyBase
{
    private NTDebug Debug;

    
    //private float _currentFrame;
    //public float StartingFrame= 0f;
    //public float FramesPerSecond = 30f;

    //public List<Texture> WalkCycle;

    public GameObject BinImpactParticalsPrefab;

    public S_AnimationFrameSet deadAnimationSet = new S_AnimationFrameSet();
    public S_AnimationFrameSet diveAnimationSet = new S_AnimationFrameSet();
    public S_AnimationFrameSet flyAnimationSet = new S_AnimationFrameSet();
    public S_AnimationFrameSet hitAnimationSet = new S_AnimationFrameSet();
    public S_AnimationFrameSet stoodAnimationSet = new S_AnimationFrameSet();
    public S_AnimationFrameSet walkAnimationSet = new S_AnimationFrameSet();

    public List<Texture> DeadFrames;
    public List<Texture> DiveFrames;
    public List<Texture> FlyFrames;
    public List<Texture> HitFrames;
    public List<Texture> StoodFrames;
    public List<Texture> WalkFrames; 

    private bool _hitFlore = false;
    public bool HitFlore
    {
        get { return _hitFlore; }
        set { _hitFlore = value; }
    }


    public float WaitInDeadPoseTime = 1f;

    public float Scalefactor = 10;

    

    public GameObject ParticalSystems;


    public S_PlayRandomAudioClip ImpactAudio;
    public S_PlayRandomAudioClip CooAudio;
    public S_PlayRandomAudioClip ShitBintAudio;

     new void Awake()
    {
        //setup ntdebug
        Debug = new NTDebug(this.gameObject);

        base.Awake();
        //deadAnimationSet.FrameSet = S_2dSpriteBase.GetAllFramesForAnimation(Application.streamingAssetsPath + "\\Pigion\\dead", "*.png");
        //diveAnimationSet.FrameSet = S_2dSpriteBase.GetAllFramesForAnimation(Application.streamingAssetsPath + "\\Pigion\\dive", "*.png");
        //flyAnimationSet.FrameSet = S_2dSpriteBase.GetAllFramesForAnimation(Application.streamingAssetsPath + "\\Pigion\\fly", "*.png");
        //hitAnimationSet.FrameSet = S_2dSpriteBase.GetAllFramesForAnimation(Application.streamingAssetsPath + "\\Pigion\\hit", "*.png");
        //stoodAnimationSet.FrameSet = S_2dSpriteBase.GetAllFramesForAnimation(Application.streamingAssetsPath + "\\Pigion\\stood", "*.png");
        //walkAnimationSet.FrameSet = S_2dSpriteBase.GetAllFramesForAnimation(Application.streamingAssetsPath + "\\Pigion\\walk", "*.png");

         deadAnimationSet.FrameSet = DeadFrames;
         diveAnimationSet.FrameSet = DiveFrames;
         flyAnimationSet.FrameSet = FlyFrames;
         hitAnimationSet.FrameSet = HitFrames;
         stoodAnimationSet.FrameSet = StoodFrames;
         walkAnimationSet.FrameSet = WalkFrames;

        GameManager.SubscribeToEnemeyDiedEvent(this);

        ParticalSystems = this.gameObject.GetComponentInChildren<S_ParticalSystemChildren>().gameObject;            


         ReturnSpriteToPool += new ReturnSpriteToPoolhandeler(OnReturnToPool);
    }

	// Use this for initialization
	void Start () {
	
	}

    

	// Update is called once per frame
	void Update () {
        //System.Diagnostics.Stopwatch preformenceTimer = new System.Diagnostics.Stopwatch();
        //preformenceTimer.Start();
        //Debug.Log("pigion update");
        DoStateMacheanAI();
        PlayCurrentAnimation();

        if (waitAndFade)
        {
            FadeOverTime(1f);
        }
        //preformenceTimer.Stop();
        //Debug.Log("Pigion update time = "+preformenceTimer.ElapsedMilliseconds);
	}

    
    //OnTriggerStay(Collider collider)
    void OnTriggerEnter(Collider collider)
    {
        //System.Diagnostics.Stopwatch preformenceTimer = new System.Diagnostics.Stopwatch();
        //preformenceTimer.Start();


        string ColliderName; // need this as cant not change Collider.name. Well, i shouldnt atleased
        if (collider.name.Contains("ProjectileBase"))
            ColliderName = "ProjectileBase";
        else
            ColliderName = collider.name;

        Debug.Log("=====================Collider Name: " + ColliderName);
        Debug.Break();
        switch (ColliderName)
        {
                case "KillCube":
                    Debug.Log("kill cube collider");
                    //Destroy(this.gameObject);
                    RaisEnemyDied(this);
                    
                    OnReturnToPool();
                    break;
            case "ProjectileBase":
                    if (this.AttackState == (int)AttackStates.Dead || this.AttackState == (int)AttackStates.Die) break;
                    if (collider.gameObject.GetComponent<S_ProjectileBase>().isVisibal)
                    {
                        //play audio when hit with projectile.
                        ImpactAudio.PlayRandomClip();

                        Health -= collider.gameObject.GetComponent<S_ProjectileBase>().Damage;
                        if (this.Health <= 0)
                        {
                            this.AttackState = (int) AttackStates.Die;
                            RaisEnemyDied(this);
                        }
                    }
                    collider.GetComponent<S_ProjectileBase>().CheckIsOneKillOnly();
                     break;
                case "Player":
                    if (this.AttackState == (int)AttackStates.Die ||
                        this.AttackState == (int)AttackStates.Dead) break;
                    this.gameObject.SetActive(false);
                    collider.gameObject.GetComponent<S_Player>().TakeDamage(Damage);
                    break;
                case "BG_L2_Bin":
                    //Debug.Log("Bin hit");
                    //Debug.Log("current Attack state: " + (AttackStates)this.AttackState + " " + this.AttackState);
                    //Debug.Break();
                    if (this.AttackState == (int)AttackStates.Dead)
                    {
                        //play shit sounds
                        ShitBintAudio.PlayRandomClip();
                        this.GameManager.AddPoints(collider.GetComponent<S_ExtraPoints>().GetExtraPoints());
                        //Debug.Log("befor corourhtean");
                        //Debug.Log("Pigion renderer disabled");
                        this.GetComponent<Renderer>().enabled = false;
                        //this.GetComponent<S_SpriteShadow>().SetActive(false);

                        
                        //GameObject ParticalSystems = (GameObject)Instantiate(Resources.Load("Prefabs/ImpactParticalSystems"));
                        GameObject ParticalSystems = (GameObject)Instantiate(BinImpactParticalsPrefab);
                        ParticalSystems.transform.position = this.transform.position;
                        S_ParticalSystemChildren ParticalSystemChilderen = ParticalSystems.GetComponent<S_ParticalSystemChildren>();
                        ParticalSystemChilderen.SetSeed(0);
                        ParticalSystemChilderen.Simulate(0.005f, true);
                        ParticalSystemChilderen.Play(true);
                        CurrentAnimationSet = this.deadAnimationSet;
                        StartCoroutine(EnemyDie(WaitInDeadPoseTime));
                        //Debug.Log("after corourhtean");
                    }
                    break;
            }
        //preformenceTimer.Stop();
        //Debug.Log("Pigion on trigger enter time = "+preformenceTimer.ElapsedMilliseconds);
    }

    

   


    //AI States
    /// <summary>
    /// AI Idel State
    /// </summary>
    protected override void Idle()
    {
        //play Idle Animation
        CurrentAnimationSet = stoodAnimationSet;
        this.IsRandomAnimation = true;
    }

    

    protected override void MoveTowadsPoint()
    {
        this.transform.eulerAngles = new Vector3(0f, 0f, -17.3f);
        //UnityEngine.Debug.Log("moving to point");
        //CurrentAnimationSet = walkAnimationSet;
        CurrentAnimationSet = flyAnimationSet;
        //play move animation
        //moveTowadsPoint

        MoveTo(TargetToMoveTo, ZeroZ: true);
        AbsRotateToDirection(TargetToMoveTo);
        //this.transform.rotation = NTUtils.RatateToFacePoint(TargetToMoveTo,
            //this.transform, offsetAngle);
        //TODO set rotation of pigion so that is faces point
    }
    

    

    protected override void AttackPlayer()
    {
        //Debug.Log("new rotation: "+temp);
        if (Movingleft)
            offsetAngle = -10;
        else
            offsetAngle = 90;
        this.transform.rotation = NTUtils.RotateToFacePoint(GameManager.PlayerObjecttRef.transform.position,
            this.transform, offsetAngle);
        Speed = DiveSpeed;
        MoveTowardsPlayer(diveAnimationSet);
    }

    protected override void CelibratePlayerDeath()
    {
        //player Celibrate player death animation
        
    }

    protected override void Die()
    {
        //UnityEngine.Debug.Log("in pigion die");
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        //play death animation
        
        //CurrentAnimationSet = hitAnimationSet.PlayOnceAndMoveToAnimiation(deadAnimationSet);
        CurrentAnimationSet = hitAnimationSet;

        this.GetComponent<Rigidbody>().useGravity = true;

        this.AttackState = (int)AttackStates.Dead;

        //grerat new geomertery for blood and fethers
        //GameObject ParticalSystems = (GameObject)Instantiate(Resources.Load("Prefabs/PigionParticalSystems"));
        ParticalSystems.transform.position = this.transform.position;
        S_ParticalSystemChildren ParticalSystemChilderen = ParticalSystems.GetComponent<S_ParticalSystemChildren>();
        ParticalSystemChilderen.Simulate(0.005f, true);
        ParticalSystemChilderen.Play(true);
        //this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("=======================collision collider name: "+collision.collider.name);
        Debug.Break();
        switch (collision.collider.name)
        {
            case "FloreColiderCube":
                this.CurrentAnimationSet = deadAnimationSet;
                StartCoroutine(EnemyDie(WaitInDeadPoseTime));
                break;
        }

    }
    protected override void OnReturnToPool()
    {
        this.SetActive(false);
        this.rigidbody.velocity = Vector3.zero;
        this.rigidbody.useGravity = false;
        //UnityEngine.Debug.Log(this.name + "Pigion return to pool **************");
    }
 
}
