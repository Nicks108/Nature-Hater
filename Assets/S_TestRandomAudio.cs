using UnityEngine;
using System.Collections;

public class S_TestRandomAudio : MonoBehaviour
{
    public S_PlayRandomAudioClip randomAudio;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Debug.Log("testing random Audio");
        randomAudio.PlayRandomClip();
    }
}
