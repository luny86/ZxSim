
using System;
using System.Collections.Generic;
using Godot;
using ZX.Platform;

namespace Platform
{
	/// <summary>
	/// Example platform user input which uses the 
	/// <see cref="ZX.Platform.IUserInput"/> interface. This maps the phyical GODOT
	/// inputs to the <ZX.Platform.UserInputFlags> and when one of the maps changes
	/// state, invokes the pressed or released event.
	/// </summary>
	internal class UserInputBridge : IUserInput
	{
		/// <summary>
		/// Keep track of which input is pressed.
		/// </summary>
		private UserInputFlags _flags = UserInputFlags.None;

		/// <summary>
		/// Maps the GODOT input names to the input flags.
		/// </summary>
		private readonly Dictionary<string, UserInputFlags> _mappings = new Dictionary<string, UserInputFlags>()
		{
			{ "jump", UserInputFlags.FireA },
			{ "left", UserInputFlags.Left },
			{ "right", UserInputFlags.Right }
		};

		/// <summary>
		/// IUserInput events.
		/// </summary>
		public event EventHandler<UserInputFlags> InputPressed;
		public event EventHandler<UserInputFlags> InputReleased;

		/// <summary>
		/// Method used by the GODOT code to update everytime
		/// an input changes.
		/// </summary>
		/// <param name="e">GODOT input event value.</param>
		public void HandleInput(InputEvent e)
		{
			UserInputFlags previous = _flags;
			bool pressed = false;
			bool released = false;

			foreach(var kvp in _mappings)
			{
				if(e.IsActionPressed(kvp.Key))
				{
					previous |= kvp.Value;
					pressed = true;
				}

				if(e.IsActionReleased(kvp.Key))
				{
					previous ^= kvp.Value;
					released = true;
				}
			}

			if(_flags != previous)
			{
				if(pressed)
				{
					InputPressed?.Invoke(this, previous);
				}

				if(released)
				{
					InputReleased?.Invoke(this, previous);
				}

				_flags = previous;
			}
		}
	}
}
