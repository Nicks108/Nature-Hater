using UnityEngine;
using System.Collections;
using NTTools;

public class S_BonusTextPool : MonoBehaviour {
    private ObjectPool<S_BonusTextScript> TextPool;
    public int MaxObjectsInPool = 10;
    void Awake()
    {
        //Debug.Log("in textpool awake");
        TextPool = new ObjectPool<S_BonusTextScript>();

        for (int i = 0; i < MaxObjectsInPool; i++)
        {
            //Debug.Log("int textpool add loop :"+i);
            GameObject newBonusText = (GameObject)Instantiate(Resources.Load("Prefabs/BonusTextPrefab"));
            S_BonusTextScript newBonusTextScript = newBonusText.GetComponent<S_BonusTextScript>();
            //newBonusTextScript.PigionDied += new S_Pigion.PigionDiedEvent(RemovePigionFromList);
            newBonusText.SetActive(false);
            newBonusTextScript.IsActive = false;
            newBonusText.transform.parent = this.transform;
            TextPool.push(newBonusTextScript);
        }
    }
    // Use this for initialization
    void Start()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void spawnNewBonusText(float X, float Y, float Z, string text, bool FadeIn = false, bool FadeOut = false, float FadeInTime = 0, float FadeOutTime = 0)
    {
        spawnNewBonusText(new Vector3(X,Y,Z), text, FadeIn, FadeOut, FadeInTime, FadeOutTime);
    }
    public void spawnNewBonusText(Vector3 location, string text, bool FadeIn = false, bool FadeOut = false, float FadeInTime = 0, float FadeOutTime = 0)
    {
        //get next inactove bonuse text from pool
        S_BonusTextScript bonusText = TextPool.GetItem(TextPool.GetIndexOfInactiveObject());
        GameObject nextInactiveBonusText = bonusText.gameObject;
        //set it up and let it go!
        bonusText.Set(location, text, FadeIn, FadeOut, FadeInTime, FadeOutTime);
        nextInactiveBonusText.SetActive(true);
        bonusText.IsActive = true;
    }
}
