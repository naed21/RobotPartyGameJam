using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPartyGameJam.Scripts
{
	public  class PlayerController
	{
		PlayerData _PlayerData;

		CardReference[] _Deck;
		CardReference[] _DiscardPile;
		CardReference[] _Hand;
		int _MaxHandSize;

		RandomNumberGenerator _Rand = new RandomNumberGenerator();

		//This class handles stuff about the Player or AI during battles
		public PlayerController(PlayerData playerData) 
		{
			_PlayerData = playerData;
			_MaxHandSize = _PlayerData.MaxHandSize;

			InitializeDeck();

			_Rand.Randomize();
		}

		public void Draw(int amount, bool shuffleDiscardPileIntoDeckWhenEmpty)
		{
			var handList = _Hand.ToList();
			var deckList = _Deck.ToList();
			
			for(int x = 0; x < amount; x++)
			{
				if(deckList.Count > 0)
				{
					handList.Add(deckList[0]);
				}
				else if (shuffleDiscardPileIntoDeckWhenEmpty && _DiscardPile.Length > 0)
				{
					deckList.AddRange(_DiscardPile);
					_DiscardPile = new CardReference[0];

					_Deck = deckList.ToArray();
					ShuffleDeck();

					deckList = _Deck.ToList();
					handList.Add(deckList[0]);
				}
				else
				{
					//No cards, can't do anything
					break;
				}
			}

			_Hand = handList.ToArray();
			_Deck = deckList.ToArray();
		}

		/// <summary>
		/// Resets the deck to use the cards from the Player Data. Then Shuffles
		/// </summary>
		public void InitializeDeck()
		{
			_Deck = (CardReference[])_PlayerData.Deck.Clone();

			ShuffleDeck();
		}

		/// <summary>
		/// Takes the cards currently in the deck and randomly arranges them
		/// </summary>
		public void ShuffleDeck()
		{
			CardReference[] temp = new CardReference[_Deck.Length];
			var deckList = _Deck.ToList();
			for(int x  = 0; x < temp.Length; x++)
			{
				int cardIndex = (int)_Rand.RandfRange(0, deckList.Count - 1);
				temp[x] = deckList[cardIndex];
				deckList.RemoveAt(cardIndex);
			}

			_Deck = temp;
		}
	}
}
