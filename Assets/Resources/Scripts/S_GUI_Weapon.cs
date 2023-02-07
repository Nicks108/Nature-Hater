using UnityEngine;
using System.Collections;

public class S_GUI_Weapon : MonoBehaviour {

    public int ProjectileDamage = 1;
    public float ProjectileResistance = 2;
    public Texture ProjectileTexture;
    public float ColiderRadius = 0.5f;
    public Vector3 ProjectileScale;
    public float Bouncibility = 1f;
    public float TimeToKill = 3f;
    public int AmmoCount = -1;
    public float MaxShootStrength = -1;
    public float MinShootStrength = -1;
    public bool isKillOneEnimyOnly = false;
    public bool UseGravity = true;
    public int PointsDeductionPerShoot = 1;

	// Use this for initialization
	void Start () {
        this.ProjectileTexture = this.gameObject.GetComponent<GUITexture>().texture;
	}
	
	// Update is called once per frame
	void Update () {

	}
    void OnMouseDown()
    {
        setCurrentWeapon();
        
    }
    public void setCurrentWeapon()
    {
        //Debug.Log("you clicked " + this.gameObject.name);

        // new projectile
        //S_ProjectileBase ProjectileScript = new S_ProjectileBase(ProjectileDamage,projectileResistance,ProjectileTexture);
        GameObject NewProjectile = (GameObject)Resources.Load("Prefabs/ProjectileBase");
        //NewProjectile.GetComponent<S_ProjectileBase>().SetUp(ProjectileDamage, projectileResistance, ProjectileTexture,ColiderRadius);
        // pass new projectile to player
        GameObject Player = GameObject.Find("Player");
        S_Player playerScript = Player.GetComponent<S_Player>();
        playerScript.Projectile = NewProjectile;
        playerScript.weaponGUIScript = this;
        //playerScript.ProjectileDamage = ProjectileDamage;
        //playerScript.ProjectileResistance = projectileResistance;
        //playerScript.ProjectileTexture = ProjectileTexture;
        //playerScript.ColiderRadius = ColiderRadius;
        //playerScript.Bouncibility = Bouncibility;

        //GameObject.Find("SelectedWeaponGUITexture").GetComponent<S_PerantClick>().GUItext = AmmoCount.ToString();
        //GameObject.Find("SelectedWeaponGUITexture").GetComponent<S_PerantClick>().GUITexture = ProjectileTexture;
    }
}
