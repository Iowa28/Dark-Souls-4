using UnityEngine;

namespace DS
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")] 
        [SerializeField]
        private Sprite itemIcon;
        [SerializeField]
        private string itemName;
    }
}