using System.Collections;
using UnityEngine;

public class AsteroidsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;

    [Header("Timing")]
    [SerializeField] private float minInterval = 0.5f;
    [SerializeField] private float maxInterval = 1.5f;

    [Header("Placement")]
    [Tooltip("Who to scatter asteroids around. Leave empty to use this object's position.")]
    [SerializeField] private Transform player;
    [Tooltip("Asteroids appear between these distances from the player, so none pop in on top of them.")]
    [SerializeField] private float minDistance = 105f;
    [SerializeField] private float maxDistance = 305f;
    [Tooltip("How far an asteroid's heading can stray from the player, in degrees. " +
             "0 aims them straight at the player, 90 is effectively a random direction.")]
    [SerializeField] private float spreadAngle = 30f;

    private IEnumerator Start()
    {
        if (asteroidPrefab == null || asteroidPrefab.GetComponent<Asteroid>() == null)
        {
            Debug.LogWarning("AsteroidsSpawner: assign an asteroid prefab that has an Asteroid " + "component, otherwise nothing will spawn.", this);
            yield break;
        }

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            Spawn(player != null ? player.position : transform.position);
        }
    }

    private void Spawn(Vector3 centre)
    {
        Vector3 offset = Random.onUnitSphere * Random.Range(minDistance, maxDistance);
        Vector3 heading = Quaternion.AngleAxis(Random.Range(-spreadAngle, spreadAngle), Random.onUnitSphere) * -offset.normalized;

        Instantiate(asteroidPrefab, centre + offset, Random.rotation)
            .GetComponent<Asteroid>()
            .Launch(heading);
    }
}
