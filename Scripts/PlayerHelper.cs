using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPartyGameJam.Scripts
{
	public enum PlayerReference : int
	{
		Dummy = -2,
		Test = -1
	}
	
	public enum OnJoinEffect
	{ 
		Health,
		Strength,
		Endurance,
		MaxMana,
		MaxHandSize
	}
	
	public static class PlayerHelper
	{
		public static PlayerData[] Data =
		{
			new PlayerData(PlayerReference.Dummy, "Test", health: 10, strength: 1, endurance:1, maxMana:10, maxHandSize:5, 
			new OnJoinEffect[]
			{
				OnJoinEffect.Health
			},
			onJoinAmount: new int[]
			{
				1
			}, 
			//deck: new CardReference[]
			//{
			//	CardReference.Test
			//},
			onJoinCards: new CardReference[]
			{
				CardReference.Test
			})
		};

		static string _DeckDataFilePath = "res://Players/PlayerDecksCsv.txt";
		static string _PlayerDataFilePath = "res://Players/PlayerDataCsv.txt";

		private static Dictionary<int, CardReference[]> _DeckDict = null;
		public static Dictionary<int, CardReference[]> DeckDict
		{
			get
			{
				if(_DeckDict == null )
				{
					_DeckDict = LoadPlayerDeckFromFile(_DeckDataFilePath);
				}

				return _DeckDict;
			}
		}

		private static Dictionary<int, PlayerData> _PlayerDict = null;
		public static Dictionary<int, PlayerData> PlayerDict
		{
			get
			{
				if (_PlayerDict == null)
				{
					_PlayerDict = LoadPlayerDataFromFile(_PlayerDataFilePath);
				}

				return _PlayerDict;
			}
		}

		public static Dictionary<int, PlayerData> LoadPlayerDataFromFile(string filePath)
		{
			Dictionary<int, PlayerData> playerList = new Dictionary<int, PlayerData>();

			bool firstLine = true;
			string[] header = new string[0];
			using (var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read))
			{
				//loop forever until broken
				while (true)
				{
					var playerText = file.GetCsvLine();

					//empty line
					if (playerText[0] == "")
						break;

					//First line is column header
					if (firstLine)
					{
						firstLine = false;
						header = playerText;
						continue;
					}

					var newPlayer = new PlayerData(header, playerText);

					if (!playerList.ContainsKey((int)newPlayer.PlayerReference))
						playerList.Add((int)newPlayer.PlayerReference, newPlayer);
				}

				return playerList;
			}
		}

		public static Dictionary<int, CardReference[]> LoadPlayerDeckFromFile(string filePath)
		{
			Dictionary<int, CardReference[]> deckList = new Dictionary<int, CardReference[]>();

			//bool firstLine = true;
			string[] header = new string[0];
			using (var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read))
			{
				//loop forever until broken
				while (true)
				{
					var deckColumns = file.GetCsvLine();

					//empty line, fine is done
					if (deckColumns[0] == "")
						break;					

					object playerRefObj;
					Enum.TryParse(typeof(PlayerReference), deckColumns[0], true, out playerRefObj);
					PlayerReference playerRef = (PlayerReference)playerRefObj;

					//skip duplicate
					if (deckList.ContainsKey((int)playerRef))
						continue;

					deckList.Add((int)playerRef, new CardReference[deckColumns.Length - 1]);

					//Zero is the player ref, so start at 1
					for (int x = 1; x < deckColumns.Length; x++)
					{
						object cardRefObj;
						Enum.TryParse(typeof(CardReference), deckColumns[x], true, out cardRefObj);
						CardReference cardRef = (CardReference)cardRefObj;

						deckList[(int)playerRef][x - 1] = cardRef;
					}
				}

				return deckList;
			}
		}
	}
}
