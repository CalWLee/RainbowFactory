using UnityEngine;
using System.Collections;

public class PipeManager : MonoBehaviour
{
    PipeCell[] cells;
    int nFilledCells;

    public int singleTime, doubleTime, tripleTime, quadTime;
    public Transform rainbowSpawnPoint;
    public generateTheSphere sphereGenerator;

    public float QuadScale = 1.0f;
    public float TripleScale = 0.8f;
    public float DoubleScale = 0.5f;
    public float SingleScale = 0.25f;

    
    
    private bool spawnedRainbow = true;
    private float spawnTimer;

    //The following three variables are only needed if we do not count the number of orange, green, and purple blocks as players make them.
    //Need to be able to count colors for each row/column
    private int numOrange = 0;
    private int numGreen = 0;
    private int numPurple = 0;

    // Use this for initialization
    void Start()
    {
        cells = gameObject.GetComponentsInChildren<PipeCell>();
        foreach (PipeCell cell in cells)
        {
            cell.target = rainbowSpawnPoint;
        }
    }

    public Color wildCard()
    {
        Color maxColor = Color.clear;
        int maxNum = 0;

        foreach (PipeCell cell in cells)
        {
            //This method of scoring goes through the blocks in a row/column and then counts how many were of each color.
            //It assumes that the secondary colors are stored in an array and that the primary colors are stroed in an array.			
            if (cell.CurrentColor == PipeCell.ORANGE)
            {
                numOrange++;

                if (numOrange > maxNum)
                {
                    maxNum = numOrange;
                    maxColor = PipeCell.ORANGE;
                }

            }
            else if (cell.CurrentColor == PipeCell.GREEN)
            {
                numGreen++;

                if (numGreen > maxNum)
                {
                    maxNum = numGreen;
                    maxColor = PipeCell.GREEN;
                }

            }
            else if (cell.CurrentColor == PipeCell.PURPLE)
            {
                numPurple++;

                if (numPurple > maxNum)
                {
                    maxNum = numPurple;
                    maxColor = PipeCell.PURPLE;
                }
            }
        }

        if (maxColor == Color.clear)
        {
            int rand = Random.Range(0, 30) % 3;

            switch(rand)
            {
                case 0:
                    maxColor = PipeCell.GREEN;
                    break;
                case 1:
                    maxColor = PipeCell.ORANGE;
                    break;
                default:
                    maxColor = PipeCell.PURPLE;
                    break;
            }
        }

        return maxColor;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer = Mathf.Max(0, spawnTimer - Time.deltaTime);

        if (spawnTimer == 0 && !spawnedRainbow)
        {
            if ((rainbowSpawnPoint.position - sphereGenerator.startPosition_1.position).sqrMagnitude == 0)
            {
                sphereGenerator.isGenerate_1 = true;
            }
            else if ((rainbowSpawnPoint.position - sphereGenerator.startPosition_2.position).sqrMagnitude == 0)
            {
                sphereGenerator.isGenerate_2 = true;
            }
            else if ((rainbowSpawnPoint.position - sphereGenerator.startPosition_3.position).sqrMagnitude == 0)
            {
                sphereGenerator.isGenerate_3 = true;
            }
            else if ((rainbowSpawnPoint.position - sphereGenerator.startPosition_4.position).sqrMagnitude == 0)
            {
                sphereGenerator.isGenerate_4 = true;
            }
            else if ((rainbowSpawnPoint.position - sphereGenerator.startPosition_5.position).sqrMagnitude == 0)
            {
                sphereGenerator.isGenerate_5 = true;
            }
            else if ((rainbowSpawnPoint.position - sphereGenerator.startPosition_6.position).sqrMagnitude == 0)
            {
                sphereGenerator.isGenerate_6 = true;
            }
            spawnedRainbow = true;
        }

        nFilledCells = 0;

        foreach (PipeCell cell in cells)
        {
            if (cell.ps == PipeState.MIXED)
            {
                nFilledCells++;
            }
        }

        if (nFilledCells == cells.Length)
        {
                numOrange = 0;
                numGreen = 0;
                numPurple = 0;

            foreach (PipeCell cell in cells)
            {

                //This method of scoring goes through the blocks in a row/column and then counts how many were of each color.
                //It assumes that the secondary colors are stored in an array and that the primary colors are stroed in an array.			
                if (cell.CurrentColor == PipeCell.ORANGE)
                {
                    numOrange++;
                }
                else if (cell.CurrentColor == PipeCell.GREEN)
                {
                    numGreen++;
                }
                else if (cell.CurrentColor == PipeCell.PURPLE)
                {
                    numPurple++;
                }

                cell.drainPipe();
            }

            spawnTimer = PipeCell.drainInterval;
            spawnedRainbow = false;

            Debug.Log("Green: " + numGreen + " Oranges: " + numOrange + " Purples: " + numPurple);



            if (numOrange == 4 || numGreen == 4 || numPurple == 4)
            {
                Scoring.AddScore(Scoring.FOUR_SCORE, quadTime);
                sphereGenerator.setScale(QuadScale);
            }
            else if (numOrange == 3 || numGreen == 3 || numPurple == 3)
            {
                Scoring.AddScore(Scoring.TRIPLE_SCORE, tripleTime);
                sphereGenerator.setScale(TripleScale);
            }
            else if (numOrange == 1 || numGreen == 1 || numPurple == 1)
            {
                Scoring.AddScore(Scoring.ONE_PAIR_SCORE, singleTime);
                sphereGenerator.setScale(SingleScale);
            }
            else
            {
                Scoring.AddScore(Scoring.TWO_PAIR_SCORE, doubleTime);
                sphereGenerator.setScale(DoubleScale);
            }
        }
    }
}
