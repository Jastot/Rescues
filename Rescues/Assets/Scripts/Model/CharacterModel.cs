using PathCreation;
using UnityEngine;


namespace Rescues
{
    public sealed class CharacterModel
    {
        #region Fields

        private float _distance;
        private readonly float _speed;
        private bool _canMove;
        private bool _isMoving;

        private readonly DragonBones.UnityArmatureComponent _characterArmature;
        private readonly DragonBones.Animation _characterAnimation;

        private readonly CapsuleCollider2D _playerCollider;
        private readonly Rigidbody2D _playerRigidbody2D;

        private CurveWay _curveWay;

        #endregion


        #region Properties

        public string Name { get; private set; }
        public bool IsMoving => _isMoving;
        public Transform Transform { get; }
        public AudioSource PlayerSound { get; }


        #endregion


        #region ClassLifeCycle

        public CharacterModel(Transform transform, PlayerData playerData)
        {
            _speed = playerData.Speed;
            Name = playerData.Name;
            _characterArmature = transform.GetComponentInChildren<DragonBones.UnityArmatureComponent>();
            _characterAnimation = _characterArmature.animation;
            _playerCollider = transform.GetComponentInChildren<CapsuleCollider2D>();
            _playerRigidbody2D = transform.GetComponentInChildren<Rigidbody2D>();
            PlayerSound = transform.GetComponentInChildren<AudioSource>();
            transform.gameObject.SetActive(false);
            Transform = transform;
        }

        #endregion


        #region StateMachine     

        public void SetWalking()
        {
            if (_characterAnimation.lastAnimationName != "Walking")
            {
                _characterAnimation.FadeIn("Walking");
            }
        }

        public void SetIdle()
        {
            if (_characterAnimation.lastAnimationName != "Idle")
            {
                _characterAnimation.FadeIn("Idle");
            }

            _isMoving = false;
        }

        public void SetPickUp(float time)
        {
            ScaleAndExecuteAimation("PickUp", time);
        }

        public void ForceSetIdle()
        {
            if (_characterAnimation.lastAnimationName != "Idle")
            {
                _characterAnimation.Stop();
                _characterAnimation.Play("Idle");
            }

            _isMoving = false;
        }

        public void StartHiding(HidingPlaceBehaviour hidingPlaceBehaviour)
        {
            PlayerSound.clip = hidingPlaceBehaviour.HidingPlaceData.HidingSound;
            _playerCollider.enabled = !_playerCollider.enabled;
            if (_playerRigidbody2D.bodyType == RigidbodyType2D.Dynamic)
            {
                _playerRigidbody2D.bodyType = RigidbodyType2D.Static;
                hidingPlaceBehaviour.HidedSprite.enabled = true; //чтобы спрайт хайдинг плейс бехевора включался только тогда, когда персонаж спрятался
            }
            else
            {
                _playerRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                hidingPlaceBehaviour.HidedSprite.enabled = false; //чтобы спрайт хайдинг плейс бехевора выключался сразу, когда персонаж начинает вылезать
            }
        }

        private void ScaleAndExecuteAimation(string animation, float referenceTime)
        {
            if (_characterAnimation.lastAnimationName != animation)
            {
                DragonBones.AnimationState anim = _characterAnimation.FadeIn(animation, -1, 1);
                anim.timeScale = anim.totalTime / referenceTime;
            }
        }

        #endregion


        #region Methods

        public void Move(float direction)
        {
            if (_canMove)
            {
                SetWalking();
                _isMoving = true;
                RotateCharacter(direction);
                direction = direction > 0 ? 1 : -1;
                _distance = Mathf.Clamp(_distance + direction * _speed * Time.deltaTime, 0, _curveWay.PathCreator.path.length);
                Transform.position = _curveWay.PathCreator.path.GetPointAtDistance(_distance, EndOfPathInstruction.Stop);
                ScaleSize();
            }
        }

        public void LocateCharacter(CurveWay curveWay)
        {
            _curveWay = curveWay;
            Transform.position = curveWay.GetStartPointPosition;
            CorrectDistance();
            ScaleSize();
            Transform.gameObject.SetActive(true);
            _canMove = true;
        }

        private void RotateCharacter(float direction)
        {
            if (direction > 0 && _characterArmature._armature.flipX)
            {
                FlipCharacter();
            }
            else if (direction < 0 && !_characterArmature._armature.flipX)
            {
                FlipCharacter();
            }
        }

        private void ScaleSize()
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

        private void FlipCharacter()
        {
            _characterArmature._armature.flipX = !_characterArmature._armature.flipX;
        }

        #endregion


    }
}
