using Godot;
using System;

namespace RobotPartyGameJam.Scripts
{

	public partial class DeckTextureButton : TextureButton
	{
		[Signal]
		public delegate void DeckChangedEventHandler(int cardsRemaining);
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			//"Deck" is parent, "CardField" is grandparent
			var node = this.GetParent().GetParent();
			var cardField = (CardField)(node.GetScript());
			this.Scale *= cardField.CardSize / this.GetRect().Size;

			DeckChanged += HandleDeckChangedEvent;
		}

		public void HandleDeckChangedEvent(int cardsRemaining)
		{
			if (cardsRemaining == 0)
				this.Disabled = true;
			else
				this.Disabled = false;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}
