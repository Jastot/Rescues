using UnityEngine;


namespace Rescues
{ 
    [CreateAssetMenu(fileName = "TrapData", menuName = "Data/Traps/KillingTrapData")]
    public sealed class KillingTrapData : BaseTrapData
    {
        public override void ActivateTrap(EnemyData activatorData)
        {
            if (_isActive)
            {
                activatorData.StateEnemy = StateEnemy.Dead;
            }
        }
    }
}
