namespace ProtoRoguelite.Managers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CallbackHelper : MonoBehaviour
    {
        private MainManager _mainManager = null;

        #region Unity Interface
        private void Start()
        {
            _mainManager = MainManager.instance;
        }
        #endregion Unity Interface

        #region TimeManager
        public void SetTimeScale(float timeScale) => _mainManager.TimeManager.SetTimeScale(timeScale);

        public void RestartBattle()
        {
            //reset battlefield
            _mainManager?.BattlefieldManager?.Init();

            //reset teams
            _mainManager?.CharacterManager.ResetTeams();
        }
        #endregion TimeManager
    }
}