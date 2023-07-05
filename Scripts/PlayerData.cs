using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RobotPartyGameJam.Scripts
{
	public class PlayerData
	{
		public string Name { get; set; }
		public int Health { get; set; }
		public int Strength { get; set; }
		public int Endurance { get; set; }
		public int MaxMana { get; set; }
		public int MaxHandSize { get; set; }

		public OnJoinEffect[] OnJoinEffects { get; set; }
		public int[] OnJoinAmount { get; set; }

		public CardReference[] Deck { get; set; }

		public CardReference[] OnJoinCards { get; set; }

		public PlayerData(string name, int health, int strength, int endurance, int maxMana, int maxHandSize,
			OnJoinEffect[] onJoinEffects, int[] onJoinAmount, CardReference[] deck, CardReference[] onJoinCards)
		{
			Name = name;
			Health = health;
			Strength = strength;
			Endurance = endurance;
			MaxMana = maxMana;
			MaxHandSize = maxHandSize;
			OnJoinEffects = onJoinEffects;
			OnJoinAmount = onJoinAmount;
			Deck = deck;
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
