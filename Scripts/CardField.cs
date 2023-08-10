using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace RobotPartyGameJam.Scripts
{
	public partial class CardField : Node2D
	{

		private PackedScene _CardBaseScene = (PackedScene)GD.Load("res://Cards/Cardbase.tscn");

		private Node _CardsNode;

		[Export]
		public Vector2 CardSize = new Vector2(125, 175);

		private PlayerController _PlayerController;
		private PlayerController _OpponentPlayerController;

		private TurnState _CurrentTurn = TurnState.None;
		private BattleState _BattleState = BattleState.Dialog;

		private List<CardBase> _CardList = new List<CardBase>();

		enum TurnState
		{
			None, //If none, battle just started. Play dialog and draw initial cards
			Player,
			Opponent
		}

		public enum BattleState
		{
			Dialog, //Playing dialog, click to continue
			Draw,
			Play,
			End,
			Switch
		}

		Vector2 _CenterCardOval;
		float _Horizontal;
		float _Vertical;

		float _Angle;
		Vector2 _OvalAngleVect;

		//Keep track of the currently in-use cardIds
		//Used to make sure we issue a unique id
		List<int> _IdTracker = new List<int>();

		int _TurnNumber = 0;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_CardsNode = this.FindChild("Cards");

			_CenterCardOval = GetViewportRect().Size * new Vector2(0.5f, 1.3f);
			_Horizontal = GetViewportRect().Size.X * 0.45f; //%
			_Vertical = GetViewportRect().Size.Y * 0.4f;

			_Angle = Mathf.DegToRad(90) - 0.5f;

			//TODO: pick the players that are in the battle somewhere?
			//Init(PlayerReference.Test, PlayerReference.Dummy);
		}

		public void Init(PlayerReference player, PlayerReference opponent)
		{
			var playerData = PlayerHelper.PlayerDict[(int)player];
			playerData.Deck = PlayerHelper.DeckDict[(int)player];

			var opponentData = PlayerHelper.PlayerDict[(int)opponent];
			opponentData.Deck = PlayerHelper.DeckDict[(int)opponent];

			_PlayerController = new PlayerController(playerData, this);
			_OpponentPlayerController = new PlayerController(opponentData, this);
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			//testing, only do things after key press
			if (Input.IsPhysicalKeyPressed(Key.F1))
			{
				Init(PlayerReference.Test, PlayerReference.Dummy);
			}
			else if (Input.IsPhysicalKeyPressed(Key.F2))
			{
				if (_CurrentTurn == TurnState.Player)
				{
					_PlayerController.ProcessHuman(delta, _TurnNumber, _BattleState, ChangeState);
				}
				else if (_CurrentTurn == TurnState.Opponent)
				{
					_OpponentPlayerController.ProcessComputer(delta, _TurnNumber, _BattleState, ChangeState);
				}
				else
				{
					// dialog?
					// How is dialog constructed? Does the opponent have all of the dialog?
					//I guess just skip for now, go right to player's turn
					ChangeState(BattleState.Dialog, BattleState.Switch);
				}
			}
		}

		public void ChangeState(BattleState from, BattleState to)
		{
			_BattleState = to;

			if(_BattleState == BattleState.Switch)
			{
				//Do things at the end, then switch players

				if (_CurrentTurn == TurnState.None)
					_CurrentTurn = TurnState.Player;
				else if (_CurrentTurn == TurnState.Player)
					_CurrentTurn = TurnState.Opponent;
				else if (_CurrentTurn == TurnState.Opponent)
					_CurrentTurn = TurnState.Player;

				_BattleState = BattleState.Draw;

				_TurnNumber++;
			}
		}

		/// <summary>
		/// Increment Ids until we get over 100, then check to see if there are removed ids to reuse instead
		/// </summary>
		/// <returns></returns>
		private int GetId()
		{
			if (_IdTracker.Count == 0)
			{
				_IdTracker.Add(1);
				return 1;
			}
			else if(_IdTracker.Count < 100)
			{
				var max = _IdTracker.Max() + 1;
				_IdTracker.Add(max);
				return max;
			}
			else
			{
				var min = _IdTracker.Min() - 1;
				if (min > 0)
				{
					_IdTracker.Add(min);
					return min;
				}

				var max = _IdTracker.Max() + 1;
				_IdTracker.Add(max);
				return max;
			}
		}

		private void RemoveId(int id)
		{
			for (int x = 0; x < _IdTracker.Count; x++)
				if (_IdTracker[x] == id)
				{
					_IdTracker.RemoveAt(x);
					break;
				}
		}

		public void DrawCard(CardData card)
		{
			//Spawn a card
			if (card.Id < 1)
			{
				var id = GetId();
				card.Id = id;
			}

			CardBase cardbase = _CardBaseScene.Instantiate<CardBase>(PackedScene.GenEditState.Instance);
			cardbase.Init(card);
			//cardbase.SetGlobalPosition(this.GetGlobalMousePosition());

			//TODO: Change angle based on number of cards in hand
			_Angle += 0.25f;
			_OvalAngleVect = new Vector2(_Horizontal * (float)Math.Cos(_Angle), -_Vertical * (float)Math.Sin(_Angle));

			cardbase.RotationDegrees = (90 - Mathf.RadToDeg(_Angle)) / 4;

			cardbase.GlobalPosition = _CenterCardOval + _OvalAngleVect - cardbase.GetRect().Size;
			cardbase.Scale *= CardSize / cardbase.GetRect().Size;

			_CardsNode.AddChild(cardbase);
		}

		/// <summary>
		/// Card was moved to the discard pile
		/// </summary>
		/// <param name="cardId"></param>
		public void DiscardCard(int cardId)
		{
			//todo, play an effect?
			foreach (var card in _CardList)
			{
				if (card.GetId() == cardId)
				{
					_CardsNode.RemoveChild(card);
					card.Dispose();
					break;
				}
			}
		}

		/// <summary>
		/// Card was removed from the game, we won't see it again
		/// </summary>
		/// <param name="cardId"></param>
		public void DestroyCard(int cardId)
		{
			//todo, play an effect?
			foreach(var card in _CardList)
			{
				if (card.GetId() == cardId)
				{
					RemoveId(cardId);

					_CardsNode.RemoveChild(card);
					card.Dispose();
					break;
				}
			}
		}
	}
}


