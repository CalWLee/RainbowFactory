using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PipeState
{
    INVALID = -1, EMPTY, MIXED, DRAINING, MAX_VALUE
}

public class PipeCell : MonoBehaviour
{
    public AudioClip MixSound;
    public AudioClip DrainSound;
    new private AudioSource audio;



    public PipeState ps;
    
    public SpriteSelector cellObject;
    public GameObject sprite;
    public Transform target;
    List<GridCell> cellList = new List<GridCell>();
    public static float drainInterval = 0.2f;

    public PipeState lastState
    {
        get;
        private set;
    }

    Color occupiedColor;

    public Color CurrentColor
    {
        get
        {
            return occupiedColor;
        }
    }

    float drainTimer;

    public static Color ORANGE = new Color(1, .5f, 0);
    public static Color PURPLE = new Color(.5f, 0, .5f);
    public static Color GREEN = Color.green;

    // Use this for initialization
    void Start()
    {
        ps = PipeState.EMPTY;
        occupiedColor = Color.clear;
        this.renderer.material.color = occupiedColor;

        audio = gameObject.GetComponentInParent<AudioSource>();
    }

    private bool isInvalid(GridCell gc)
    {
        return gc.State == CellState.INVALID;
    }

    void Update()
    {
        cellList.RemoveAll(isInvalid);
        switch (ps)
        {
            case PipeState.EMPTY:
                bool allIdle = cellList.Count >= 2;
                bool mixReady = false;

                if (cellList.Count >= 2)
                {
                    //Debug.Log(this.transform.position);
                }

                foreach (GridCell gc in cellList)
                {
                    allIdle = allIdle && gc.State == CellState.IDLE;
                    mixReady = mixReady || gc.isMixReady();
                }

                if (allIdle && mixReady)
                {

                    Color one = Color.clear, two = Color.clear;
                    bool killCells = false;

                    GridCell[] pair = cellList.ToArray();

                    one = pair[0].CellColor;
                    two = pair[1].CellColor;

                    if (pair[0].isWildCard || pair[1].isWildCard)
                    {
                        killCells = this.mixWildCard();                        
                    }
                    else
                    {
                        killCells = this.mixColor(one, two);
                    }

                    if (killCells)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            pair[i].killCell();
                        }
                        cellList.Clear();
                    }
                }
                break;
            case PipeState.DRAINING:
                this.drainTimer -= Time.deltaTime;
                this.occupiedColor = Color.Lerp(occupiedColor, Color.clear, (2 - this.drainTimer) / 2);

                sprite.transform.position = Vector3.Lerp(this.transform.position, target.position, (drainInterval - this.drainTimer) / drainInterval);

                //Debug.Log("Draining...");
                if (drainTimer <= 0)
                {
                    Destroy(sprite);
                    //generateTheSphere.isGenerate_1 = true;
                    this.ps = PipeState.EMPTY;
                }

                break;
            default:
                break;
        }

        //this.renderer.material.color = occupiedColor;
    }

    public bool mixWildCard()
    {

        audio.PlayOneShot(MixSound);

        PipeManager pm = this.GetComponentInParent<PipeManager>();
        this.occupiedColor = pm.wildCard();

        sprite = Instantiate(cellObject.gameObject, transform.position, new Quaternion()) as GameObject;
        sprite.GetComponent<SpriteSelector>().chooseColorSprite(occupiedColor, transform.parent.localEulerAngles.z);

        ps = PipeState.MIXED;
        return true;
    }

    public bool mixColor(Color cOne, Color cTwo)
    {
        if ((cOne == Color.red && cTwo == Color.yellow) || (cTwo == Color.red && cOne == Color.yellow))
        {
            audio.PlayOneShot(MixSound);
            //Camera.main.GetComponent<SoundManager>().PlaySound(SoundManager.Sound.MIX);
            occupiedColor = ORANGE;
        }
        else if ((cOne == Color.blue && cTwo == Color.yellow) || (cTwo == Color.blue && cOne == Color.yellow))
        {
            audio.PlayOneShot(MixSound);
            //Camera.main.GetComponent<SoundManager>().PlaySound(SoundManager.Sound.MIX);
            occupiedColor = Color.green;
        }
        else if ((cOne == Color.blue && cTwo == Color.red) || (cTwo == Color.blue && cOne == Color.red))
        {
            audio.PlayOneShot(MixSound);
            //Camera.main.GetComponent<SoundManager>().PlaySound(SoundManager.Sound.MIX);
            occupiedColor = PURPLE;
        }
        else
        {
            occupiedColor = Color.clear;
        }

        if (occupiedColor != Color.clear)
        {
            sprite = Instantiate(cellObject.gameObject, transform.position, new Quaternion()) as GameObject;
            sprite.GetComponent<SpriteSelector>().chooseColorSprite(occupiedColor, transform.parent.localEulerAngles.z);

            ps = PipeState.MIXED;
            //Debug.Log("Mixed!");
        }

        return ps == PipeState.MIXED;
    }

    public void drainPipe()
    {
        audio.PlayOneShot(DrainSound); 

        //Debug.Log("DrainPipe");
        drainTimer = drainInterval;
        ps = PipeState.DRAINING;

    }

    void OnTriggerEnter(Collider other)
    {
        GridCell gc = other.GetComponent<GridCell>();
        if (gc)
        {
            if (gc.State == CellState.INVALID)
            {
                //Debug.Log("Uh oh, at " + gc.transform.position);
            }
            cellList.Add(gc);
        }
    }


    void OnTriggerExit(Collider other)
    {
        GridCell gc = other.GetComponent<GridCell>();
        //Debug.Log(gc);
        cellList.Remove(gc);
    }

    public void changeState(PipeState p)
    {
        lastState = ps;
        ps = p;
    }

}
