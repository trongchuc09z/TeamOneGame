using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip loseSound;

    public int targetScore = 8;
    private int currentScore = 0;
    private bool gameEnded = false;
    public GameObject gameOverPanel;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore()
    {
        if (gameEnded) return;
        currentScore += 1;
        Debug.Log("Score: " + currentScore + " / " + targetScore);
        if (currentScore >= targetScore)
        {
            WinGame();
        }
    }

    public void WinGame()
    {
        if (gameEnded) return;
        gameEnded = true;
        Debug.Log("You Win!");
        if (winSound != null) PlaySound(winSound);
        EnemyTungTungController.Instance.PlayRunSequence();
    }

    public void LoseGame()
    {
        if (gameEnded) return;
        gameEnded = true;
        Debug.Log("You Lose!");
        if (loseSound != null) PlaySound(loseSound);
        EnemyTungTungController.Instance.PlayDetectSequence();

    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
