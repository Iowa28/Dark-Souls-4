using UnityEngine;

namespace DS
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject selectWindow;

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }
    }
}