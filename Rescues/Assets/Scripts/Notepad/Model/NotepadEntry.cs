using System;
using System.Collections.Generic;


namespace Rescues
{
    [Serializable]
    public sealed class NotepadEntry 
    {
        #region Fields

        public string EntryName;
        public bool IsCrossedOut;
        public List<NotepadBulletpoint> BulletPoints;

        #endregion


        #region Methods

        public void AddBulletpoint(int id)
        {
            var index = BulletPoints.FindIndex(x => x.Id.Equals(id));

            if (index < 0)
                BulletPoints.Add(new NotepadBulletpoint { Id = id});
        }

        public void CrossOutBulletpoint(int id)
        {
            var index = BulletPoints.FindIndex(x => x.Id.Equals(id));

            if (index >= 0)
            {
                var bulletPoint = BulletPoints[index];

                bulletPoint.IsCrossedOut = true;

                BulletPoints[index] = bulletPoint;
            }
        }

        public void RemoveBulletpoint(int id)
        {
            var index = BulletPoints.FindIndex(x => x.Id.Equals(id));
            if (index >= 0)
                BulletPoints.RemoveAt(index);
        }

        public void CrossOutEntry()
        {
            IsCrossedOut = true;
            BulletPoints.Clear();
        }

        #endregion
    }
}