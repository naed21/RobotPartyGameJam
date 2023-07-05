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
			new PlayerData("Test", health: 10, strength: 1, endurance:1, maxMana:10, maxHandSize:5, 
			new OnJoinEffect[]
			{
				OnJoinEffect.Health
			},
			onJoinAmount: new int[]
			{
				1
			}, 
			deck: new CardReference[]
			{
				CardReference.Test
			}, 
			onJoinCards: new CardReference[]
			{
				CardReference.Test
			})
		};

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

					var newPlayer = new CardData(header, playerText);

					if (!playerList.ContainsKey((int)newPlayer.PlayerReference))
						playerList.Add((int)newPlayer.PlayerReference, newPlayer);
				}

				return playerList;
			}
		}

	}
}
