using UnityEngine;
using System.Collections;
using NTTools.Data_Structures;
using NTTools.Path;

public class S_PathSpawner : S_2dSpriteBase
{
    public float PigionSpeedOnThisPath = 0.5f;

    public  S_EnameyPacing PacingData;
    public  S_Path PathData;
    public S_EnemyPool EnemyPoolData;
    private S_GameManager _gameManager;

    private int _currentEventIndex = 0;
    private NTTools.Data_Structures.NTEnamyPaceData _curentSpawnEvent;

    private float _nextSpawnTime = 0;
    private int _currentPigontoSpawn = 0;
    private bool _isRepeating;

    private bool _isSpawnerFinished = false;

    private float _StartTimeOfThisSpawnCycle;

    new void Awake()
    {
        this.PacingData = this.gameObject.GetComponent<S_EnameyPacing>();
        this.PathData = this.gameObject.GetComponent<S_Path>();
        this.EnemyPoolData = GameObject.Find("PigionPool").GetComponent<S_EnemyPool>();
        this._gameManager = GameObject.Find("GameManager").GetComponent<S_GameManager>();

        SetupWaitTimeBeforSpawnsStartArray();
        _curentSpawnEvent = PacingData.PaceEvents[_currentEventIndex];

        _isRepeating = _curentSpawnEvent.isRepeat;

        _StartTimeOfThisSpawnCycle = Time.time;
    }

    private float[] _waitTimeBeforSpawnsStartArray;
    private void SetupWaitTimeBeforSpawnsStartArray()
    {
        this._waitTimeBeforSpawnsStartArray = new float[PacingData.PaceEvents.Count];
        float totalTime = 0;
        for (int i = 0; i < PacingData.PaceEvents.Count; i++)
        {
            var pacingEvent = PacingData.PaceEvents[i];
            this._waitTimeBeforSpawnsStartArray[i] = pacingEvent.WaitTimeBeforSpawnStarts + totalTime;
            totalTime += this._waitTimeBeforSpawnsStartArray[i];
        }
    }

    // Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
        

        if(_isSpawnerFinished) return;
        //Debug.Log("spawner still active");
	    if (!_gameManager.gameStarted) return; // if game not started,  return
        //Debug.Log("Game started");
	    //this._curentSpawnEvent

        if (!(this._waitTimeBeforSpawnsStartArray[_currentEventIndex] + _StartTimeOfThisSpawnCycle <= Time.time)) return;
        //Debug.Log("Event Name " + _curentSpawnEvent.NodeName);
        //Debug.Log("can start spawn event");
        
	    if (EnemyPoolData.Pool.CountInactive <= 0)return;
       // Debug.Log("pigions are in the pool, having a swim :)");
        //Debug.Log("number of swimming pigions "+ PigionPoolData.PigionPool.CountInactive);

        //Debug.Log("_currentPigontoSpawn " + _currentPigontoSpawn);
        //Debug.Log("this._curentSpawnEvent.NumberOfEnimeys " + this._curentSpawnEvent.NumberOfEnimeys);
        if (_currentPigontoSpawn < this._curentSpawnEvent.NumberOfEnimeys)
        {
            //Debug.Log("this event still has pigions to spawn");
            //Debug.Log("current pigion to spawn "+ _currentEventIndex);

            //Debug.Log("current time.time " + Time.time);
            //Debug.Log("next spawn time "+ _nextSpawnTime);
            if (Time.time >= _nextSpawnTime) // if time greater than next spawntime, spawn another pigion
            {
               // Debug.Log("Event Name " + _curentSpawnEvent.NodeName);
                //Debug.Log("its time for the next pigion in the event to be spawned");
                //spawn pigion
                S_Pigion pigionScript = (S_Pigion) EnemyPoolData.Pool.GetItem(EnemyPoolData.Pool.GetIndexOfInactiveObject());
                GameObject nextInactivePigion = pigionScript.gameObject;
                ResetEnemy(nextInactivePigion, new Vector3(-2.1f, 1.1f, 0f), new Vector3(3.5f, 0.9f, 0f),  this._gameManager.PlayerObjecttRef.transform.position, (int)AttackStates.MoveTowardsPoint);
                _currentPigontoSpawn++;
                
                    //Debug.Log("current event index < PacingData.PacingEvents.count");
                    if (_currentPigontoSpawn == this._curentSpawnEvent.NumberOfEnimeys ) // if last pigion in spawn event
                    {
                       // Debug.Log("this is the last pigion in the event");
                        _currentEventIndex++;
                        //Debug.Log("_currentEventIndex++ "+ _currentEventIndex);

                        //Debug.Log("current Event index " + _currentEventIndex);
                        //Debug.Log("PacingData.PaceEvents.Count " + PacingData.PaceEvents.Count);

                        _currentPigontoSpawn = 0;

                    }
                    if (_currentEventIndex < PacingData.PaceEvents.Count)
                    {
                        _curentSpawnEvent = PacingData.PaceEvents[_currentEventIndex];
                        ResetNextSpawnTimer();
                    }
                    else
                    {
                        if (_isRepeating)
                        {
                            //Debug.Log("is repeating");
                            _currentEventIndex = 0;
                            _currentPigontoSpawn = 0;
                            this._StartTimeOfThisSpawnCycle = Time.time;
                        }
                        else
                            _isSpawnerFinished = true;
                    }
                //SetupWaitTimeBeforSpawnsStartArray();
                
                
            }
            
        }
	}

    private void ResetNextSpawnTimer()
    {
        _nextSpawnTime = Time.time + (this._curentSpawnEvent.TimeBetweenSpawns);                
    }

    private void ResetEnemy(GameObject pigion, Vector3 startPosition, Vector3 moveTarget, Vector3 shootTraget, int initialStateState)
    {
        //if (!pigion.GetComponent<S_Pigion>())
        //    pigion.AddComponent<S_Pigion>();

        pigion.transform.position = startPosition;
        S_EnemyBase newPigionScript = pigion.GetComponent<S_EnemyBase>();
        newPigionScript.AttackState = (int)AttackStates.MoveTowardsPoint;
        //NewPigionScript.TargetToMoveTo = MoveTarget;
        newPigionScript.transform.eulerAngles = new Vector3(0f, 0f, -17.3f);
        newPigionScript.IsActive = true;
        pigion.GetComponent<Renderer>().enabled = true;
        newPigionScript.Speed = this.PigionSpeedOnThisPath;

        newPigionScript.Path = this.PathData;
        Debug.Log("Path info: " + newPigionScript.Path);
        newPigionScript.currentPathPoint = newPigionScript.Path.PathPoints[0];
        Debug.Log("just set new path point in spawner: " + newPigionScript.currentPathPoint.name);
        newPigionScript.TargetToMoveTo = newPigionScript.currentPathPoint.transform.position;

        newPigionScript.SetActive(true);
        pigion.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
