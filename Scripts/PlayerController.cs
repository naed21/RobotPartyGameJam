using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RobotPartyGameJam.Scripts.CardField;

namespace RobotPartyGameJam.Scripts
{
	public  class PlayerController
	{
		PlayerData _PlayerData;

		CardData[] _Deck;
		CardData[] _DiscardPile;
		CardData[] _Hand;
		int _CurrentHealth;

		RandomNumberGenerator _Rand = new RandomNumberGenerator();

		CardField _ParentNode;

		//This class handles stuff about the Player or AI during battles
		public PlayerController(PlayerData playerData, CardField parent) 
		{
			_PlayerData = playerData;
			_CurrentHealth = _PlayerData.Health;
			_ParentNode = parent;

			InitializeDeck();

			_Rand.Randomize();
		}

		public void ProcessHuman(double delta, int turnNum, BattleState state, Action<BattleState, BattleState> changeStateAction)
		{
			if(state == BattleState.Dialog)
			{
				//Click to skip dialog
			}
			else if(state == BattleState.Draw)
			{
				//Wait for draw animation or shuffle animation if needed
				InternalWait(500);

				DiscardHand();
				Draw(5, true);

				InternalWait(500);

				changeStateAction(state, BattleState.Play);
			}
			else if(state == BattleState.Play)
			{
				//handle input for playing cards
			}
			else if(state == BattleState.End)
			{
				//Check cards in hand if they have end of turn effects

				changeStateAction(state, BattleState.Switch);

			}
		}

		public void ProcessComputer(double delta, int turnNum, BattleState state, Action<BattleState, BattleState> changeStateAction)
		{
			if (state == BattleState.Dialog)
			{
				//Click to skip dialog
			}
			else if (state == BattleState.Draw)
			{
				//Wait for draw animation or shuffle animation if needed
				//InternalWait(500);

				DiscardHand();
				Draw(5, true);

				//InternalWait(500);

				changeStateAction(state, BattleState.Play);
			}
			else if (state == BattleState.Play)
			{
				//handle input for playing cards
			}
			else if (state == BattleState.End)
			{
				//Check cards in hand if they have end of turn effects

				changeStateAction(state, BattleState.Switch);

			}
		}

		private void DiscardHand()
		{
			if (_DiscardPile == null)
				_DiscardPile = new CardData[0];

			if (_Hand == null)
			{
				_Hand = new CardData[0];
				return;
			}
			
			var tempDiscard = _DiscardPile.ToList();
			var tempHand = new List<CardData>();

			int totalDraw = 0;
			List<int> discardedCardIds = new List<int>();

			foreach(var card in _Hand)
			{
				if(card.OnDiscardEffect == OnDiscardEffect.Draw)
				{
					totalDraw += card.DiscardPower;
				}
				else if(card.OnDiscardEffect == OnDiscardEffect.ModifyHealth)
				{
					_CurrentHealth += card.DiscardPower;
				}

				discardedCardIds.Add(card.Id);
				tempDiscard.Add(card);
			}

			foreach (var id in discardedCardIds)
				_ParentNode.DiscardCard(id);

			_DiscardPile = tempDiscard.ToArray();

			_Hand = new CardData[0];

			if (totalDraw > 0)
				Draw(totalDraw, true);
		}

		private async void InternalWait(int milliseconds)
		{
			await Task.Delay(milliseconds);
		}

		public void Draw(int amount, bool shuffleDiscardPileIntoDeckWhenEmpty)
		{
			var handList = _Hand.ToList();
			var deckList = _Deck.ToList();
			CardData newCard;
			
			for(int x = 0; x < amount; x++)
			{
				if(deckList.Count > 0)
				{
					newCard = deckList[0];
					handList.Add(newCard);
					_ParentNode.DrawCard(newCard);
					deckList.RemoveAt(0);
				}
				else if (shuffleDiscardPileIntoDeckWhenEmpty && _DiscardPile.Length > 0)
				{
					deckList.AddRange(_DiscardPile);
					_DiscardPile = new CardData[0];

					_Deck = deckList.ToArray();
					ShuffleDeck();

					deckList = _Deck.ToList();
					newCard = deckList[0];
					handList.Add(newCard);
					_ParentNode.DrawCard(newCard);
					deckList.RemoveAt(0);
				}
				else
				{
					break;
				}
			}

			//Tells our custom event in the DeckTextureButton that the deck size changed
			//Currently just used when the deck is zero and it changes the texture
			_ParentNode.EmitSignal("DeckChanged", deckList.Count);

			_Hand = handList.ToArray();
			_Deck = deckList.ToArray();
		}

		/// <summary>
		/// Resets the deck to use the cards from the Player Data. Then Shuffles
		/// </summary>
		public void InitializeDeck()
		{
			_Deck = new CardData[_PlayerData.Deck.Length];

			for (int x = 0; x < _PlayerData.Deck.Length; x++)
			{
				_Deck[x] = CardHelper.GetCard(_PlayerData.Deck[x]).Clone();
			}

			ShuffleDeck();
		}

		/// <summary>
		/// Takes the cards currently in the deck and randomly arranges them
		/// </summary>
		public void ShuffleDeck()
		{
			CardData[] temp = new CardData[_Deck.Length];
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
