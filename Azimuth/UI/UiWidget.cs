using Azimuth.GameObjects;

using System.Numerics;

namespace Azimuth.UI
{
	public abstract class UiWidget : GameObject
	{
		public Vector2 size;

		protected UiWidget(Vector2 _size, Vector2 _pos)
		{
			position = _pos;
			size = _size;
		}

		public override void Draw()
		{
			OnGUI();
		}

		protected abstract void OnGUI();
	}
}