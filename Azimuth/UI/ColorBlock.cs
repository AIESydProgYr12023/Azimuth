using Raylib_cs;

namespace Azimuth.UI
{
	public struct ColorBlock
	{
		public Color normal;
		public Color hovered;
		public Color selected;
		public Color disabled;

		public ColorBlock(Color _normal, Color _hovered, Color _selected, Color _disabled)
		{
			normal = _normal;
			hovered = _hovered;
			selected = _selected;
			disabled = _disabled;
		}
	}
}