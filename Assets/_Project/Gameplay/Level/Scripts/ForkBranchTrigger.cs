using UnityEngine;

namespace FinalRide.Gameplay.Level
{
    [RequireComponent(typeof(Collider2D))]
    public class ForkBranchTrigger : MonoBehaviour
    {
        [Header("Branch Properties")]
        [SerializeField] private bool isLeftBranch;
        [SerializeField] private GameObject oppositeBranchRoot;

        private void Awake()
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.isTrigger = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Inform the generator that this specific branch path has been locked in by the player
                RoadGenerator generator = FindObjectOfType<RoadGenerator>();
                if (generator != null)
                {
                    generator.RegisterBranchSelection(isLeftBranch, oppositeBranchRoot);
                }

                // Destroy this trigger object since the choice is already finalized
                Destroy(this);
            }
        }
    }
}
