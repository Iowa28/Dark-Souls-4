using UnityEngine;

namespace DS
{
    public class CharacterManager : MonoBehaviour
    {
        [SerializeField]
        private Transform lockOnTransform;

        #region Getters

        public Transform GetLockOnTransform()
        {
            return lockOnTransform;
        }

        #endregion
    }
}