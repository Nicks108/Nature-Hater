using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTTools.Data;
using NTTools.Path;
using UnityEngine;

public class S_EventToDo : MonoBehaviour
{
    public EventNode EventNode;

    private S_GameManager _gameManager;
    public S_EnemyPool EnemyPoolData;
    private float _eventStartTime = 0;
    private float _nextSpawnTime = 0;
    private int _currentEnemySpawn = 0;
    private bool _checkDependancys = true;

    public void Reset()
    {
        _eventStartTime = 0;
        _nextSpawnTime = 0;
        _currentEnemySpawn = 0;
        _checkDependancys = true;
        this.EventNode.EventState = EventNode.EventStates.waiting;
    }


    void Awake()
    {
        this._gameManager = GameObject.Find("GameManager").GetComponent<S_GameManager>();


        //this.EnemyPoolData = GameObject.Find("PigionPool").GetComponent<S_EnemyPool>();
        
    }

    void Start()
    {
        //this.EventNode.EventState = EventNode.EventStates.waiting;
        this.EnemyPoolData = EventNode.EnemyPoolRef;
        Reset();
    }
    void OnApplicationQuit()
    {
        //UnityEngine.Debug.Log("Destroying event toDO");
        DestroyThis();
    }
    public void DestroyThis()
    {
        Destroy(this);
    }
    
    void Update()
    {

        if (!_gameManager.gameStarted)return;
        if (!_gameManager.GameCanPlay) return;
        if (_gameManager.PlayerIsDead) return;
        //Debug.Log("Game started");
       // Debug.Log("event: "+EventNode.NodeName);
        bool _CanSpawn = true;
        if (_checkDependancys)
        {
            //Debug.Log("checking dependancys");
            foreach (var dependancy in EventNode.DependancyList)
            {
                if (!dependancy.HasSpawnFinished)
                    _CanSpawn = false;
            }
        }
        //Debug.Log("can spawn = " + _CanSpawn);
        if(_CanSpawn)
        {
            //Debug.Log("can spawn");
            if(EventNode.HasSpawnFinished)
                this.EventNode.EventState = EventNode.EventStates.spawnFInished;
            else
                this.EventNode.EventState = EventNode.EventStates.spawning;

            _checkDependancys = false;
            if (_eventStartTime == 0) _eventStartTime = Time.time;


            if (EventNode.HasSpawnFinished) return;
            DoSpawn();
        }

    }

    private void DoSpawn()
    {
        //Debug.Log("beginning spawn function");
        //Debug.Log("number of pigions to spawn:" + EventNode.NumberOfEnimeys);
        if (_eventStartTime + EventNode.WaitTimeBeforSpawnStarts > Time.time) return ;



        if (EnemyPoolData.Pool.CountInactive <= 0) return ;
        if (_currentEnemySpawn < this.EventNode.NumberOfEnimeys)
        {
            if (Time.time >= _nextSpawnTime) // if time greater than next spawntime, spawn another pigion
            {
                //spawn pigion
                S_EnemyBase EnemyScript = (S_EnemyBase)EnemyPoolData.Pool.GetItem(EnemyPoolData.Pool.GetIndexOfInactiveObject());
                GameObject nextInactiveEnemy = EnemyScript.gameObject;
                Vector3 SpawnPoint = this.gameObject.GetComponent<S_Path>().PathPoints[0].transform.position;
                Vector3 NextPathPoint = this.gameObject.GetComponent<S_Path>().PathPoints[1].transform.position;
                ResetEnemy(nextInactiveEnemy, SpawnPoint, NextPathPoint, this._gameManager.PlayerObjecttRef.transform.position, (int)S_2dSpriteBase.AttackStates.MoveTowardsPoint);
                _currentEnemySpawn++;
                _nextSpawnTime = Time.time + (this.EventNode.TimeBetweenSpawns);

                if (_currentEnemySpawn == EventNode.NumberOfEnimeys)
                {
                    EventNode.HasSpawnFinished = true;
                    _currentEnemySpawn = 0;
                    //Debug.Log("spawn has finished");
                    //Debug.Break();
                }
            }
        }
    }


    private void ResetEnemy(GameObject enemy, Vector3 startPosition, Vector3 moveTarget, Vector3 shootTraget, int initialStateState)
    {
        //if (!pigion.GetComponent<S_Pigion>())
        //    pigion.AddComponent<S_Pigion>();

        enemy.transform.position = startPosition;
        S_EnemyBase newPigionScript = enemy.GetComponent<S_EnemyBase>();
        newPigionScript.AttackState = (int)S_2dSpriteBase.AttackStates.MoveTowardsPoint;
        //NewPigionScript.TargetToMoveTo = MoveTarget;
        
        newPigionScript.IsActive = true;
        enemy.GetComponent<Renderer>().enabled = true;
        newPigionScript.Speed = this.EventNode.Speed;

        //newPigionScript.Path = this.EventNode.Path;
        newPigionScript.Path =
            GameObject.FindGameObjectsWithTag("Path").ToList().Find(
                e => e.GetComponent<S_Path>().GUID == this.EventNode.PathGUID)
                .GetComponent<S_Path>();
        //Debug.Log("Path info: " + newPigionScript.Path);
        newPigionScript.currentPathPoint = newPigionScript.Path.PathPoints[0];
        //Debug.Log("just set new path point in spawner: " + newPigionScript.currentPathPoint.name);
        newPigionScript.TargetToMoveTo = newPigionScript.currentPathPoint.transform.position;

        newPigionScript.SetActive(true);
    }


    public void SetUp(EventNode eventNode)
    {
        this.EventNode = eventNode;
        //Reset();
    }
    //public void ResetEvent()
    //{
    //    _eventStartTime = 0;
    //    _nextSpawnTime = 0;
    //    _currentPigontoSpawn = 0;
    //}


    public new string ToString()
    {
        var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy;
        System.Reflection.PropertyInfo[] infos = this.GetType().GetProperties(flags);

        StringBuilder sb = new StringBuilder();

        string typeName = this.GetType().Name;
        sb.AppendLine(typeName);
        sb.AppendLine(string.Empty.PadRight(typeName.Length + 5, '='));

        foreach (var info in infos)
        {
            object value = info.GetValue(this, null);
            sb.AppendFormat("{0}: {1}{2}", info.Name, value != null ? value : "null", Environment.NewLine);
        }

        return sb.ToString();
    }
}

