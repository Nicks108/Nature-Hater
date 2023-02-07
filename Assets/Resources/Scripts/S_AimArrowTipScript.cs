using UnityEngine;
using System.Collections;

public class S_AimArrowTipScript : MonoBehaviour {

    public void rotateArrowTipToMatchOppositFingurePositionsFromPlayer(Vector3 playerPosition)
    {
        rotateArrowTipToMatchOppositFingurePositionsFromPlayer(playerPosition, 0);
    }
    public void rotateArrowTipToMatchOppositFingurePositionsFromPlayer(Vector3 playerPosition, float offset)
    {
        float xp = playerPosition.x - transform.position.x;
        float yp = playerPosition.y - transform.position.y;

        float theta = Mathf.Atan(yp/xp) * Mathf.Rad2Deg;
        theta += 90 + offset;
        //this.transform.Rotate(0, 0, theta, Space.Self);
        //Quaternion angleToRotateTo = Quaternion..Euler(transform.rotation.x, transform.position.y ,theta);
        //Debug.Log("Quaternian: " + transform.rotation);
        //transform.rotation = angleToRotateTo;
        //transform.Rotate(0, 0, theta);
        Vector3 temp = transform.rotation.eulerAngles;
        temp.Set(temp.x, temp.y, theta);
        transform.rotation = Quaternion.Euler(temp);
    }

    public void positionArrowTipAtOppositSideOfPlayerRelativeToMousePoint(Vector3 MousePosition, Vector3 playerPosition)
    {
        float DistanceFromPlayerToMainCam = Vector3.Distance(playerPosition, Camera.main.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(MousePosition);
        Vector3 MousePositionInWorldSpace = ray.origin + (ray.direction * DistanceFromPlayerToMainCam);
        //Debug.Log("mouse position world space: " + MousePositionInWorldSpace);
        //Debug.Log("mouse position: "+MousePosition);
        //Debug.Log("player position: " + playerPosition);
        Vector3 DistanceRelativeToPlayer = playerPosition - MousePositionInWorldSpace;
        //Debug.Log("distance relative to player: " + DistanceRelativeToPlayer);


        this.transform.position = DistanceRelativeToPlayer;
        this.transform.position += playerPosition;
        Vector3 PositionWithZReset = this.transform.position;
        PositionWithZReset.z = playerPosition.z;
        this.transform.position = PositionWithZReset;

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    //rotateArrowTipToMatchOppositFingurePositionsFromPlayer(target.transform.position);
	}
}
