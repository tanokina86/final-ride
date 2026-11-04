using UnityEngine;
using System.Collections;
using FinalRide.Gameplay.Interfaces;

namespace FinalRide.Gameplay.Level
{
    [RequireComponent(typeof(Collider2D))]
    public class AbyssTrigger : MonoBehaviour
    {
        [Header("Fall Animation Settings")]
        [SerializeField] private float fallDuration = 1f;
        [SerializeField] private float initialFallDelay = 0.15f;
        [SerializeField] private float rotationSpeedMultiplier = 30f;

        private void Awake()
        {
            Collider2D triggerCollider = GetComponent<Collider2D>();
            if (triggerCollider != null)
            {
                triggerCollider.isTrigger = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            IDamageable damageableEntity = collision.gameObject.GetComponent<IDamageable>();

            if (damageableEntity != null && !damageableEntity.IsInvincible)
            {
                if (IsVehicleTouchingSafeRoad(collision))
                {
                    return;
                }

                StartCoroutine(AnimateFallRoutine(collision.gameObject, damageableEntity));
            }
        }

        /// <summary>
        /// Evaluates if the intersecting vehicle collider is currently overlapping the specific road surface of this tile.
        /// </summary>
        /// <param name="vehicleCollider">The collider component of the entering vehicle.</param>
        /// <returns>True if the vehicle maintains physical contact with its own tile's safe road layout.</returns>
        private bool IsVehicleTouchingSafeRoad(Collider2D vehicleCollider)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.useTriggers = true;

            Collider2D[] overlappedColliders = new Collider2D[10];
            int currentOverlapCount = Physics2D.OverlapCollider(vehicleCollider, filter, overlappedColliders);

            // Directly find the top-level RoadGenerator tile script on our own root container
            RoadGenerator ourGenerator = GetComponentInParent<RoadGenerator>();

            for (int i = 0; i < currentOverlapCount; i++)
            {
                if (overlappedColliders[i] != null && overlappedColliders[i].gameObject.CompareTag("Road"))
                {
                    // Check horizontal boundaries of the detected road collider first
                    Bounds roadBounds = overlappedColliders[i].bounds;
                    float vehicleX = vehicleCollider.transform.position.x;

                    if (vehicleX >= roadBounds.min.x && vehicleX <= roadBounds.max.x)
                    {
                        // If the road segment is part of our active moving world layout, it is safe
                        if (overlappedColliders[i].transform.GetComponentInParent<RoadGenerator>() == ourGenerator)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Animates the vehicle shrinking and spinning as it falls into the abyss, then destroys it.
        /// </summary>
        /// <param name="victim">The vehicle GameObject to animate.</param>
        /// <param name="damageable">The target interface to apply final damage.</param>
        private IEnumerator AnimateFallRoutine(GameObject victim, IDamageable damageable)
        {
            Rigidbody2D rb = victim.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            // Small cinematic delay before the intense shrinking and spinning begins
            yield return new WaitForSeconds(initialFallDelay);

            if (victim == null) yield break;

            Vector3 originalScale = victim.transform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < fallDuration)
            {
                if (victim == null) yield break;

                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / fallDuration;

                victim.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);

                // Controlled, smoother rotation speed over time
                victim.transform.Rotate(0f, 0f, rotationSpeedMultiplier * Time.deltaTime);

                yield return null;
            }

            if (victim != null)
            {
                damageable.TakeDamage(9999f);
            }
        }
    }
}