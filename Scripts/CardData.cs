using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;

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

		[JsonIgnore]
		//used to track cards between UI and player controller
		public int Id { get; set; }

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

		public CardData(string[] header, string[] data)
		{
			Dictionary<string, PropertyInfo> propDict = new Dictionary<string, PropertyInfo>();

			foreach (var prop in this.GetType().GetProperties())
			{
				propDict.Add(prop.Name, prop);
			}

			for (int x = 0; x < header.Length; x++)
			{
				var prop = propDict[header[x]];
				if (prop.PropertyType.IsEnum)
				{
					object enumValue = null;
					Enum.TryParse(prop.PropertyType, data[x], true, out enumValue);
					prop.SetValue(this, enumValue);
				}
				else if(prop.PropertyType == typeof(int))
				{
					int temp = 0;
					int.TryParse(data[x], out temp);
					prop.SetValue(this, temp);
				}
				else //if (prop.PropertyType == typeof(string))
					prop.SetValue(this, data[x]);
			}
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
			//Converts the data into a string list
			foreach (var prop in this.GetType().GetProperties())
			{
				//Hijacking the json ignore to also ignore the prop here
				if (Attribute.IsDefined(prop, typeof(JsonIgnoreAttribute)))
					continue;	
				
				if(prop.PropertyType.IsEnum)
				{
					values.Add(Enum.GetName(prop.PropertyType, prop.GetValue(this)));
				}
				else //if(prop.PropertyType == typeof(string))
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
