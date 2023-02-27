using Raylib_cs;

using System.Numerics;

namespace Azimuth.UI
{
	public abstract class InteractableWidget : UiWidget
	{
		public enum InteractionState
		{
			Normal,
			Hovered,
			Selected,
			Disabled
		}

		public InteractionState State { get; private set; } = InteractionState.Normal;

		public bool Interactable { get; set; } = true;
		
		protected abstract Rectangle InteractionBoundary { get; }

		private ColorBlock colors;

		protected InteractableWidget(Vector2 _size, Vector2 _pos, ColorBlock _colors) : base(_size, _pos)
		{
			colors = _colors;
		}

		public override void Update(float _deltaTime)
		{
			bool hovered = Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), InteractionBoundary);
			bool clicked = Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT);

			InteractionState oldState = State;

			if(State == InteractionState.Selected && !clicked)
			{
				State = hovered ? InteractionState.Hovered : InteractionState.Normal;
			}
			else if(clicked && hovered)
			{
				State = InteractionState.Selected;
			}
			else if(hovered)
			{
				State = InteractionState.Hovered;
			}
			else
			{
				State = InteractionState.Normal;
			}

			if(!Interactable)
				State = InteractionState.Disabled;
			
			OnInteracted(State, oldState);
		}

		protected abstract void OnInteracted(InteractionState _state, InteractionState _oldState);

		protected Color ColorFromState()
		{
			return State switch
			{
				InteractionState.Normal => colors.normal,
				InteractionState.Hovered => colors.hovered,
				InteractionState.Selected => colors.selected,
				InteractionState.Disabled => colors.disabled,
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}
}