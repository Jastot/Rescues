using System;

namespace Rescues
{
    public interface IGate
    {
        string ThisLevelName { get; set; }
        string ThisLocationName { get; set; }
        int ThisGateId { get; }
        bool RestartGate { get; }
        string GoToLevelName { get; }
        string GoToLocationName { get; }
        int GoToGateId { get; }
        void LoadWithTransferTime(Action onLoadComplete);
    }
}