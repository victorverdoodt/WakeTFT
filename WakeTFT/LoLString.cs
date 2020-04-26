using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WakeTFT
{
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct StringUnion
	{
		[FieldOffset(0)]
		public char* strPtr; // 0x0
		[FieldOffset(0)]
		public fixed char strVal[16]; // 0x0
	}
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct BigStringUnion
	{
		[FieldOffset(0)]
		public char* strPtr; // 0x0
		[FieldOffset(0)]
		public fixed char strVal[255]; // 0x0
	}

	public unsafe struct LoLBigString
	{
		public BigStringUnion data;
		public char* c_str()
		{
			fixed (char* charPtr = data.strVal)
			{
				return charPtr;
			}
		}
	}

	public unsafe struct LoLString
	{
		public StringUnion data;


		public unsafe char* c_str(int max)
		{
			if(max > 15)
			{
				var buffer = Engine.driver.read<LoLBigString>(Engine.Game.ProcessId, (UInt64)(data.strPtr));
				return buffer.data.strVal;
			}
			fixed (char* charPtr = data.strVal)
			{
				return charPtr;
			}
		}
	}
}
