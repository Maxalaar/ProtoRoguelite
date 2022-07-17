namespace ProtoRoguelite.Managers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class Team
    {
        public int teamNumber;
        public int teamSize;
        public GameObject characterPrefab;
        public Transform spawnPivot;
        [HideInInspector] public List<Character> characters;
    }

    public class CharacterManager : MonoBehaviour
    {
        #region Fields

        #region Serialized Fields
        [SerializeField] private List<Team> _teams = new List<Team>();
        #endregion Serialized Fields

        #region Private Fields
        private MainManager _mainManager = null;

        private Dictionary<int, Team> _characterTeamsDico = new Dictionary<int, Team>();

        private List<Character> _characters = new List<Character>();
        #endregion Private Fields

        #region Properties
        #endregion Properties

        #endregion Fields

        #region Methods

        #region Unity Interface
        private void Update()
        {
            if (_characters == null)
            {
                Debug.LogWarning("CharacterManager._characters is null !");
                return;
            }

            foreach (Character character in _characters)
            {
                if (character == null)
                    continue;

                character.UpdateCharacter();
            }
        }

        private void Start()
        {
            _mainManager = MainManager.instance;

            foreach (Team team in _teams)
            {
                _characterTeamsDico.Add(team.teamNumber, team);
            }

            InitTeams();
        }
        #endregion Unity Interface

        #region Private Methods
        private void InitTeams()
        {
            foreach (Team team in _characterTeamsDico.Values)
            {
                InitTeam(team);
            }
        }

        private void InitTeam(Team team)
        {
            if (team == null || team.characterPrefab == null)
            {
                Debug.LogWarning(string.Format("CharacterManager a team has wrong values !"));
                return;
            }

            //spawn new characters which number is defined by teamSize
            for (int i = 0; i < team.teamSize; i++)
            {
                GameObject characterInstance = Instantiate(team.characterPrefab, transform);
                Character character = characterInstance.GetComponent<Character>();

                if (character == null)
                {
                    Debug.LogWarning(string.Format("CharacterManager team {0}'s charPrefab has " +
                        "no Character component !", team.teamNumber));
                    return;
                }

                //set spawn position
                if (team.spawnPivot != null)
                {
                    Vector3 spawnPos = team.spawnPivot.transform.position;
                    spawnPos.x += UnityEngine.Random.Range(-1f, 1f);
                    spawnPos.y += UnityEngine.Random.Range(-1f, 1f);

                    character.transform.position = spawnPos;
                }

                AddCharacter(character, team);
            }
            
        }

        private void AddCharacter(Character character, Team team)
        {
            character.Team = team;

            if (!team.characters.Contains(character))
                team.characters.Add(character);

            if (!_characters.Contains(character))
                _characters.Add(character);
        }

        private void RemoveCharacter(Character character)
        {
            Team team = character.Team;

            if (team == null)
                return;

            if (team.characters.Contains(character))
                team.characters.Remove(character);

            if (_characters.Contains(character))
                _characters.Remove(character);
        }
        #endregion Private Methods

        #region Public Methods
        public Character FindTarget(Character character)
        {
            Character newTarget = null;

            //TODO : better find target
            //go through all teams
            foreach (Team team in _characterTeamsDico.Values)
            {
                //if the team is the same as the character's, or has no characters, return
                if (team == character.Team || team.characters.Count == 0)
                    continue;

                //pick a random character of the team as a new target
                int randomIndex = UnityEngine.Random.Range(0, team.characters.Count);
                newTarget = team.characters[randomIndex];
            }

            return newTarget;
        }
        #endregion Public Methods

        #endregion Methods
    }
}