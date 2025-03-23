using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    public Camera mainCamera;            // The main camera
    public GameObject[] targets;         // The GameObjects we want to point towards
    public GameObject indicatorPrefab;   // The dot/arrow prefab that appears on the edge of the screen
    public GameObject canvas;
    public float indicatorDistance = 50f;  // Distance from the screen edge

    private GameObject[] indicators;     // Store instances of the indicators

    void Start()
    {
        // Create indicators for each target
        indicators = new GameObject[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            indicators[i] = Instantiate(indicatorPrefab);
            indicators[i].transform.parent = canvas.transform;
        }
    }

    void Update()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(targets[i].transform.position);

            if (screenPos.z > 0 && IsOffScreen(screenPos))
            {
                // If target is off screen, show indicator on the edge of the screen
                Vector3 clampedPosition = ClampToScreenEdge(screenPos);
                indicators[i].transform.position = clampedPosition;

                // Rotate the indicator to point towards the target
                Vector3 direction = targets[i].transform.position - mainCamera.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                indicators[i].transform.rotation = Quaternion.Euler(0, 0, angle - 90); // Adjust the rotation to point correctly
                indicators[i].SetActive(true);

                if (targets[i].name.Equals("MBot"))
                {
                    if (!targets[i].GetComponent<MBotController>().enabled)
                    {
                        indicators[i].SetActive(false);
                    }
                    else
                    {
                        indicators[i].SetActive(true);
                    }
                    indicators[i].gameObject.GetComponent<Image>().color = new Color(0.9f, 0.3f, 0.3f, 1);
                }
            }
            else
            {
                // Hide the indicator if the target is on screen
                indicators[i].SetActive(false);
            }
        }
    }

    bool IsOffScreen(Vector3 screenPos)
    {
        return screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;
    }

    Vector3 ClampToScreenEdge(Vector3 screenPos)
    {
        Vector3 clampedPos = screenPos;

        if (screenPos.x < indicatorDistance)
            clampedPos.x = indicatorDistance;
        else if (screenPos.x > Screen.width - indicatorDistance)
            clampedPos.x = Screen.width - indicatorDistance;

        if (screenPos.y < indicatorDistance)
            clampedPos.y = indicatorDistance;
        else if (screenPos.y > Screen.height - indicatorDistance)
            clampedPos.y = Screen.height - indicatorDistance;

        return clampedPos;
    }
}
