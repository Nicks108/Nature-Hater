using UnityEngine;
using System.Collections;

public class S_ParticalSystemChildren : MonoBehaviour
{

    ParticleSystem[] ChildPSArray;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private ParticleSystem[] getParticalSystems()
    {
        System.Collections.Generic.List<ParticleSystem> pslist = new System.Collections.Generic.List<ParticleSystem>();
        foreach (Transform child in this.transform)
        {
            ParticleSystem ps = child.gameObject.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                pslist.Add(ps);
            }
        }

        return pslist.ToArray();
    }
    public void Simulate(float f, bool b)
    {
        ChildPSArray = getParticalSystems();
        foreach (var child in ChildPSArray)
        {
            child.Simulate(f, b);
        }
    }
    /// <summary>
    /// passing 0 will tell systems to use random seed.
    /// </summary>
    /// <param name="RandomSeed"></param>
    public void SetSeed(uint RandomSeed)
    {
        ChildPSArray = getParticalSystems();
        foreach (var child in ChildPSArray)
        {
            child.randomSeed = RandomSeed;
        }
    }
    public void Play(bool b)
    {
        ChildPSArray = getParticalSystems();
        foreach (var child in ChildPSArray)
        {
            child.Play(b);
        }
        //Debug.Log("particals now playing");
    }
}