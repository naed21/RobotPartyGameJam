using Godot;
using RobotPartyGameJam.Scripts;
using System;
using System.Diagnostics;

public partial class CardBase : MarginContainer
{
	CardData _CardData;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//TEST
		//CardHelper.ExportToCsv();

		_CardData = CardHelper.GetCard(CardReference.Test);

		//Stops the code here if we mess up data entry
		Debug.Assert(CardHelper.Arts.Length > (int)_CardData.BackgroundAsset);
		Debug.Assert(CardHelper.Arts.Length > (int)_CardData.MiddlegroundAsset);
		Debug.Assert(CardHelper.Arts.Length > (int)_CardData.ForegroundAsset);

		var backgroundAsset = CardHelper.Arts[(int)_CardData.BackgroundAsset];
		var backgroundTexture = ResourceLoader.Load<Texture2D>(backgroundAsset);
		if(backgroundTexture == null)
			GD.Print("No background: " + _CardData.BackgroundAsset);
		var backgroundSprite = (Sprite2D)this.FindChild("Background", false);
		backgroundSprite.Texture = backgroundTexture;
		var size = this.GetRect().Size;
		backgroundSprite.Scale = size / backgroundTexture.GetSize();

		var middleGroundAsset = CardHelper.Arts[(int)_CardData.MiddlegroundAsset];
		var middlegroundTexture = ResourceLoader.Load<Texture2D>(middleGroundAsset);
		if (middlegroundTexture == null)
			GD.Print("No middleground: " + _CardData.MiddlegroundAsset);
		var middlegroundSprite = (Sprite2D)this.FindChild("Middleground", false);
		middlegroundSprite.Texture = middlegroundTexture;
		size = this.GetRect().Size;
		middlegroundSprite.Scale = size / middlegroundTexture.GetSize();

		var foreGroundAsset = CardHelper.Arts[(int)_CardData.ForegroundAsset];
		var foregroundTexture = ResourceLoader.Load<Texture2D>(foreGroundAsset);
		if (foregroundTexture == null)
			GD.Print("No middleground: " + _CardData.ForegroundAsset);
		var foregroundSprite = (TextureRect)this.FindChild("TextureRect", true);
		foregroundSprite.Texture = foregroundTexture;
		//size = this.GetRect().Size;
		//foregroundSprite.Scale = size / foregroundTexture.GetSize();

		var nameLabel = (Label)this.FindChild("Label_Name", true);
		nameLabel.Text = _CardData.Name;

		var costLabel = (Label)this.FindChild("Label_Cost", true);
		costLabel.Text = _CardData.Cost.ToString();

		var abilityLabel = (Label)this.FindChild("Label_Ability", true);
		abilityLabel.Text = _CardData.AbilityText;


		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsMouseButtonPressed(MouseButton.Left))
		{
			//CardHelper.ExportToCsv();
			CardHelper.LoadCardDataFromFile("res://Cards/CardDataCsv.txt");
		}
	}
}
