using UnityEngine;
using TMPro;  // For UI Text (or TMPro for TextMeshPro)

public class MessagePopup : MonoBehaviour
{
    public Transform player;             // The player or object that approaches
    public GameObject targetObject;      // The target GameObject with the text
    public float fadeDistance = 5f;      // The distance at which the text starts fading in
    public float maxDistance = 1f;       // The minimum distance for full visibility
    public TextMeshProUGUI uiText;                  // The UI Text or TextMeshPro text to fade in

    public bool hasArrived;

    private Color originalColor;

    void Start()
    {
        // Store the original color of the text (with full alpha)
        originalColor = uiText.color;

        // Ensure the text starts as fully transparent
        Color transparentColor = originalColor;
        transparentColor.a = 0;
        uiText.color = transparentColor;
    }

    void Update()
    {
        // Calculate the distance between the player and the target object
        float distance = Vector3.Distance(player.position, targetObject.transform.position);

        // Calculate the fade amount based on distance
        float fadeAmount = Mathf.Clamp01((fadeDistance - distance) / (fadeDistance - maxDistance));

        // Apply the fade amount to the text's alpha
        Color newColor = originalColor;
        newColor.a = fadeAmount;
        uiText.color = newColor;

        if (fadeAmount > 0.1)
        {
            hasArrived = true;
        }
    }
}
