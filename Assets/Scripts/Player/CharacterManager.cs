using UnityEngine;

namespace DS
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock On Transform")]
        [SerializeField]
        private Transform lockOnTransform;

        [Header("Combat Colliders")]
        [SerializeField]
        private BoxCollider backStabBoxCollider;

        protected BackStabCollider backStabCollider;

        public int pendingCriticalDamage { get; set; }

        #region Getters

        public Transform GetLockOnTransform() => lockOnTransform;
        
        public BackStabCollider GetBackStabCollider() => backStabCollider;

        #endregion
    }
}