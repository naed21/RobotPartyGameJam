using Godot;
using System;

namespace RobotPartyGameJam.Scripts
{
	public class PlayerData
	{
		public string Name { get; set; }
		public int Health { get; set; }
		public int Strength { get; set; }
		public int Endurance { get; set; }
		public int MaxMana { get; set; }
		public int MaxHandSize { get; set; }

		public OnJoinEffect[] OnJoinEffects { get; set; }
		public int[] OnJoinAmount { get; set; }

		public CardReference[] Deck { get; set; }

		public CardReference[] OnJoinCards { get; set; }

		public PlayerData(string name, int health, int strength, int endurance, int maxMana, int maxHandSize,
			OnJoinEffect[] onJoinEffects, int[] onJoinAmount, CardReference[] deck, CardReference[] onJoinCards)
		{
			Name = name;
			Health = health;
			Strength = strength;
			Endurance = endurance;
			MaxMana = maxMana;
			MaxHandSize = maxHandSize;
			OnJoinEffects = onJoinEffects;
			OnJoinAmount = onJoinAmount;
			Deck = deck;
			OnJoinCards = onJoinCards;
		}
	}
}
