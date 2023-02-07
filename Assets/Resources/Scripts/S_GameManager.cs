using UnityEngine;
using System;
using System.Collections;

public class S_GameManager : MonoBehaviour {

    private GameObject _scoreGUI;
    public GameObject ScoreGUI
    {
        get { return _scoreGUI; }
    }

    private GameObject _playerObjectRef;
    public GameObject PlayerObjecttRef
    {
        get { return _playerObjectRef; }
    }
    private GameObject _bonusTextPoolRef;
    public GameObject BonusTextPoolRef
    {
        get { return _bonusTextPoolRef; }
    }
    //private float _targetDistanceTweek = 5;
    //public float TargetDistanceTweek 
    //{
    //    get { return _targetDistanceTweek; }
    //    set { _targetDistanceTweek = value; }
    //}
    public float _targetSwitchDistanceThreshold = 6; 
    public float TargetSwitchDistanceThreshold
    {
        get { return _targetSwitchDistanceThreshold; }
        set { _targetSwitchDistanceThreshold = value; }
    }

    public int _numberOfKills = 0;
    public int _numberOfKillsInRow = 0;

    public bool _gameStarted = false;
    public bool gameStarted
    {
        get { return _gameStarted; }
        set
        {
            _gameStarted = value;
        }
    }

    public bool GameCanPlay = false;
    public bool PlayerIsDead = false;
    public bool GameFinished = false;

    public GameObject StartGameMessage;
    public float StartGameMessageUpTime = 3f;
    public GameObject GameOverScreen;
    public bool GameOverScreenShown = false;
    public float WaitBeforShowingGameOverScreen = 5f;
    public GameObject RetryScreen;
    public float WaitBeforShowingRetryScreen = 5f;

    public float TimeBeforLevelCompleteShow = 2f;
    public GameObject LevelCompleteMessage;
    public float DeanWaitBeforWalk = 0;
    public float TimeBeforLoadNextLevel = 3f;
    public string NextLevelName = "";

    public bool TestEndLevelScreen = false;

    public S_EventToDo[] Events;

	// Use this for initialization
	void Start () {
        _scoreGUI = GameObject.Find("GUI Points Score");
        _playerObjectRef = GameObject.FindGameObjectWithTag("Player");
        _bonusTextPoolRef = GameObject.Find("BonusTextPool");
        if (_bonusTextPoolRef == null)
        {
            _bonusTextPoolRef = new GameObject();
            _bonusTextPoolRef.AddComponent<S_BonusTextPool>();
            _bonusTextPoolRef.name = "BonusTextPool";
        }


        GameOverScreen.SetActive(false);
        RetryScreen.SetActive(false);
        StartGameMessage.SetActive(false);

	    Events = Resources.FindObjectsOfTypeAll<S_EventToDo>();
	}
	
	// Update is called once per frame

    public float GameSpeed = 1f;

    void Update()
    {
        Time.timeScale = GameSpeed;
        if(!GameCanPlay)
            StartCoroutine(DoBeginGame());

        //Debug.Log("in Gm update");
        if ((_numberOfKills > 0) && (gameStarted == false))
        {
            gameStarted = true;
        }

        //check if level has finished
        if (CheckIfEventsArFinished() || TestEndLevelScreen)
        {
            bool NoActiveEnemys = true;
            foreach (S_EnemyPool Pool in Resources.FindObjectsOfTypeAll<S_EnemyPool>())
            {
                if (Pool.Pool.CountActive > 0)//there are no active enemys.
                    NoActiveEnemys = false;
            }

            //if game finished
            if (NoActiveEnemys && (PlayerIsDead == false))
            {
                GameFinished = true;
                StartCoroutine(DoLevelComplete());

                saveScore();
            }
        }

        if (!GameOverScreenShown)
            StartCoroutine(CheckPlayerDead());
    }

    private void saveScore()
    {
        //save score
        string savedScore = PlayerPrefs.GetString(Application.loadedLevelName);
        string currentScore = _scoreGUI.GetComponent<GUIText>().text;
        //UnityEngine.Debug.Log("Saved score: "+ savedScore+ " current score: "+ currentScore);
        if (savedScore == "" || (Convert.ToInt32(savedScore) < Convert.ToInt32(currentScore)))
            PlayerPrefs.SetString(Application.loadedLevelName, _scoreGUI.GetComponent<GUIText>().text);
    }

    private bool CheckIfEventsArFinished()
    {
        bool eventsFinished = true;
        foreach (S_EventToDo _event in Events)
        {
            if (_event.EventNode.HasSpawnFinished == false)
                eventsFinished = false;
        }
        return eventsFinished;
    }

    IEnumerator DoLevelComplete()
    {
        yield return new WaitForSeconds(DeanWaitBeforWalk);
        PlayerObjecttRef.GetComponent<S_Player>().PLayWalkAnimation();
        yield return new WaitForSeconds(TimeBeforLevelCompleteShow);
        LevelCompleteMessage.SetActive(true);
        yield return new WaitForSeconds(TimeBeforLoadNextLevel);
        Application.LoadLevel(NextLevelName);
    }

    IEnumerator DoBeginGame()
    {
        StartGameMessage.SetActive(true);
        yield return new WaitForSeconds(StartGameMessageUpTime);
        this.GameCanPlay = true;
        StartGameMessage.SetActive(false);
    }

    IEnumerator CheckPlayerDead()
    {
        if(PlayerIsDead)
        {
            saveScore();
            if (!GameOverScreen.activeSelf)
            {
                yield return new WaitForSeconds(WaitBeforShowingGameOverScreen);
                GameOverScreen.SetActive(true);
                GameOverScreenShown = true;
            }
            if(GameOverScreen.activeSelf)
            {
                yield return new WaitForSeconds(WaitBeforShowingRetryScreen);
                GameOverScreen.SetActive(false);
                RetryScreen.SetActive(true);
            }
        }
	}

    public void SubscribeToEnemeyDiedEvent(S_2dSpriteBase EnemyScript)
    {
        EnemyScript.EnemyDied += new S_2dSpriteBase.EnemyDiedEvent(DoPigionCleanUp);
    }

    private void DoPigionCleanUp(S_2dSpriteBase PigionScript, System.EventArgs e)
    {
        if (PigionScript.AttackState == (int) S_2dSpriteBase.AttackStates.Die)
        {
            UpdateScorePoints(PigionScript, e);
        }
    }

    private void UpdateScorePoints(S_2dSpriteBase PigionScript, System.EventArgs e)
    {
        //System.Diagnostics.Stopwatch preformenceTimer = new System.Diagnostics.Stopwatch();
        //preformenceTimer.Start();
        //GUIText pointsGUIText = _scoreGUI.GetComponent<GUIText>();

        AddPoints(PigionScript.Points);

        _numberOfKills++;
        _numberOfKillsInRow++;
        ShowBonusNumOfKillsInRow(PigionScript.gameObject.transform.position);

        //preformenceTimer.Stop();
        //Debug.Log("Game manager update score = "+preformenceTimer.ElapsedMilliseconds);
    }

    //public void ResetNumberOfKillInRow()
    //{
    //    _numberOfKillsInRow = 0;
    //}

    public void AddPoints(int value)
    {
        GUIText pointsGUIText = _scoreGUI.GetComponent<GUIText>();
        int PointNum = Convert.ToInt32(pointsGUIText.text);
        PointNum += value;
        pointsGUIText.text = addLeadingZeros(PointNum.ToString());
    }
    public void SubTractPointPoints(int value)
    {
        GUIText pointsGUIText = _scoreGUI.GetComponent<GUIText>();
        int PointNum = Convert.ToInt32(pointsGUIText.text);
        if(PointNum > 0)
            PointNum -= value;
        pointsGUIText.text = addLeadingZeros(PointNum.ToString());
    }

    private string addLeadingZeros(string input)
    {
        return S_GameManager.AddLeadingZeros(input);
    }
    public static string AddLeadingZeros(string input)
    {
        int NumberOfZeros = 6;
        int initialInputLength = input.Length;
        for(int i =0; i< NumberOfZeros - initialInputLength; i++)
            input =input.Insert(0, "0");
        return input;
    }

    private void ShowBonusNumOfKillsInRow(Vector3 Position)
    {
        foreach (int i in BonusKillsInRow)
        {
            if ( _numberOfKillsInRow== i )
            {
                BonusTextPoolRef.GetComponent<S_BonusTextPool>().spawnNewBonusText(Position, _numberOfKillsInRow + " In A Row!", FadeOut: true, FadeOutTime: 2f);
                break;
            }
        }
    }
    public int [] BonusKillsInRow;

    public void ResetGame()
    {

        gameStarted = false;
        _numberOfKills = 0;
        _numberOfKillsInRow = 0;

        GUIText pointsGUIText = _scoreGUI.GetComponent<GUIText>();
        int PointNum = Convert.ToInt32(pointsGUIText.text);
        PointNum = 0;

        GameObject[] pathArray = GameObject.FindGameObjectsWithTag("Path");
        foreach (GameObject path in pathArray)
        {
            foreach (S_EventToDo eventToDo in path.GetComponents<S_EventToDo>())
            {
                eventToDo.Reset();
            }
        }
        
    }
}
