using DG.Tweening;
using UnityEngine;


namespace Rescues
{
    public interface IBootScreen
    {
	    Sequence DOTsequnce { get; set; }
        void ShowBootScreen(Services services, TweenCallback onCompleteFade, TweenCallback onCompleteClear);
        void Destroy();
    }
}