using UnityEngine;
using System.Collections.Generic;

public class S_PlayRandomAudioClip : MonoBehaviour
{
    public bool EnsureLastClipNotRepeated = false;
    public AudioSource[] AudioScourseArray;
    private int LastClipIndex;
    void Awake()
    {
        FindAllAudioSoursesForAudioLIst();
    }
	
    public void PlayRandomClip()
    {
        int randomIndex = Random.Range(0, AudioScourseArray.Length);
        //Debug.Log("random audio index: "+randomIndex);

        if (EnsureLastClipNotRepeated)
        {
            if(LastClipIndex == randomIndex)
            {
                randomIndex = (LastClipIndex + 1)%AudioScourseArray.Length;
                //Debug.Log("last clip was the same as this one, ensuring differenc: " + randomIndex);
            }
        }

        AudioScourseArray[randomIndex].Play();
        LastClipIndex = randomIndex;
    }

    public void FindAllAudioSoursesForAudioLIst()
    {
        AudioScourseArray =this.gameObject.GetComponents<AudioSource>();
    }
}
