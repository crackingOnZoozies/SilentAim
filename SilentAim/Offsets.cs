﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilentAim
{
    public static class Offsets
    {
        //buttons.cs
        public static int dwForceAttack = 0x182F6B0;

        //offsets.cs
        public static int dwViewAngles = 0x1A3DCC0;
        public static int dwLocalPlayerPawn = 0x1836BB8;
        public static int dwEntityList = 0x19D1A98;

        public static int dwViewMatrix = 0x1A33E30; // offset for circle

        //client.dll.cs
        public static int m_hPlayerPawn = 0x80C;
        public static int m_iHealth = 0x344;
        public static int m_vOldOrigin = 0x1324;
        public static int m_iTeamNum = 0x3E3;
        public static int m_vecViewOffset = 0xCB0;
        public static int m_lifeState = 0x348;

        public static int m_modelState = 0x170; // head
        public static int m_pGameSceneNode = 0x328;

        public static int m_entitySpottedState = 0x23B8;
        public static int m_bSpotted = 0x8;

        public static int m_iIDEntIndex = 0x1458;

        // x and y addies
        public static IntPtr yAddress = new IntPtr(0x1A3DCC0);
        public static IntPtr xAddress = new IntPtr(0x1A3DCC0 + 0x4);

    }
}