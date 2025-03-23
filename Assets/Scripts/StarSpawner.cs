using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab;           // Prefab for the star
    public Camera mainCamera;               // The main camera
    public int maxStars = 100;              // Maximum number of stars in the camera's view
    public float starSpawnDistance = 5f;    // Star spawn area buffer from the edge of the camera view
    public float fadeInDuration = 1.0f;     // Duration of the fade-in effect

    private float spawnAreaWidth;
    private float spawnAreaHeight;
    private Transform cameraTransform;
    private GameObject[] stars;             // Pool of stars
    private SpriteRenderer[] starRenderers; // Sprite renderers for each star

    // Color palette for stars (blue, green, purple)
    private Color[] starColors = new Color[]
    {
        new Color(0.6f, 0.8f, 1f),   // Light Blue
        new Color(0.8f, 1f, 0.8f),   // Light Green
        new Color(0.8f, 0.6f, 1f),    // Light Purple
        new Color(0.9f, 0.9f, 1f)    // Light Purple
    };

    void Start()
    {
        cameraTransform = mainCamera.transform;

        // Get camera's bounds (viewport size at distance 0)
        spawnAreaHeight = 2f * mainCamera.orthographicSize + starSpawnDistance;
        spawnAreaWidth = spawnAreaHeight * mainCamera.aspect + starSpawnDistance;

        // Initialize stars pool and sprite renderers
        stars = new GameObject[maxStars];
        starRenderers = new SpriteRenderer[maxStars];

        // Generate initial stars
        for (int i = 0; i < maxStars; i++)
        {
            Vector3 spawnPosition = GetRandomPositionInView();
            stars[i] = Instantiate(starPrefab, spawnPosition, Quaternion.identity);
            stars[i].transform.parent = transform.parent;
            stars[i].transform.rotation = new Quaternion(0, 0, Random.Range(0, 180), 0);
            starRenderers[i] = stars[i].GetComponent<SpriteRenderer>();

            // Assign a random color from the palette to the star
            starRenderers[i].color = GetRandomStarColor();

            // Start the stars with zero alpha (invisible)
            Color starColor = starRenderers[i].color;
            starColor.a = 0;
            starRenderers[i].color = starColor;

            // Start the fade-in effect
            StartCoroutine(FadeInStar(starRenderers[i]));
        }
    }

    void Update()
    {
        for (int i = 0; i < maxStars; i++)
        {
            // If the star goes out of bounds, reposition it to a new random position
            if (!IsInView(stars[i].transform.position))
            {
                // Reset alpha to 0 and assign a new random color
                Color starColor = GetRandomStarColor();
                starColor.a = 0;
                starRenderers[i].color = starColor;

                stars[i].transform.position = GetRandomPositionInView();

                StartCoroutine(FadeInStar(starRenderers[i]));
            }
        }
    }

    Vector3 GetRandomPositionInView()
    {
        // Calculate random position within the camera's view bounds (plus buffer)
        float x = Random.Range(cameraTransform.position.x - spawnAreaWidth / 2, cameraTransform.position.x + spawnAreaWidth / 2);
        float y = Random.Range(cameraTransform.position.y - spawnAreaHeight / 2, cameraTransform.position.y + spawnAreaHeight / 2);

        return new Vector3(x, y, 0); // Stars are in 2D, Z is zero.
    }

    bool IsInView(Vector3 position)
    {
        // Check if the star is within the camera's view bounds
        float camXMin = cameraTransform.position.x - spawnAreaWidth / 2;
        float camXMax = cameraTransform.position.x + spawnAreaWidth / 2;
        float camYMin = cameraTransform.position.y - spawnAreaHeight / 2;
        float camYMax = cameraTransform.position.y + spawnAreaHeight / 2;

        return (position.x > camXMin && position.x < camXMax && position.y > camYMin && position.y < camYMax);
    }

    Color GetRandomStarColor()
    {
        // Pick a random color from the starColors array (blue, green, purple)
        int randomIndex = Random.Range(0, starColors.Length);
        return starColors[randomIndex];
    }

    System.Collections.IEnumerator FadeInStar(SpriteRenderer starRenderer)
    {
        float elapsedTime = 0f;
        Color starColor = starRenderer.color;

        // Gradually increase the alpha value from 0 to 1
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
            starColor.a = alpha;
            starRenderer.color = starColor;

            yield return null;
        }

        // Ensure the alpha is fully set to 1 at the end
        starColor.a = 1;
        starRenderer.color = starColor;
    }
}
