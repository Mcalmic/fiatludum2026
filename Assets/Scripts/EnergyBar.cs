using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [Header("Bar Images")]
    [SerializeField] private Image currentBar;
    [SerializeField] private Image ghostBar;

    [Header("Ghost Settings")]
    [SerializeField] private float ghostDelay = 0.6f;
    [SerializeField] private float ghostDrainSpeed = 0.4f;

    private float currentFill = 1f;
    private float ghostFill = 1f;
    private float delayTimer = 0f;

    private void Start()
    {
        UpdateBars();
    }

    public void SetValue(float normalizedValue)
    {
        normalizedValue = Mathf.Clamp01(normalizedValue);

        if (normalizedValue < currentFill)
        {
            bool ghostWasSettled = ghostFill <= currentFill;
            ghostFill = Mathf.Max(ghostFill, currentFill);
            if (ghostWasSettled)
                delayTimer = ghostDelay;
        }
        else
        {
            ghostFill = normalizedValue;
        }

        currentFill = normalizedValue;
        UpdateBars();
    }

    private void Update()
    {
        if (ghostFill <= currentFill) return;

        if (delayTimer > 0f)
        {
            delayTimer -= Time.deltaTime;
            return;
        }

        ghostFill = Mathf.Max(currentFill, ghostFill - ghostDrainSpeed * Time.deltaTime);
        if (ghostBar != null) ghostBar.fillAmount = ghostFill;
    }

    private void UpdateBars()
    {
        if (currentBar != null) currentBar.fillAmount = currentFill;
        if (ghostBar != null) ghostBar.fillAmount = ghostFill;
    }
}
