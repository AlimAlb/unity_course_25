using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    public static GameManager I { get; private set; }

    [Header("Difficulty")]
    [SerializeField] private float startSpeedMultiplier = 1f;
    [SerializeField] private float maxSpeedMultiplier = 2.2f;
    [SerializeField] private float secondsToMaxSpeed = 120f;

    [Header("Score")]
    [SerializeField] private int scorePerSecond = 1;

    private float t0;
    private bool running = true;

    public int Score { get; private set; }
    public float SpeedMultiplier { get; private set; } = 1f;

    private void Awake() {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        t0 = Time.time;
        SpeedMultiplier = startSpeedMultiplier;
        Score = 0;
        running = true;
    }

    private void Update() {
        if (!running) return;

        float elapsed = Time.time - t0;
        float k = (secondsToMaxSpeed <= 0f) ? 1f : Mathf.Clamp01(elapsed / secondsToMaxSpeed);
        SpeedMultiplier = Mathf.Lerp(startSpeedMultiplier, maxSpeedMultiplier, k);

        Score += Mathf.RoundToInt(scorePerSecond * Time.deltaTime);
    }

    public void AddScore(int amount) {
        Score += Mathf.Max(0, amount);
    }

    public void StopAndRestart(float delaySeconds = 0.5f){
        if (!running) return;
        running = false;
        Invoke(nameof(Reload), delaySeconds);
    }

    private void Reload() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        t0 = Time.time;
        SpeedMultiplier = startSpeedMultiplier;
        Score = 0;
        running = true;
    }
}