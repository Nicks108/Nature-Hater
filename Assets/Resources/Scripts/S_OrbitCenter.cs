using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_OrbitCenter : MonoBehaviour {

    
    public GameObject[] WeaponArray;
    public float[] WeaponAngles;

    public int SelectedWeaponIndex;

    public float Distance;
    public float _angle;
    public float Angle
    {
        get { return _angle; }
        set { _angle = value % (Mathf.Deg2Rad * 360); }
    }
    
    public float AdditiveAngle;

    private Vector3 _lastMouse;

    public GameObject GetSelectedWeapon()
    {
        return WeaponArray[SelectedWeaponIndex];
    }




	// Use this for initialization
	void Start () {

        WeaponAngles = new float[WeaponArray.Length];

        AdditiveAngle =  Mathf.Deg2Rad * (360 / WeaponArray.Length);
        //AdditiveAngle -= AdditiveAngle;

        for (int i = 0; i < WeaponArray.Length; i++)
            WeaponAngles[i] = (i%3) * AdditiveAngle;

        RotateWeapons(Angle);
	}

    void OnGUI()
    {
        RotateWeapons(Angle);
    }

    public float Lim;
    private void WeaponRotateSnapToSelection()
    {

        for (int i = 0; i < WeaponAngles.Length; i++)
        {
            float angleToCompare = 0;
            if (WeaponAngles[i] >= 0)
            {
                angleToCompare = AdditiveAngle;
            }
            else
            {
                angleToCompare = WeaponAngles[WeaponAngles.Length - 1];
            }
            if (Mathf.Abs(WeaponAngles[i]) < angleToCompare + Lim && Mathf.Abs(WeaponAngles[i])  > angleToCompare - Lim)
            {
                //Debug.Log("Weapon About to be selected");
                //for (int j = i; j < i + (WeaponAngles.Length - 1); j++)
                //{
                //    WeaponAngles[j] = AdditiveAngle * ((j%WeaponAngles.Length-1) + 1);
                //}
                WeaponArray[i].GetComponent<S_GUI_Weapon>().setCurrentWeapon();
            }
        }

    }

    private void RotateWeapons(float fAdditivAngle)
    {
        float CurrentAngle = 0;
        for (int i = 0; i < WeaponArray.Length; i++ )
        {
            CurrentAngle = WeaponAngles[i];
            Vector2 NewPosition = OrbitArountPoint2D((Vector2)WeaponArray[i].transform.position, (Vector2)this.transform.position, CurrentAngle, Distance);
            Vector3 newVec3 = new Vector3(NewPosition.x, NewPosition.y);
            WeaponArray[i].transform.position = newVec3;
            CurrentAngle += fAdditivAngle;
        }
    }

	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        //RotateWeapons(360/WeaponArray.Length);
        //_lastMouse = Input.mousePosition;
    }

    void OnMouseUp()
    {
        WeaponRotateSnapToSelection();
    }

    public float RotationSpeed = 1;

    void OnMouseDrag()
    {
        //if (Input.mousePosition.magnitude > _lastMouse.magnitude * RotationSpeed)
        //{
        //    //SelectedWeaponIndex++;
        //    Angle++;
        //}
        //else if (Input.mousePosition.magnitude * RotationSpeed < _lastMouse.magnitude)
        //{
        //    Angle--;
        //}

        Angle = (Input.mousePosition.magnitude - _lastMouse.magnitude)/ RotationSpeed;
        for (int i = 0; i < WeaponAngles.Length; i++ )
        {
            WeaponAngles[i] = (Angle + WeaponAngles[i]) % (Mathf.Deg2Rad * 360);
            //Debug.Log("New weapon angle: "+ WeaponAngles[i]);
            if (WeaponAngles[i] < 0)
            {
                //Debug.Log("max rad: " + Mathf.Deg2Rad * 360);
                //Debug.Log("max Rad + Weapon angle" + ((Mathf.Deg2Rad * 360) - WeaponAngles[i]));
                WeaponAngles[i] = ((Mathf.Deg2Rad * 360) - WeaponAngles[i]);
                //% AdditiveAngle*(WeaponArray.Length-1)
            }
        }
        _lastMouse = Input.mousePosition;
    }

    public Vector2 OrbitArountPoint2D(Vector2 OrigionaPositon, Vector2 CenterPoint, float fAngle, float fDistance)
    {
        float newAngle = fAngle % 360;
        float NewX = CenterPoint.x + fDistance * Mathf.Cos(newAngle);
        float NewY = CenterPoint.y + fDistance * Mathf.Sin(newAngle);
        return new Vector2(NewX, NewY);
    }
}
