using System;
using System.Collections.Generic;
using UnityEngine;


namespace Rescues
{
    [CreateAssetMenu(fileName = "LocationData", menuName = "Data/LocationData")]
    public class LocationData : ScriptableObject
    {
        #region Fileds

        [NonSerialized] public List<Gate> Gates;
        [NonSerialized] public string LevelName;
        [SerializeField] private string _locationName;
        [SerializeField] private Location _locationPrefab;
        [SerializeField] private Color _backgroundColor;
        [SerializeField] private CameraData _cameraData;
        [SerializeField, Tooltip("Будет использоватсья при загрузке этой локации. Необязательное поле")] 
        private BootScreen _customBootScreen;

        private BootScreen _customBootScreenInstance;
        private Location _locationInstance;
        
        #endregion
        
        
        #region Properties
        
        public string LocationName => _locationName;

        public BootScreen CustomBootScreenInstance
        {
           get => _customBootScreenInstance;
           set => _customBootScreenInstance = value;
        }

        public BootScreen CustomBootScreenPrefab => _customBootScreen;

        public bool LocationActiveSelf => _locationInstance.gameObject.activeSelf;
        
        public Location LocationInstance
        {
            get => _locationInstance;
            set
            {
                if (_locationInstance == null)
                    _locationInstance = value;
            }
        }
        public Location LocationPrefab => _locationPrefab;
        public Color BackgroundColor => _backgroundColor;
        public CameraData CameraData => _cameraData;
        
        #endregion


        #region Methods
        
        public void LoadLocation()
        {
            LocationInstance.gameObject.SetActive(true);
        }
        
        public void  DisableOnScene()
        {
            LocationInstance.gameObject.SetActive(false);
        }
        
        public void Destroy()
        {
            if (CustomBootScreenInstance)
                CustomBootScreenInstance.Destroy();
            
            LocationInstance.Destroy();
        }

        private void OnDisable()
        {
            _locationInstance = null;
            _customBootScreenInstance = null;
            Gates = null;
        }

        #endregion
        
    }
}