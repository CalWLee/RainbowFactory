using UnityEngine;
using System.Collections;

public class TetherColumn : MonoBehaviour {

    TetheringTrigger[] tethers;
    TetheringTrigger minTether;
    byte activeCount;

	// Use this for initialization
	void Start () {
        tethers = gameObject.GetComponentsInChildren<TetheringTrigger>();

        //getNextTether();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void reevalColumns()
    {
        TetheringTrigger nextOccupied;

        for (int i = 0; i < tethers.Length - 1; i++)
        {
            if (tethers[i].getTetherState() == TetherStatus.READY)
            {
                nextOccupied = tethers[i];
                for (int j = i; j < tethers.Length; j++)
                {
                    nextOccupied = tethers[j];
                    if (nextOccupied.Attached)
                    {
                        break;
                    }
                }

                if (nextOccupied.Attached)
                {
                    tethers[i].forceAttach(nextOccupied.Attached);
                    nextOccupied.cutTether();
                }
            }
        }
    }

    //public Vector3 getNextTether()
    //{
    //    if (checkActiveTethers() >= tethers.Length)
    //    {
    //        return -20 * Vector3.one;
    //    }

    //    Vector3 min = tethers[tethers.Length - 1].transform.position;
    //    minTether = tethers[tethers.Length - 1];

    //    foreach (TetheringTrigger t in tethers)
    //    {
    //        //Debug.Log(t.transform.position + " Occupied: " + t.isOccupied() + " vs " + min);
    //        if (t.transform.position.y < min.y && t.getTetherState() == TetherStatus.READY)
    //        {
    //            //Debug.Log(t.transform.position + " is new Min");
    //            min = t.transform.position;
    //            minTether = t;
    //        }
    //    }
    //    //Debug.Log("Min: " + min);
        

    //    foreach (TetheringTrigger t in tethers)
    //    {
    //        if (t.transform.position.y > minTether.transform.position.y && t.getTetherState() == TetherStatus.READY)
    //        {
    //            t.collider.enabled = false;
    //        }
    //    }

    //    minTether.collider.enabled = true;

    //    return minTether.transform.position;
    //}

    //public int checkActiveTethers()
    //{
    //    activeCount = 0;
    //    //Check the status of each tether
    //    foreach (TetheringTrigger t in tethers)
    //    {
    //        if (t.isOccupied())
    //        {
    //            activeCount++;
    //        }
    //    }
    //    return activeCount;
    //}
}
