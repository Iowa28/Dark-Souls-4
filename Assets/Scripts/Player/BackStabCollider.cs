using UnityEngine;

namespace DS
{
    public class BackStabCollider : MonoBehaviour
    {
        [SerializeField]
        private Transform backStabberStandPoint;

        #region Getters

        public Transform GetBackStabberStandPoint() => backStabberStandPoint;

        #endregion
    }
}

