using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WakeTFT
{
	public class server_shared
	{
		public const UInt32 packet_magic = 0x12345568;
		public const string server_ip = "127.0.0.1"; // 127.0.0.1
		public const UInt32 server_port = 27015;

		public enum PacketType
		{
			packet_copy_memory,
			packet_get_base_address,
			packet_completed
		}

		public struct PacketCopyMemory
		{
			public UInt32 dest_process_id;
			public UInt64 dest_address;

			public UInt32 src_process_id;
			public UInt64 src_address;

			public UInt32 size;
		};

		public struct PacketGetBaseAddress
		{
			public UInt32 process_id;
		};

		public struct PackedCompleted
		{
			public UInt64 result;
		};

		public struct PacketHeader
		{
			public UInt32 magic;
			public PacketType type;
		};

		[StructLayout(LayoutKind.Explicit)]
		public struct MyUnion
		{
			[FieldOffset(0)]
			public PacketCopyMemory copy_memory;
			[FieldOffset(0)]
			public PacketGetBaseAddress get_base_address;
			[FieldOffset(0)]
			public PackedCompleted completed;
		}
		public struct Packet
		{
			public PacketHeader header;
			public MyUnion data;
		};
	}
}
