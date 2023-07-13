using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class InteractableUI : MonoBehaviour
    {
        [SerializeField]
        private Text interactableText;
        
        [SerializeField]
        private Text itemText;
        [SerializeField]
        private RawImage itemImage;

        #region Setters

        public void SetInteractableText(string text) => interactableText.text = text;

        #endregion
    }
}