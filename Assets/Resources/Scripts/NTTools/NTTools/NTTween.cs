using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NTTools
{
	public class NTTween
	{
        public float TweenTime = 0f;
        public Vector2 To2D = new Vector2();
        public Vector3 To3D = new Vector3();
        public GameObject From3D;
        public Rect From2D = new Rect();
        public float FrameTime = 0f;

        public NTTween()
        {
        }

        public NTTween(Vector2 _To, float tweenTime, float frameTime, Rect _From)
        {
            SetTween2D(_To, tweenTime, frameTime,_From);
        }

        public NTTween(Vector3 _To, float tweenTime, float frameTime, GameObject _From)
        {
            SetTween3D(_To, tweenTime, frameTime, _From);
        }

        public Vector2 Tween2D(Rect fromPos, Vector2 _To, float frameTime, ref float tweenTime)
        {
            Vector2 MoveAmountPerFrame2D = new Vector2();
            //Debug.Log("Tweentime " + TweenTime);
            if (tweenTime > 0f)
            {
                Vector2 Difference = new Vector2();
                Difference.x = _To.x - fromPos.x;
                Difference.y = _To.y - fromPos.y;

                //Debug.Log("To2D " + To2D);
                //Debug.Log("from2D " + fromPos);
                //Debug.Log("Difference " + Difference);

                //Debug.Log("FrameTime " + FrameTime);

                if (TweenTime == 0)
                    TweenTime = 1f;
                //TODO why is this not smooth? 
                //why is it jittery at long time intervals
                MoveAmountPerFrame2D = (Difference / tweenTime) * frameTime;
                tweenTime -= frameTime;

                //Debug.Log("Difference / TweenTime " + (Difference / TweenTime));
                //Debug.Log("move amount: " + MoveAmountPerFrame2D);
            }
            return MoveAmountPerFrame2D;
        }

        public void SetTween2D(Vector2 _To, float tweenTime, float frameTime, Rect _From)
        {
            To2D = _To;
            TweenTime = tweenTime;
            From2D = _From;
            FrameTime = frameTime;
            
        }

        public void SetTween3D(Vector3 _To, float tweenTime, float frameTime, GameObject _From)
        {
            To3D = _To;
            TweenTime = tweenTime;
            From3D = _From;
            FrameTime = frameTime;

            Vector3 Difference = new Vector3();

            Difference.x = To3D.x - From3D.transform.position.x;
            Difference.y = To3D.y - From3D.transform.position.y;
            Difference.z = To3D.z - From3D.transform.position.z;


        }

        public static Vector2 Tween2D(Vector2 to, Vector2 from, float frameTime, AnimationCurve curve)
        {
            float x = NTTween.Tween(to.x, from.x, frameTime, curve);
            float y = NTTween.Tween(to.y, from.y, frameTime, curve);
            return new Vector2(x, y);
        }
        public static float Tween(float startVal, float endVal, float time, AnimationCurve curve)
        {
            return Mathf.Lerp(startVal, endVal, curve.Evaluate(time));
        }

	}
}
