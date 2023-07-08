using Godot;
using RobotPartyGameJam.Scripts;
using System;

public partial class CardField : Node2D
{

	private PackedScene _CardBaseScene = (PackedScene)GD.Load("res://Cards/Cardbase.tscn");

	private Node _CardsNode;

	[Export]
	private Vector2 _CardSize = new Vector2(125, 175);

	private PlayerController _PlayerController;
	private PlayerController _OpponentPlayerController;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_CardsNode = this.FindChild("Cards");
	}

	public void Init(PlayerData challenger, PlayerData opponent)
	{
		_PlayerController = new PlayerController(challenger);
		_OpponentPlayerController = new PlayerController(opponent);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Testing
		if(Input.IsKeyPressed(Key.Key1))
		{
			//Spawn a card
			//var testCard = CardHelper.GetCard(CardReference.Test);
			CardBase cardbase = _CardBaseScene.Instantiate<CardBase>(PackedScene.GenEditState.Instance);
			cardbase.Init(CardReference.Test);
			cardbase.SetGlobalPosition(this.GetGlobalMousePosition());
			cardbase.Scale *= _CardSize / cardbase.GetRect().Size;

			_CardsNode.AddChild(cardbase);
		}
	}
}
