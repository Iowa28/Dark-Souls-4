using UnityEngine;

namespace DS
{
    public class EnemyAction : ScriptableObject
    {
        [SerializeField]
        private string actionAnimation;

        #region Getters

        public string GetActionAnimation() => actionAnimation;

        #endregion
    }
}