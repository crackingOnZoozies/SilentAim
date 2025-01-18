using SilentAim;
using Swed64;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;


Swed swed = new Swed("cs2");
IntPtr client = swed.GetModuleBase("client.dll");

Vector2 screenSize = new Vector2(1920, 1080);

Renderer renderer = new Renderer();
renderer.Start().Wait();

List<Entity> entities = new List<Entity>();
Entity localPlayer = new Entity();

const int hotKeyAimSwitch = 0x06;//mouse 5 0x06;//mouse 6

const int hotKeyLeft = 0x01; // mouse left

const int PlusAttack = 65537;
const int MinusAttack = 256;
const int attackMinus2 = 16777472;



//aimbot loop
while (true)
{
    entities.Clear();

    //get entity list
    IntPtr entityList = swed.ReadPointer(client, Offsets.dwEntityList);

    //firstentry
    IntPtr listEntry = swed.ReadPointer(entityList, 0x10);

    localPlayer.pawnAddress = swed.ReadPointer(client, Offsets.dwLocalPlayerPawn);
    localPlayer.team = swed.ReadInt(localPlayer.pawnAddress, Offsets.m_iTeamNum);
    localPlayer.origin = swed.ReadVec(localPlayer.pawnAddress, Offsets.m_vOldOrigin);
    localPlayer.view = swed.ReadVec(localPlayer.pawnAddress, Offsets.m_vecViewOffset);

    int shotsFired = swed.ReadInt(localPlayer.pawnAddress, Offsets.m_iShotsFired);

    //loop thogh entity list
    for (int i = 0; i < 64; i++)
    {
        //if (GetAsyncKeyState(hotKey)<0) renderer.aimbot = !renderer.aimbot;
        if (!renderer.aimbot) continue;
        if (listEntry == IntPtr.Zero)
        {
            Console.WriteLine(listEntry);
        }

        IntPtr currentController = swed.ReadPointer(listEntry, i * 0x78);

        if (currentController == IntPtr.Zero) continue;

        //get pawn
        int pawnHandle = swed.ReadInt(currentController, Offsets.m_hPlayerPawn);

        if (pawnHandle == 0) continue;

        //second entry and now we get specific pawn
        IntPtr listEntry2 = swed.ReadPointer(entityList, 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);
        IntPtr currentPawn = swed.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FF));

        if (currentPawn == localPlayer.pawnAddress) continue;

        //get scene node
        IntPtr sceneNode = swed.ReadPointer(currentPawn, Offsets.m_pGameSceneNode);

        //get bone array
        IntPtr boneMatrix = swed.ReadPointer(sceneNode, Offsets.m_modelState + 0x80);

        uint lifeState = swed.ReadUInt(currentPawn, Offsets.m_lifeState);

        if (lifeState != 256) continue;

        int health = swed.ReadInt(currentPawn, Offsets.m_iHealth);
        int team = swed.ReadInt(currentPawn, Offsets.m_iTeamNum);
        bool spotted = swed.ReadBool(currentPawn, Offsets.m_entitySpottedState + Offsets.m_bSpotted);

        if (!spotted && renderer.aimOnSpotted) continue;

        if (team == localPlayer.team && !renderer.aimOnTeam)
        {
            continue;
        }


        Entity entity = new Entity();

        //get matrix
        float[] viewMatrix = swed.ReadMatrix(client + Offsets.dwViewMatrix);

        entity.pawnAddress = currentPawn;
        entity.controllerPawn = currentController;

        entity.health = health;
        entity.team = team;
        entity.lifeState = lifeState;

        entity.origin = swed.ReadVec(currentPawn, Offsets.m_vOldOrigin);
        entity.view = swed.ReadVec(currentPawn, Offsets.dwViewAngles);

        entity.distance = Vector3.Distance(entity.origin, localPlayer.origin);
        entity.head = swed.ReadVec(boneMatrix, 6 * 32); // 6 =bone id,  32 = steps between  bones

        //get 2d info
        entity.head2d = Calculate.WordToScreen(viewMatrix, entity.head, screenSize);
        entity.pixelDistance = Vector2.Distance(entity.head2d, new Vector2(screenSize.X / 2, screenSize.Y / 2));

        entities.Add(entity);

        //draw to cmd
        Console.ForegroundColor = team == localPlayer.team ? ConsoleColor.Green : ConsoleColor.Red;

        Console.WriteLine($"{entity.health} hp, head coord: {entity.head}");
        Console.ResetColor();
    }

    localPlayer.scopped =  swed.ReadBool(Offsets.dwLocalPlayerPawn, Offsets.m_bOldIsScoped);

    

    if (!renderer.aimbot) { continue; }
    
    bool swedBool = swed.ReadInt(client + Offsets.dwForceAttack) == PlusAttack;
    if (renderer.aimKeySecond) swedBool = GetAsyncKeyState(hotKeyAimSwitch) < 0;
    if(renderer.autoLock) swedBool = true;
    if (renderer.aimOnClosest)
    {
        entities = entities.OrderBy(o => o.distance).ToList();
        if (entities.Count > 0 && (swedBool))
        {

            float y, x;
            x = swed.ReadFloat(client + Offsets.xAddress);
            y = swed.ReadFloat(client + Offsets.yAddress);

            //get view pos
            Vector3 playerView = Vector3.Add(localPlayer.origin, localPlayer.view);
            Vector3 entityView = Vector3.Add(entities[0].origin, entities[0].view);

            //get angles
            Vector2 newAngles = Calculate.CalculateAngles(playerView, entities[0].head);
            Vector3 newNagles3D = new Vector3(newAngles.Y, newAngles.X, 0.0f);

            swed.WriteVec(client, Offsets.dwViewAngles, newNagles3D);
            Thread.Sleep(renderer.aimDelay);



            if (renderer.silent)
            {
                swed.WriteFloat(client + Offsets.xAddress, x);
                swed.WriteFloat(client + Offsets.yAddress, y);
            }


        }
    }
    else
    {
        entities = entities.OrderBy(o => o.pixelDistance).ToList();
        if (entities.Count > 0 && (swedBool))
        {

            float y, x;
            x = swed.ReadFloat(client + Offsets.xAddress);
            y = swed.ReadFloat(client + Offsets.yAddress);

            //get view pos
            Vector3 playerView = Vector3.Add(localPlayer.origin, localPlayer.view);
            Vector3 entityView = Vector3.Add(entities[0].origin, entities[0].view);

            //get angles
            Vector2 newAngles = Calculate.CalculateAngles(playerView, entities[0].head);
            Vector3 newNagles3D = new Vector3(newAngles.Y, newAngles.X, 0.0f);

            if (entities[0].pixelDistance < renderer.FOV && renderer.useFov)
            {
                swed.WriteVec(client, Offsets.dwViewAngles, newNagles3D);
                Thread.Sleep(renderer.aimDelay);



                if (renderer.silent)
                {
                    swed.WriteFloat(client + Offsets.xAddress, x);
                    swed.WriteFloat(client + Offsets.yAddress, y);
                }

            }
            else if (renderer.FOV > 0 && !renderer.useFov)
            {
                swed.WriteVec(client, Offsets.dwViewAngles, newNagles3D);
                Thread.Sleep(renderer.aimDelay);



                if (renderer.silent)
                {
                    swed.WriteFloat(client + Offsets.xAddress, x);
                    swed.WriteFloat(client + Offsets.yAddress, y);
                }

            }


        }
    }


}
//imports 
[DllImport("user32.dll")]
static extern short GetAsyncKeyState(int vKey);
