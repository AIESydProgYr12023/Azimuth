using Raylib_cs;

namespace Azimuth
{
	public sealed class Window
	{
		public int Width { get; }
		public int Height { get; }
		public string Title { get; }
		public Color ClearColor { get; }
		public KeyboardKey QuitKey { get; }

		public Window()
		{
			Width = Config.Get<int>("Window", "width");
			Height = Config.Get<int>("Window", "height");
			Title = Config.Get<string>("Application", "name")!;
			ClearColor = Config.Get<Color>("Window", "clearColor");
			QuitKey = (KeyboardKey) Config.Get<int>("Application", "quitKey");
		}

		public void Open()
		{
			Raylib.SetExitKey(QuitKey);
			Raylib.InitWindow(Width, Height, Title);
		}

		public void Clear()
		{
			Raylib.ClearBackground(ClearColor);
		}

		public void Close()
		{
			Raylib.CloseWindow();
		}
	}
}