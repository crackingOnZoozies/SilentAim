using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SilentAim
{
    public class Entity
    {
        public IntPtr pawnAddress { get; set; }
        public IntPtr controllerPawn { get; set; }
        public Vector3 origin { get; set; }
        public Vector3 view { get; set; }
        public Vector3 head { get; set; }
        public Vector2 head2d { get; set; }
        public int health { get; set; }
        public int team { get; set; }
        public uint lifeState { get; set; }
        public float distance { get; set; }
        public float pixelDistance { get; set; }
        public bool spotted { get; set; }
        public bool scopped { get; set; }

    }
}
