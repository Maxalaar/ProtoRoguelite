namespace ProtoRoguelite.Managers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MainManager : MonoBehaviour
    {
        #region Fields

        #region Serialized Fields
        [SerializeField] private BattlefieldManager _battlefieldManager = null;
        [SerializeField] private CameraManager _cameraManager = null;
        [SerializeField] private CharacterManager _characterManager = null;
        #endregion Serialized Fields

        #region Private Fields
        #endregion Private Fields

        #region Properties
        public BattlefieldManager BattlefieldManager => _battlefieldManager;
        public CameraManager CameraManager => _cameraManager;
        public CharacterManager CharacterManager => _characterManager;
        #endregion Properties

        public static MainManager instance = null;

        #endregion Fields

        #region Methods

        #region Unity Interface
        private void Awake()
        {
            //if another MainManager already exists, destroy itself
            //TODO : Singleton
            if (MainManager.instance != null)
                Destroy(gameObject);

            MainManager.instance = this;
        }
        #endregion Unity Interface

        #region Private Methods
        #endregion Private Methods

        #region Public Methods
        #endregion Public Methods

        #endregion Methods
    }
}