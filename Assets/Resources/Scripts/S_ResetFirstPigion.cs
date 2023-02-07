using UnityEngine;
using System.Collections;

public class S_ResetFirstPigion : MonoBehaviour
{
    public void Reset()
    {
        S_Pigion pigionScript = this.gameObject.GetComponent<S_Pigion>();

        pigionScript.Health = 1;
        pigionScript.attackState = S_2dSpriteBase.AttackStates.Idle;
    }

}
