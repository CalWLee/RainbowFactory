using UnityEngine;
using System.Collections;

public enum RotationDirection
{
    Clockwise,
    CounterClockwise,
}

public class RotatingThing : MonoBehaviour {

    public RotationDirection Direction;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        switch (Direction)
        {
            case RotationDirection.CounterClockwise:
                transform.Rotate(Vector3.forward, -45 * Time.deltaTime);
                break;
            case RotationDirection.Clockwise:
                transform.Rotate(Vector3.forward, 45 * Time.deltaTime);
                break;
        }
	}
}
