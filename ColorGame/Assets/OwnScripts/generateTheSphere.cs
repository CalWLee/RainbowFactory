using UnityEngine;
using System.Collections;

public class generateTheSphere : MonoBehaviour 
{
    [HideInInspector]
    public bool isGenerate_1 = false;
    [HideInInspector]
    public bool isGenerate_2 = false;
    [HideInInspector]
    public bool isGenerate_3 = false;
    [HideInInspector]
    public bool isGenerate_4 = false;
    [HideInInspector]
    public bool isGenerate_5 = false;
    [HideInInspector]
    public bool isGenerate_6 = false;

    public Transform startPosition_1;
    public Transform startPosition_2;
    public Transform startPosition_3;
    public Transform startPosition_4;
    public Transform startPosition_5;
    public Transform startPosition_6;
    public Rigidbody sphere;
    private float initialSpeed = -15f;

    public void setScale(float scale)
    {
        sphere.transform.localScale = new Vector3(.47f, .47f, 1) * scale;
    }

	void Update () 
    {
        if (isGenerate_1)
        {
            isGenerate_1 = false;
            Rigidbody rainbow = Instantiate(sphere, startPosition_1.position, startPosition_1.rotation) as Rigidbody;
            rainbow.rigidbody.velocity = (new Vector3(0, -initialSpeed, 0));
        }
        if (isGenerate_2)
        {
            isGenerate_2 = false;
            Rigidbody rainbow = Instantiate(sphere, startPosition_2.position, startPosition_2.rotation) as Rigidbody;
            rainbow.rigidbody.velocity = (new Vector3(0, -initialSpeed, 0));
        }
        if (isGenerate_3)
        {
            isGenerate_3 = false;
            Rigidbody rainbow = Instantiate(sphere, startPosition_3.position, startPosition_3.rotation) as Rigidbody;
            rainbow.rigidbody.velocity = (new Vector3(0, -initialSpeed, 0));
        }
        if (isGenerate_4)
        {
            isGenerate_4 = false;
            Rigidbody rainbow = Instantiate(sphere, startPosition_4.position, startPosition_4.rotation) as Rigidbody;
            rainbow.rigidbody.velocity = (new Vector3(initialSpeed, 0, 0));
        }
        if (isGenerate_5)
        {
            isGenerate_5 = false;
            Rigidbody rainbow = Instantiate(sphere, startPosition_5.position, startPosition_5.rotation) as Rigidbody;
            rainbow.rigidbody.velocity = (new Vector3(initialSpeed, 0, 0));
        }
        if(isGenerate_6)
        {
            isGenerate_6 = false;
            Rigidbody rainbow = Instantiate(sphere,startPosition_6.position,startPosition_6.rotation) as Rigidbody;
            rainbow.rigidbody.velocity = (new Vector3(initialSpeed, 0, 0));
        }
	}
}
