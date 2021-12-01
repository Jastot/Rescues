using PathCreation;
using UnityEngine;


namespace Rescues
{
    public sealed class CharacterModel
    {
        #region Fields

        private float _distance;
        private float _speed;
        private bool _canMove;
        private bool _isMoving;

        private DragonBones.UnityArmatureComponent _characterArmature;
        private DragonBones.Animation _characterAnimation;

        private CapsuleCollider2D _playerCollider;
        private Rigidbody2D _playerRigidbody2D;
        private MeshRenderer _playerMesh;

        private CurveWay _curveWay;

        #endregion


        #region Properties

        public string Name { get; private set; }
        public bool IsMoving { get => _isMoving; }
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
            _playerMesh = transform.GetComponentInChildren<MeshRenderer>();
            _playerMesh.enabled = false;

            _playerCollider = transform.GetComponentInChildren<CapsuleCollider2D>();
            _playerRigidbody2D = transform.GetComponentInChildren<Rigidbody2D>();
            PlayerSound = transform.GetComponentInChildren<AudioSource>();
            Transform = transform;
        }

        #endregion


        #region StateMachine     

        public void Move(float direction)
        {
            if (_canMove)
            {
                if (_characterAnimation.lastAnimationName != "Walking")
                {
                    _characterAnimation.FadeIn("Walking");
                }

                if (direction > 0 && _characterArmature._armature.flipX)
                {
                    FlipCharacter();
                }
                else if (direction < 0 && !_characterArmature._armature.flipX)
                {
                    FlipCharacter();
                }

                _isMoving = true;
                ChangePosition(direction);
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
                _playerMesh.enabled = false; //чтобы спрайт выключался сразу, когда идет процесс пряток
            }
            else
            {
                _playerRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                hidingPlaceBehaviour.HidedSprite.enabled = false; //чтобы спрайт хайдинг плейс бехевора выключался сразу, когда персонаж начинает вылезать
                _playerMesh.enabled = true; //чтобы спрайт выключался только тогда, когда персонаж уже вылез
            }
        }

        #endregion


        #region Methods

        public void LocateCharacter(CurveWay curveWay)
        {
            _curveWay = curveWay;
            Transform.position = curveWay.GetStartPointPosition;
            CorrectDistance();
            SetScale();
            _playerMesh.enabled = true;
            _canMove = true;
        }

        public void LoadCharacterPosition(Vector3 playersSavedPos)
        {
            Transform.position = _curveWay.StartCharacterPosition = playersSavedPos;
            CorrectDistance();
            SetScale();
            _playerMesh.enabled = true;
            _canMove = true;
            ChangePosition(0);
        }
        private void ChangePosition(float direction)
        {
            if (direction != 0)
            {
                direction = direction > 0 ? 1 : -1;
            }

            _distance = Mathf.Clamp(_distance + direction * _speed * Time.deltaTime, 0, _curveWay.PathCreator.path.length);
            Transform.position = _curveWay.PathCreator.path.GetPointAtDistance(_distance, EndOfPathInstruction.Stop);

            SetScale();
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

        private void FlipCharacter()
        {
            _characterArmature._armature.flipX = !_characterArmature._armature.flipX;
        }

        #endregion


    }
}
