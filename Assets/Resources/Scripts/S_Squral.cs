using System.Collections.Generic;
using NTTools;
using UnityEngine;

public class S_Squral : S_EnemyBase {
    private NTDebug Debug;

    public S_AnimationFrameSet DeadAnimationSet = new S_AnimationFrameSet();
    public S_AnimationFrameSet AttackAnimationSet = new S_AnimationFrameSet();
    public S_AnimationFrameSet DyingAnimationSet = new S_AnimationFrameSet();
    public S_AnimationFrameSet RunAnimationSet = new S_AnimationFrameSet();

    public List<Texture> DeadFrames;
    public List<Texture> AttackFrames;
    public List<Texture> DyingFrames;
    public List<Texture> RunFrames;

    public bool _hitFlore = false;

    public float WaitInDeadPoseTime = 1f;

    public float Scalefactor = 10;

    public GameObject ParticalSystems;
    public float ArcHight = 5;

    //public S_PlayRandomAudioClip ImpactAudio;
    //public S_PlayRandomAudioClip CooAudio;
    //public S_PlayRandomAudioClip ShitBintAudio;

    void Awake()
    {
        //setup ntdebug
        Debug = new NTDebug(this.gameObject);

        base.Awake();

        DeadAnimationSet.FrameSet = DeadFrames;
        AttackAnimationSet.FrameSet = AttackFrames;
        DyingAnimationSet.FrameSet = DyingFrames;
        RunAnimationSet.FrameSet = RunFrames;

        GameManager.SubscribeToEnemeyDiedEvent(this);

        ParticalSystems = this.gameObject.GetComponentInChildren<S_ParticalSystemChildren>().gameObject;


        ReturnSpriteToPool += new ReturnSpriteToPoolhandeler(OnReturnToPool);
    }

    protected override void OnReturnToPool()
    {
        this.SetActive(false);
        this.rigidbody.velocity = Vector3.zero;
        this.rigidbody.useGravity = false;
        this.setVelocityOnce = false;
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        //UnityEngine.Debug.Log(this.name+" doing squirel return to pool");
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        DoStateMacheanAI();
        PlayCurrentAnimation();

        if (waitAndFade)
        {
            FadeOverTime(1f);
        }
	}


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
                    //ImpactAudio.PlayRandomClip();

                    Health -= collider.gameObject.GetComponent<S_ProjectileBase>().Damage;
                    if (this.Health <= 0)
                    {
                        this.AttackState = (int)AttackStates.Die;
                        RaisEnemyDied(this);
                    }
                }
                collider.GetComponent<S_ProjectileBase>().CheckIsOneKillOnly();
                break;
            case "Player":
                if (this.AttackState == (int)AttackStates.Die ||
                    this.AttackState == (int)AttackStates.Dead) break;
                this.gameObject.SetActive(false);
                OnReturnToPool();
                collider.gameObject.GetComponent<S_Player>().TakeDamage(Damage);
                break;
            case "BG_L2_Bin":
                //Debug.Log("Bin hit");
                //Debug.Log("current Attack state: " + (AttackStates)this.AttackState + " " + this.AttackState);
                //Debug.Break();
                if (this.AttackState == (int)AttackStates.Dead)
                {
                    //play shit sounds
                    //ShitBintAudio.PlayRandomClip();
                    this.GameManager.AddPoints(collider.GetComponent<S_ExtraPoints>().GetExtraPoints());
                    //Debug.Log("befor corourhtean");
                    //Debug.Log("Pigion renderer disabled");
                    this.GetComponent<Renderer>().enabled = false;
                    //this.GetComponent<S_SpriteShadow>().SetActive(false);

                    GameObject ParticalSystems = (GameObject)Instantiate(Resources.Load("Prefabs/ImpactParticalSystems"));
                    ParticalSystems.transform.position = this.transform.position;
                    S_ParticalSystemChildren ParticalSystemChilderen = ParticalSystems.GetComponent<S_ParticalSystemChildren>();
                    ParticalSystemChilderen.Simulate(0.005f, true);
                    ParticalSystemChilderen.Play(true);
                    CurrentAnimationSet = this.DeadAnimationSet;
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
        CurrentAnimationSet = RunAnimationSet;
        this.IsRandomAnimation = true;
    }

    protected override void MoveTowadsPoint()
    {
        //UnityEngine.Debug.Log("moving to point");
        //CurrentAnimationSet = walkAnimationSet;
        CurrentAnimationSet = RunAnimationSet;
        //play move animation
        //moveTowadsPoint

        MoveTo(TargetToMoveTo, ZeroZ: true);
        AbsRotateToDirection(TargetToMoveTo);
        //this.transform.rotation = NTUtils.RatateToFacePoint(TargetToMoveTo,
        //this.transform, offsetAngle);
        //TODO set rotation of pigion so that is faces point
    }

    private bool setVelocityOnce = false;
    public float Gravity = Physics.gravity.y;

    protected override void AttackPlayer()
    {

        Speed = DiveSpeed;
        CurrentAnimationSet = AttackAnimationSet;
        this.rigidbody.useGravity = false;

        this.transform.rotation = NTUtils.RotateToFacePoint(GameManager.PlayerObjecttRef.transform.position,
            this.transform);

        if (!setVelocityOnce)
        {
            //UnityEngine.Debug.Log(this.name+" setting velocity");
            float DistanceToTarge = Vector3.Distance(this.transform.position,
                                                 GameManager.PlayerObjecttRef.transform.position);
            float gravityDifference = Physics.gravity.y/Gravity;
            //UnityEngine.Debug.Log(this.name + " gravityDifference " + gravityDifference);
            Vector3 velocity = new Vector3();
            velocity.x = Mathf.Sqrt((2 * Mathf.Pow(DistanceToTarge, 2) * Mathf.Abs(Gravity)) / ArcHight) /(3);
            velocity.y = Mathf.Sqrt((ArcHight * Mathf.Abs(Gravity))/4 );

            rigidbody.rigidbody.velocity = velocity;
            //rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y - (0.5f * Gravity) * Time.deltaTime, rigidbody.velocity.z);
            setVelocityOnce = true;
        }
        else
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y + (0.5f * Gravity) * Time.deltaTime, rigidbody.velocity.z);    
        }
        
    }


    protected override void CelibratePlayerDeath()
    {
        //player Celibrate player death animation

    }

    protected override void Die()
    {
        //Debug.Log("in pigion die");
        
        //play death animation

        //CurrentAnimationSet = hitAnimationSet.PlayOnceAndMoveToAnimiation(deadAnimationSet);
        CurrentAnimationSet = DyingAnimationSet;
        transform.eulerAngles = new Vector3(0f, 0f, -90f);

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
        Debug.Log("=======================collision collider name: " + collision.collider.name);
        Debug.Break();
        //UnityEngine.Debug.Log(this.name+" On Colision");
        
        switch (collision.collider.name)
        {
            case "FloreColiderCube":
                if (attackState == AttackStates.AttackPlayer)
                {
                    attackState = AttackStates.Dead;
                }
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                //UnityEngine.Debug.Log(this.name + " On Colision, in case");
                this.CurrentAnimationSet = DeadAnimationSet;
                this.rigidbody.useGravity = false;
                StartCoroutine(EnemyDie(WaitInDeadPoseTime));
                break;
        }
    }
}
