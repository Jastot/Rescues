using System;
using System.Collections;
using UnityEngine;


namespace Rescues
{
    /// <summary>
    /// От этого класса наследуются модели пазлов
    /// </summary>
    public abstract class Puzzle : MonoBehaviour
    {
        #region Fileds

        [SerializeField] private float _delayAfterFinish = 3f;
        public event Action<Puzzle> Activated = puzzle => { };
        public event Action<Puzzle> Closed = puzzle => { };
        public event Action<Puzzle> Finished = puzzle => { };
        public event Action<Puzzle> ResetValuesToDefault = puzzle => { };
        public event Action<Puzzle> CheckCompleted = puzzle => { };

        public bool IsFinished = false;
        public bool IsActive = false;

        #endregion


        #region Methods

        [ContextMenu("Activate puzzle")]
        public void Activate()
        {
            if (IsFinished || IsActive) return;
            Activated.Invoke(this);
            IsActive = true;
        }

        public void Close()
        {
            if (_delayAfterFinish > 0)
            {
                StartCoroutine(CloseWithDelay());
            }
            else
            {
                ForceClose();
            }

            IsActive = false;
        }

        public void Finish()
        {
            Finished.Invoke(this);
        }

        public void ResetValues()
        {
            ResetValuesToDefault.Invoke(this);
        }

        public void CheckComplete()
        {
            CheckCompleted.Invoke(this);
        }

        private IEnumerator CloseWithDelay()
        {
            yield return new WaitForSeconds(_delayAfterFinish);
            ForceClose();
        }

        public void ForceClose()
        {
            gameObject.SetActive(false);
            IsActive = false;
        }
        
        #endregion
    }
}