using UnityEngine;
using System.Collections;

public class TouchLoogic : MonoBehaviour 
{
    //public Transform card;
    private Transform card;
    private GridCell gcCard;


    private Vector3 nextPosition;
    //private RaycastHit rayhit = new RaycastHit();
    //private Transform cameraTrans;
    //private float speed = 20f;
    //private float Force = 8.35f;
    //private float validAngular_x = 20f;
    //private float validAngular_y = 70f;
    //private float validSpeed = 300;
    //private float validDistance = 1f;
    //private bool ifdetect = false;
    //private int screenWidth;
    //private int screenHeight;
    //private float cardMoveSpeed = 2f;
    //for the direction
    //private bool right = false;
    //private bool left = false;
    //private bool up = false;
    //private bool down = false;
    //private float distanceToMove = 6f;
	// Use this for initialization
    void Start()
    {
        //cameraTrans = Camera.main.transform;
        //screenWidth = Screen.width;
        //screenHeight = Screen.height;    
    }
	
	// Update is called once per frame
	void Update ()
    {

        //This is Calvin's Picking up code
        if (Input.touches.Length >= 1)        //On Touch
        {
            //print("get1");
            if (Input.GetTouch(0).phase == TouchPhase.Began && Scoring.State == GameState.IN_GAME)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); //when you touch the screen, your finger shot a ray into screen and will hit a card, then the card you hit is the card you want to control;
                Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 10, LayerMask.GetMask("cards")))
                {
                    gcCard = hit.transform.gameObject.GetComponent<GridCell>();

                    if (gcCard && gcCard.State == CellState.IDLE && gcCard.Anchor.Type == TetherType.NORMAL)
                    {
                        gcCard.switchState(CellState.PICKED_UP);
                        gcCard.setTouchPosition(Input.GetTouch(0).position);
                    }
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (gcCard)
                {
                    gcCard.setTouchPosition(Input.GetTouch(0).position);
                }
            }

            //switchState(CellState.PICKED_UP);

            else if (Input.GetTouch(0).phase == TouchPhase.Ended) //OnRelease
            {
                if (gcCard)
                {
                    gcCard.switchState(CellState.IDLE); //turn it back to the idle state
                }
            }
        }
        //print(LayerMask.GetMask("cards"));
      /*  if(Input.touches.Length >= 1)
        {
             
            Touch temp = Input.GetTouch(0);

                if(temp.phase == TouchPhase.Began)
                {
                    ifdetect = true;
                    Ray ray = Camera.main.ScreenPointToRay(temp.position); //when you touch the screen, your finger shot a ray into screen and will hit a card, then the card you hit is the card you want to control;
                    Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
                    if(Physics.Raycast(ray,out rayhit,10.0f,LayerMask.GetMask("cards")))
                    {
                        //print("hitTheCardsLayer");
                        card = rayhit.transform;

                    }
                }

                if(temp.phase == TouchPhase.Moved&&ifdetect)
                {

                    //direction = new Vector2(0,0);
                    Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;  //direction you have swiped
                    float distance = deltaPosition.magnitude;     //distance you have swiped
                    float speedOfYourSwipe = distance / Input.GetTouch(0).deltaTime;//speed you have swipe;
                    float swipeAngle = Mathf.Atan(deltaPosition.y / deltaPosition.x) * Mathf.Rad2Deg; // get the angle

                   if(Mathf.Abs(swipeAngle) < validAngular_x)
                    {
                        if(speedOfYourSwipe > validSpeed)
                        {
                            if (distance > validDistance)
                            {
                               if(deltaPosition.normalized.x>0)//right
                               {

                                   print("right");
                                   nextPosition = new Vector3(card.position.x + distanceToMove, card.position.y, card.position.z);
                                   right = true;
                                   ifdetect = false;//forbid extra control
                                   
                               }
                                else if(deltaPosition.normalized.x<0)//left
                               {
                                   print("left");
                                   nextPosition = new Vector3(card.position.x - distanceToMove, card.position.y, card.position.z);
                                   left = true;
                                   ifdetect = false;//forbid extra control
                                   
                               }
                            }
                        }  
                    }
                   else if(Mathf.Abs(swipeAngle)>validAngular_y)
                   {
                       if (speedOfYourSwipe > validSpeed)
                       {
                           if (distance > validDistance)
                           {
                               if(deltaPosition.normalized.y>0)
                               {
                                   print("Up");
                                   nextPosition = new Vector3(card.position.x, card.position.y + distanceToMove, card.position.z);
                                   up = true;
                                   ifdetect = false;
                                   
                               }
                               else if(deltaPosition.normalized.y<0)
                               {
                                   print("down");
                                   nextPosition = new Vector3(card.position.x, card.position.y - distanceToMove, card.position.z);
                                   down = true;
                                   ifdetect = false;
                                   
                               }
                              // card.rigidbody.AddForce(new Vector2(0, deltaPosition.normalized.y) * Force, ForceMode.Impulse);
                           }
                       }
                      
                   }
                   //print("speed"+speedOfYourSwipe);
                   //print("angle"+swipeAngle);
                   //print("xposition"+card.transform.position.x);
                   // print("distance"+distance);
                   //print("width" + screenWidth);
                   // print("height"+screenHeight);         
                   /* Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                    if (Physics.Raycast(ray, out rayhit,1000,8))
                    {
                        rayhit.rigidbody.AddForce(Input.GetTouch(0).deltaPosition* Force,ForceMode.Impulse);
                    }*/
                }
                
        }
/*	}

    void FixedUpdate()
    {
        if(right)
        {
            
            card.transform.position = Vector3.Lerp(card.position, nextPosition, cardMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(card.position, nextPosition) < 0.05f)
            {
                card.transform.position = nextPosition;
                right = false;
                
            }
                
        }
        else if(left)
        {
           
            card.transform.position = Vector3.Lerp(card.position, nextPosition, cardMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(card.position, nextPosition) < 0.05f)
            {
                card.position = nextPosition;
                left = false;
               
            }
        }
        else if(up)
        {
            card.transform.position = Vector3.Lerp(card.position, nextPosition, cardMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(card.position, nextPosition) < 0.05f)
            {
                card.position = nextPosition;
                up = false;
                
            }
        }
        else if(down)
        {
            card.transform.position = Vector3.Lerp(card.position, nextPosition, cardMoveSpeed * Time.deltaTime);
            if (Vector3.Distance(card.position, nextPosition) < 0.05f)
            {
                card.position = nextPosition;
                down = false;
                
            }
        }
    }
}*/
