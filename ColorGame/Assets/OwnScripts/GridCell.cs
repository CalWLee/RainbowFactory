using UnityEngine;
using System.Collections;

public enum CellState
{
    INVALID, FREE_FALL, IDLE, PICKED_UP, MAX
}

public class GridCell : MonoBehaviour {

    public const float LOCK_THRESH = .5f, CARD_GAP = 1.25f, GAP_SQRD = CARD_GAP * CARD_GAP;    //Constants used for moving the card
    public TetheringTrigger Anchor
    {
        get
        {
            return this.anchor;
        }
    }

    public Sprite[] sprites;

    public Color CellColor
    {
        get
        {
            return color;
        }
    }

    public CellState State
    {
        get
        {
            return cellState;
        }
    }

    public bool isWildCard
    {
        get;
        private set;
    }

    public bool isMixReady()
    {
        return lastState == CellState.PICKED_UP && cellState == CellState.IDLE;
    }

    private SpriteRenderer sr;
    private Rigidbody rb;
    private BoxCollider bc;
    private CellState cellState, lastState;
    private TetheringTrigger anchor;
    private Color color;
    private Vector3 lockedDir, touchPosition;
    private Vector3 origScale;
    private float colorTimer;
    private Color[] colorCycle = { Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta };
    
	// Use this for initialization
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        bc = this.GetComponent<BoxCollider>();

        if (!rb)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        if (!bc)
        {
            bc = gameObject.AddComponent<BoxCollider>();
        }

        switchState(CellState.FREE_FALL);

        this.color = GridManager.getRandomColor();
        sr = this.GetComponentInChildren<SpriteRenderer>();

        isWildCard = false;

        if (this.color == Color.red)
        {
            sr.sprite = sprites[0];
        }
        else if (this.color == Color.blue)
        {
            sr.sprite = sprites[1];
        }
        else if (this.color == Color.yellow)
        {
            sr.sprite = sprites[2];
        }
        else
        {
            isWildCard = true;
            sr.sprite = sprites[sprites.Length-1];
        }
        sr.transform.localScale = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 velocity = Vector3.zero;
        Ray ray;
        RaycastHit hit;

        sr.enabled = transform.position.y < 11;

        if (isWildCard)
        {
            colorTimer += Time.deltaTime;

            int cIndex = Mathf.RoundToInt(colorTimer);

            sr.color = Color.Lerp(sr.color, colorCycle[(cIndex) % 6], 6*Time.fixedDeltaTime);
        }

        switch (cellState)
        {
            case CellState.IDLE:

                if ((transform.position - anchor.transform.position).sqrMagnitude > 0)
                {
                    this.transform.position = Vector3.SmoothDamp(transform.position, anchor.transform.position, ref velocity, 0.1f);
                }
                if (anchor.Type == TetherType.NORMAL)
                {
                    this.squeeze(transform.position - anchor.transform.position);
                }
                else
                {
                    this.squeeze(Vector3.one);
                }
                break;
            case CellState.PICKED_UP:
                //This is the code to move the card 
                               
                ray = Camera.main.ScreenPointToRay(touchPosition);
                Vector3 temp = ray.GetPoint(10);
                
                //Originall used to determine which direction to move in case of stray swipes
                float cx, cy;

                    if ((temp - anchor.transform.position).sqrMagnitude >= LOCK_THRESH)
                    {
                        cx = Mathf.Abs(temp.x - anchor.transform.position.x);
                        cy = Mathf.Abs(temp.y - anchor.transform.position.y);

                        if (cx > cy)
                        {
                            lockedDir = Vector3.right;
                            temp.y = anchor.transform.position.y;
                        }
                        else
                        {
                            lockedDir = Vector3.up;
                            temp.x = anchor.transform.position.x;
                        }
                    }
                

                //Cap the positions so it doesn't run too far from it's anchor
                temp.x = Mathf.Clamp(temp.x, anchor.transform.position.x - CARD_GAP, anchor.transform.position.x + CARD_GAP);
                temp.y = Mathf.Clamp(temp.y, anchor.transform.position.y - CARD_GAP, anchor.transform.position.y + CARD_GAP);

                //Clamp for Walls
                temp.x = Mathf.Clamp(temp.x, GridManager.MinBounds.x, GridManager.MaxBounds.x);
                temp.y = Mathf.Clamp(temp.y, GridManager.MinBounds.y, GridManager.MaxBounds.y);

                this.transform.position = temp;

                //Transmit the difference to an adjacent block
                float angle = Vector3.Dot(lockedDir, (temp - anchor.transform.position) / ((temp - anchor.transform.position).magnitude * Vector3.up.magnitude));

                ray = new Ray(anchor.transform.position, lockedDir * Mathf.Sign(angle));

                //determine which adjacent block to move when moving the block that's being touched.
                if (Physics.Raycast(ray, out hit, 4 * CARD_GAP, LayerMask.GetMask("tethers")))
                {
                    TetheringTrigger neighbor = hit.transform.gameObject.GetComponent<TetheringTrigger>();

                    if (neighbor)
                    {
                        neighbor.moveAttached(transform.position - anchor.transform.position);
                        //Debug.Log("Move");
                    }
                }

                this.squeeze(transform.position - anchor.transform.position);
                
                break;
            default:
                break;
        }
	}

    public void squeeze(Vector3 relativePos)
    {
        float mag = Mathf.Clamp(relativePos.sqrMagnitude, 0, 3);

        if (mag > 0.1f)
        {
            //Debug.Log(mag);
        }
        sr.transform.localScale = Vector3.one * (1 - (mag/6));
    }

    public void switchState(CellState cs)
    {
        lastState = cellState;
        cellState = cs;
    }

    public void forceAnchor(TetheringTrigger tt)
    {
        this.anchor = tt;
        this.anchor.forceAttach(this);
    }

    public void setAnchor(TetheringTrigger tt)
    {        
        this.anchor = tt;
        
        switchState(CellState.IDLE);

        this.rigidbody.velocity = Vector3.zero;
        this.rigidbody.useGravity = false;
    }

    public void killCell()
    {
        this.anchor.cutTether();
        this.switchState(CellState.INVALID);
        GridManager.removeCell(this);
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }

    public void setTouchPosition(Vector3 touch)
    {
        touchPosition = touch;
    }
}
