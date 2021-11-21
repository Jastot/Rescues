namespace Rescues
{
    public sealed class MainLateControllers : Controllers
    {
        #region ClassLifeCycles

        public MainLateControllers(GameContext context, Services services)
        {
            Add(new CameraController(context, services));
            Add(new InitializeGameMenuController(context, services));
            Add(new InitializeSaveLoadController(context, services));
        }

        #endregion
    }
}
