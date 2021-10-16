using PathCreation;
using System;
using UnityEngine;


namespace Rescues
{
    public sealed class CharacterModel
    {
        #region Fields

        public Action<float> OnSceneLoad = (distance) => { };

        private float _distance;
        private float _speed;

        private DragonBones.UnityArmatureComponent _characterArmature;
        private DragonBones.Animation _characterAnimation;

        private CapsuleCollider2D _playerCollider;
        private Rigidbody2D _playerRigidbody2D;

        private CurveWay _curveWay;

        #endregion


        #region Properties

        public Transform Transform { get; }
        public AudioSource PlayerSound { get; }


        #endregion


        #region ClassLifeCycle

        public CharacterModel(Transform transform, PlayerData playerData)
        {
            _speed = playerData.Speed;

            _characterArmature = transform.GetComponentInChildren<DragonBones.UnityArmatureComponent>();
            _characterAnimation = _characterArmature.animation;

            _playerCollider = transform.GetComponentInChildren<CapsuleCollider2D>();
            _playerRigidbody2D = transform.GetComponentInChildren<Rigidbody2D>();
            PlayerSound = transform.GetComponentInChildren<AudioSource>();
            Transform = transform;
        }

        #endregion


        #region StateMachine     

        public void Moving(float direction)
        {
            if (direction != 0 && _characterAnimation.lastAnimationName != "Walking")
            {
                _characterAnimation.FadeIn("Walking");
            }

            if (direction == 0 && _characterAnimation.lastAnimationName != "Idle")
            {
                _characterAnimation.FadeIn("Idle");
            }

            if (direction != 0)
            {
                direction = direction > 0 ? 1 : -1; 
            }

            _distance += direction * _speed * Time.deltaTime;
            Transform.position = _curveWay.PathCreator.path.GetPointAtDistance(_distance, EndOfPathInstruction.Stop);

            if (direction > 0 && _characterArmature._armature.flipX)
            {
                Flip();
            }
            else if (direction < 0 && !_characterArmature._armature.flipX)
            {
                Flip();
            }

            SetScale();
        }

        public void StartHiding(HidingPlaceBehaviour hidingPlaceBehaviour)
        {
            PlayerSound.clip = hidingPlaceBehaviour.HidingPlaceData.HidingSound;
            _playerCollider.enabled = !_playerCollider.enabled;
            if (_playerRigidbody2D.bodyType == RigidbodyType2D.Dynamic)
            {
                _playerRigidbody2D.bodyType = RigidbodyType2D.Static;
                hidingPlaceBehaviour.HidedSprite.enabled = true; //чтобы спрайт хайдинг плейс бехевора включался только тогда, когда персонаж спрятался
                _characterArmature.enabled = false; //чтобы спрайт выключался сразу, когда идет процесс пряток
            }
            else
            {
                _playerRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                hidingPlaceBehaviour.HidedSprite.enabled = false; //чтобы спрайт хайдинг плейс бехевора выключался сразу, когда персонаж начинает вылезать
                _characterArmature.enabled = true; //чтобы спрайт выключался только тогда, когда персонаж уже вылез
            }
        }

        #endregion


        #region Methods

        public void LocateCharacter(CurveWay curveWay)
        {
            _curveWay = curveWay;
            Transform.position = curveWay.GetStartPointPosition;
            CorrectDistance();
            OnSceneLoad = Moving;
        }

        private void SetScale()
        {
            Transform.localScale = _curveWay.GetScale(Transform.position);
        }

        private void CorrectDistance()
        {
            _distance = 0;
            //расчет сколько нам понадобится чтобы дойти до точки от 0(начало кривой).
            _distance = _curveWay.LeftmostPoint.x < 0 ?
                _curveWay.StartCharacterPosition.x - _curveWay.LeftmostPoint.x :
                _curveWay.StartCharacterPosition.x + _curveWay.LeftmostPoint.x;
        }

        private void Flip()
        {
            _characterArmature._armature.flipX = !_characterArmature._armature.flipX;
        }

        #endregion


    }
}
