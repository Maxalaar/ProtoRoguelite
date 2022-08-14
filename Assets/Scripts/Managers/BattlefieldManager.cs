using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ProtoRoguelite.Managers
{
    public class BattlefieldManager : MonoBehaviour
    {
        #region Fields

        #region Serialized Fields
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private GameObject _obstaclePrefab;
        [SerializeField] private NavMeshSurface2d _navMeshSurface;

        [SerializeField] private int _obstaclesNumber = 5;
        #endregion Serialized Fields

        #region Private Fields
        #endregion Private Fields

        #region Properties
        #endregion Properties

        #endregion Fields

        #region Methods

        #region Unity Interface
        private void Awake()
        {
            GenerateObstacles();

            //rebake navmesh
            Physics2D.SyncTransforms();
            _navMeshSurface.BuildNavMesh();
        }
        #endregion Unity Interface

        #region Private Methods
        private void GenerateObstacles()
        {
            //get background width and height
            float bgWidth = _background.transform.localScale.x;
            float bgHeight = _background.transform.localScale.y;

            for (int i = 0; i < _obstaclesNumber; i++)
            {
                //get a random position on the background
                int obstacleX = Mathf.RoundToInt(Random.Range(-bgWidth / 2, bgWidth / 2));
                int obstacleY = Mathf.RoundToInt(Random.Range(-bgHeight / 2, bgHeight / 2));

                //generate object
                //TODO : pool obstacles
                GameObject newObstacle = Instantiate(_obstaclePrefab, _navMeshSurface.transform);
                newObstacle.transform.position = new Vector3(obstacleX, obstacleY, 0);
                newObstacle.transform.rotation = _background.transform.rotation;
            }
        }
        #endregion Private Methods

        #region Public Methods
        public void Init()
        {
            
        }
        #endregion Public Methods

        #endregion Methods
    }
}