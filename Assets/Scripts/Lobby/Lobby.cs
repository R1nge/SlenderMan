using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lobby
{
    public class Lobby : MonoBehaviour
    {
        private Dictionary<ulong, LobbyData> _lobbyData = new();

        public static Lobby Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("Multiple Lobbies defined!");
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }


        public int GetPlayerCount() => _lobbyData.Count;

        public ulong GetSlenderId()
        {
            foreach (var pair in _lobbyData)
            {
                if (pair.Value.Team == Teams.Slender)
                {
                    return pair.Value.Id;
                }
            }
            
            Debug.LogError("Slender ID not found", this);

            return 0;
        }

        public void SelectSlender(string name, ulong id)
        {
            var data = new LobbyData(name, id, Teams.Slender);
            if (_lobbyData.ContainsKey(id))
            {
                Debug.Log("Lobby: Swapped team to Slender");
                _lobbyData[id] = data;
            }
            else
            {
                _lobbyData.Add(id, data);
            }
        }

        public void SelectHuman(string name, ulong id)
        {
            var data = new LobbyData(name, id, Teams.Human);
            if (_lobbyData.ContainsKey(id))
            {
                Debug.Log("Lobby: Swapped team to Human");
                _lobbyData[id] = data;
            }
            else
            {
                _lobbyData.Add(id, data);
            }
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