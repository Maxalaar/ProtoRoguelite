using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Fields

    #region Serialized Fields
    [SerializeField] private float _zoomSpeed = 1f;
    [SerializeField] private float _smoothSpeed = 2.0f;
    [SerializeField] private float _minOrtho = 1.0f;
    [SerializeField] private float _maxOrtho = 20.0f;
    #endregion Serialized Fields

    #region Private Fields
    private float _targetOrtho;
    #endregion Private Fields

    #region Properties
    #endregion Properties

    #endregion Fields

    #region Methods

    #region Unity Interface    
    void Start()
    {
        _targetOrtho = Camera.main.orthographicSize;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0.0f)
        {
            _targetOrtho -= scroll * _zoomSpeed;
            _targetOrtho = Mathf.Clamp(_targetOrtho, _minOrtho, _maxOrtho);
        }

        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, _targetOrtho, _smoothSpeed * Time.deltaTime);
    }
    #endregion Unity Interface

    #region Private Methods
    #endregion Private Methods

    #region Public Methods
    #endregion Public Methods

    #endregion Methods
}
