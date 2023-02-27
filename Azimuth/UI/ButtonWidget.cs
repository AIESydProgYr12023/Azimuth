using Raylib_cs;

using System.Numerics;

namespace Azimuth.UI
{
	public class ButtonWidget : InteractableWidget
	{
		public struct RenderSettings
		{
			public ColorBlock colors;
			public string text;
			public float roundedness;
			public int fontSize;
			public float fontSpacing;
			public string? fontId;
			public Color textColor;
		}
		
		public delegate void OnClickEvent();

		private OnClickEvent? onClicked;

		private readonly float roundedness;
		
		private readonly string text;
		private readonly int fontSize;
		private readonly float fontSpacing;
		
		private readonly Font font;
		private readonly Color textColor;
		private readonly Vector2 textSize;

		protected override Rectangle InteractionBoundary => new(position.X - size.X * 0.5f, position.Y - size.Y * 0.5f, size.X, size.Y);

		public ButtonWidget(Vector2 _size, Vector2 _pos, RenderSettings _render) : base(_size, _pos, _render.colors)
		{
			roundedness = _render.roundedness;
			
			text = _render.text;
			fontSize = _render.fontSize;
			fontSpacing = _render.fontSpacing;
			
			font = string.IsNullOrEmpty(_render.fontId) ? Raylib.GetFontDefault() : Assets.Find<Font>(_render.fontId);
			textColor = _render.textColor;
			textSize = Raylib.MeasureTextEx(font, text, fontSize, fontSpacing) * 0.5f;
		}

		public void AddListener(OnClickEvent? _event)
		{
			if(_event == null)
				return;
			
			if(onClicked == null)
			{
				onClicked = _event;
			}
			else
			{
				onClicked += _event;
			}
		}

		public void RemoveListener(OnClickEvent? _event)
		{
			if(_event == null)
				return;

			onClicked -= _event;
		}
		
		protected override void OnGUI()
		{
			Raylib.DrawRectangleRounded(InteractionBoundary, roundedness, 5, ColorFromState());
			Raylib.DrawTextPro(font, text, position - textSize, Vector2.Zero, 0f, fontSize, fontSpacing, textColor);
		}
		
		protected override void OnInteracted(InteractionState _state, InteractionState _oldState)
		{
			if(_oldState == InteractionState.Selected && _state != InteractionState.Selected)
				onClicked?.Invoke();
		}
	}
}