using Azimuth;
using Azimuth.UI;

using System.Numerics;

namespace Azimuth_Test
{
	public class AzimuthTestGame : Game
	{
		private ImageWidget image;
		private Button button;
		
		public override void Load()
		{
			image = new ImageWidget(Vector2.Zero, new Vector2(200, 400), "imagewidget");
			button = new Button(Vector2.Zero, new Vector2(150, 75), Button.RenderSettings.normal);
			button.SetDrawLayer(100);
			
			UIManager.Add(button);
			UIManager.Add(image);
		}

		public override void Unload()
		{
			
		}
	}
}