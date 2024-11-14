using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentAim
{
    public static class Offsets
    {
        //buttons.cs
        public static int dwForceAttack = 0x184E4D0;

        //offsets.cs
        public static int dwViewAngles = 0x1A5E650;
        public static int dwLocalPlayerPawn = 0x1855CE8;
        public static int dwEntityList = 0x19F2488;

        public static int dwViewMatrix = 0x1A54550; // offset for circle

        //client.dll.cs
        public static int m_hPlayerPawn = 0x80C;
        public static int m_iHealth = 0x344;
        public static int m_vOldOrigin = 0x1324;
        public static int m_iTeamNum = 0x3E3;
        public static int m_vecViewOffset = 0xCB0;
        public static int m_lifeState = 0x348;

        public static int m_modelState = 0x170; // head
        public static int m_pGameSceneNode = 0x328;

        public static int m_entitySpottedState = 0x23D0;
        public static int m_bSpotted = 0x8;

        public static int m_iIDEntIndex = 0x1458;

        public static int m_bOldIsScoped = 0x242C; // bool

        // x and y addies
        public static IntPtr yAddress = dwViewAngles;
        public static IntPtr xAddress = dwViewAngles + 0x4;

    }
}
