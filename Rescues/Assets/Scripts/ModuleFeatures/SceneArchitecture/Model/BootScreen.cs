using DG.Tweening;
using UnityEngine;


namespace Rescues
{
    public class BootScreen : MonoBehaviour, IBootScreen
    {
        #region Fileds
        
        [SerializeField] private float _alphaTweenTime;
        [SerializeField] private float _screenDelay;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        #endregion


        #region Properties

        public Sequence DOTsequnce { get; set; }
        
        #endregion
        
        
        #region Methods
        
        public void ShowBootScreen(Services services, TweenCallback onCompleteFade, TweenCallback onCompleteClear)
        {
            var cameraPos = services.CameraServices.CameraMain.transform.position;
            transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z);
            
            gameObject.SetActive(true);
            SetSpriteAlpha(0);

            DOTsequnce?.Kill();
            DOTsequnce = DOTween.Sequence();
            DOTsequnce.Append(_spriteRenderer.DOFade(1, _alphaTweenTime).OnComplete(onCompleteFade));
            DOTsequnce.AppendInterval(_screenDelay);
            DOTsequnce.Append(_spriteRenderer.DOFade(0, _alphaTweenTime).OnComplete(onCompleteClear));
            DOTsequnce.AppendCallback(() => gameObject.SetActive(false));
            DOTsequnce.Play();
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void SetSpriteAlpha(float value)
        {
            var color = _spriteRenderer.color;
            color.a = value;
            _spriteRenderer.color = color;
        }

        #endregion       
    }
}