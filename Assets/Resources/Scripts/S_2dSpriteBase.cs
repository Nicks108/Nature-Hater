//Author Nicholas David Thomas
//Email ndt597@googlemail.com
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using NTTools.Path;

public class S_2dSpriteBase : MonoBehaviour {

    public delegate void ReturnSpriteToPoolhandeler();
    public event ReturnSpriteToPoolhandeler ReturnSpriteToPool;
    private NTTools.NTDebug Debug;

    public float randomFramTimeMinRange = 5;
    public float randomFramTimeMaxRange = 20;

    protected S_GameManager GameManager;
    public int Health = 100;
    public float Speed = 0.5f;
    public static float DefaultSpeed = 0.5f;
    public float DiveSpeed = 0.8f;

    public bool Movingleft;

    public bool isVisibal;

    protected Vector3 _targetToMoveTo;
    public Vector3 TargetToMoveTo
    {
        get { return _targetToMoveTo; }
        set 
        { 
            _targetToMoveTo = value;
        }
    }
    public GameObject TargetToAttack;

    public enum AttackStates
    {
        Donothing =-1,
        Idle,
        MoveTowardsPlayer,
        MoveTowardsPoint,
        AttackPlayer,
        CelibratePlayerDeath,
        Die,
        Dead,
        ReturnToPool
    };
    public AttackStates attackState = AttackStates.Idle;
    public int AttackState
    {
        get { return (int) attackState; }
        set
        {
           //UnityEngine.Debug.Log(this.name);
            //UnityEngine.Debug.Log("AttackState changing: Old:" + AttackState+" New: "+ value);
            //UnityEngine.Debug.Log(value);
            attackState = (AttackStates)value;
            //UnityEngine.Debug.Log(AttackState);
            //UnityEngine.Debug.Break();
        }
    }

    public S_PathPoint currentPathPoint;
    
    public S_Path Path;
    public S_EnemyPool EnemyPoolRef;

    public int Points = 10;
    public int Damage = 10;

    //event
    public event EnemyDiedEvent EnemyDied;
    public EventArgs e = null;
    public delegate void EnemyDiedEvent(S_2dSpriteBase EnemyScript, EventArgs e);
    //end of event

    protected bool waitAndFade = false;

    public void RaisEnemyDied(S_2dSpriteBase raiser)
    {
        EnemyDied(raiser, e);
    }


    protected void FadeOverTime(float Time)
    {
        float r = this.renderer.material.color.r;
        float g = this.renderer.material.color.g;
        float b = this.renderer.material.color.b;
        float a = this.renderer.material.color.a;

        a += 2f;

        this.renderer.material.color = new Color(r, g, b, a);
    }
    protected IEnumerator EnemyDie(float t)
    {
        this.GetComponent<Rigidbody>().useGravity = false;
        this.rigidbody.velocity = Vector3.zero;
        //this.GetComponent<Rigidbody>().useGravity = false;
        //this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        

        //Destroy(this);
        //Debug.Log("Pigion flore collider befor yield");
        //Debug.Log("time "+ Time.realtimeSinceStartup);
        yield return new WaitForSeconds(t);
        //Debug.Log("Pigion flore collider after yield");
        //Debug.Log("time " + Time.realtimeSinceStartup);
        //this.SetActive(false);
        OnReturnToPool();
        //UnityEngine.Debug.Log(this.name+ " returning to pool");
        waitAndFade = true;
    }
    protected void Awake()
    {
        //setup my debugger
        //UnityEngine.Debug.Log("seting up debugger");
        Debug = new NTTools.NTDebug(this.gameObject);

        GameManager = GameObject.Find("GameManager").GetComponent<S_GameManager>();
        
        if (TargetToAttack == null)
        {
            TargetToAttack = GameObject.Find("Player");
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //DoStateMacheanAI();
	}

    //load images
    //animation
    public static List<Texture> GetAllFramesForAnimation(string path, string fileType)
    {

        return NTTools.Image.GetAllFramesForAnimation(path, fileType);
        ////Debug.Log("Loading from: " + path);
        //List<Texture> AnimationCollection = new List<Texture>();
        //string[] Files = Directory.GetFiles(path, fileType);

        ////Debug.Log(Files.Length);

        //foreach (string file in Files)
        //{
        //    AnimationCollection.Add(GetSingleImage(file));
        //}
        //return AnimationCollection;
    } 
    //static
    public static Texture GetSingleImage(string path, string fileType)
    {
        return NTTools.Image.GetSingleImage(path, fileType);
       // return GetSingleImage(path + fileType);
    }
    public static Texture GetSingleImage(string FileAndType)
    {
        return NTTools.Image.GetSingleImage(FileAndType);
        ////Debug.Log("Loading from: " + path);
        
        //WWW www = new WWW("file://" + FileAndType);
        //while (!www.isDone)
        //{
        //    //Debug.Log("loading");
        //}
        ////Debug.Log("Load compleat: " + file);
        //return www.texture;
    } 

    //2d rotate

    public void SetTargetToMoveTo(GameObject TargetObject)
    {
        SetTargetToMoveTo(TargetObject.transform.position);
    }

    public void SetTargetToMoveTo(Vector3 point)
    {
        TargetToMoveTo = point;
    }

    protected void DoStateMacheanAI()
    {
        //Debug.Log("do statmachean");
        //UnityEngine.Debug.Log(this.name+ " attack state = "+ Enum.GetName(typeof(AttackStates), AttackState));
        //IEnumerable ReturnValue = null;
        this.IsRandomAnimation = false;

        switch (AttackState)
        {
            case (int)AttackStates.Idle:
                Idle();
                break;
            case (int)AttackStates.MoveTowardsPlayer:
                //FollowPath();
                MoveTowardsPlayer();
                break;
            case (int)AttackStates.MoveTowardsPoint:
                //UnityEngine.Debug.Log("moving towards point");
                FollowPath();
                MoveTowadsPoint();
                break;
            case (int)AttackStates.AttackPlayer:
                //FollowPath();
                //UnityEngine.Debug.Log("attacking player");
                AttackPlayer();
                break;
            case (int)AttackStates.CelibratePlayerDeath:
                CelibratePlayerDeath();
                break;
            case (int)AttackStates.Die:
                //Debug.Log("case die");
                Die();
                break;
            case (int)AttackStates.ReturnToPool:
                ReturnSpriteToPool();
                break;
            default:
                //do nothing or Dead states
                break;
        }
    }

    void FollowPath()
    {
        //distance from target node is less than threshhold distance
        //Debug.Log(this.gameObject.transform.position);
        Debug.Log("following path");
        Debug.Log("current target "+currentPathPoint.gameObject.name);
        Debug.Log("distance to target " + Vector3.Distance(this.gameObject.transform.position, currentPathPoint.transform.position));
        //Debug.Log("distance to target + tweek " + (Vector3.Distance(this.gameObject.transform.position, currentPathPoint.transform.position) ));
        Debug.Log("distance threshold " + GameManager.TargetSwitchDistanceThreshold);
        if (Vector3.Distance(this.gameObject.transform.position, currentPathPoint.transform.position)  < GameManager.TargetSwitchDistanceThreshold)
        {
            Debug.Log("Changeing path point");
            //get index of next node by getting index of this node and adding 1
            int NextPathPointIndex = Path.PathPoints.IndexOf(currentPathPoint);
            //Debug.Log("index of current node: "+NextPathPointIndex);
            //Debug.Log("path point count: " + Path.PathPoints.Count);
            if (NextPathPointIndex < Path.PathPoints.Count-1)
                NextPathPointIndex++;

            //Debug.Log("index of New node: " + NextPathPointIndex);

            Debug.Log("current pathpoint type: " + currentPathPoint.PointType);
            switch (currentPathPoint.PointType)
            {
                case S_PathPoint.PointTypes.none:
                case S_PathPoint.PointTypes.spawn:
                    this.AttackState = (int)AttackStates.MoveTowardsPoint;
                    Debug.Log("setting AttackState = Moveing Towards point");
                    break;
                case S_PathPoint.PointTypes.AttackPLayer:
                    this.AttackState = (int)AttackStates.AttackPlayer;
                    Debug.Log("setting AttackState = Attacking player");
                    break;
                case S_PathPoint.PointTypes.Dive:
                    break;
                case S_PathPoint.PointTypes.EndPoint:
                    this.AttackState = (int)AttackStates.Die;
                    break;
                case S_PathPoint.PointTypes.ReturnToPool:
                    this.attackState = AttackStates.ReturnToPool;
                    //UnityEngine.Debug.Log("returning to pool");
                    break;
            }
            currentPathPoint = Path.PathPoints[NextPathPointIndex];
            TargetToMoveTo = currentPathPoint.transform.position;
        }
    }

    protected virtual void Idle()
    {
        throw new System.NotImplementedException("Idle Not Implemented for "+this.gameObject.name);
        //play Idle Animation
    }

    protected virtual void MoveTowardsPlayer()
    {
        throw new System.NotImplementedException("Idle Not Implemented for " + this.gameObject.name);
        //play move animation
        //moveTowadsPoint
        //MoveTo();
    }

    protected virtual void MoveTowadsPoint()
    {
        throw new System.NotImplementedException("Idle Not Implemented for " + this.gameObject.name);
        //play move animation
        //moveTowadsPoint
        //MoveTo();
    }

    protected virtual void AttackPlayer()
    {
        throw new System.NotImplementedException("Idle Not Implemented for " + this.gameObject.name);
        //play shoot animation
        // instantiate new projectile
    }

    protected virtual void CelibratePlayerDeath()
    {
        throw new System.NotImplementedException("Idle Not Implemented for " + this.gameObject.name);
        //player Celibrate player death animation
    }

    protected virtual void Die()
    {
        throw new System.NotImplementedException("Idle Not Implemented for " + this.gameObject.name);
        //play death animation
    }


    protected void MoveTo(Vector3 pointToMoveTo, bool ZeroX= false, bool ZeroY=false, bool ZeroZ=false)
    {
        Vector3 moveDirectionVector = (pointToMoveTo- transform.position).normalized;
        //Debug.Log("movedirection vector: "+ moveDirectionVector);
        if (ZeroX)
            moveDirectionVector.x = 0f;

        if (ZeroY)
            moveDirectionVector.y = 0f;

        if (ZeroZ)
            moveDirectionVector.z = 0f;

        //Debug.Log("Move direction vector: "+moveDirectionVector);

        Move(moveDirectionVector);
        //Move(pointToMoveTo);
    }

    protected void Move(Vector3 moveDirectionVector)
    {
        //Debug.Log("transform befor: "+transform.position);
        transform.Translate(moveDirectionVector * Speed * Time.deltaTime, Space.World);
        //Debug.Log("transform After: " + transform.position);
        //Vector3 velocity = Vector3.zero;
        //transform.position = Vector3.SmoothDamp(this.transform.position, moveDirectionVector, ref velocity, 0.3f);
    }

    /// <summary>
    /// this function do not actualy move that object, but sets the objects state to move to target
    /// </summary>
    public void SetTaragetPointAndBegin(Vector3 Point)
    {
        _targetToMoveTo = Point;
        AttackState = (int)AttackStates.MoveTowardsPoint;
    }


    public void Kill()
    {
        //user this to kill the pigion
    }
    protected S_AnimationFrameSet CurrentAnimationSet;
    
    public float StartingFrame = 0f;
    public float FramesPerSecond = 2f;

    //private float _currentFrame;

    public bool IsRandomAnimation = false;
    public void PlayCurrentAnimation(bool isRandomAnimation)
    {
        IsRandomAnimation = isRandomAnimation;
        PlayCurrentAnimation();
    }

    public void PlayCurrentAnimation()
    {
        if (IsRandomAnimation)
        {
            PlayRandomAnimation();
        }
        else
        {
            PlayLiniyerAnimation(FramesPerSecond);
        }
    }

    float StartTimePoint =0;
    float RandomTimeLength = 0;
    private void PlayRandomAnimation()
    {
        while ((Time.realtimeSinceStartup - StartTimePoint) > RandomTimeLength)
        {
            float RandomFrame = UnityEngine.Random.Range(0, CurrentAnimationSet.FrameSet.Count);
            StartTimePoint = Time.realtimeSinceStartup;
            RandomTimeLength = UnityEngine.Random.Range(randomFramTimeMinRange, randomFramTimeMaxRange);
            Debug.Log("Random Time Length: " + RandomTimeLength);
            Texture nextFrame = CurrentAnimationSet.FrameSet[Convert.ToInt32(RandomFrame)] as Texture;
            this.gameObject.renderer.material.mainTexture = nextFrame;

            renderer.material.shader = Shader.Find("Custom/Simple Shader");
        }
    }

    private void PlayLiniyerAnimation(float framesPerSecond)
    {
        //Debug.Log("playing linier animation");
        if (CurrentAnimationSet == null)
            Debug.Log("current animation set is null");
        if(CurrentAnimationSet.FrameSet == null)
            Debug.Log("current animation.frame set is null");

        if (CurrentAnimationSet.FrameSet.Count > 0)//make sure there are frames in the set
        {
            //Debug.Log("frame set count > 0");
            CurrentAnimationSet.Currentframe += Time.deltaTime * framesPerSecond;

            //if frame num is greater than number of frames
            //if end of animation
            //Debug.Log("current frame number " + CurrentAnimationSet.Currentframe);
            //Debug.Log("current animation framset count= " + CurrentAnimationSet.FrameSet.Count);
            if (CurrentAnimationSet.getCurrentFrameNumberAsInt() >= CurrentAnimationSet.FrameSet.Count)
            {
                //Debug.Log("current fram number as int is >= frameset count");
                CurrentAnimationSet.Currentframe = CurrentAnimationSet.Currentframe - CurrentAnimationSet.FrameSet.Count;
                Debug.Log("is play once: " + CurrentAnimationSet.IsPlayOnce);
                if (CurrentAnimationSet.IsPlayOnce)
                {
                    Debug.Log("play once animation changing");
                    CurrentAnimationSet = CurrentAnimationSet.GetNextAnimation();
                }
            }

            //catch case where current fram could be less than 0 (safty)
            if (CurrentAnimationSet.getCurrentFrameNumberAsInt() < 0)
                CurrentAnimationSet.Currentframe += CurrentAnimationSet.FrameSet.Count;

            Texture nextFrame = CurrentAnimationSet.FrameSet[CurrentAnimationSet.getCurrentFrameNumberAsInt()] as Texture;
            this.gameObject.renderer.material.mainTexture = nextFrame;

            renderer.material.shader = Shader.Find("Custom/Simple Shader");
        }
    }

    void OnBecameInvisible()
    {
        this.isVisibal = false;
    }
    void OnBecameVisible()
    {
        this.isVisibal = true;
    }
    protected virtual void OnReturnToPool()
    {
            UnityEngine.Debug.Log("this should have been overriden!!!!!");
    }
    public void SetActive(bool b)
    {
        this.gameObject.SetActive(b);
        //this.gameObject.GetComponent<S_SpriteShadow>().SetActive(b);
    }

    protected void MoveTowardsPlayer(S_AnimationFrameSet Frames)
    {
        //todo need to deside which set to use based on positon of pigion 
        //CurrentAnimationSet = walkAnimationSet;
        CurrentAnimationSet = Frames;
        //play move animation
        //moveTowadsPoint
        MoveTo(GameManager.PlayerObjecttRef.transform.position);

        //TODO set rotation of pigion so that is faces player
    }
    protected void AbsRotateToDirection(Vector3 pointToMoveTo)
    {
        Vector3 EulerAngle = this.transform.rotation.eulerAngles;

        //UnityEngine.Debug.Log(this.name);
        //UnityEngine.Debug.Log("move to: "+ pointToMoveTo);
        //UnityEngine.Debug.Log("this position: "+this.transform.position);

        if (pointToMoveTo.x <= this.transform.position.x)
            Movingleft = true;
        else
            Movingleft = false;

        if (Movingleft)
            this.transform.rotation = Quaternion.Euler(EulerAngle.x, 180, EulerAngle.z);
        else
            this.transform.rotation = Quaternion.Euler(EulerAngle.x, 0, EulerAngle.z);
    }

    public float offsetAngle = 0;
}   

