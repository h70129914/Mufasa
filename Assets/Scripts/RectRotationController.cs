using UnityEngine;

public class RectRotationController : MonoBehaviour
{


    public RectTransform targetRect;

    private float maxAngle = 70f;

    public void UpdateValue(float normalizedValue)
    {
        if (targetRect != null)
        {
            // Map normalized value (0 to 1) to angle (70 to -70)
            float angle = Mathf.Lerp(maxAngle, -maxAngle, normalizedValue);
            targetRect.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
