using UnityEngine;


namespace Rescues
{
    public sealed class InitializeGameMenuController : IInitializeController
    {
        #region Fields

        private readonly GameContext _context;
        private readonly PhysicalServices _physicServices; 

        #endregion


        #region ClassLifeCycles

        public InitializeGameMenuController(GameContext context, Services services)
        {
            _context = context;
            _physicServices = services.PhysicalServices;
        } 

        #endregion


        #region IInitializeController

        public void Initialize()
        {
            _context.gameMenu = Object.FindObjectOfType<GameMenuBehaviour>();
            _context.gameMenu.ShowUI += _physicServices.Pause;
            _context.gameMenu.HideUI += _physicServices.UnPause;
            _context.gameMenu.gameObject.SetActive(false);
        } 

        #endregion
    }
}
