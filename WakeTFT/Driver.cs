using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static WakeTFT.server_shared;

namespace WakeTFT
{
    class Driver
    {
        Socket socket = null;
        IPEndPoint remoteEP;
        public Driver()
        {
            remoteEP = new IPEndPoint(IPAddress.Parse(server_ip), (int)server_port);
            socket = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Debug.WriteLine("CreateCliente\n");
        }

        public void connect()
        {
            socket.Connect(remoteEP);
            Debug.WriteLine("Socket Connected\n");
        }

        public void disconect()
        {
            if (socket.Connected)
            {
                socket.Close();
                Debug.WriteLine("Socket disconnected\n");
            }
        }

        T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            T stuff;
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
            return stuff;
        }

        bool send_packet(Packet packet, out UInt64 out_result)
        {
           int structureSize = Marshal.SizeOf(packet);

            // Builds byte array
            byte[] byteArray = new byte[structureSize];

            IntPtr memPtr = IntPtr.Zero;

            try
            {
                // Allocate some unmanaged memory
                memPtr = Marshal.AllocHGlobal(structureSize);

                // Copy struct to unmanaged memory
                Marshal.StructureToPtr(packet, memPtr, true);

                // Copies to byte array
                Marshal.Copy(memPtr, byteArray, 0, structureSize);
            }
            finally
            {
                if (memPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(memPtr);
                }
            }

            Packet completion_packet = new Packet();
            unsafe
            {
                if (socket.Send(byteArray, sizeof(Packet), 0) == -1)
                {
                    out_result = 0;
                    Debug.WriteLine("dont send");
                    return false;
                }

                // Builds byte array
                byte[] buffer = new byte[structureSize];
                var result = socket.Receive(buffer, sizeof(Packet), 0);
                completion_packet = ByteArrayToStructure<Packet>(buffer);

                if (result < sizeof(PacketHeader) || completion_packet.header.magic != packet.header.magic || completion_packet.header.type != PacketType.packet_completed)
                {
                    Debug.WriteLine("error param");
                    out_result = 0;
                    return false;
                }
            }

            out_result = completion_packet.data.completed.result;
            return true;
        }

        UInt32 copy_memory(UInt32 src_process_id, UInt64 src_address, UInt32 dest_process_id, UIntPtr dest_address, UInt32 size)
        {
            Packet packet = new Packet();

            packet.header.magic = server_shared.packet_magic;
            packet.header.type = PacketType.packet_copy_memory;

            packet.data.copy_memory.src_process_id = src_process_id;
            packet.data.copy_memory.src_address = (UInt64)src_address;
            packet.data.copy_memory.dest_process_id = dest_process_id;
            packet.data.copy_memory.dest_address = (UInt64)dest_address;
            packet.data.copy_memory.size = size;

            UInt64 result = 0;
            if (send_packet(packet, out result))
                return (UInt32)result;

            return 0;
        }

        UInt32 read_memory(UInt32 process_id, UInt64 address, UIntPtr buffer, UInt32 size)
        {
            return copy_memory(process_id, address, (UInt32)Process.GetCurrentProcess().Id, buffer, size);
        }

        public T read<T>(UInt32 process_id, UInt64 address) where T : unmanaged
        {
            T buffer = new T();

            unsafe
            {
                T* tPointer = &buffer;

                read_memory(process_id, address, new UIntPtr(&buffer), (UInt32)Marshal.SizeOf(default(T)));
            }
            
            return buffer;
	    }

        public UInt64 get_process_base_address(UInt32 process_id)
        {
            if (socket.Connected)
            {
                Packet packet = new Packet();

                packet.header.magic = packet_magic;
                packet.header.type = PacketType.packet_get_base_address;

                packet.data.get_base_address.process_id = process_id;

                UInt64 result = 0;
                if (send_packet(packet, out result))
                    return result;
            }

            return 0;
        }
    }
}
