using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NTTools;

public class S_ProjectileBase : S_2dSpriteBase, IObjectPool
{
    NTDebug Debug;
    public float rotationSpeed = 400;
    public bool isOneKillOnly = false;
    public bool IsActive
    {
        get { return this.gameObject.activeSelf; }
        set { this.gameObject.SetActive(value); }
    }


    public void SetUp(int damage, Texture projectileImage, float ColiderRadius, float bouncibility, bool KillOneEnimyOnly, float timeToKill)
    {
        this.Damage= damage;
        _projectileResistance = ProjectileResistance;
        ProjectileImage = projectileImage;
        isOneKillOnly = KillOneEnimyOnly;
        this.gameObject.renderer.material.mainTexture = ProjectileImage;
        this.renderer.material.shader = Shader.Find("Custom/Simple Shader");
        //this.GetComponent<MeshRenderer>().materials[0].mainTexture = ProjectileImage;
        this.GetComponent<SphereCollider>().radius = ColiderRadius;
        Bouncibility = bouncibility;
        this.TimeToKill = timeToKill;
    }

    new void Awake()
    {
        Debug = new NTDebug(this.gameObject);
        Destroy(this.gameObject, TimeToKill);
    }

    private Vector3 _scale = new Vector3(1, 1, 1);
    public Vector3 Scale
    {
        get { return _scale; }
        set { _scale = value; }
    }

    public List<Vector3> _trajectoryPath = new List<Vector3>();
	public List<Vector3> TrajectoryPath
	{
		get { return _trajectoryPath;}
		set {_trajectoryPath = value;}
	}

    //public int Damage = 10;

    private float _projectileResistance = 1;
    public float ProjectileResistance
    {
        get { return _projectileResistance; }
        set { _projectileResistance = value; }
    }
    public float Bouncibility = 1.1f;

    public float TimeToKill = 3f;

    public Texture ProjectileImage;


	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        //FollowTrajectory();
        this.transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
	}

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("collider name" + collider.name);
        if (collider.name.Contains("FloreColiderCube"))
        {
            if(Bouncibility != 0)
            {
                Debug.Log("bounce position: " + this.transform.position);
                //Debug.Log("bounce");
                Rigidbody rigidbody = this.GetComponent<Rigidbody>();
                //Debug.Log("bouncabuility: "+ Bouncibility);
                //Debug.Log("velovity / bouncibuility: " + ((rigidbody.velocity.y * -1) / Bouncibility));
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, (rigidbody.velocity.y*-1)/Bouncibility,
                                                 rigidbody.velocity.z);
                //Destroy(this.gameObject);
            }
            else
            {
                this.DestroyProjectile();
            }
    }
}

    public void CheckIsOneKillOnly()
    {
        //UnityEngine.Debug.Log("projectile collision: " + isOneKillOnly);
        if (isOneKillOnly)
            this.DestroyProjectile();
    }
    
	public void DestroyProjectile()
	{
		Destroy(this.gameObject);
	}

    float D = 1f;
    private float GetDistance(GameObject A, GameObject B)
    {
        //h(n) = D * sqrt((n.x-goal.x)^2 + (n.y-goal.y)^2)
        float Sx = A.transform.position.x;
        float Sy = A.transform.position.y;
        float Sz = A.transform.position.z;

        float Tx = B.transform.position.x;
        float Ty = B.transform.position.y;
        float Tz = B.transform.position.z;

        return D * Mathf.Sqrt(Mathf.Pow((Sx - Tx), 2) + Mathf.Pow((Sy - Ty), 2) + Mathf.Pow((Sz - Tz), 2));
    }
}
