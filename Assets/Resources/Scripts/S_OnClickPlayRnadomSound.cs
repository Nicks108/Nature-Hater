using UnityEngine;
using System.Collections;

public class S_OnClickPlayRnadomSound : MonoBehaviour {

    public S_PlayRandomAudioClip randomAudio;
    public string AudioClipCollection;
    void Awake()
    {
        if (randomAudio == null)
            randomAudio = GameObject.Find(AudioClipCollection).GetComponent<S_PlayRandomAudioClip>();
    }

    void OnMouseDown()
    {
        Debug.Log("testing random Audio");
        randomAudio.PlayRandomClip();
    }
}
