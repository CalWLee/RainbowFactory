using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour
{
    public float speed = 250f;


    void Update()
    {
        transform.Rotate(-Vector3.forward, speed * Time.deltaTime);
    }
}