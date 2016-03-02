using UnityEngine;
using System.Collections;

public class BGM : MonoBehaviour
{
    public AudioClip MainMusic;
    public AudioClip PanicMusic;
    public AudioClip GameOverMusic;


	void Awake ()
    {
        DontDestroyOnLoad(gameObject);
	
	}

    void Start ()
    {
        gameObject.audio.clip = MainMusic;
        gameObject.audio.Play();
    }
	
}
