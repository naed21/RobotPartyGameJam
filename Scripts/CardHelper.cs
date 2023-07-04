using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RobotPartyGameJam.Scripts
{
	//Use this to find the card data you want
	public enum CardReference : int
	{
		Test2 = -2,
		Test = -1
	}

	public enum ArtReference : int
	{
		Background_1 = 0,
		Midground_1 = 1,
		Foreground_1 = 2
	}

	//When the card is played
	public enum OnPlayEffect: int
	{
		None = -1,
		Damage,
		Heal,
		Draw,
		Discard
	}

	//When the card is discarded
	public enum OnDiscardEffect: int
	{
		None = -1,
		ModifyHealth, //Negative power = loss health
		Draw,
		Discard
	}

	//If you don't play a card
	public enum OnHeldEffect : int
	{
		None = -1
	}

	//Happens when a different card is discarded
	public enum OnDiscardModifyEffect:int
	{
		None = -1
	}

	public static class CardHelper
	{
		static string _CardDataFilePath = "res://Cards/CardDataCsv.txt";
		
		static string _DefaultBackground = "res://Assets/magiccardspack/face/face_bg.png";
		static string _DefaultMidground = "res://Assets/magiccardspack/face/face_bg_02.png";
		static string _DefaultForeground = "res://Assets/magiccardspack/magic/magic_02.png";

		public static string[] Arts =
		{
			_DefaultBackground,
			_DefaultMidground,
			_DefaultForeground
		};

		////Holds all of the card data
		private static Dictionary<int, CardData> _Data = new Dictionary<int, CardData>();

		public static CardData GetCard(CardReference cardRef)
		{
			if(_Data.Count == 0)
			{
				_Data = LoadCardDataFromFile(_CardDataFilePath);
			}

			if (_Data.ContainsKey((int)cardRef))
				return _Data[(int)cardRef];

			return null;
		}

		private static Dictionary<int, CardData> LoadCardDataFromFile(string filePath)
		{
			Dictionary<int, CardData> cardList = new Dictionary<int, CardData>();

			bool firstLine = true;
			using (var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read))
			{
				//loop forever until broken
				while(true)
				{
					var card = file.GetCsvLine();
					if (card.Length == 0)
						break;

					//First line is column name, skip
					if(firstLine)
					{
						firstLine = false;
						continue;
					}

					var newCard = new CardData(card);

					cardList.Add((int)newCard.CardReference, newCard);
				}

				return cardList;
			}
		}

		public static void ExportToCsv()
		{
			if (_Data.Keys.Count == 0)
			{
				_Data.Add((int)CardReference.Test,
					new CardData(CardReference.Test, "Test", 1, "This is {P} a test",
					ArtReference.Background_1, ArtReference.Midground_1, ArtReference.Foreground_1,
					OnPlayEffect.None, OnDiscardEffect.None, OnHeldEffect.None, OnDiscardModifyEffect.None)
				);
				_Data.Add((int)CardReference.Test2,
					new CardData(CardReference.Test, "Test2", 1, "This is {P} a test2",
					ArtReference.Background_1, ArtReference.Midground_1, ArtReference.Foreground_1,
					OnPlayEffect.None, OnDiscardEffect.None, OnHeldEffect.None, OnDiscardModifyEffect.None)
				);
			}

			using (var file = FileAccess.Open(_CardDataFilePath, FileAccess.ModeFlags.Write))
			{
				var firstKey = _Data.Keys.First();
				var properyArray = _Data[firstKey].GetCsvHeader();
				file.StoreCsvLine(properyArray);
				
				string[] csvFile = new string[_Data.Keys.Count];
				//Keys might not be incremental starting from zero, so grab them like this
				var keys = _Data.Keys.ToArray();
				for(int x = 0; x< _Data.Keys.Count; x++)
				{
					//csvFile[x] = Data[keys[x]].ToCsv();
					file.StoreCsvLine(_Data[keys[x]].ToCsv());
				}

				//file.StoreCsvLine(csvFile);
			}
		}
	}
}
