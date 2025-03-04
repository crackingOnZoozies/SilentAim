using ClickableTransparentOverlay;
using ImGuiNET;
using System;
using System.Numerics;

namespace SilentAim
{
    public class Renderer : Overlay
    {
        public Vector2 screenSize = new Vector2(1920, 1080); // my screen size
        public float FOV = 50; // in pixels

        public bool aimbot = false;
        public bool silent = false;
        public bool aimOnTeam = false;
        public bool aimOnSpotted = true;
        public bool autoShoot = false;
        public bool useFov = false;
        public bool aimOnClosest = false;
        public bool followRecoil = false;
        public bool autoLock = false;

        public bool aimKeySecond = false;
        public int millisecondsDelay = 0;
        public bool triggerAsAutoShoot = true;

        public int aimDelay = 10;

        public Vector4 circleColor = new Vector4(1, 0, 1, 1);

        protected override void Render()
        {
            ImGui.Begin("aimbot");

            ImGui.Checkbox("aimbot in/off", ref aimbot);
            if (aimbot)
            {
                ImGui.Checkbox("aim on closest by diatance", ref aimOnClosest);
                ImGui.Checkbox("silent", ref silent);
                ImGui.DragInt("aim delay", ref aimDelay);
                ImGui.Checkbox("aim on spotted", ref aimOnSpotted);
                ImGui.Checkbox("use mouse 6 for aiming", ref aimKeySecond);
                ImGui.Checkbox("autoLock", ref autoLock);
                ImGui.Checkbox("autoShoot", ref autoShoot);
                if (autoShoot)
                {
                    ImGui.Checkbox("use normal triggerbot", ref triggerAsAutoShoot);
                    ImGui.DragInt("delay", ref millisecondsDelay);
                }

                if (!aimOnClosest) ImGui.Checkbox("fov", ref useFov);

                if (useFov)
                {
                    ImGui.SliderFloat("fov", ref FOV, 10, 300.0f);
                    if (ImGui.CollapsingHeader("Fov circle color"))
                    {
                        ImGui.ColorPicker4("##fovcolor", ref circleColor);
                    }
                }
            }

            ImGui.End();

            if (useFov && aimbot)
            {
                DrawOverlay(screenSize);
            }
        }

        // draw overlay
        void DrawOverlay(Vector2 screenSize)
        {
            ImGui.SetNextWindowSize(screenSize);
            ImGui.SetNextWindowPos(new Vector2(0, 0));
            ImGui.Begin("overlay", ImGuiWindowFlags.NoDecoration
                | ImGuiWindowFlags.NoBackground
                | ImGuiWindowFlags.NoBringToFrontOnFocus
                | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoInputs
                | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoScrollbar
                | ImGuiWindowFlags.NoScrollWithMouse
                );

            ImDrawListPtr drawList = ImGui.GetWindowDrawList();
            drawList.AddCircle(new Vector2(screenSize.X / 2, screenSize.Y / 2), FOV, ImGui.ColorConvertFloat4ToU32(circleColor));

            ImGui.End();
        }
    }
}