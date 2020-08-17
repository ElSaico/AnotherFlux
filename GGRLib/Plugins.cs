using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace GGRLib
{
    public class Plugins
    {
		public List<T> GetPlugins<T>(string folder)
		{
			string[] files = Directory.GetFiles(folder, "*.dll");
			List<T> list = new List<T>();
			foreach (string path in files)
			{
				try
				{
					Assembly assembly = Assembly.LoadFile(path);
					foreach (Type type in assembly.GetTypes())
					{
						if (type.IsClass && !type.IsNotPublic)
						{
							Type[] interfaces = type.GetInterfaces();
							if (((ICollection<Type>)interfaces).Contains(typeof(T)))
							{
								var obj = Activator.CreateInstance(type);
								list.Add((T)obj);
							}
						}
					}
				}
				catch (ReflectionTypeLoadException)
				{
				}
			}
			return list;
		}
	}
}
