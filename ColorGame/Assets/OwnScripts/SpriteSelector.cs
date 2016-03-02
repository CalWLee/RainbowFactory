using UnityEngine;
using System.Collections;

public enum Orientation
{
    INVALID,
    VERTICAL,
    HORIZONTAL,
    MAX
}

public class SpriteSelector : MonoBehaviour {

    //an array of srpites to choose from
    public Sprite[] alternateSprites;

    private Sprite chosenSprite;
    private SpriteRenderer spriteRenderer;
    private int layer;

	// Use this for initialization
	void Start () {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = chosenSprite;
        spriteRenderer.sortingOrder = layer;
	}

    public void chooseColorSprite(Color color, float angle = 0)
    {
        int index = alternateSprites.Length - 1;

        if (color.Equals(PipeCell.ORANGE))
        {
            index = 0;
        }
        else if (color.Equals(PipeCell.GREEN))
        {
            index = 1;
        }
        else if (color.Equals(PipeCell.PURPLE))
        {
            index = 2;
        }
         layer = 1;

        if (Mathf.Round(angle) == 0)
        {
            index += 3;
            layer = 2;
        }

        chosenSprite = alternateSprites[index];
    }

	// Update is called once per frame
	void Update () {
	
	}
}
