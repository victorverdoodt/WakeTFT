using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WakeTFT.D3DXMath;

namespace WakeTFT
{
    class GameInstance
    {
        public UInt64 Address { get; set; }
        public UInt32 ProcessId { get; set; }
        public float Time { get; set; }
        public LolRenderer Renderer { get; set; }

        public Object LocalPlayer { get; set; }
        public class Object
        {
            public UIntPtr ObjectBase { get; set; }
            public float Health { get; set; }
            public float MaxHealth { get; set; }
            public D3DXVECTOR3 Position { get; set; }

            public string Name;


        }
        public class LolRenderer
        {
            public UIntPtr RendererBase { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public D3DXMATRIX viewMatrix { get; set; }
            public D3DXMATRIX projMatrix { get; set; }
        }
    }
}
