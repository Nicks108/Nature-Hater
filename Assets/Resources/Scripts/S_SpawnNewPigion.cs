using NTTools.Path;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NTTools;

public class S_SpawnNewPigion : S_2dSpriteBase
{
    //public List<GameObject> EnemyList = new List<GameObject>();
    //public int NumberOfActiveEnemyLim = 10;
    public int MaxNumInPool = 10;
    public int MinRandomSpawnTime = 1;
    public int MaxRandomSpawnTime = 2;

    private ObjectPool<S_Pigion> PigionPool; 

    //private GameObject _pigionOriginal;
    //public GameObject PigionOriginal
    //{
    //    get { return _pigionOriginal; }
    //}

    //new void Awake ()
    //{
    //    //Debug.Log("Pigion spawner awake",this);
    //    base.Awake();

    //    PigionPool = new ObjectPool<S_Pigion>();
    //    for (int i = 0; i < MaxNumInPool; i++)
    //    {
    //        GameObject newPigion = (GameObject)Instantiate(Resources.Load("Prefabs/PigionEnemeyBase"));
    //        S_Pigion NewPigionScript = newPigion.GetComponent<S_Pigion>();
    //        NewPigionScript.EnemyDied += new S_2dSpriteBase.EnemyDiedEvent(RemovePigionFromList);
    //        //newPigion.GetComponent<S_SpriteShadow>().InstantiateThis();
    //        NewPigionScript.SetActive(false);
    //        PigionPool.push(NewPigionScript);
    //    }
    //}

	// Use this for initialization
	void Start () {
	}

    private float nextSpawnTime = 0;
	// Update is called once per frame
	void Update () {
        //System.Diagnostics.Stopwatch preformenceTimer = new System.Diagnostics.Stopwatch();
        //preformenceTimer.Start();
        //Debug.Log("spawner update");
        if (GameManager.gameStarted) // if the game has started
        {
            if (Time.time > nextSpawnTime)
            {
                if (PigionPool.CountInactive > 0) // aslong as there is atlease 1 inactive pigion
                {
                    S_Pigion pigionScript = PigionPool.GetItem(PigionPool.GetIndexOfInactiveObject());
                    GameObject nextInactivePigion = pigionScript.gameObject;
                    ResetPigion(nextInactivePigion, new Vector3(-2.1f, 1.1f, 0f), new Vector3(3.5f, 0.9f, 0f), GameManager.PlayerObjecttRef.transform.position, (int)AttackStates.MoveTowardsPoint);
                    nextSpawnTime = Time.time + Random.Range(MinRandomSpawnTime, MaxRandomSpawnTime);
                }
            }
        }

        //preformenceTimer.Stop();
        //Debug.Log("SpawnPigion update = "+preformenceTimer.ElapsedMilliseconds);
	}
    

    public void ResetPigion(GameObject Pigion, Vector3 StartPosition, Vector3 MoveTarget, Vector3 ShootTraget, int InitialStateState)
    {
        //System.Diagnostics.Stopwatch preformenceTimer = new System.Diagnostics.Stopwatch();
        //preformenceTimer.Start();

        //GameObject NewPigion;
        //GameObject NewPigion = (GameObject)Instantiate(PigionOriginal);
        //if(!Pigion.GetComponent<S_EnemyBase>())
        //    Pigion.AddComponent<S_Pigion>();

        Pigion.transform.position = StartPosition;
        S_EnemyBase NewPigionScript = Pigion.GetComponent<S_EnemyBase>();
        NewPigionScript.AttackState = (int)AttackStates.MoveTowardsPoint;
        //NewPigionScript.TargetToMoveTo = MoveTarget;
        NewPigionScript.transform.eulerAngles = new Vector3(0f,0f,-17.3f);
        NewPigionScript.IsActive = true;
        Pigion.GetComponent<Renderer>().enabled = true;
        NewPigionScript.Speed = S_2dSpriteBase.DefaultSpeed;

        //Pigion.GetComponent<BoxCollider>().enabled = true;

        NewPigionScript.Path = GetRandomPath();
        Debug.Log("Path info: "+NewPigionScript.Path);
        NewPigionScript.currentPathPoint = NewPigionScript.Path.PathPoints[0];
        Debug.Log("just set new path point in spawner: " + NewPigionScript.currentPathPoint.name);
        NewPigionScript.TargetToMoveTo = NewPigionScript.currentPathPoint.transform.position;

        //Pigion.SetActive(true);
        NewPigionScript.SetActive(true);


        //preformenceTimer.Stop();
        //Debug.Log("SpawnPigion Spawn = " + preformenceTimer.ElapsedMilliseconds);
    }

    public S_Path GetRandomPath()
    {
        GameObject PathCollection = GameObject.Find("PathCollection");
        //Debug.Log(PathCollection);

        int ChildPathCount = PathCollection.transform.childCount;
        //Debug.Log(ChildPathCount);

        //Random.Range(0, ChildPathCount);
        int randomint = Random.Range(0, ChildPathCount);
        //Debug.Log("print my random int "+randomint);
        Transform RandomChild = PathCollection.transform.GetChild(randomint);
        //Debug.Log(RandomChild.gameObject.name);

        //S_Path test = RandomChild.gameObject.GetComponent<S_Path>();
        //Debug.Log("test path point count "+test.PathPoints.Count);
        //Debug.Log(RandomChild.gameObject.GetComponent<ES_Path>().PathPoints.Count);
        return RandomChild.gameObject.GetComponent<S_Path>();
    }

    private void RemovePigionFromList(S_2dSpriteBase EnemyScriptScript, System.EventArgs e)
    {
        //Debug.Log("Pigion Died");
        //PigionList.Remove(PigionScript.gameObject);
    }

}
