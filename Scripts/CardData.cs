using Godot;
using RobotPartyGameJam.Scripts;
using System;

public class CardData
{
	public string Name { get; set; }
	public int Cost { get; set; }
	/// <summary>
	/// What we tell the player the card does
	/// </summary>
	public string AbilityText { get; set; }
	public ArtReference BackgroundAsset { get; set; }
	/// <summary>
	/// The image that goes on the background to give the appearance of a border
	/// </summary>
	public ArtReference MiddlegroundAsset { get; set; }
	/// <summary>
	/// The image that goes in the middle of the character or item
	/// </summary>
	public ArtReference ForegroundAsset { get; set; }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="name"></param>
	/// <param name="cost"></param>
	/// <param name="abilityText">What we tell the player the card does</param>
	/// <param name="backgroundAsset"></param>
	/// <param name="middleground">Image inbetween the background and character or item</param>
	/// <param name="foregroundAsset">The character or item image</param>
	public CardData(string name, int cost, string abilityText, ArtReference background, ArtReference middleground, ArtReference foreground)
	{
		Name = name;
		Cost = cost;
		AbilityText = abilityText;
		BackgroundAsset = background;
		MiddlegroundAsset = middleground;
		ForegroundAsset = foreground;
	}
}
