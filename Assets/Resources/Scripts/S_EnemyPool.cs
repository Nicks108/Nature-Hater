using UnityEngine;
using System.Collections;
using NTTools;

public class S_EnemyPool : MonoBehaviour
{
    public ObjectPool<S_EnemyBase> Pool;
    public int MaxNumInPool =1;
    public GameObject EnemyPreFab;
    public string EnemyName;

    void Awake()
    {
        Pool = new ObjectPool<S_EnemyBase>();
        for (int i = 0; i < MaxNumInPool; i++)
        {
            GameObject newEnemy = (GameObject)Instantiate(EnemyPreFab);
            S_EnemyBase NewEnemyScript = newEnemy.GetComponent<S_EnemyBase>();
            //NewPigionScript.PigionDied += new S_Pigion.PigionDiedEvent(RemovePigionFromList);
            //newPigion.GetComponent<S_SpriteShadow>().InstantiateThis();
            //Debug.Log("NewEnemyScript in "+this.name+" " + NewEnemyScript);
            NewEnemyScript.SetActive(false);
            newEnemy.name = EnemyName + i;
            NewEnemyScript.EnemyPoolRef = this;
            Pool.push(NewEnemyScript);
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
