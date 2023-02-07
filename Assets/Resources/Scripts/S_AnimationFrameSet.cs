using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_AnimationFrameSet {

    public List<Texture> FrameSet;
    public int StartFrame = 0;
    public int FrameRate = 2;
    public float Currentframe = 0; // set to 0 as a precoution

    public int getCurrentFrameNumberAsInt()
    {
        return System.Convert.ToInt32(Currentframe);
    }

    public void Awake()
    {
        Currentframe = StartFrame; //animation starts at start frame
    }

    public void InsertFrame(Texture frame)
    {
        FrameSet.Add(frame);
    }
    public void InsertFrame(Texture frame, int position)
    {
        FrameSet.Insert(position, frame);
    }

    public Texture GetNextFrame()
    {
        return GetFrame(getCurrentFrameNumberAsInt() + 1);
    }
    public Texture getCurrentframe()
    {
        return GetFrame(getCurrentFrameNumberAsInt());
    }

    /// <summary>
    /// Increments the curent frame number
    /// rolles frame number over is >= to FrameSet.count()
    /// </summary>
    /// <returns>returns the result of incrementing the current fram number</returns>
    public int IncrementCurrentFrame()
    {
        Currentframe++;
        if (Currentframe >= FrameSet.Count)
        {
            Currentframe = 0;
        }
        return getCurrentFrameNumberAsInt();
    }

    public Texture GetFrame(int frameNumber)
    {
        if (frameNumber >= FrameSet.Count)
            throw new System.Exception("requested frame number " + frameNumber + " greater than or = to number of frames in FrameSet" + FrameSet.Count);

        return FrameSet[frameNumber];
    }

    private bool _isPlayOnce = false;
    public bool IsPlayOnce
    {
        get { return _isPlayOnce; }
    }
    S_AnimationFrameSet NextAnimationSet;
    public S_AnimationFrameSet PlayOnceAndMoveToAnimiation(S_AnimationFrameSet nextAnimation)
    {
        Currentframe = 0;
        NextAnimationSet = nextAnimation;
        _isPlayOnce = true;
        return this;
    }
    public S_AnimationFrameSet GetNextAnimation()
    {
        _isPlayOnce = false;
        return NextAnimationSet;
    }
}
