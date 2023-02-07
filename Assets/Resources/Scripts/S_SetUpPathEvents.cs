using NTTools.Path;
using UnityEngine;
using NTTools.Data;
using System.Linq;

public class S_SetUpPathEvents : MonoBehaviour
{
    public TextAsset PacingData;
    void Awake()
    {
        SetUpEentsToDoFromEvetnFile();
    }

    public void LoadEvents()
    {

        UnityEngine.Debug.Log("loading events");
        UnityEngine.Debug.Log(NTTools.NTUtils.GetGameDataFolderPathForCurrentScene(Application.loadedLevelName) +
                              ".unity");
        //EventNode.Load(NTTools.NTUtils.GetGameDataFolderPathForCurrentScene(Application.loadedLevelName)+".unity", "tempEvents.xml");
        
        //tempEvents
        PacingData = (TextAsset)Resources.Load("GameData/Assets/Levels/" + (Application.loadedLevelName + ".unity") + "/Pacing Data/PacingEventData", typeof(TextAsset));
        //UnityEngine.Debug.Log(PacingData.ToString());
        EventNode.LoadEventsFromString(PacingData.text);
    }
    private void SetUpEentsToDoFromEvetnFile()
    {
        LoadEvents();
    //UnityEngine.Debug.Log("Application.dataPath " + Application.dataPath+"\\Game Data\\Assets\\Levels\\1-1.unity\\Pacing Data");
        foreach (var eventNode in EventNode.EventList)
        {
            //eventNode.Path.AddEventToObject(eventNode);

            GameObject.FindGameObjectsWithTag("Path").ToList().Find(
                e => e.GetComponent<S_Path>().GUID == eventNode.PathGUID)
                .GetComponent<S_Path>().AddEventToObject(eventNode);
        }
    }
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

}
