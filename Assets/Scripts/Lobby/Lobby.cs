using System.Collections.Generic;

namespace Lobby
{
    public class Lobby
    {
        private Dictionary<ulong, LobbyData> _lobbyData = new();

        public void SelectSlender(string name, ulong id)
        {
            var data = new LobbyData(name, id, Teams.Slender);
            _lobbyData.Add(id, data);
        }
        
        public void SelectHuman(string name, ulong id)
        {
            var data = new LobbyData(name, id, Teams.Human);
            _lobbyData.Add(id, data);
        }

        public LobbyData GetData(ulong id) => _lobbyData[id];
    }

    public struct LobbyData
    {
        public LobbyData(string name, ulong id, Teams team)
        {
            Name = name;
            Id = id;
            Team = team;
        }

        public string Name;
        public ulong Id;
        public Teams Team;
    }

    public enum Teams
    {
        Slender,
        Human
    }
}