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


        [SerializeField] private float _backgroundMinWidth = 15;
        [SerializeField] private float _backgroundMaxWidth = 25;
        [SerializeField] private float _backgroundMinHeight = 8;
        [SerializeField] private float _backgroundMaxHeight = 15;

        #endregion Serialized Fields

        #region Private Fields
        private List<GameObject> _obstacles = new List<GameObject>();
        #endregion Private Fields

        #region Properties
        public float BackgroundWidth => _background.transform.localScale.x;
        public float BackgroundHeight => _background.transform.localScale.y;
        #endregion Properties

        #endregion Fields

        #region Methods

        #region Unity Interface
        private void Awake()
        {
            Init();
        }
        #endregion Unity Interface

        #region Private Methods
        private void GenerateObstacles()
        {
            DestroyObstacles();

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

                _obstacles.Add(newObstacle);
            }
        }

        private void DestroyObstacles()
        {
            GameObject[] obstaclesTemp = _obstacles.ToArray();

            foreach (GameObject obstacle in obstaclesTemp)
            {
                Destroy(obstacle);
            }

            _obstacles.Clear();
        }

        private void GenerateBackground()
        {
            Vector3 newScale = Vector3.zero;

            float newWidth = Random.Range(_backgroundMinWidth, _backgroundMaxWidth);
            newScale.x = newWidth;

            float newHeight = Random.Range(_backgroundMinHeight, _backgroundMaxHeight);
            newScale.y = newHeight;

            _background.transform.localScale = newScale;
        }
        #endregion Private Methods

        #region Public Methods
        public void Init()
        {
            GenerateBackground();
            GenerateObstacles();

            //rebake navmesh
            Physics2D.SyncTransforms();
            _navMeshSurface.BuildNavMesh();
        }
        #endregion Public Methods

        #endregion Methods
    }
}