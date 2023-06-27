using Godot;
using System;

//Use this to find the card data you want
public enum CardReference : int
{
	Test = 0
}


public static class CardHelper
{
	static string _DefaultBackground = "res://Assets/magiccardspack/face/face_bg.png";
	static string _DefaultForeground = "res://Assets/magiccardspack/face/face_bg_02.png";

	//Holds all of the card data
	public static CardData[] Data =
	{
		new CardData("Test", 1, "This is a test", _DefaultBackground, _DefaultForeground)
	};
}

public class CardData
{
	public string Name { get; set; }
	public int Cost { get; set; }
	public string Text { get; set; }
	public string BackgroundAsset { get; set; }
	public string ForegroundAsset { get; set; }

	public CardData(string name, int cost, string text, string backgroundAsset, string foregroundAsset)
	{
		Name = name;
		Cost = cost;
		Text = text;
		BackgroundAsset = backgroundAsset;
		ForegroundAsset = foregroundAsset;
	}
}
