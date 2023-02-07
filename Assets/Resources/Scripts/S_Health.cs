using UnityEngine;
using System.Collections;

public class S_Health : MonoBehaviour {

    public int CurrentHealth = 100;
    public int TargetHealth = 100;
    public float HitTime;
    public float HealthRuductionSpeed = 1;
    //public S_2dSpriteBase Entity ;
    public GUIText HealthText;


    public Vector2 HealthBarGUIPosition;
    public Rect HealthBar;
    public float StartWidth;
    public Texture HealthBarTexture;
    public GUIStyle HealthGUIStyle;
    void Awake()
    {
        //Health = Entity.Health;
        //HealthText.text = Health.ToString();
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        
        
	}
    public void HealthChange(int health)
    {
        this.TargetHealth = health;
        HitTime = Time.time;
    }

    void OnGUI()
    {
        //Debug.Log("(Time.time - HitTime) * HealthRuductionSpeed " + (Time.time - HitTime) * HealthRuductionSpeed);
        CurrentHealth = (int)Mathf.Lerp(CurrentHealth, TargetHealth, (Time.time - HitTime) * HealthRuductionSpeed);
        HealthText.text = CurrentHealth.ToString();
        //Debug.Log("(Health / 100): " +  ((float)Health / 100));
        HealthBar.width = StartWidth* ((float)CurrentHealth / 100);
        HealthBar.x = Screen.width*HealthBarGUIPosition.x;
        HealthBar.y = Screen.height*HealthBarGUIPosition.y;
        //GUI.color = Color.green;
        if (HealthBarTexture != null)
            GUI.DrawTexture(HealthBar, HealthBarTexture, ScaleMode.StretchToFill);
        //GUI.color = Color.gray;



        //Debug.Log("screen resolution: "+Screen.GetResolution);
        
    }

}
