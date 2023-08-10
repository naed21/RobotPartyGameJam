using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace RobotPartyGameJam.Scripts
{
	public class PlayerData
	{
		public PlayerReference PlayerReference { get; set; }
		public string Name { get; set; }
		public int Health { get; set; }
		public int Strength { get; set; }
		public int Endurance { get; set; }
		public int MaxMana { get; set; }
		public int MaxHandSize { get; set; }
		//public int[] IntOnJoinEffects {
		//	get
		//	{
		//		return OnJoinEffects.Select(x => (int)x).ToArray();
		//	}
		//	set
		//	{
		//		OnJoinEffects = value.Select(x => (OnJoinEffect)x).ToArray();
		//	}
		//}
		//[JsonIgnore]
		public OnJoinEffect[] OnJoinEffects { get; set; }
		public int[] OnJoinAmount { get; set; }

		[JsonIgnore]
		public CardReference[] Deck { get; set; }
		//[JsonIgnore]
		public CardReference[] OnJoinCards { get; set; }

		public PlayerData(PlayerReference playerReference, string name, int health, int strength, int endurance, int maxMana, int maxHandSize,
			OnJoinEffect[] onJoinEffects, int[] onJoinAmount, CardReference[] onJoinCards)
		{
			PlayerReference = playerReference;
			Name = name;
			Health = health;
			Strength = strength;
			Endurance = endurance;
			MaxMana = maxMana;
			MaxHandSize = maxHandSize;
			OnJoinEffects = onJoinEffects;
			OnJoinAmount = onJoinAmount;
			//Deck = deck;
			OnJoinCards = onJoinCards;
		}

		public PlayerData(string[] header, string[] data)
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
				else if (prop.PropertyType.IsArray && prop.PropertyType.GetElementType().IsEnum)
				{
					//Below was a massive pain in the ass, can't figure out how to dynamically cast
					// just do direct type casting

					var splits = data[x].Split(';');
					//object[] array = (object[])Array.CreateInstance(prop.PropertyType, splits.Length);
					int[] array = new int[splits.Length];

					object enumValue = null;
					var values = Enum.GetValues(prop.PropertyType.GetElementType());
					for (int y = 0; y < splits.Length; y++)
					{
						if (Enum.TryParse(prop.PropertyType.GetElementType(), splits[y], true, out enumValue))
						{
							//if (prop.PropertyType.GetElementType() == typeof(OnJoinEffect))
							{
								array[y] = (int)enumValue;
							}
							//else if (prop.PropertyType.GetElementType() == typeof(CardReference))
							//{
							//	array.SetValue((int)enumValue, y);
							//}
						}
						else //Get the first entry as a default
						{
							if (prop.PropertyType.GetElementType() == typeof(OnJoinEffect))
							{
								array[y] = (int)OnJoinEffect.Health;//.SetValue((int)OnJoinEffect.Health, y);
							}
							else if(prop.PropertyType.GetElementType() == typeof(CardReference))
							{
								array[y] = (int)CardReference.Test;
							}
						}
					}

					if (prop.PropertyType.GetElementType() == typeof(OnJoinEffect))
					{
						prop.SetValue(this, array.Select(x => (OnJoinEffect)x).ToArray());
					}
					else if (prop.PropertyType.GetElementType() == typeof(CardReference))
					{
						prop.SetValue(this, array.Select(x => (CardReference)x).ToArray());
					}

					//prop.SetValue(this, array);
				}
				else if (prop.PropertyType.IsArray && prop.PropertyType.GetElementType() == typeof(int))
				{
					var splits = data[x].Split(';');
					int[] array = new int[splits.Length];
					int temp = 0;
					for (int y = 0; y < splits.Length; y++)
					{
						if (int.TryParse(splits[y], out temp))
							array[y] = temp;
						else
							array[y] = 0;
					}

					prop.SetValue(this, array);
				}
				else if (prop.PropertyType == typeof(int))
				{
					int temp = 0;
					int.TryParse(data[x], out temp);
					prop.SetValue(this, temp);
				}
				else //if (prop.PropertyType == typeof(string))
					prop.SetValue(this, data[x]);
			}
		}

		public string[] GetCsvHeader()
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
				if (prop.PropertyType == typeof(Enum))
				{
					if(prop.PropertyType.IsArray)
					{
						var array = (object[])prop.GetValue(this);
						List<string> temp = new List<string>();
						foreach(var obj in array)
						{
							temp.Add(Enum.GetName(prop.GetType(), obj));
						}

						values.Add(string.Join(';', temp));
					}
					else
						values.Add(Enum.GetName(prop.GetType(), prop.GetValue(this)));
				}
				else //if(prop.PropertyType == typeof(string))
				{
					if (prop.PropertyType.IsArray)
					{
						var array = (object[])prop.GetValue(this);
						List<string> temp = new List<string>();
						foreach (var obj in array)
						{
							temp.Add(Enum.GetName(prop.GetType(), obj));
						}

						values.Add(string.Join(';', temp));
					}
					else
						values.Add(prop.GetValue(this).ToString());
				}
			}

			return values.ToArray();
		}
	}
}
