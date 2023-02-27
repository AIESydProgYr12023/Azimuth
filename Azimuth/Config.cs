using Raylib_cs;

using System.Numerics;

namespace Azimuth
{
	public class Config
	{
		private static Config? instance;

		public static void Create()
		{
			instance ??= new Config();
		}

		public static VALUE? Get<VALUE>(string _category, string _key)
		{
			if(instance == null)
			{
				Console.WriteLine("[ERROR] Config not yet initialised!");

				return default;
			}

			Type valueType = typeof(VALUE);

			if(valueType == typeof(Vector2))
			{
				return (VALUE) Convert.ChangeType(instance.vector2s[_category][_key], valueType);
			}

			if(valueType == typeof(Vector3))
			{
				return (VALUE) Convert.ChangeType(instance.vector3s[_category][_key], valueType);
			}

			if(valueType == typeof(Color))
			{
				return (VALUE) Convert.ChangeType(instance.colors[_category][_key], valueType);
			}

			if(valueType == typeof(int))
			{
				return (VALUE) Convert.ChangeType(instance.ints[_category][_key], valueType);
			}

			if(valueType == typeof(float))
			{
				return (VALUE) Convert.ChangeType(instance.floats[_category][_key], valueType);
			}

			if(valueType == typeof(bool))
			{
				return (VALUE) Convert.ChangeType(instance.bools[_category][_key], valueType);
			}

			if(valueType == typeof(string))
			{
				return (VALUE) Convert.ChangeType(instance.strings[_category][_key], valueType);
			}
			
			Console.WriteLine($"[ERROR] Attempted to get config value for type '{valueType}' with Key '{_key}' in '{_category}'.");

			return default;
		}

		public static string FilePath => $"{Directory.GetCurrentDirectory()}\\Assets\\config.cfg";

		private Dictionary<string, Dictionary<string, Vector2>> vector2s;
		private Dictionary<string, Dictionary<string, Vector3>> vector3s;
		private Dictionary<string, Dictionary<string, Color>> colors;
		private Dictionary<string, Dictionary<string, int>> ints;
		private Dictionary<string, Dictionary<string, float>> floats;
		private Dictionary<string, Dictionary<string, bool>> bools;
		private Dictionary<string, Dictionary<string, string>> strings;

		private Config()
		{
			vector2s = new Dictionary<string, Dictionary<string, Vector2>>();
			vector3s = new Dictionary<string, Dictionary<string, Vector3>>();
			colors = new Dictionary<string, Dictionary<string, Color>>();
			ints = new Dictionary<string, Dictionary<string, int>>();
			floats = new Dictionary<string, Dictionary<string, float>>();
			bools = new Dictionary<string, Dictionary<string, bool>>();
			strings = new Dictionary<string, Dictionary<string, string>>();

			Load();
		}

		private void Load()
		{
			FileInfo file = new FileInfo(FilePath);

			if(file.DirectoryName == null)
				return;

			if(!Directory.Exists(file.DirectoryName))
			{
				Directory.CreateDirectory(file.DirectoryName);
				return;
			}
			
			using(StreamReader reader = new StreamReader(FilePath))
			{
				string? line;
				string currentCategory = "";
				
				while((line = reader.ReadLine()) != null)
				{
					currentCategory = ProcessLine(line, currentCategory);
				}
			}
		}

		private string ProcessLine(string _line, string _category)
		{
			// This is a comment line or an empty line, so ignore it
			if(_line.Contains('#') || _line.Length == 0)
				return _category;
			
			string category = ProcessCategory(_line, _category);

			// If the category was changed, leave function early
			if(_category != category)
				return category;
			
			ProcessValue(_line, _category);
			
			return _category;
		}

		private static string ProcessCategory(string _line, string _category)
		{
			return _line[0] == '[' ? _line.Substring(1, _line.Length - 2) : _category;
		}
		
		private void ProcessValue(string _line, string _category)
		{
			int equalIndex = _line.IndexOf('=');
			
			string varName = _line.Substring(0, equalIndex);
			string val = _line.Substring(equalIndex + 1, _line.Length - equalIndex - 1);
			
			if(val.Contains('.'))
			{
				ProcessDecimalValues(varName, val, _category);
			}
			else
			{
				if(int.TryParse(val, out int iVal))
				{
					ProcessSimpleValue(varName, iVal, _category, ints);
				}
				else if(bool.TryParse(val, out bool bVal))
				{
					ProcessSimpleValue(varName, bVal, _category, bools);
				}
				else
				{
					ProcessSimpleValue(varName, val, _category, strings);
				}
			}
		}

		private void ProcessDecimalValues(string _varName, string _val, string _category)
		{
			string[] split = _val.Split(',');
			if(split.Length == 1)
			{
				ProcessSimpleValue(_varName, float.Parse(split[0]), _category, floats);
			}
			else
			{
				float[] converted = new float[split.Length];
				for(int i = 0; i < converted.Length; i++)
					converted[i] = float.Parse(split[i]);

				if(converted.Length == 2)
				{
					ProcessVector2(_varName, converted, _category);
				}
				else if(converted.Length == 3)
				{
					ProcessVector3(_varName, converted, _category);
				}
				else if(converted.Length == 4)
				{
					ProcessColor(_varName, converted, _category);
				}
			}
		}

		private void ProcessVector2(string _varName, float[] _values, string _category)
		{
			ValidateCategory(_category, vector2s);
			vector2s[_category].Add(_varName, new Vector2(_values[0], _values[1]));
		}

		private void ProcessVector3(string _varName, float[] _values, string _category)
		{
			ValidateCategory(_category, vector3s);
			vector3s[_category].Add(_varName, new Vector3(_values[0], _values[1], _values[2]));
		}

		private void ProcessColor(string _varName, float[] _values, string _category)
		{
			ValidateCategory(_category, colors);
			colors[_category].Add(_varName, new Color((int)_values[0], (int)_values[1], (int)_values[2], (int)_values[3]));
		}

		private static void ProcessSimpleValue<VALUE_TYPE>(string _varName, VALUE_TYPE _value, string _category, Dictionary<string, Dictionary<string, VALUE_TYPE>> _values)
		{
			ValidateCategory(_category, _values);
			
			_values[_category].Add(_varName, _value);
		}

		private static void ValidateCategory<VALUE_TYPE>(string _category, Dictionary<string, Dictionary<string, VALUE_TYPE>> _values)
		{
			if(!_values.ContainsKey(_category))
				_values.Add(_category, new Dictionary<string, VALUE_TYPE>());
		}
	}
}