    using UnityEngine;

    public class PanelToggler : MonoBehaviour
    {
        public GameObject panelToToggle; 
        public KeyCode toggleKey = KeyCode.Q; 

        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                
                panelToToggle.SetActive(!panelToToggle.activeSelf);
            }
        }
    }