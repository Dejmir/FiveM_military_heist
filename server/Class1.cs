using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Newtonsoft.Json.Linq;

using CitizenFX.Core;

namespace fivem_military_heist_server
{
    public class Class1 : BaseScript
    {
        dynamic ESX;
        public Class1()
        {
            TriggerEvent("esx:getSharedObject", new object[] { new Action<dynamic>(esx => {
                ESX = esx;
            })});

            EventHandlers["military_heist:loadcfg:server"] += new Action<Player, string>(LoadConfig);

            EventHandlers["military_heist:giveitem:server"] += new Action<Player, int, string, int, string, int, bool>(GiveItem);
            EventHandlers["military_heist:removeitem:server"] += new Action<Player, int, string, int>(RemoveItem);
            EventHandlers["military_heist:haveitems:server"] += new Action<Player, int>(Items);

            EventHandlers["military_heist_haveitem:hackingtool:server"] += new Action<Player, int>(HaveHackingtool);
            EventHandlers["military_heist_haveitem:c4:server"] += new Action<Player, int>(HaveC4);

            EventHandlers["military_heist:openentry:server"] += new Action<Player>(OpenEntry);

            EventHandlers["military_heist_getloot:server"] += new Action<Player, int, string>(LoadLoot);
        }

        private void LoadConfig([FromSource]Player player, string action)
        {
            var configFileText = File.ReadAllText(@"resources\military_heist\config.json");
            JObject config = JObject.Parse(configFileText);
            float[] sp1 = new float[4]; float[] sp2 = new float[4]; float[] sp3 = new float[4]; float[] sp4 = new float[4]; float[] powder = new float[3]; float[] makec4 = new float[3];
            float[] hackingtool = new float[3]; float[] hack = new float[3];
            sp1[0] = (float)config.SelectToken("SpawnLocation1.X"); sp1[1] = (float)config.SelectToken("SpawnLocation1.Y"); sp1[2] = (float)config.SelectToken("SpawnLocation1.Z");
            sp1[3] = (float)config.SelectToken("SpawnLocation1.H");
            sp2[0] = (float)config.SelectToken("SpawnLocation2.X"); sp2[1] = (float)config.SelectToken("SpawnLocation2.Y"); sp2[2] = (float)config.SelectToken("SpawnLocation2.Z");
            sp2[3] = (float)config.SelectToken("SpawnLocation2.H");
            sp3[0] = (float)config.SelectToken("SpawnLocation3.X"); sp3[1] = (float)config.SelectToken("SpawnLocation3.Y"); sp3[2] = (float)config.SelectToken("SpawnLocation3.Z");
            sp3[3] = (float)config.SelectToken("SpawnLocation3.H");
            sp4[0] = (float)config.SelectToken("SpawnLocation4.X"); sp4[1] = (float)config.SelectToken("SpawnLocation4.Y"); sp4[2] = (float)config.SelectToken("SpawnLocation4.Z");
            sp4[3] = (float)config.SelectToken("SpawnLocation4.H");

            powder[0] = (float)config.SelectToken("PowderLocation.X"); powder[1] = (float)config.SelectToken("PowderLocation.Y"); powder[2] = (float)config.SelectToken("PowderLocation.Z");
            makec4[0] = (float)config.SelectToken("MakeC4Location.X"); makec4[1] = (float)config.SelectToken("MakeC4Location.Y"); makec4[2] = (float)config.SelectToken("MakeC4Location.Z");

            hackingtool[0] = (float)config.SelectToken("HackingtoolLocation.X"); hackingtool[1] = (float)config.SelectToken("HackingtoolLocation.Y");
            hackingtool[2] = (float)config.SelectToken("HackingtoolLocation.Z");
            hack[0] = (float)config.SelectToken("HackLocation.X"); hack[1] = (float)config.SelectToken("HackLocation.Y"); hack[2] = (float)config.SelectToken("HackLocation.Z");


            player.TriggerEvent("military_heist:loadcfg", "client", sp1, sp2, sp3, sp4, powder, makec4, hackingtool, hack);
        }
        private async void LoadLoot([FromSource] Player player, int playerid, string heisttype)
        {
            var configFileText = File.ReadAllText(@"resources\military_heist\config.json");
            JObject config = JObject.Parse(configFileText);
            if(heisttype == "loud")
            {
                int minCash = (int)config.SelectToken("Loot.Loud.Cash.min");
                int maxCash = (int)config.SelectToken("Loot.Loud.Cash.max");
                int cash = new Random().Next(minCash, maxCash);
                await Delay(2500);

                bool documents = false;
                int documentsChance = (int)config.SelectToken("Loot.Loud.DocumentsChance");
                if (new Random().Next(1, 100) < documentsChance) documents = true;
                await Delay(2500);

                string weapon = "";
                int weaponChance = (int)config.SelectToken("Loot.Loud.WeaponChance");
                var weaponsnotparsed = config.SelectToken("Loot.Loud.Weapons");
                string[] weapons = new string[weaponsnotparsed.Count()];
                for (int i = 0; i < weaponsnotparsed.Count(); i++) {weapons[i] = weaponsnotparsed.ToArray()[i].ToString();}
                if(new Random().Next(1, 100) < weaponChance) { int weaponIndex = new Random().Next(1, weapons.Length); weapon = weapons[weaponIndex]; }
                await Delay(2500);

                string longWeapon = "";
                int longWeaponChance = (int)config.SelectToken("Loot.Loud.LongWeaponChance");
                var longweaponsnotparsed = config.SelectToken("Loot.Loud.LongWeapons");
                string[] longWeapons = new string[longweaponsnotparsed.Count()];
                for (int i = 0; i < longweaponsnotparsed.Count(); i++) { longWeapons[i] = longweaponsnotparsed.ToArray()[i].ToString(); }
                if (new Random().Next(1, 1000) < longWeaponChance) { int longWeaponIndex = new Random().Next(1, longWeapons.Length); longWeapon = longWeapons[longWeaponIndex]; }
                await Delay(2500);

                object[] additionalItem1 = new object[2]; additionalItem1[0] = "";
                if ((bool)config.SelectToken("Loot.Loud.AdditionalItem1.enabled")){
                    if(new Random().Next(1, 100) < (int)config.SelectToken("Loot.Loud.AdditionalItem1.chance")){additionalItem1[0] = (string)config.SelectToken("Loot.Loud.AdditionalItem1.item"); 
                    additionalItem1[1] = new Random().Next((int)config.SelectToken("Loot.Loud.AdditionalItem1.min"), (int)config.SelectToken("Loot.Loud.AdditionalItem1.max"));}
                } await Delay(2500);
                object[] additionalItem2 = new object[2]; additionalItem2[0] = "";
                if ((bool)config.SelectToken("Loot.Loud.AdditionalItem2.enabled")){
                    if(new Random().Next(1, 100) < (int)config.SelectToken("Loot.Loud.AdditionalItem2.chance")){additionalItem2[0] = (string)config.SelectToken("Loot.Loud.AdditionalItem2.item");
                    additionalItem2[1] = new Random().Next((int)config.SelectToken("Loot.Loud.AdditionalItem2.min"), (int)config.SelectToken("Loot.Loud.AdditionalItem2.max"));}
                } await Delay(2500);
                object[] additionalItem3 = new object[2]; additionalItem3[0] = "";
                if ((bool)config.SelectToken("Loot.Loud.AdditionalItem3.enabled")){
                    if(new Random().Next(1, 100) < (int)config.SelectToken("Loot.Loud.AdditionalItem3.chance")){additionalItem3[0] = (string)config.SelectToken("Loot.Loud.AdditionalItem3.item");
                    additionalItem3[1] = new Random().Next((int)config.SelectToken("Loot.Loud.AdditionalItem3.min"), (int)config.SelectToken("Loot.Loud.AdditionalItem3.max"));}
                } await Delay(2500);
                object[] additionalItem4 = new object[2]; additionalItem4[0] = "";
                if ((bool)config.SelectToken("Loot.Loud.AdditionalItem4.enabled")){
                    if(new Random().Next(1, 100) < (int)config.SelectToken("Loot.Loud.AdditionalItem4.chance")){additionalItem4[0] = (string)config.SelectToken("Loot.Loud.AdditionalItem4.item");
                    additionalItem4[1] = new Random().Next((int)config.SelectToken("Loot.Loud.AdditionalItem4.min"), (int)config.SelectToken("Loot.Loud.AdditionalItem4.max"));}
                } await Delay(2500);

                var xPlayer = ESX.GetPlayerFromId(playerid);

                xPlayer.addAccountMoney("black_money", cash);
                if (documents) xPlayer.addInventoryItem("fajnyheist_documents", 1);
                if(weapon != "") xPlayer.addWeapon(weapon, 0);
                if (longWeapon != "") xPlayer.addWeapon(longWeapon, 0);
                if((string)additionalItem1[0] != "") xPlayer.addInventoryItem(additionalItem1[0], additionalItem1[1]);
                if((string)additionalItem2[0] != "") xPlayer.addInventoryItem(additionalItem2[0], additionalItem2[1]);
                if((string)additionalItem3[0] != "") xPlayer.addInventoryItem(additionalItem3[0], additionalItem3[1]);
                if((string)additionalItem4[0] != "") xPlayer.addInventoryItem(additionalItem4[0], additionalItem4[1]);

                player.TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 255, 255 },
                    args = new[] { $"Twój łup to: pieniędzy:{cash}, dokumenty rządowe:{documents} broń:{weapon}, broń długa:{longWeapon}" }
                });
            }
            else if(heisttype == "silent")
            {
                int minCash = (int)config.SelectToken("Loot.Silent.Cash.min");
                int maxCash = (int)config.SelectToken("Loot.Silent.Cash.max");
                int cash = new Random().Next(minCash, maxCash);
                await Delay(2500);

                bool documents = false;
                int documentsChance = (int)config.SelectToken("Loot.Silent.DocumentsChance");
                if (new Random().Next(1, 100) < documentsChance) documents = true;
                await Delay(2500);

                string weapon = "";
                int weaponChance = (int)config.SelectToken("Loot.Silent.WeaponChance");
                var weaponsnotparsed = config.SelectToken("Loot.Silent.Weapons");
                string[] weapons = new string[weaponsnotparsed.Count()];
                for (int i = 0; i < weaponsnotparsed.Count(); i++) {weapons[i] = weaponsnotparsed.ToArray()[i].ToString();}
                if(new Random().Next(1, 100) < weaponChance) { int weaponIndex = new Random().Next(1, weapons.Length); weapon = weapons[weaponIndex]; }
                await Delay(2500);

                string longWeapon = "";
                int longWeaponChance = (int)config.SelectToken("Loot.Silent.LongWeaponChance");
                var longweaponsnotparsed = config.SelectToken("Loot.Silent.LongWeapons");
                string[] longWeapons = new string[longweaponsnotparsed.Count()];
                for (int i = 0; i < longweaponsnotparsed.Count(); i++) { longWeapons[i] = longweaponsnotparsed.ToArray()[i].ToString(); }
                if (new Random().Next(1, 1000) < longWeaponChance) { int longWeaponIndex = new Random().Next(1, longWeapons.Length); longWeapon = longWeapons[longWeaponIndex]; }
                await Delay(2500);

                object[] additionalItem1 = new object[2]; additionalItem1[0] = "";
                if ((bool)config.SelectToken("Loot.Silent.AdditionalItem1.enabled")){
                    if(new Random().Next(1, 100) < (int)config.SelectToken("Loot.Silent.AdditionalItem1.chance")){additionalItem1[0] = (string)config.SelectToken("Loot.Silent.AdditionalItem1.item"); 
                    additionalItem1[1] = new Random().Next((int)config.SelectToken("Loot.Silent.AdditionalItem1.min"), (int)config.SelectToken("Loot.Silent.AdditionalItem1.max"));}
                } await Delay(2500);
                object[] additionalItem2 = new object[2]; additionalItem2[0] = "";
                if ((bool)config.SelectToken("Loot.Silent.AdditionalItem2.enabled")){
                    if(new Random().Next(1, 100) < (int)config.SelectToken("Loot.Silent.AdditionalItem2.chance")){additionalItem2[0] = (string)config.SelectToken("Loot.Silent.AdditionalItem2.item");
                    additionalItem2[1] = new Random().Next((int)config.SelectToken("Loot.Silent.AdditionalItem2.min"), (int)config.SelectToken("Loot.Silent.AdditionalItem2.max"));}
                } await Delay(2500);
                object[] additionalItem3 = new object[2]; additionalItem3[0] = "";
                if ((bool)config.SelectToken("Loot.Silent.AdditionalItem3.enabled")){
                    if(new Random().Next(1, 100) < (int)config.SelectToken("Loot.Silent.AdditionalItem3.chance")){additionalItem3[0] = (string)config.SelectToken("Loot.Silent.AdditionalItem3.item");
                    additionalItem3[1] = new Random().Next((int)config.SelectToken("Loot.Silent.AdditionalItem3.min"), (int)config.SelectToken("Loot.Silent.AdditionalItem3.max"));}
                } await Delay(2500);
                object[] additionalItem4 = new object[2]; additionalItem4[0] = "";
                if ((bool)config.SelectToken("Loot.Silent.AdditionalItem4.enabled")){
                    if(new Random().Next(1, 100) < (int)config.SelectToken("Loot.Silent.AdditionalItem4.chance")){additionalItem4[0] = (string)config.SelectToken("Loot.Silent.AdditionalItem4.item");
                    additionalItem4[1] = new Random().Next((int)config.SelectToken("Loot.Silent.AdditionalItem4.min"), (int)config.SelectToken("Loot.Silent.AdditionalItem4.max"));}
                } await Delay(2500);

                var xPlayer = ESX.GetPlayerFromId(playerid);

                xPlayer.addAccountMoney("black_money", cash);
                if (documents) xPlayer.addInventoryItem("fajnyheist_documents", 1);
                if(weapon != "") xPlayer.addWeapon(weapon, 0);
                if (longWeapon != "") xPlayer.addWeapon(longWeapon, 0);
                if((string)additionalItem1[0] != "") xPlayer.addInventoryItem(additionalItem1[0], additionalItem1[1]);
                if((string)additionalItem2[0] != "") xPlayer.addInventoryItem(additionalItem2[0], additionalItem2[1]);
                if((string)additionalItem3[0] != "") xPlayer.addInventoryItem(additionalItem3[0], additionalItem3[1]);
                if((string)additionalItem4[0] != "") xPlayer.addInventoryItem(additionalItem4[0], additionalItem4[1]);

                player.TriggerEvent("chat:addMessage", new
                {
                    color = new[] { 255, 255, 255 },
                    args = new[] { $"Twój łup to: pieniędzy:{cash}, dokumenty rządowe:{documents} broń:{weapon}, broń długa:{longWeapon}" }
                });
            }
        }

        private void GiveItem([FromSource] Player player, int playerID, string item, int count, string itemtoremove, int counttoremove, bool restriction)
        {
            var xPlayer = ESX.GetPlayerFromId(playerID);
            if (restriction)
            {
                if (xPlayer.getInventoryItem(item).count < 1)
                {
                    xPlayer.addInventoryItem(item, count);
                    if (itemtoremove != "") xPlayer.removeInventoryItem(itemtoremove, counttoremove);
                }
            }
            else
            {
                xPlayer.addInventoryItem(item, count);
                if (itemtoremove != "") xPlayer.removeInventoryItem(itemtoremove, counttoremove);
            }
        }
        private void RemoveItem([FromSource] Player player, int playerID, string item, int count)
        {
            var xPlayer = ESX.GetPlayerFromId(playerID);
            xPlayer.removeInventoryItem(item, count);
        }

        private void Items([FromSource] Player player, int playerID)
        {
            var xPlayer = ESX.GetPlayerFromId(playerID);
            var c1 = xPlayer.getInventoryItem("fajnyheist_enter_loud").count;
            var c2 = xPlayer.getInventoryItem("fajnyheist_enter_silent").count;
            /*player.TriggerEvent("chat:addMessage", new
            {
                color = new[] { 255, 255, 255 },
                args = new[] { $"{c1.ToString()} i {c2.ToString()}" }
            });*/

            if (c1 == 1) { player.TriggerEvent("military_heist:start", "loud", true); }
            else if (c2 == 1) { player.TriggerEvent("military_heist:start", "silent", true); }
        }

        private void HaveHackingtool([FromSource] Player player, int playerID)
        {
            var xPlayer = ESX.GetPlayerFromId(playerID);
            var count = xPlayer.getInventoryItem("fajnyheist_hackingtool").count;
            bool have = false;
            if (count == 1) have = true;
            player.TriggerEvent("military_heist:haveitem:hackingtool", have);
        }
        private void HaveC4([FromSource] Player player, int playerID)
        {
            var xPlayer = ESX.GetPlayerFromId(playerID);
            var count = xPlayer.getInventoryItem("fajnyheist_c4").count;
            bool have = false;
            if (count == 1) { have = true; }
            /*player.TriggerEvent("chat:addMessage", new
            {
                color = new[] { 255, 255, 255 },
                args = new[] { $"Server: {have.ToString()}" }
            });*/
            player.TriggerEvent("military_heist:haveitem:c4", have);
        }

        private void OpenEntry([FromSource] Player player)
        {
            TriggerClientEvent("military_heist:openentry", "");
        }
    }
}
