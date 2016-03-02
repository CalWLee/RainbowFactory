﻿using UnityEngine;
using System.Collections;

public class gameOverBoy : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        if (Input.touches.Length >= 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
                RaycastHit rayhit;
                if (Physics.Raycast(ray, out rayhit, 10.0f, LayerMask.GetMask("startButton")))
                {
                    if (this.gameObject.transform == rayhit.transform)
                    {
                        Application.Quit();
                    }
                }
            }
        }

    }
}
