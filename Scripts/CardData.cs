using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace RobotPartyGameJam.Scripts
{
	public class CardData
	{
		public CardReference CardReference { get; set; }
		
		public string Name { get; set; }
		public int Cost { get; set; }
		public int PlayPower { get; set; }
		public int DiscardPower { get; set; }
		public int HeldPower { get; set; }
		public int DiscardModifyEffect { get; set; }
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

		public OnPlayEffect OnPlayEffect { get; set; }
		public OnDiscardEffect OnDiscardEffect { get; set; }
		public OnHeldEffect OnHeldEffect { get; set; }
		public OnDiscardModifyEffect OnDiscardModifyEffect { get; set; }

		public CardData(CardReference cardReference, string name, int cost, string abilityText, ArtReference backgroundAsset, ArtReference middlegroundAsset, ArtReference foregroundAsset, OnPlayEffect onPlayEffect, OnDiscardEffect onDiscardEffect, OnHeldEffect onHeldEffect, OnDiscardModifyEffect onOtherDiscardEffect)
		{
			CardReference = cardReference;
			Name = name;
			Cost = cost;
			AbilityText = abilityText;
			BackgroundAsset = backgroundAsset;
			MiddlegroundAsset = middlegroundAsset;
			ForegroundAsset = foregroundAsset;
			OnPlayEffect = onPlayEffect;
			OnDiscardEffect = onDiscardEffect;
			OnHeldEffect = onHeldEffect;
			OnDiscardModifyEffect = onOtherDiscardEffect;
		}

		public CardData(string[] cvs)
		{
			
		}

		public string [] GetCsvHeader()
		{
			List<string> values = new List<string>();
			foreach (var prop in this.GetType().GetProperties())
			{
				values.Add(prop.Name);
			}

			return values.ToArray();
		}

		public string[] ToCsv()
		{
			List<string> values = new List<string>();
			//Converts the data into a dictionary string
			foreach(var prop in this.GetType().GetProperties())
			{
				if(prop.GetType() == typeof(Enum))
				{
					values.Add(Enum.GetName(prop.GetType(), prop.GetValue(this)));
				}
				else //if(prop.GetType() == typeof(string))
				{
					values.Add(prop.GetValue(this).ToString());
				}
			}

			//ugh, it wants a list
			//return string.Join(',', values);
			return values.ToArray();
		}
	}
}
