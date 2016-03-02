using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public GridCell instance;
    private static List<GridCell> cellList;
    private const int MAX_RAND = 30;
    private static float wildcardCD = 0;
    private static Vector3 minBounds = Vector3.zero, maxBounds = Vector3.zero;

    public static Vector3 MinBounds
    {
        get
        {
            return minBounds;
        }
    }

    public static Vector3 MaxBounds
    {
        get
        {
            return maxBounds;
        }
    }

    private TetherColumn[] columns;
    private TetheringTrigger[] allTriggers;
    private static Color[] colorSet = { Color.red, Color.blue, Color.yellow };
    private static int colorFreq;
    private static Color lastColor = Color.clear;

    void Start()
    {
        columns = gameObject.GetComponentsInChildren<TetherColumn>();
        allTriggers = gameObject.GetComponentsInChildren<TetheringTrigger>();
        cellList = new List<GridCell>();

        foreach (BoxCollider bc in GetComponentsInChildren<BoxCollider>())
        {
            if (!bc.isTrigger)
            {
                minBounds.x = Mathf.Min(minBounds.x, bc.transform.position.x);
                minBounds.y = Mathf.Min(minBounds.y, bc.transform.position.y);

                maxBounds.x = Mathf.Max(maxBounds.x, bc.transform.position.x);
                maxBounds.y = Mathf.Max(maxBounds.y, bc.transform.position.y);
            }
        }
        initializeBoard();
    }

    void initializeBoard()
    {
        for (int i = 0; i < allTriggers.Length; i++)
        {
            Vector3 spawnLoc = allTriggers[i].transform.position;
            GameObject cellObj = (GameObject)GameObject.Instantiate(instance.gameObject, spawnLoc, new Quaternion());

            allTriggers[i].markForUse();
            cellList.Add(cellObj.GetComponent<GridCell>());
        }
    }

    public static Color getRandomColor()
    {
        int rand = Random.Range(0, 3);
        Color outputColor;

        rand = Random.Range(0, 15);
        if (rand == 0)
        {
            outputColor = Color.white;
        }
        else
        {
            rand = rand % 3;
            outputColor = colorSet[rand];
        }


        for (int i = 0; i < 3; i++)
        {
            if (outputColor.Equals(lastColor))
            {
                rand = Random.Range(0, 15);
                if (rand == 0 && wildcardCD >= 3)
                {
                    outputColor = Color.white;
                    wildcardCD = 0;
                }
                else
                {
                    rand = rand % 3;
                    outputColor = colorSet[rand];
                }
            }
            else
            {
                break;
            }
        }

        lastColor = outputColor;

        return outputColor;
    }

    public static void removeCell(GridCell gc)
    {
        cellList.Remove(gc);
    }

    public Vector3 getNextLoc()
    {
        Vector3 next = Vector3.zero;

        for (int i = 0; i < allTriggers.Length; i++)
        {
            if (allTriggers[i].getTetherState() == TetherStatus.READY)
            {
                allTriggers[i].markForUse();
                next = allTriggers[i].transform.position;
                next.y = 12;
                break;
            }
        }

        return next;
    }

    void LateUpdate()
    {
        /*
        Ray ray;
        RaycastHit hit;
        TetheringTrigger otherTether;        
        */
        wildcardCD += Time.deltaTime;
        if (cellList.Count < 20)
        {
            foreach (TetherColumn col in columns)
            {
                col.reevalColumns();
            }

            /*
            foreach (GridCell gc in cellList)
            {
                if (gc.transform.position.y > -6 && gc.Anchor)
                {
                    ray = new Ray(gc.Anchor.transform.position, (Vector3.down));

                    if (Physics.Raycast(ray, out hit, 4 * GridCell.CARD_GAP, LayerMask.GetMask("tethers")))
                    {
                        otherTether = hit.transform.gameObject.GetComponent<TetheringTrigger>();
                        if (otherTether.getTetherState() == TetherStatus.READY)
                        {
                            gc.Anchor.cutTether();
                            gc.forceAnchor(otherTether);
                            Debug.Log("New Anchor");
                        }
                    }
                }
            }*/

            Vector3 spawnLoc = getNextLoc();
            //Debug.Log("NextLoc: " + spawnLoc);
            GameObject cellObj = (GameObject)GameObject.Instantiate(instance.gameObject, spawnLoc, new Quaternion());
            cellList.Add(cellObj.GetComponent<GridCell>());
        }
    }
}