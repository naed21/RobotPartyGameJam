using Godot;
using System;
using System.Diagnostics;

public partial class CardBase : MarginContainer
{
	CardData _CardData;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_CardData = CardHelper.Data[(int)CardReference.Test];

		var backgroundTexture = ResourceLoader.Load<Texture2D>(_CardData.BackgroundAsset);
		if(backgroundTexture == null)
			GD.Print("No background: " + _CardData.BackgroundAsset);
		var backgroundSprite = (Sprite2D)this.FindChild("Background", false);
		backgroundSprite.Texture = backgroundTexture;
		var size = this.GetRect().Size;
		backgroundSprite.Scale = size / backgroundTexture.GetSize();

		var foregroundTexture = ResourceLoader.Load<Texture2D>(_CardData.ForegroundAsset);
		if (foregroundTexture == null)
			GD.Print("No foreground: " + _CardData.ForegroundAsset);
		var foregroundSprite = (Sprite2D)this.FindChild("Foreground", false);
		foregroundSprite.Texture = foregroundTexture;
		size = this.GetRect().Size;
		foregroundSprite.Scale = size / foregroundTexture.GetSize();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	
	}
}
