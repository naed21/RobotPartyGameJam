using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotPartyGameJam.Scripts
{
	//Use this to find the card data you want
	public enum CardReference : int
	{
		Test = 0
	}

	public enum ArtReference : int
	{
		Background_1 = 0,
		Midground_1 = 1,
		Foreground_1 = 2
	}

	public static class CardHelper
	{
		static string _DefaultBackground = "res://Assets/magiccardspack/face/face_bg.png";
		static string _DefaultMidground = "res://Assets/magiccardspack/face/face_bg_02.png";
		static string _DefaultForeground = "res://Assets/magiccardspack/magic/magic_02.png";

		public static string[] Arts =
		{
			_DefaultBackground,
			_DefaultMidground,
			_DefaultForeground
		};

		//Holds all of the card data
		public static CardData[] Data =
		{
			new CardData("Test", 1, "This is a test",
				ArtReference.Background_1, ArtReference.Midground_1, ArtReference.Foreground_1)
		};
	}
}
