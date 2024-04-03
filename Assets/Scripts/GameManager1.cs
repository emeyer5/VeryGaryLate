using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 Instance;
    public int score = 0;
    public float timeLeft = 60.0f;
    public Text scoreText;
    public Text timerText;

    // Track the current level
    public static int currentLevelIndex; // Making it static to access from other scripts

    // Audio components
    private AudioSource audioSource;
    public AudioClip music; // Now a public variable to be set in the Inspector

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // Exit if another GameManager1 instance already exists.
        }

        audioSource = gameObject.AddComponent<AudioSource>();

        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
    }

    void Start()
    {
        // Play the music when the game starts
        audioSource.clip = music;
        audioSource.loop = true; // Set to true if you want the music to loop
        audioSource.Play();

        UpdateScore();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentLevelIndex = scene.buildIndex;

        // Stop the music when the scene changes
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        timerText.text = "Time Left: " + Mathf.Max(0, Mathf.RoundToInt(timeLeft));

        if (timeLeft <= 0)
        {
            AddScore(Mathf.RoundToInt(timeLeft));
            timeLeft = 0;
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
    }
}
