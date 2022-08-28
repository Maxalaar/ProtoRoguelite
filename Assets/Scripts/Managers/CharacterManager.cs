namespace ProtoRoguelite.Managers
{
    using ProtoRoguelite.Characters;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Pool;

    public class CharacterManager : MonoBehaviour
    {
        #region Fields

        #region Serialized Fields
        [SerializeField] private GameObject _characterPrefab;

        [SerializeField] private List<Team> _teams = new List<Team>();
        [SerializeField] private int nbBlue;
        [SerializeField] private int nbRed;

        [SerializeField] private CharacterArchetypeSO _characterArchetypeSO = null;
        #endregion Serialized Fields

        #region Private Fields
        private MainManager _mainManager = null;

        private List<Character> _characters = new List<Character>();
        private ObjectPool<GameObject> _charactersPool;
        #endregion Private Fields

        #region Properties
        #endregion Properties

        #endregion Fields

        #region Methods

        #region Unity Interface
        private void Start()
        {
            _mainManager = MainManager.instance;

            Team blueTeam = new Team("Blue", Color.blue);
            Team redTeam = new Team("Red", Color.red);

            blueTeam.AddAdversary(redTeam);

            _teams.Add(blueTeam);
            _teams.Add(redTeam);

            _charactersPool = new ObjectPool<GameObject>
            (
                () => {
                    return Instantiate(_characterPrefab, transform);
                },
                character => {
                    character.gameObject.SetActive(true);           
                },
                character => {
                    character.gameObject.SetActive(false);
                },
                character => {
                    Destroy(character.gameObject);
                },
                true,
                nbBlue + nbRed,
                nbBlue + nbRed
            );

            for (int i = 0; i < nbBlue; i++)
            {                
                Character character = AddCharacter(blueTeam);
            }

            for (int i = 0; i < nbRed; i++)
            {
                Character character = AddCharacter(redTeam);
            }

            foreach (Character character in _characters)
            {
                character.SetTargetRandomAdversary();
            }

            InvokeRepeating("FillTeamCharacters", 15f, 15f);
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
        private void FillTeamCharacters()
        {
            int blueCount = nbBlue - _teams[0].Characters.Count;
            for (int i = 0; i < blueCount; i++)
            {
                AddCharacter(_teams[0]);
            }

            int redCount = nbRed - _teams[1].Characters.Count;
            for (int i = 0; i < redCount; i++)
            {
                AddCharacter(_teams[1]);
            }
        }

        private Character AddCharacter(Team team = null)
        {
            GameObject characterInstance = _charactersPool.Get();
            Character character = characterInstance.GetComponent<Character>();
            character?.Init(_characterArchetypeSO);

            if (character == null)
            {
                Debug.Log("CharacterManager has character prefab without Character component.");
                return null;
            }

            float width = _mainManager.BattlefieldManager.BackgroundWidth / 2;
            float height = _mainManager.BattlefieldManager.BackgroundHeight / 2;
            Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(-width, width), UnityEngine.Random.Range(-height, height), 0);
            character.transform.position = spawnPos;

            if (_characters.Contains(character) == true)
            {
                Debug.LogWarning("Attempt to add character to the CharacterManager but he is already in the CharacterManager.");
                return null;
            }
            _characters.Add(character);

            if (team != null)
            {
                if (_teams.Contains(team) == false)
                {
                    Debug.LogWarning("Team is not in the CaracterManager.");
                    return null;
                }
                team.AddCharacter(character);
            }

            return character;
        }
        #endregion Private Methods

        #region Public Methods
        public void RemoveCharacter(Character character)
        {
            if (_characters.Contains(character) == false)
            {
                Debug.LogWarning("Attempt to remove character to the CharacterManager but he is not in the CharacterManager.");
                return;
            }
            _characters.Remove(character);

            _charactersPool.Release(character.gameObject);

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

        public void ResetTeams()
        {
            Character[] charactersTemp = _characters.ToArray();

            foreach (Character character in charactersTemp)
            {
                RemoveCharacter(character);
            }

            FillTeamCharacters();
        }
        #endregion Public Methods

        #endregion Methods
    }
}