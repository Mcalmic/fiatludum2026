using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI timeElapsedText;

    [Header("Progress Bar")]
    [SerializeField] private RectTransform barTrack;
    [SerializeField] private Image progressFill;
    [SerializeField] private RectTransform shipIcon;

    private float timeElapsed = 0f;
    private float startDistance = -1f;

    void Start()
    {
        if (GameProgressManager.instance != null)
            startDistance = GameProgressManager.instance.GetDistanceToDestination();
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);
        timeElapsedText.text = "Time Elapsed: " + minutes.ToString("00") + ":" + seconds.ToString("00");

        float dist = GameProgressManager.instance != null ? GameProgressManager.instance.GetDistanceToDestination() : 0f;
        distanceText.text = "Distance to Destination: " + Mathf.CeilToInt(dist) + " km";

        UpdateProgressBar(dist);
    }

    private void UpdateProgressBar(float currentDist)
    {
        if (startDistance <= 0f) return;

        float progress = Mathf.Clamp01(1f - (currentDist / startDistance));

        if (progressFill != null)
            progressFill.fillAmount = progress;

        if (shipIcon != null && barTrack != null)
        {
            float minX = barTrack.rect.xMin;
            float maxX = barTrack.rect.xMax;
            shipIcon.anchoredPosition = new Vector2(Mathf.Lerp(minX, maxX, progress), shipIcon.anchoredPosition.y);
        }
    }
}
