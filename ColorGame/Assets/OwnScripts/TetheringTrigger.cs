using UnityEngine;
using System.Collections;

public enum TetherStatus
{
    INVALID, READY, MARKED, OCCUPIED, MAX
}

public enum TetherType
{
    INVALID, NORMAL, PREVIEW, MAX
}

public class TetheringTrigger : MonoBehaviour {
    public TetherType Type;

    //Make this a trinary conidition: Empty, Marked, Occupied
    private TetherStatus inUse = TetherStatus.READY;
    private TetheringTrigger[] neighbors;
    private GridCell attached;

    public GridCell Attached
    {
        get
        {
            return attached;
        }
    }

    public TetherStatus getTetherState()
    {
        return inUse;
    }

	// Update is called once per frame
	void OnTriggerEnter(Collider other) 
    {
        GridCell gc = other.GetComponent<GridCell>();

        //Debug.Log("Hi");
        if (gc)
        {
            if (gc.State == CellState.FREE_FALL && inUse != TetherStatus.OCCUPIED)
            {
                forceAttach(gc);
            }
        }
	}

    public void markForUse()
    {
        this.inUse = TetherStatus.MARKED;
    }

    public void forceAttach(GridCell gc)
    {
        attached = gc;
        gc.setAnchor(this);
        inUse = TetherStatus.OCCUPIED;
    }

    public void moveAttached(Vector3 v)
    {
        if (this.Type == TetherType.NORMAL)
        {
            if (this.attached)
            {
                this.attached.transform.position = this.transform.position - v;
            }
        }
    }

    public void cutTether()
    {
        //Debug.Log("Tether cut");
        inUse = TetherStatus.READY;
        attached = null;
    }
}
