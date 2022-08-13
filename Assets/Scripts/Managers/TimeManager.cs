namespace ProtoRoguelite.Managers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TimeManager : MonoBehaviour
    {
        #region Fields

        #region Serialized Fields
        #endregion Serialized Fields

        #region Private Fields
        // private float _timeScale = 1f;
        #endregion Private Fields

        #region Properties
        #endregion Properties

        #endregion Fields

        #region Methods

        #region Constructor
        #endregion Constructor

        #region Unity Interface
        #endregion Unity Interface

        #region Private Methods
        #endregion Private Methods

        #region Public Methods
        public void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
        #endregion Public Methods

        #endregion Methods
    }
}