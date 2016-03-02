using UnityEngine;
using System.Collections;

public class giveVelocity_Deatination_1 : MonoBehaviour 
{
    public Transform ChangeDirection1;
    public Transform ChangeDirection2;
    public Transform DestroyPosition;
    private float velocity = 15f;
    private const float distanceThreshold = 0.15f;

    void FixedUpdate()
    {
        Vector2 position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

        if (Vector2.Distance(position, ChangeDirection1.position) < distanceThreshold)
        {
            //print(Vector3.Distance(this.gameObject.transform.position, destination_1));
            gameObject.transform.position = ChangeDirection1.position;
            gameObject.rigidbody.velocity = new Vector2(0, velocity);
        }

        else if (Vector2.Distance(position, ChangeDirection2.position) < distanceThreshold)
        {
            //print(Vector3.Distance(this.gameObject.transform.position, destination_2));
            gameObject.transform.position = ChangeDirection2.position;
            gameObject.rigidbody.velocity = new Vector2(velocity, 0);
        }

        else if(Vector2.Distance(position, DestroyPosition.position) < distanceThreshold) //what happens when it reaches the canister
        {
            Destroy(gameObject);
        }

    }

	
}
