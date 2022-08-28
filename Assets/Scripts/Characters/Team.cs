using ProtoRoguelite.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private Color _color;
    #endregion Private Fields

    #region Properties
    public Color Color => _color;

    public List<Team> AdversaryTeams
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

    #region Constructor
    public Team(string name, Color color)
    {
        _name = name;
        _color = color;
    }
    #endregion Constructor

    #region Private Methods
    #endregion Private Methods

    #region Public Methods

    public void AddCharacter(Character character)
    {
        if (_characters.Contains(character) == true)
        {
            Debug.LogWarning("Attempt to add character to team but he is already in the team");
            return;
        }
        _characters.Add(character);
        character.Team = this;
        character.SpriteRenderer.color = _color;

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

    public void AddAdversary(Team newAdversary)
    {
        if (_adeversaryTeams.Contains(newAdversary))
        {
            return;
        }
        if (_allyTeams.Contains(newAdversary))
        {
            _allyTeams.Remove(newAdversary);
        }
        _adeversaryTeams.Add(newAdversary);
        newAdversary.AddAdversary(this);
    }
    #endregion Public Methods

    #endregion Methods
}
