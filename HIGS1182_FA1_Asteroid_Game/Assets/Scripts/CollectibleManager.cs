using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    [SerializeField] private GameObject scrapPrefab;
    [SerializeField] private int scrapCount = 10;

    [Header("Placement")]
    [Tooltip("Who to scatter the scrap around. Leave empty to use this object's position.")]
    [SerializeField] private Transform player;
    [Tooltip("Scrap is placed between these distances from the player, so none of it starts on top of them.")]
    [SerializeField] private float minDistance = 40f;
    [SerializeField] private float maxDistance = 150f;

    private int remaining;

    // Scrap is spawned once here in Start, unlike the asteroids which keep respawning.
    private void Start()
    {
        if (scrapPrefab == null)
        {
            Debug.LogWarning("CollectibleManager: assign a scrap prefab, otherwise nothing will spawn.", this);
            return;
        }

        Vector3 centre = player != null ? player.position : transform.position;

        for (int i = 0; i < scrapCount; i++)
        {
            Vector3 position = centre + Random.onUnitSphere * Random.Range(minDistance, maxDistance);

            Instantiate(scrapPrefab, position, Random.rotation)
                .AddComponent<ScrapPickup>()
                .Collected += OnScrapCollected;
        }

        remaining = scrapCount;
        Debug.Log($"Generated all {scrapCount} pieces of scrap.");
    }

    private void OnScrapCollected()
    {
        remaining--;
        Debug.Log($"Scrap collected. {remaining} piece(s) left.");
    }
}

// Added to each piece at runtime, so the prefab itself needs no extra setup.
public class ScrapPickup : MonoBehaviour
{
    public System.Action Collected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() == null)
        {
            return;
        }

        Collected?.Invoke();
        Destroy(gameObject);
    }
}
