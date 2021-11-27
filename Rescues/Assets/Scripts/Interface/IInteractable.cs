namespace Rescues
{
    public interface IInteractable
    {
        string Id { get; set; }
        bool IsInteractable { get; set; }
        /// <summary>
        /// Блокирует взаимодействие игрока с этим триггером
        /// </summary>
        bool IsInteractionLocked { get; set; }
        string Description { get; set; }
    }
}
