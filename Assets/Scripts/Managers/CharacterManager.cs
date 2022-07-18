namespace ProtoRoguelite.Managers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class Team
    {
        #region Fields

        #region Serialized Fields
        #endregion Serialized Fields

        #region Private Fields
        [SerializeField] private string _name;
        [SerializeField] private List<Character> _characters = new List<Character>();
        [SerializeField] private List<Team> _adeversaryTeams = new List<Team>();
        [SerializeField] private List<Team> _allyTeams = new List<Team>();
        #endregion Private Fields

        #region Properties
        public List<Team> AdeversaryTeams
        {
            get { return _adeversaryTeams; }
        }
        public string Name
        {
            get { return _name; }
        }

        public List<Character> Characters
        {
            get { return _characters; }
        }
        #endregion Properties

        #endregion Fields

        #region Methods

        #region Unity Interface
        #endregion Unity Interface

        #region Private Methods
        #endregion Private Methods

        #region Public Methods

        #region Constructor
            public Team(string name)
            {
                _name = name;
            }
        #endregion Constructor
        public void AddCharacter(Character character)
        {
            if (_characters.Contains(character) == true)
            {
                Debug.LogWarning("Attempt to add character to team but he is already in the team");
                return;
            }
            _characters.Add(character);
            character.Team = this;
        }

        public void RemoveCharacter(Character character)
        {
            if (_characters.Contains(character) == false)
            {
                Debug.LogWarning("Attempt to remove character to team but he is not in the team");
                return;
            }
            _characters.Remove(character);
            character.Team = null;
        }

        public void AddAdeversary(Team newAdeversary)
        {
            if (_adeversaryTeams.Contains(newAdeversary))
            {
                return;
            }
            if (_allyTeams.Contains(newAdeversary))
            {
                _allyTeams.Remove(newAdeversary);
            }
            _adeversaryTeams.Add(newAdeversary);
            newAdeversary.AddAdeversary(this);
        }
        #endregion Public Methods

        #endregion Methods
    }

    public class CharacterManager : MonoBehaviour
    {
        #region Fields

        #region Serialized Fields
        [SerializeField] public GameObject BluePrefab;
        [SerializeField] public GameObject RedPrefab;
        #endregion Serialized Fields

        #region Private Fields
        private MainManager _mainManager = null;

        [SerializeField] private List<Team> _teams = new List<Team>();

        private List<Character> _characters = new List<Character>();
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

            for (int i = 0; i < 10; i++)
            {
                float spawnEmplitude = 5f;
                GameObject characterInstance = Instantiate(BluePrefab, transform);
                Character newCharacter = characterInstance.GetComponent<Character>();
                
                Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(-spawnEmplitude, spawnEmplitude), UnityEngine.Random.Range(-spawnEmplitude, spawnEmplitude), 0);
                newCharacter.transform.position = spawnPos;
                
                AddCharacter(newCharacter, blueTeam);
            }

            for (int i = 0; i < 10; i++)
            {
                float spawnEmplitude = 5f;
                GameObject characterInstance = Instantiate(RedPrefab, transform);
                Character newCharacter = characterInstance.GetComponent<Character>();

                Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(-spawnEmplitude, spawnEmplitude), UnityEngine.Random.Range(-spawnEmplitude, spawnEmplitude), 0);
                newCharacter.transform.position = spawnPos;

                AddCharacter(newCharacter, redTeam);
            } 

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