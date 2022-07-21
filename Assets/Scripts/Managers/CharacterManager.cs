namespace ProtoRoguelite.Managers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Pool;

    public class CharacterManager : MonoBehaviour
    {
        #region Fields

        #region Serialized Fields
        [SerializeField] private GameObject _bluePrefab;
        [SerializeField] private GameObject _redPrefab;

        [SerializeField] private List<Team> _teams = new List<Team>();
        [SerializeField] private int nbBlue;
        [SerializeField] private int nbRed;
        #endregion Serialized Fields

        #region Private Fields
        private MainManager _mainManager = null;

        private List<Character> _characters = new List<Character>();
        private ObjectPool<GameObject> _charactersPoolBlue;
        private ObjectPool<GameObject> _charactersPoolRed;
        #endregion Private Fields

        #region Properties
        #endregion Properties

        #endregion Fields

        #region Methods

        #region Unity Interface
        private void Start()
        {
            _mainManager = MainManager.instance;

            Team blueTeam = new Team("Blue");
            Team redTeam = new Team("Red");

            blueTeam.AddAdeversary(redTeam);

            _teams.Add(blueTeam);
            _teams.Add(redTeam);

            _charactersPoolBlue = new ObjectPool<GameObject>
            (
                () => {
                    return Instantiate(_bluePrefab, transform);
                },
                character => {
                    character.gameObject.SetActive(true);
                },
                character => {
                    character.gameObject.SetActive(true);
                },
                character => {
                    Destroy(character.gameObject);
                },
                true,
                50,
                100
            );

            _charactersPoolRed = new ObjectPool<GameObject>
            (
                () => {
                    return Instantiate(_redPrefab, transform);
                },
                character => {
                    character.gameObject.SetActive(true);
                },
                character => {
                    character.gameObject.SetActive(true);
                },
                character => {
                    Destroy(character.gameObject);
                },
                true,
                50,
                100
            );

            for (int i = 0; i < nbBlue; i++)
            {
                float spawnEmplitude = 5f;
                GameObject characterInstance = _charactersPoolBlue.Get();
                Character newCharacter = characterInstance.GetComponent<Character>();

                if (newCharacter == null)
                {
                    Debug.Log("CharacterManager has character prefab without Character component.");
                    break;
                }

                Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(-spawnEmplitude, spawnEmplitude), UnityEngine.Random.Range(-spawnEmplitude, spawnEmplitude), 0);                              
                newCharacter.transform.position = spawnPos;
                
                AddCharacter(newCharacter, blueTeam);
            }

            for (int i = 0; i < nbRed; i++)
            {
                float spawnEmplitude = 5f;
                GameObject characterInstance = _charactersPoolRed.Get();
                Character newCharacter = characterInstance.GetComponent<Character>();

                if (newCharacter == null)
                {
                    Debug.Log("CharacterManager has character prefab without Character component.");
                    break;
                }

                Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(-spawnEmplitude, spawnEmplitude), UnityEngine.Random.Range(-spawnEmplitude, spawnEmplitude), 0);
                newCharacter.transform.position = spawnPos;
                
                AddCharacter(newCharacter, redTeam);
            }

            // for (int i = 0; i < 10; i++)
            // {
            //     float spawnEmplitude = 5f;
            //     GameObject characterInstance = Instantiate(_bluePrefab, transform);
            //     Character newCharacter = characterInstance.GetComponent<Character>();
                
            //     Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(-spawnEmplitude, spawnEmplitude), UnityEngine.Random.Range(-spawnEmplitude, spawnEmplitude), 0);
            //     newCharacter.transform.position = spawnPos;
                
            //     AddCharacter(newCharacter, blueTeam);
            // }

            // for (int i = 0; i < 10; i++)
            // {
            //     float spawnEmplitude = 5f;
            //     GameObject characterInstance = Instantiate(_redPrefab, transform);
            //     Character newCharacter = characterInstance.GetComponent<Character>();

            //     Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(-spawnEmplitude, spawnEmplitude), UnityEngine.Random.Range(-spawnEmplitude, spawnEmplitude), 0);
            //     newCharacter.transform.position = spawnPos;

            //     AddCharacter(newCharacter, redTeam);
            // } 

            foreach (Character character in _characters)
            {
                character.setTargetRandomAdeversaryCharacter();
            }
        }
        private void Update()
        {
            if (_characters == null)
            {
                Debug.LogWarning("CharacterManager._characters is null.");
                return;
            }

            foreach (Character character in _characters)
            {
                if (character == null)
                    continue;

                character.UpdateCharacter();
            }
        }
        #endregion Unity Interface

        #region Private Methods
        private void AddCharacter(Character character, Team team = null)
        {
            if (_characters.Contains(character) == true)
            {
                Debug.LogWarning("Attempt to add character to the CharacterManager but he is already in the CharacterManager.");
                return;
            }
            _characters.Add(character);

            if (team != null)
            {
                if (_teams.Contains(team) == false)
                {
                    Debug.LogWarning("Team is not in the CaracterManager.");
                    return;
                }
                team.AddCharacter(character);
            }
        }

        private void RemoveCharacter(Character character)
        {
            if (_characters.Contains(character) == true)
            {
                Debug.LogWarning("Attempt to remove character to the CharacterManager but he is not in the CharacterManager.");
                return;
            }
            _characters.Add(character);

            if (character.Team != null)
            {
                if (_teams.Contains(character.Team) == false)
                {
                    Debug.LogWarning("Team is not in the CaracterManager.");
                    return;
                }
                character.Team.RemoveCharacter(character);
            }
        }
        #endregion Private Methods

        #region Public Methods
        #endregion Public Methods

        #endregion Methods
    }
}