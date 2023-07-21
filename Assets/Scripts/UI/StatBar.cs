using UnityEngine;
using UnityEngine.UI;

namespace DS
{
    public class StatBar : MonoBehaviour
    {
        protected Slider slider;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }
    }
}