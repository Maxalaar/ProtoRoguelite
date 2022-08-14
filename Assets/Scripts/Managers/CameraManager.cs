namespace ProtoRoguelite.Managers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CameraManager : MonoBehaviour
    {
        #region Fields

        #region Serialized Fields
        [SerializeField] private float _moveSpeed = 0.01f;
        [SerializeField] private float _zoomSpeed = 100f;
        [SerializeField] private float _smoothSpeed = 2.0f;
        [SerializeField] private float _minOrtho = 1.0f;
        [SerializeField] private float _maxOrtho = 20.0f;
        #endregion Serialized Fields

        #region Private Fields
        //Followed input system tutorial from
        //https://www.youtube.com/watch?v=m5WsmlEOFiA
        private InputActions _inputActions;

        private float _targetOrtho;

        private Vector2 _move;
        private Vector2 _zoom;
        #endregion Private Fields

        #region Properties
        #endregion Properties

        #endregion Fields

        #region Methods

        #region Unity Interface 
        private void Awake()
        {
            _inputActions = new InputActions();

            _targetOrtho = Camera.main.orthographicSize;

            //_inputActions.Battle.Move.performed += ctx => _move = ctx.ReadValue<Vector2>();
            //_inputActions.Battle.Zoom.performed += ctx => _zoom = ctx.ReadValue<Vector2>();
        }

        private void OnEnable()
        {
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        private void Update()
        {
            HandleMove();
            HandleZoom();
        }
        #endregion Unity Interface

        #region Private Methods
        #endregion Private Methods

        #region Public Methods
        public void HandleZoom()
        {
            //get input value
            _zoom = _inputActions.Battle.Zoom.ReadValue<Vector2>();

            //if no input detected or camera already moving, do not change _targetOrtho value
            if (_zoom != Vector2.zero && Camera.main.orthographicSize == _targetOrtho)
            {
                _targetOrtho -= _zoom.y * _zoomSpeed;
                _targetOrtho = Mathf.Clamp(_targetOrtho, _minOrtho, _maxOrtho);
            }

            //move camera towards _targetOrtho
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, _targetOrtho, _smoothSpeed * Time.deltaTime);
        }

        public void HandleMove()
        {
            //get input value
            _move = _inputActions.Battle.Move.ReadValue<Vector2>();

            //move camera
            Camera.main.transform.Translate(_move * _moveSpeed * Time.timeScale);
        }
        #endregion Public Methods

        #endregion Methods
    }
}