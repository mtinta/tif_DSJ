using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public static float ElapsedTime { get; private set; } = 0f;

    void Update()
    {
        ElapsedTime += Time.deltaTime;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(ElapsedTime / 60F);
        int seconds = Mathf.FloorToInt(ElapsedTime % 60F);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public static string FormattedTime()
    {
        int minutes = Mathf.FloorToInt(ElapsedTime / 60F);
        int seconds = Mathf.FloorToInt(ElapsedTime % 60F);
        return $"{minutes:00}:{seconds:00}";
    }

    public static void ResetTimer()
    {
        ElapsedTime = 0f;
    }
}
