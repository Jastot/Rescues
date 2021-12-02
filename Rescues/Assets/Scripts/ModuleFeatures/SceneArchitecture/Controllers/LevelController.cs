using System;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Rescues
{
    public class LevelController : IInitializeController
    {

        #region Fileds

        private LocationController _locationController;
        private CurveWayController _curveWayController;
        private CurveWay _activeCurveWay;
        private LevelsData _levelsData;
        private BootScreen _defaultBootScreen;
        private BootScreen _customBootScreen;
        private GameContext _context;
        private Services _services;
        private GameObject _levelParent;
        private GateController _gateController;
        private bool restartingFlag = false;
        private AudioController _audioController;


        #endregion


        #region Private

        public LevelController(GameContext context, Services services)
        {
            _context = context;
            _services = services;
            _gateController = new GateController(context);
            _audioController = new AudioController();
        }

        public void Initialize()
        {
            _levelParent = new GameObject("Locations");
            var path = AssetsPathGameObject.Object[GameObjectType.Levels];
            var levelsData = Resources.LoadAll<LevelsData>(path);
            _levelsData = levelsData[0];
            _defaultBootScreen = Object.Instantiate((BootScreen)_levelsData.BootScreen, _levelParent.transform);
            _defaultBootScreen.name = "DefaultBootScreen";
            _defaultBootScreen.gameObject.SetActive(false);
            _context.WorldGameData.RestartLevel += RestartLevel;
            _audioController.LoadMainMusicTheme(_levelParent.transform);

            LoadLevel(_levelsData.GetGate);
        }

        #endregion

        #region Methods

        public void LoadLevel(IGate gate)
        {
            if (_locationController == null || _locationController.LevelName != gate.GoToLevelName)
                LoadAndUnloadPrefabs(gate.GoToLevelName);

            var bootLocation = _locationController.Locations.Find(l => l.LocationName == gate.GoToLocationName);
            if (!bootLocation)
                throw new Exception(_locationController.LevelName + " не содержит локации с именем " + gate.GoToLocationName);

            _customBootScreen = bootLocation.CustomBootScreenInstance;

            if (gate.ThisLevelName != gate.GoToLevelName || gate.ThisLocationName != gate.GoToLocationName)
            {
                var bootScreen = _customBootScreen == null ? _defaultBootScreen : _customBootScreen;
                bootScreen.ShowBootScreen(_services, LoadLevelPart);
            }
            else
            {
                gate.LoadWithTransferTime(LoadLevelPart);
            }

            void LoadLevelPart()
            {
                var activeLocation = _locationController.Locations.Find(l => l.LocationActiveSelf);
                if (activeLocation)
                    activeLocation.DisableOnScene();
                var enterGate = bootLocation.Gates.Find(g => g.ThisGateId == gate.GoToGateId);
                if (!enterGate)
                    throw new Exception("В " + gate.GoToLevelName + " - " + gate.GoToLocationName +
                                   " нет Gate c ID = " + gate.GoToGateId);

                bootLocation.LoadLocation();
                _levelsData.SetLastLevelGate = gate;
                _context.activeLocation = bootLocation;
                _curveWayController = new CurveWayController(bootLocation.LocationInstance.СurveWays);
                _activeCurveWay = _curveWayController.GetCurve(enterGate, WhoCanUseCurve.Character);
                _context.character.LocateCharacter(_activeCurveWay);
                _context.WorldGameData.SetLastGate(gate);
                if (!_context.WorldGameData.LookForLevelByNameBool(bootLocation.LocationName))
                    _context.WorldGameData.AddNewLocation(bootLocation.LocationInstance);

                else
                    _context.WorldGameData.OpenCurrentLocation(
                        bootLocation.LocationInstance,
                        _context.GetTriggers(InteractableObjectType.EventSystem),
                        _context.inventory);
                _context.WorldGameData.SavePlayersProgress(
                    _context.WorldGameData.LookForLevelByNameInt(bootLocation.LocationName));
                Debug.Log(restartingFlag);
                if (restartingFlag)
                {
                    Debug.Log("Poss");
                    _context.character.LoadCharacterPosition(_context.WorldGameData.LoadPlayerPosition());
                    restartingFlag = false;
                }
            }
        }

        private void RestartLevel()
        {
            restartingFlag = true;
            LoadLevel(_context.WorldGameData.GetLastGate());
        }

        private void LoadAndUnloadPrefabs(string loadLevelName)
        {
            _locationController?.UnloadData();
            _curveWayController?.UnloadData();
            _gateController?.TearDown();
            _locationController = new LocationController(this, _context, loadLevelName, _levelParent.transform);
            _gateController?.Initialize();
        }
        #endregion
    }
}