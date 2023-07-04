using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPartyGameJam.Scripts
{
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
	}
}
