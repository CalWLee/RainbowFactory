using UnityEngine;
using System.Collections;

public enum GameState
{
    INVALID,
    START_GAME,
    IN_GAME,
    END_GAME,
    MAX
}

public class Scoring : MonoBehaviour
{   
    public static GameState State { get; set; }

    //scoring is below. Don't be afraid to change numbers if they don't feel right!
    public const int ONE_PAIR_SCORE = 100; //base score for completing a row without two pairs or all colors the same
    public const int TWO_PAIR_SCORE = 200;
    public const int TRIPLE_SCORE = 300;
    public const int FOUR_SCORE = 400;

    public float GUILeft, GUITop, GUIWidth, GUIHeight;
    
    public GUIStyle style;
    public Texture[] Textures;

    public Sprite[] numbers;
    
    private static int score;
    private static float timer, startTimer;
    public float MaxTime = 60f;

    private const int comboBonus = 50;
    private static int comboCount = 0; //counts the number of combos in a row.
    private static float timeFromLast;

    private TextMesh countdown;

    private GameObject bgm;
    public GameObject Rainbow;

    [Range(0f,1f)]
    public float PanicTime = 0.2f;
    private float panicTime;

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        bgm = GameObject.FindGameObjectWithTag("BGM");

        panicTime = MaxTime * PanicTime;

    }

    void Start()
    {
        score = 0;
        timer = MaxTime;
        startTimer = 3.75f;
        State = GameState.START_GAME;
        countdown = gameObject.GetComponentInChildren<TextMesh>();
        Time.timeScale = 1;
        UpdateScore();
    }

    void Update()
    {
        gameObject.GetComponent<Animator>().SetFloat("timer", timer/MaxTime * 100);

        switch (State)
        {
            case GameState.START_GAME:                
                startTimer -= Time.deltaTime;
                startTimer = Mathf.Max(0, startTimer);

                countdown.text = Mathf.RoundToInt((startTimer/1.25f) + .5f) + "";

                if (startTimer <= 0)
                {
                    countdown.gameObject.SetActive(false);
                    State = GameState.IN_GAME;
                }
                break;
            case GameState.IN_GAME:
                timer -= Time.deltaTime;
                timer = Mathf.Max(0, timer);

                if (timer <= 0)
                {
                    bgm.audio.clip = bgm.GetComponent<BGM>().GameOverMusic;
                    bgm.audio.Play();
                    Rainbow.GetComponent<Animator>().SetBool("panic", false);

                    State = GameState.END_GAME; 
                    DontDestroyOnLoad(this.gameObject);
                    Application.LoadLevel(2);
                    Debug.Log("EndGame");
                }
                else if (timer <= panicTime)
                {
                    if ( bgm.audio.clip != bgm.GetComponent<BGM>().PanicMusic)
                    {
                        bgm.audio.clip = bgm.GetComponent<BGM>().PanicMusic;
                        bgm.audio.Play();
                        Rainbow.GetComponent<Animator>().SetBool("panic", true);

                    }
                }
                else if (bgm.audio.clip != bgm.GetComponent<BGM>().MainMusic)
                {
                    bgm.audio.clip = bgm.GetComponent<BGM>().MainMusic;
                    bgm.audio.Play();
                    Rainbow.GetComponent<Animator>().SetBool("panic", false);
                }

                break;
            case GameState.END_GAME:
                //Time.timeScale = 0;
                break;
            default:
                break;
        }
    }

    public static void AddScore(int newScoreValue, int newTimeValue = 0)
    {
        score += newScoreValue;
        //not even remotely sure this part will work correctly:
        if (Time.time - timeFromLast < 2)
        {
            comboCount++;
            score += (comboBonus * comboCount);
            //print Combo x comboCount!
        }
        timer += (float)newTimeValue;
        timeFromLast = Time.time;

        UpdateScore();
    }

    static void UpdateScore()
    {
        Debug.Log("Score: " + score);
    }

    void OnGUI()
    {
        //int index, denominator = 100000;
        string Score;
        GUIStyle other = new GUIStyle(style);
        other.normal.textColor = Color.black;

        Score = string.Format("{0:000000}", score);

        switch (State)
        {
            case GameState.START_GAME:
                style.fontSize = 120;
                GUI.TextField(new Rect(220, Screen.height / 2 - 40, 300, 300), Mathf.RoundToInt((startTimer / 1.25f) + .5f) + "", style);
                break;
            case GameState.END_GAME:
                GUI.TextField(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 15, 160, 50), Score, style);
                break;
            default:
                style.fontSize = 80;
                    GUI.TextField(new Rect(GUILeft+3, GUITop+3, GUIWidth, GUIHeight), Score, other);
                    GUI.TextField(new Rect(GUILeft, GUITop, GUIWidth, GUIHeight), Score, style);

                //GUI.Box(new Rect(0, GUITop, GUIWidth, GUIHeight), "Score:\n" + score);
                //GUI.Box(new Rect(Screen.width / 2 - 40, 0, 80, 30), "Timer: " + (int)(timer + .5f));
                break;
        }
    }


    public static void resetGame()
    {
        score = 0;
        timer = 99;
        State = GameState.IN_GAME;
    }
}
