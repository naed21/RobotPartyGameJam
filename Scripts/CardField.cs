using Godot;
using System;

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

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_CardsNode = this.FindChild("Cards");
		}

		public void Init(PlayerData player, PlayerData opponent)
		{
			_PlayerController = new PlayerController(player, this);
			_OpponentPlayerController = new PlayerController(opponent, this);
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if (_CurrentTurn == TurnState.Player)
			{
				_PlayerController.ProcessHuman(delta, _BattleState, ChangeState);
			}
			else if (_CurrentTurn == TurnState.Opponent)
			{
				_OpponentPlayerController.ProcessComputer(delta, _BattleState, ChangeState);
			}
			else
			{
				// dialog?
				// How is dialog constructed? Does the opponent have all of the dialog?
				//I guess just skip for now, go right to player's turn
				ChangeState(BattleState.Dialog, BattleState.Switch);
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
			}
		}

		public void DrawCard()
		{
			//Testing
			if (Input.IsKeyPressed(Key.Key1))
			{
				//Spawn a card
				//var testCard = CardHelper.GetCard(CardReference.Test);
				CardBase cardbase = _CardBaseScene.Instantiate<CardBase>(PackedScene.GenEditState.Instance);
				cardbase.Init(CardReference.Test);
				cardbase.SetGlobalPosition(this.GetGlobalMousePosition());
				cardbase.Scale *= CardSize / cardbase.GetRect().Size;

				_CardsNode.AddChild(cardbase);
			}
		}
	}
}
