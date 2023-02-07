using NTTools;
using UnityEngine;
using System.Collections;

public class S_EnemyBase : S_2dSpriteBase, IObjectPool
{
    private bool _isActive;
    public bool IsActive
    {
        get { return this.gameObject.activeSelf; }
        set { this.gameObject.SetActive(value); }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
