using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakeTFT
{
    class GameOffsets
    {
        public enum GameObject
        {
            oObjChampionName = 0x3578, //Pattern: 8B 87 ? ? ? ? 89 41 ? 8B CF
            oObjType = 0x18,
            oObjIndex = 0x20,
            oObjTeam = 0x58, //Pattern: FF 74 24 ? E8 ? ? ? ? 8B 4C 24 ? 83 C4 ? 5D
            oObjTFTTeam = 0x384, //81 45 ? ? ? ? ? 5D
            oObjName = 0x6C, //Pattern: 8D 43 ? 8B 48 ? 72 ? 8B 00 51 50 8D 4C 24 ?
            oObjPos = 0x1D8, //Pattern: F3 0F 5C 81 ? ? ? ? 8B 44 24 ? Pattern: 8B 80 ? ? ? ? FF D0 84 C0 75 ? 33 C0 5E C3
            oObjHealth = 0xF88,
            oObjMaxHealth = 0xF98, //oObjHealth +10
            oObjLevel = 0x4964,
            oObjGold = 0x1CBC //mGold
        };

        public enum GamePointer
        {
            oObjManager = 0x1BFF8FC, //Pattern: B9 ? ? ? ? E8 ? ? ? ? A1 ? ? ? ? 8A 80 ? ? ? ? //8B 0D ? ? ? ? E8 ? ? ? ? FF 77
            oLocalPlayer = 0x34A1908, //Reconnect being processed or Pattern: 56 8B 74 24 ? 57 BF ? ? ? ? 85 F6 74 ? 3B 35 ? ? ? ?
            oGameTime = 0x349A5CC,  //Pattern: F3 0F 5C 0D ? ? ? ? 0F 2F C1 F3 0F 11 4C 24 ?
            oGameInfo = 0x349BC18,  //A1 ? ? ? ? 83 78 08 02 0F 94 C0
            oGameVersion = 0x5390A0,    //8B 44 24 04 BA ? ? ? ? 2B D0
            oD3DRenderer = 0x34C1B34,   //A1 ? ? ? ? 89 54 24 18
            oZoomClass = 0x3499F84, //A3 ? ? ? ? 83 FA 10 72 32
            oNetClient = 0x34A2670, //8B 0D ? ? ? ? 85 C9 74 07 8B 01 6A 01 FF 50 08 8B
            oUnderMouseObject = 0x348F0B4,  //8B 0D ? ? ? ? 89 0D ? ? ? ? 3B 44 24 30
            oGameShop = 0x2FA99EC,
            oHudInstance = 0x1BFF934,   //8B 0D ? ? ? ? 6A 00 8B 49 34 E8 ? ? ? ? B0
            oRenderer = 0x34BF550 //Pattern: 8B 0D ? ? ? ? 8B 81 ? ? ? ? 66 0F 6E C0 F3 0F E6 C0 C1 E8 ? F2 0F 58 04 C5 ? ? ? ? 8B 81 ? ? ? ?
        };

        public enum RenderOffsets
        {
            oScreenWidth = 0x14,
            oScreenHeight = 0x18,
            oViewMatrix = 0x6C,
            oProjection = 0xAC
        };
    }
}
