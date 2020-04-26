using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakeTFT
{
    class GameInstance
    {
        public UInt64 Address { get; set; }
        public UInt32 ProcessId { get; set; }
        public float Time { get; set; }
        public UIntPtr Renderer { get; set; }
    }
}
