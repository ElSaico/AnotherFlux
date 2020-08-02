using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace GGRLib
{
	[Serializable]
	public class TreeNodeExt : ISerializable
	{
		public bool bSelectable = true;

		public Color DefForeColor = Color.FromKnownColor(KnownColor.WindowText);

		public byte[] nData;

		public TreeNodeExt() { }

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public void GetObjectData(SerializationInfo si, StreamingContext context) { }
	}

}
