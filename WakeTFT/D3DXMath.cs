using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WakeTFT
{
    public class D3DXMath
    {
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct D3DXMATRIX
        {
            public Single _11;
            public Single _12;
            public Single _13;
            public Single _14;
            public Single _21;
            public Single _22;
            public Single _23;
            public Single _24;
            public Single _31;
            public Single _32;
            public Single _33;
            public Single _34;
            public Single _41;
            public Single _42;
            public Single _43;
            public Single _44;
        }

        [DllImport("d3dx9_43.dll", EntryPoint = "D3DXMatrixMultiply", CallingConvention = CallingConvention.StdCall,
           SetLastError = false)]
        public static extern IntPtr D3DXMatrixMultiply(
            out D3DXMATRIX pOut,
            ref D3DXMATRIX pM1,
            ref D3DXMATRIX pM2
           );

        [StructLayout(LayoutKind.Sequential)]
        public struct D3DXVECTOR4
        {
            public float x;
            public float y;
            public float z;
            public float w;

            static D3DXVECTOR4()
            {

            }

            public D3DXVECTOR4(float x, float y, float z, float w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

        }
        [StructLayout(LayoutKind.Sequential)]
        public struct D3DXVECTOR3
        {
            public float x;
            public float y;
            public float z;

            static D3DXVECTOR3()
            {

            }

            public D3DXVECTOR3(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

        }
        [StructLayout(LayoutKind.Sequential)]
        public struct D3DXVECTOR2
        {
            public float x;
            public float y;

            static D3DXVECTOR2()
            {

            }

            public D3DXVECTOR2(float x, float y)
            {
                this.x = x;
                this.y = y;
            }

        }
    }
}
