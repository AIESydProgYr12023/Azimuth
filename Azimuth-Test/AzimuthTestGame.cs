using Azimuth;
using Azimuth.GameObjects;
using Azimuth.UI;

using Raylib_cs;

using System.Numerics;

namespace Azimuth_Test
{
	public class AzimuthTestGame : Game
	{
		public ButtonWidget? button;

		public override void Load()
		{
			Vector2 buttonSize = new(150, 75);

			button = new ButtonWidget(buttonSize, buttonSize * 0.5f, new ButtonWidget.RenderSettings
			{
				colors = new ColorBlock
				{
					disabled = new Color(255, 255, 255, 128),
					hovered = Color.DARKGRAY,
					normal = Color.LIGHTGRAY,
					selected = Color.BLACK
				},
				fontSize = 30,
				fontSpacing = 1f,
				roundedness = 0.1f,
				text = "Button",
				fontId = null,
				textColor = Color.WHITE
			});

			button.AddListener(() => Console.Write("Help!"));

			GameObjectManager.Spawn(button);
		}

		public override void Unload()
		{
			if(button != null)
				GameObjectManager.Destroy(button);
		}
	}
}