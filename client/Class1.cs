using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Newtonsoft.Json.Linq;

using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

using static fivem.FuncHelper;

namespace fivem_military_heist
{
    public class Class1 : BaseScript
    {
        dynamic ESX;
        public Class1()
        {
            TriggerEvent("esx:getSharedObject", new object[] { new Action<dynamic>(esx => {
                ESX = esx;
            })});

            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            EventHandlers["military_heist:loadcfg"] += new Action<string, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>, List<dynamic>>(LoadConfigTrigger);

            EventHandlers["military_heist:contact:start"] += new Action<string>(ContactStart);

            EventHandlers["military_heist:start"] += new Action<string, bool>(HeistStart);
            EventHandlers["military_heist:haveitem:hackingtool"] += new Action<bool>(HaveHackingtool);
            EventHandlers["military_heist:haveitem:c4"] += new Action<bool>(HaveC4);

            EventHandlers["military_heist:c4callback"] += new Action<string>(C4);
            EventHandlers["military_heist:alarmcallback"] += new Action<string>(Alarm);

            EventHandlers["military_heist:openentry"] += new Action<string>(OpenEntry);

            EventHandlers["military_heist:server1callback"] += new Action<string>(Server1);
            EventHandlers["military_heist:server2callback"] += new Action<string>(Server2);
            EventHandlers["military_heist:server3callback"] += new Action<string>(Server3);
        }
        float[] RandomSpawnPoint1 = new float[4];
        float[] RandomSpawnPoint2 = new float[4];
        float[] RandomSpawnPoint3 = new float[4];
        float[] RandomSpawnPoint4 = new float[4];

        float[] PowderLocation = new float[3];
        float[] Makec4Location = new float[3];

        float[] HackingtoolLocation = new float[3];
        float[] HackLocation = new float[3];

        bool restartUsed = false;
        private void OnClientResourceStart(string resourceName)
        {
            if (API.GetCurrentResourceName() != resourceName) return;

            TriggerServerEvent("military_heist:loadcfg:server", "load");

			//Debug commands
            /*API.RegisterCommand("locationnow", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                var pos = LocalPlayer.Character.Position;
                ChatMessage($"X: {pos.X} Y: {pos.Y} Z: {pos.Z} H: {LocalPlayer.Character.Heading}");
                await Delay(5000);
                ChatMessage(".");
                await Delay(5000);
                ChatMessage(".");
                await Delay(5000);
                ChatMessage(".");
            }), false);
            API.RegisterCommand("tpm", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                Vector3 newpos = World.WaypointPosition; newpos.Z = newpos.Z + 2;
                LocalPlayer.Character.Position = newpos;
            }), false);*/
            API.RegisterCommand("restartheistgps", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                if(!restartUsed) TriggerServerEvent("military_heist:haveitems:server", LocalPlayer.ServerId);
                restartUsed = true;
            }), false);

            Tick -= BlockDoors;
            Tick += LeaveMarker;
        }

        private void LoadConfigTrigger(string action, List<dynamic> sp1, List<dynamic> sp2, List<dynamic> sp3, List<dynamic> sp4, List<dynamic> powder, List<dynamic> c4, List<dynamic> hacking, List<dynamic> hack)
        {
            if (action == "client")
            {
                RandomSpawnPoint1[0] = sp1[0]; RandomSpawnPoint1[1] = sp1[1]; RandomSpawnPoint1[2] = sp1[2]; RandomSpawnPoint1[3] = sp1[3];
                RandomSpawnPoint2[0] = sp2[0]; RandomSpawnPoint2[1] = sp2[1]; RandomSpawnPoint2[2] = sp2[2]; RandomSpawnPoint2[3] = sp2[3];
                RandomSpawnPoint3[0] = sp3[0]; RandomSpawnPoint3[1] = sp3[1]; RandomSpawnPoint3[2] = sp3[2]; RandomSpawnPoint3[3] = sp3[3];
                RandomSpawnPoint4[0] = sp4[0]; RandomSpawnPoint4[1] = sp4[1]; RandomSpawnPoint4[2] = sp4[2]; RandomSpawnPoint4[3] = sp4[3];

                PowderLocation[0] = powder[0]; PowderLocation[1] = powder[1]; PowderLocation[2] = powder[2];
                Makec4Location[0] = c4[0]; Makec4Location[1] = c4[1]; Makec4Location[2] = c4[2];

                HackingtoolLocation[0] = hacking[0]; HackingtoolLocation[1] = hacking[1]; HackingtoolLocation[2] = hacking[2];
                HackLocation[0] = hack[0]; HackLocation[1] = hack[1]; HackLocation[2] = hack[2];
            }
            else
            {
                TriggerServerEvent("military_heist:loadcfg:server");
            }
        }

        bool contactstarted = false;
        Ped contactPed = null; Blip contactBlip = null;
        private async void ContactStart(string action)
        {
            if (contactstarted == true) return;
            contactstarted = true;
            int rng = new Random().Next(1, 4);
            Vector3 pos = new Vector3();
            float heading = 0;
            if (rng == 1) { pos = new Vector3(RandomSpawnPoint1[0], RandomSpawnPoint1[1], RandomSpawnPoint1[2]-1); heading = RandomSpawnPoint1[3]; }
            if (rng == 2) { pos = new Vector3(RandomSpawnPoint2[0], RandomSpawnPoint2[1], RandomSpawnPoint2[2]-1); heading = RandomSpawnPoint2[3]; }
            if (rng == 3) { pos = new Vector3(RandomSpawnPoint3[0], RandomSpawnPoint3[1], RandomSpawnPoint3[2]-1); heading = RandomSpawnPoint3[3]; }
            if (rng == 4) { pos = new Vector3(RandomSpawnPoint4[0], RandomSpawnPoint4[1], RandomSpawnPoint4[2]-1); heading = RandomSpawnPoint4[3]; }
            await World.CreatePed(new Model(PedHash.ChemSec01SMM), pos, heading);
            var peds = World.GetAllPeds();
            foreach (var ped in peds)
            {
                if (ped.Position.DistanceToSquared(pos) < 5f && ped.Model == new Model(PedHash.ChemSec01SMM)) contactPed = ped;
            }
            contactPed.IsPersistent = true;
            contactPed.BlockPermanentEvents = true;
            Function.Call(Hash.SET_ENTITY_INVINCIBLE, contactPed.Handle, true);
            contactPed.IsInvincible = true;
            await Delay(3000);
            //contactPed.IsPositionFrozen = true;
            contactBlip = CreateBlipWithExisting(pos, BlipSprite.Deathmatch, BlipColor.Red, "!Kontakt-napad");
            ShowNotification("~r~Widzę że chcesz dorobić huh ? Przesyłam ~b~gps~r~, nie każ na siebie czekać!");
            Tick += Talking;
            await Delay(2500);
            contactPed.IsPositionFrozen = true;

        }
        private void ShowNotification(string text, bool manual = false)
        {
            Function.Call(Hash.BEGIN_TEXT_COMMAND_THEFEED_POST, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            if(!manual) Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_MESSAGETEXT, "CHAR_GANGAPP", "CHAR_GANGAPP", false, 2, "Kontakt", "");
            Function.Call(Hash.END_TEXT_COMMAND_THEFEED_POST_TICKER, false, true);
        }

        private async Task Talking()
        {
            if (LocalPlayer.Character.Position.DistanceToSquared2D(contactPed.Position) < 2f)
            {
                ShowHelp(HelpType.onestring, "Naciśnij ~INPUT_PICKUP~ aby wybrać styl napadu");
                if(API.IsControlJustReleased(1, 51))
                {
                    TriggerEvent("military_heist:showui:lua", LocalPlayer.ServerId, "select");
                }
            }
        }

        Vector3 powderVector = new Vector3(); Vector3 makec4Vector = new Vector3();
        Blip powderBlip = null; Blip makec4Blip = null;

        Vector3 hackingtoolVector = new Vector3(); Vector3 hackVector = new Vector3();
        Blip hackingtoolBlip = null; Blip hackBlip = null;

        Blip baseEntryBlip = null;

        string heist_type = "";
        private async void HeistStart(string type, bool fix)
        {
            if (contactPed != null)
            {
                Tick -= Talking;
                contactBlip.Delete();
                contactPed.IsPositionFrozen = false;
                contactPed.Task.FleeFrom(LocalPlayer.Character, -1);
            }
            baseEntryBlip = CreateBlipWithExisting(enterBaseVector, BlipSprite.ArmoredTruck, BlipColor.Red, "!Napad - baza");

            if (type == "loud")
            {
                heist_type = type;
                if (!fix) { TriggerServerEvent("military_heist:giveitem:server", LocalPlayer.ServerId, "fajnyheist_enter_loud", 1, "fajnyheist_contact", 1);}
                powderVector = new Vector3(PowderLocation[0], PowderLocation[1], PowderLocation[2]-1);
                makec4Vector = new Vector3(Makec4Location[0], Makec4Location[1], Makec4Location[2]-1);
                powderBlip = CreateBlipWithExisting(powderVector, BlipSprite.Bar, BlipColor.Red, "!Napad - proch");
                makec4Blip = CreateBlipWithExisting(makec4Vector, BlipSprite.Grenade, BlipColor.Red, "!Napad - zrób c4");
                Tick += RenderMarkers;
                Tick += WorkingMarkers;
                await Delay(2000);
                ShowNotification("~r~Przesyłam ci ważne miejsca na gps'a !");
                TriggerServerEvent("military_heist_haveitem:c4:server", LocalPlayer.ServerId);
            }
            else if (type == "silent")
            {
                heist_type = type;
                //ChatMessage("cichy napad");
                if (!fix) TriggerServerEvent("military_heist:giveitem:server", LocalPlayer.ServerId, "fajnyheist_enter_silent", 1, "fajnyheist_contact", 1);
                hackingtoolVector = new Vector3(HackingtoolLocation[0], HackingtoolLocation[1], HackingtoolLocation[2] - 1);
                hackVector = new Vector3(HackLocation[0], HackLocation[1], HackLocation[2] - 1);
                hackingtoolBlip = CreateBlipWithExisting(hackingtoolVector, BlipSprite.Package, BlipColor.Blue, "!Napad - urządzenie hakierskie");
                hackBlip = CreateBlipWithExisting(hackVector, BlipSprite.Camera, BlipColor.Blue, "!Napad - alarm");
                Tick += RenderMarkers;
                Tick += WorkingMarkers;
                await Delay(2000);
                ShowNotification("~r~Przesyłam ci ważne miejsca na gps'a !");
                TriggerServerEvent("military_heist_haveitem:hackingtool:server", LocalPlayer.ServerId);
            }
        }

        Vector3 enterBaseVector = new Vector3(-354.205f, 4825.323f, 144.295f-1);
        Vector3 leaveBaseVector = new Vector3(219.038f, 6118.905f, -159.4223f-1);

        Vector3 serverDetonateVector = new Vector3(269.102f, 6189.02f, -154.420f-1);
        Vector3 panelVector = new Vector3(261.736f, 6163.906f, -146.422f-1);

        Vector3 serverHack1Vector = new Vector3(220.356f, 6136.174f, -154.420f-1);
        Vector3 serverHack2Vector = new Vector3(268.193f, 6138.702f, -154.420f-1);
        Vector3 serverHack3Vector = new Vector3(220.320f, 6189.162f, -154.420f-1);
        private async Task RenderMarkers()
        {
            if (heist_type == "loud")
            {
                World.DrawMarker(MarkerType.VerticalCylinder, powderVector, powderVector, new Vector3(1, 1, 1), new Vector3(1, 1, 1), System.Drawing.Color.FromArgb(150, 255, 0, 0));
                World.DrawMarker(MarkerType.VerticalCylinder, makec4Vector, makec4Vector, new Vector3(1, 1, 1), new Vector3(1, 1, 1), System.Drawing.Color.FromArgb(125, 61, 0, 0));
                World.DrawMarker(MarkerType.VerticalCylinder, serverDetonateVector, serverDetonateVector, new Vector3(1, 1, 1), new Vector3(1, 1, 1), System.Drawing.Color.FromArgb(125, 255, 0, 0));
            }
            else if (heist_type == "silent")
            {
                if (LocalPlayer.Character.Position.DistanceToSquared(hackingtoolVector) < 500f)
                    World.DrawMarker(MarkerType.VerticalCylinder, hackingtoolVector, hackingtoolVector, new Vector3(1, 1, 1), new Vector3(1, 1, 1), System.Drawing.Color.FromArgb(125, 38, 121, 255));
                if (LocalPlayer.Character.Position.DistanceToSquared(hackVector) < 500f)
                    World.DrawMarker(MarkerType.VerticalCylinder, hackVector, hackVector, new Vector3(1, 1, 1), new Vector3(1, 1, 1), System.Drawing.Color.FromArgb(125, 38, 121, 255));
                
                World.DrawMarker(MarkerType.VerticalCylinder, serverHack1Vector, serverHack1Vector, new Vector3(1, 1, 1), new Vector3(1, 1, 1), System.Drawing.Color.FromArgb(125, 38, 121, 245));
                World.DrawMarker(MarkerType.VerticalCylinder, serverHack2Vector, serverHack2Vector, new Vector3(1, 1, 1), new Vector3(1, 1, 1), System.Drawing.Color.FromArgb(126, 37, 122, 235));
                World.DrawMarker(MarkerType.VerticalCylinder, serverHack3Vector, serverHack3Vector, new Vector3(1, 1, 1), new Vector3(1, 1, 1), System.Drawing.Color.FromArgb(127, 39, 123, 230));
                
            }

            World.DrawMarker(MarkerType.VerticalCylinder, enterBaseVector, enterBaseVector, new Vector3(1, 1, 1), new Vector3(1, 1, 1), System.Drawing.Color.FromArgb(125, 255, 0, 0));
            World.DrawMarker(MarkerType.VerticalCylinder, panelVector, panelVector, new Vector3(1, 1, 1), new Vector3(1, 1, 1), System.Drawing.Color.FromArgb(125, 255, 0, 0));
        }

        bool havePowder = false; bool haveC4 = false;
        bool haveHackingtool = false; bool hacked = false;
        bool insideBase = false;
        bool serverDetonated = false; bool serversHacked = false;
        bool robbed = false;
        int robbingTime = 300;
        private void HaveHackingtool(bool have)
        {
            haveHackingtool = have;
        }
        private void HaveC4(bool have)
        {
            haveC4 = have;
        }
        bool info = false;
        private async Task WorkingMarkers()
        {
            if (heist_type == "loud")
            {
                if (LocalPlayer.Character.Position.DistanceToSquared(powderVector) < 1.3f)
                {
                    if (!havePowder)
                    {
                        ShowHelp(HelpType.onestring, "Naciśnij ~INPUT_PICKUP~ aby zebrać proch");
                        if (API.IsControlJustReleased(1, 51))
                        {
                            havePowder = true;
                            TriggerServerEvent("military_heist:giveitem:server", LocalPlayer.ServerId, "fajnyheist_dustpowder", 1, "", 0, false);
                        }
                    }
                }

                if (LocalPlayer.Character.Position.DistanceToSquared2D(makec4Vector) < 0.5f)
                {
                    if (!haveC4)
                    {
                        ShowHelp(HelpType.onestring, "Naciśnij ~INPUT_PICKUP~ aby zrobić c4");
                        if (API.IsControlJustReleased(1, 51))
                        {
                            haveC4 = true;
                            TriggerEvent("military_heist:showui:lua", LocalPlayer.ServerId, "c4");
                            await Delay(1000);
                            haveC4 = false;
                        }
                    }
                }

                if(LocalPlayer.Character.Position.DistanceToSquared(enterBaseVector) < 500f && !info)
                {
                    ShowNotification("~r~Upewnij się że wchodząc do bazy masz wszystko co potrzebne (~y~C4)");
                    info = true;
                }
            }
            else if (heist_type == "silent")
            {
                if (LocalPlayer.Character.Position.DistanceToSquared2D(hackingtoolVector) < 0.5f)
                {
                    if (!haveHackingtool)
                    {
                        ShowHelp(HelpType.onestring, "Naciśnij ~INPUT_PICKUP~ aby podnieść urządzenie hakerskie");
                        if (API.IsControlJustReleased(1, 51))
                        {
                            haveHackingtool = true;
                            TriggerServerEvent("military_heist:giveitem:server", LocalPlayer.ServerId, "fajnyheist_hackingtool", 1, "", 0, true);
                        }
                    }
                }
                if (LocalPlayer.Character.Position.DistanceToSquared2D(hackVector) < 0.7f)
                {
                    if (!hacked && haveHackingtool)
                    {
                        ShowHelp(HelpType.onestring, "Naciśnij ~INPUT_PICKUP~ aby zacząć hakować");
                        if (API.IsControlJustReleased(1, 51))
                        {
                            hacked = true;
                            TriggerEvent("military_heist:showui:lua", LocalPlayer.ServerId, "alarmHack");
                        }
                    }
                }

                if (LocalPlayer.Character.Position.DistanceToSquared(enterBaseVector) < 500f && !info)
                {
                    ShowNotification("~r~Upewnij się że wchodząc do bazy zhakowałeś alarm i masz urządzenie hakerskie");
                    info = true;
                }
            }

            if (LocalPlayer.Character.Position.DistanceToSquared(enterBaseVector) < 1.2f && !insideBase)
            {
                ShowHelp(HelpType.onestring, "Naciśnij ~INPUT_PICKUP~ aby wejść do bazy (~y~wejście zostanie otwarte na 60 sekund~s~)");
                if(API.IsControlJustReleased(1, 51))
                {
                    LocalPlayer.Character.Position = leaveBaseVector;
                    if (heist_type == "loud")
                    {
                        TriggerServerEvent("military_heist:removeitem:server", LocalPlayer.ServerId, "fajnyheist_enter_loud", 1);
                        TriggerEvent("military_heist:policealarm", LocalPlayer.ServerId);
                    }
                    if (heist_type == "silent")
                    {
                        TriggerServerEvent("military_heist:removeitem:server", LocalPlayer.ServerId, "fajnyheist_enter_silent", 1);
                        if (!hacked)
                        {
                            RemoveBlipsAndMarkers(Blips.silent);
                            TriggerEvent("military_heist:policealarm", LocalPlayer.ServerId);
                            TriggerServerEvent("military_heist:removeitem:server", LocalPlayer.ServerId, "fajnyheist_hackingtool", 1);
                            ShowNotification("~r~Nie wyłączyłeś alarmu... tragedia..");
                        }
                    }

                    /*Vector3 center = new Vector3(244f, 6164f, -159f);
                    var props = World.GetAllProps();
                    foreach (var prop in props)
                    {
                        if(prop.Position.DistanceToSquared(center) < 250f)
                        {
                            prop.IsPositionFrozen = true;
                        }
                    }*/
                    await Delay(3600);
                    Tick += BlockDoors;
                    if (!hacked) return;

                    insideBase = true;
                    if (/*heist_type != "" && */heist_type == "loud")
                    {
                        TriggerServerEvent("military_heist:openentry:server");
                        ShowNotification("~r~Uważajcie tam w środku, mam informację że ktoś tam może być !");
                        Vector3 enemyVector = new Vector3(250.545f, 6177.714f, -161.0222f);
                        int rng = new Random().Next(3, 6);
                        for (int i = 0; i < rng; i++)
                        {
                            await World.CreatePed(new Model(0x63858A4A), enemyVector);
                            await Delay(100);
                        }
                        /*rng = new Random().Next(1, 2);
                        for (int i = 0; i < rng; i++)
                        {
                            await World.CreatePed(new Model(0x63858A4A), enemyVector);
                            await Delay(100);
                        }*/
                        var peds = World.GetAllPeds();
                        for (int i = 0; i < peds.Length; i++)
                        {
                            if(peds[i].Position.DistanceToSquared2D(enemyVector) < 15f)
                            {
                                peds[i].Weapons.Give(WeaponHash.Pistol, 60, true, true);
                                if (i == 2) peds[i].Weapons.Give(WeaponHash.AssaultRifle, 120, true, true);
                                RelationshipGroup guardsGroup = new RelationshipGroup(0x90C7DA60);
                                peds[i].RelationshipGroup = guardsGroup;
                                LocalPlayer.Character.RelationshipGroup.SetRelationshipBetweenGroups(guardsGroup, Relationship.Hate);
                                peds[i].Task.FightAgainstHatedTargets(200f);
                                peds[i].Task.FightAgainst(LocalPlayer.Character);
                                //Blip enemy = CreateBlipWithExisting(enemyVector, BlipSprite.Enemy, BlipColor.Red, "Ochroniarz");
                            }
                        }
                        await Delay(20000);
                        ShowNotification("~r~Jak już ich rozjebiecie to poszukaj ~b~serwera~r~ na środkowym piętrze i go wysadź !");
                    }
                    else if (heist_type == "silent")
                    {
                        ShowNotification("~r~Poszukaj 3 ~b~serwery~r~ i je zhakuj !");
                    }
                }
            }

            if (insideBase && heist_type == "loud")
            {
                if(LocalPlayer.Character.Position.DistanceToSquared(serverDetonateVector) < 1.2f)
                {
                    if (!haveC4)
                    {
                        ShowNotification("~r~Kurwa co ja Ci mówiłem kretynie, gdzie masz to jebane c4 ? aktywowałeś alarm chuj wie po co");
                        Tick -= RenderMarkers;
                        Tick -= WorkingMarkers;
                        RemoveBlipsAndMarkers(Blips.loud);
                    }
                    else { ShowHelp(HelpType.onestring, "Naciśnij ~INPUT_PICKUP~ aby wysadzić serwer !"); }
                    if(API.IsControlJustPressed(1, 51) && haveC4)
                    {
                        /*Function.Call(Hash.TASK_PLAY_ANIM, LocalPlayer.Character.Handle, "weapons@projectile@sticky_bomb", "plant_vertical", 8f, 8f, 1500, 0, 0, 0, 0, 0);
                        Function.Call(Hash.TASK_PLAY_ANIM, LocalPlayer.Character.Handle, "misstrevor2ig_7", "plant_bomb", 8f, 8f, 4000, 0, 0, 0, 0, 0);*/
                        LocalPlayer.Character.Task.PlayAnimation("misstrevor2ig_7", "plant_bomb", 8f, 4000, AnimationFlags.None);
                        await Delay(4000);
                        TriggerServerEvent("military_heist:removeitem:server", LocalPlayer.ServerId, "fajnyheist_c4", 1);
                        haveC4 = false;
                        Screen.ShowSubtitle("Bomba wybuchnie za 10 sekund !", 5000);
                        await Delay(10000);
                        Function.Call(Hash.ADD_EXPLOSION, serverDetonateVector[0], serverDetonateVector[1], serverDetonateVector[2], 2, 1f, true, false, 4f);
                        serverDetonated = true;
                        ShowNotification("~r~Wysadziłeś serwer ? Świetnie, na najwyższym piętrze są 4 panele, znajdź odpowiedni !");
                    }
                }
                if(LocalPlayer.Character.Position.DistanceToSquared(panelVector) < 1.2f && serverDetonated && !robbed)
                {
                    ShowHelp(HelpType.onestring, "Naciśnij ~INPUT_PICKUP~ aby zacząć rabować !");
                    if(API.IsControlJustPressed(1, 51))
                    {
                        robbed = true;
                        Tick += Robbing;
                        //Function.Call(Hash.TASK_PLAY_ANIM, LocalPlayer.Character.Handle, "random@shop_robbery", "robbery_intro_loop_bag", 8f, 8f, 300000, 0, 0, 0, 0, 0);
                        LocalPlayer.Character.Task.PlayAnimation("random@atmrobberygen@male", "idle_a", 8f, 350000, AnimationFlags.Loop);
                    }
                }
            }
            else if (insideBase && heist_type == "silent")
            {
                if (!haveHackingtool)
                {
                    ShowNotification("~r~Kurwa co ja Ci mówiłem kretynie, gdzie masz to jebane urządzenie hakerskie ?");
                    RemoveBlipsAndMarkers(Blips.silent);
                }
                if(LocalPlayer.Character.Position.DistanceToSquared(serverHack1Vector) < 1.5f && !server1hacked)
                {
                    ShowHelp(HelpType.onestring, "Naciśnij ~INPUT_PICKUP~ aby zhakować serwer (1 z 3) !");
                    if(API.IsControlJustPressed(1, 51))
                    {
                        LocalPlayer.Character.Task.PlayAnimation("missheist_jewel@hacking", "hack_loop", 8f, 5000, AnimationFlags.Loop);
                        await Delay(4500);
                        TriggerEvent("military_heist:showui:lua", LocalPlayer.ServerId, "hackServer1");
                    }
                }
                else if (LocalPlayer.Character.Position.DistanceToSquared(serverHack2Vector) < 1.5f && !server2hacked)
                {
                    ShowHelp(HelpType.onestring, "Naciśnij ~INPUT_PICKUP~ aby zhakować serwer (1 z 3) !");
                    if (API.IsControlJustPressed(1, 51))
                    {
                        LocalPlayer.Character.Task.PlayAnimation("missheist_jewel@hacking", "hack_loop", 8f, 5000, AnimationFlags.Loop);
                        ShowHelp(HelpType.onestring, "Powstrzymaj pakiety przed wysłaniem niszcząc je klikając ~INPUT_ATTACK~");
                        await Delay(5500);
                        TriggerEvent("military_heist:showui:lua", LocalPlayer.ServerId, "hackServer2");
                    }
                }
                else if (LocalPlayer.Character.Position.DistanceToSquared(serverHack3Vector) < 1.5f && !server3hacked)
                {
                    ShowHelp(HelpType.onestring, "Naciśnij ~INPUT_PICKUP~ aby zhakować serwer (1 z 3) !");
                    if (API.IsControlJustPressed(1, 51))
                    {
                        LocalPlayer.Character.Task.PlayAnimation("missheist_jewel@hacking", "hack_loop", 8f, 5000, AnimationFlags.Loop);
                        ShowHelp(HelpType.onestring, "Utrzymuj cursor w punkcie aby zhakować serwer !");
                        await Delay(5500);
                        TriggerEvent("military_heist:showui:lua", LocalPlayer.ServerId, "hackServer3");
                    }
                }

                if (LocalPlayer.Character.Position.DistanceToSquared(panelVector) < 1.2f && serversHacked && !robbed)
                {
                    ShowHelp(HelpType.onestring, "Naciśnij ~INPUT_PICKUP~ aby zacząć rabować !");
                    if (API.IsControlJustPressed(1, 51))
                    {
                        TriggerEvent("military_heist:policealarm", LocalPlayer.ServerId);
                        robbed = true;
                        Tick += Robbing;
                        //Function.Call(Hash.TASK_PLAY_ANIM, LocalPlayer.Character.Handle, "random@shop_robbery", "robbery_intro_loop_bag", 8f, 8f, 300000, 0, 0, 0, 0, 0);
                        LocalPlayer.Character.Task.PlayAnimation("random@atmrobberygen@male", "idle_a", 8f, 350000, AnimationFlags.Loop);
                    }
                }
            }
        }
        private async Task Robbing()
        {
            if(robbingTime == 0)
            {
                Tick -= Robbing;
                if (heist_type == "loud") RemoveBlipsAndMarkers(Blips.loud);
                if (heist_type == "silent") { RemoveBlipsAndMarkers(Blips.silent); TriggerServerEvent("military_heist:removeitem:server", LocalPlayer.ServerId, "fajnyheist_hackingtool", 1); }
                /*makec4Blip.Delete();
                powderBlip.Delete();
                baseEntryBlip.Delete();*/
                //TriggerServerEvent("military_heist:removeitem:server", LocalPlayer.ServerId, "fajnyheist_enter_loud", 1);
            }
            else if (robbingTime == 1)
            {
                TriggerServerEvent("military_heist_getloot:server", LocalPlayer.ServerId, heist_type);
                ShowNotification("~y~[INFO] Dodawanie łupu do eq... poczekaj 20s", true);
            }
            else if (LocalPlayer.Character.Position.DistanceToSquared(panelVector) > 1.5f)
            {
                Tick -= Robbing;
                ShowHelp(HelpType.onestring, "Przestałeś rabować !");
            }
            Screen.ShowSubtitle($"Pozostało : ~p~{robbingTime} ~s~sekund");
            robbingTime--;
            await Delay(1000);
        }

        private void C4(string action)
        {
            if(action == "good")
            {
                TriggerServerEvent("military_heist:giveitem:server", LocalPlayer.ServerId, "fajnyheist_c4", 1, "fajnyheist_dustpowder", 1, true);
                ShowNotification("~r~Masz już wszystko co potrzebne, możesz rozjebać tą baze !");
                haveC4 = true;
            }
            else if (action == "boom")
            {
                var pos = LocalPlayer.Character.Position;
                Function.Call(Hash.ADD_EXPLOSION, pos[0], pos[1], pos[2], 34, 10f, true, false, 5f);
                TriggerServerEvent("military_heist:removeitem:server", LocalPlayer.ServerId, "fajnyheist_enter_loud", 1);
                TriggerServerEvent("military_heist:removeitem:server", LocalPlayer.ServerId, "fajnyheist_dustpowder", 1);
                ShowNotification("~r~Zjebałeś totalnie... maybe next time");
            }
        }

        private void Alarm(string action)
        {
            //TriggerServerEvent("military_heist:removeitem:server", LocalPlayer.ServerId, "fajnyheist_hackingtool", 1);
            if (action == "good")
            {
                ShowNotification("~r~Brawo że udało Ci się to wyłączyć, teraz możesz udać się prosto do ~g~bazy~r~ !");
            }
            else if (action == "bad")
            {
                ShowNotification("~r~No i chuj zjebałeś... no nic może się jeszcze kiedyś zobaczymy...");
                TriggerServerEvent("military_heist:removeitem:server", LocalPlayer.ServerId, "fajnyheist_hackingtool", 1);
                TriggerServerEvent("military_heist:removeitem:server", LocalPlayer.ServerId, "fajnyheist_enter_silent", 1);
            }
        }
        bool server1hacked = false; bool server2hacked = false; bool server3hacked = false;
        private void Server1(string action)
        {
            if(action == "good") { 
                server1hacked = true; 
                if (server2hacked && server3hacked) 
                { 
                    serversHacked = true;
                    ShowNotification("~r~Wszystkie serwery zhakowane ? Świetnie, na samej górze znajdź odpowiedni panel !");
                } 
            }
            else if(action == "bad")
            {
                ShowNotification("~r~Zjebałeś... może następnym razem...");
                RemoveBlipsAndMarkers(Blips.silent);
            }
        }
        private void Server2(string action)
        {
            if (action == "good")
            {
                server2hacked = true;
                if (server1hacked && server3hacked)
                {
                    serversHacked = true;
                    ShowNotification("~r~Wszystkie serwery zhakowane ? Świetnie, na samej górze znajdź odpowiedni panel !");
                }
            }
            else if (action == "bad")
            {
                ShowNotification("~r~Zjebałeś... może następnym razem...");
                RemoveBlipsAndMarkers(Blips.silent);
            }
        }
        private void Server3(string action)
        {
            if (action == "good")
            {
                server3hacked = true;
                if (server1hacked && server2hacked)
                {
                    serversHacked = true;
                    ShowNotification("~r~Wszystkie serwery zhakowane ? Świetnie, na samej górze znajdź odpowiedni panel !");
                }
            }
            else if (action == "bad")
            {
                ShowNotification("~r~Zjebałeś... może następnym razem...");
                RemoveBlipsAndMarkers(Blips.silent);
            }
        }

        private async void OpenEntry(string x)
        {
            if (heist_type != "") return;
            Tick += WorkingMarkers;
            await Delay(60000);
            Tick -= WorkingMarkers;
        }

        float[] headings = new float[500];
        private async Task BlockDoors()
        {
            Vector3 center = new Vector3(244f, 6164f, -159f);
            var props = World.GetAllProps();
            for (int i = 0; i < props.Length; i++)
            {
                if (/*props[i].Position.DistanceToSquared(center) < 1400f && (*/props[i].Model == 1530559551 || props[i].Model == 676238968 ||
                    props[i].Model == 1092250630 || props[i].Model == 1885457048 || props[i].Model == -1102215075 || props[i].Model == 1289692550
                    || props[i].Model == -911307453 || props[i].Model == -913076967)
                {
                    if (props[i].Position.DistanceToSquared2D(center) > 1200f) continue;
                    Vector3 dontBlock = new Vector3(229.0499f, 6133.029f, -159.2575f);
                    Vector3 dontBlock2 = new Vector3(221f, 6121f, -159f);
                    //if (props[i].Position.DistanceToSquared(dontBlock) < 4f && props[i].Position.DistanceToSquared(dontBlock2) < 4f) continue;
                    //props[i].IsPositionFrozen = true;
                    if (headings[i] != 0) headings[i] = props[i].Heading;
                    if(props[i].Model == 1092250630) { props[i].Heading = 180f; continue; }
                    if(props[i].Model == 1885457048) { props[i].Heading = 180f; continue; }
                    props[i].Heading = headings[i];
                }
            }
        }

        private async Task LeaveMarker()
        {
            World.DrawMarker(MarkerType.VerticalCylinder, leaveBaseVector, leaveBaseVector, new Vector3(1, 1, 1), new Vector3(1, 1, 1), System.Drawing.Color.FromArgb(125, 255, 0, 0));
            if (LocalPlayer.Character.Position.DistanceToSquared(leaveBaseVector) < 1.2f)
            {
                ShowHelp(HelpType.onestring, "Naciśnij ~INPUT_PICKUP~ aby wyjść");
                if (API.IsControlJustPressed(1, 51))
                {
                    var pos = enterBaseVector; pos.X -= 2;
                    await Delay(500);
                    LocalPlayer.Character.Position = enterBaseVector;
                    Tick -= BlockDoors;
                }
            }
        }

        enum Blips
        {
            loud = 1,
            silent = 2
        }
        private void RemoveBlipsAndMarkers(Blips type)
        {
            baseEntryBlip.Delete();
            Tick -= RenderMarkers;
            Tick -= WorkingMarkers;
            if ((int)type == 1)
            {
                makec4Blip.Delete();
                powderBlip.Delete();
            }
            else if ((int)type == 2)
            {
                hackBlip.Delete();
                hackingtoolBlip.Delete();
            }
        }
    }
}
