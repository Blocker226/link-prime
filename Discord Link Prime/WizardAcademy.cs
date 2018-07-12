using Discord.Addons.InteractiveCommands;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Discord_Link_Prime {
    public class WizardAcademy : InteractiveModuleBase {
        //MOVE CREATION WIZARDRY STUFF HERE IF AT ALL POSSIBLE

        public async Task CreateNewEntry(string type, string name, string arg = null) {
            bool validInput = false;
            do {
                switch (type.ToUpper()) {
                    case "WEAPON":
                        await CreateWeapon(name, arg);
                        validInput = true;
                        break;
                    case "WARFRAME":
                        await CreateWarframe(name);
                        validInput = true;
                        break;
                    case "COMPANION":
                    case "SENTINEL":
                    case "PET":
                        await CreateWeapon(name);
                        validInput = true;
                        break;
                    case "MOD":
                        await CreateWeapon(name);
                        validInput = true;
                        break;
                    case "C":
                        await Context.Channel.SendMessageAsync("Creation cancelled.");
                        validInput = true;
                        break;
                    default:
                        await Context.Channel.SendMessageAsync("Invalid type. It can only be a Warframe, Companion, Weapon or Mod.");
                        break;
                }
            } while (validInput == false);
        }

        public async Task EditEntry(string name, string property, string value) {
            bool foundItem = false;
            //First search weapons
            for (int i = 0; i < Program.weaponArray.Length; i++) {
                if (name == Program.weaponArray[i].name) {
                    switch (property.ToUpper()) {
                        case "NAME":
                            string oldName = Program.weaponArray[i].name;
                            Program.weaponArray[i].name = value;
                            await Context.Channel.SendMessageAsync("Changed name of " + oldName + " to " + Program.weaponArray[i].name + ".");
                            break;
                        case "DESCRIPTION":
                            Program.weaponArray[i].description = value;
                            await Context.Channel.SendMessageAsync(Program.weaponArray[i].name + " description updated.");
                            break;
                        case "TYPE":
                            WeaponType nType;
                            if (Enum.TryParse(value, out nType)) {
                                Program.weaponArray[i].type = nType;
                                await Context.Channel.SendMessageAsync("Changed type of " + Program.weaponArray[i].name + " to " + SearchWarframe.GetEnumDescription(Program.weaponArray[i].type) + ".");
                            }
                            else {
                                await Context.Channel.SendMessageAsync("Invalid input given. Edit rejected.");
                            }
                            break;
                    }
                    Program.WriteCodex();
                    foundItem = true;
                    break;
                }
            }
            if (!foundItem) {
                //Second search warframes
                for (int i = 0; i < Program.warframeArray.Length; i++) {
                    if (name == Program.warframeArray[i].name) {
                        switch (property.ToUpper()) {
                            case "NAME":
                                string oldName = Program.warframeArray[i].name;
                                Program.weaponArray[i].name = value;
                                await Context.Channel.SendMessageAsync("Changed name of " + oldName + " to " + Program.warframeArray[i].name + ".");
                                break;
                            case "DESCRIPTION":
                            case "DESC":
                                Program.weaponArray[i].description = value;
                                await Context.Channel.SendMessageAsync(Program.warframeArray[i].name + " description updated.");
                                break;
                            case "MAXHEALTH":
                            case "HEALTH":
                                int newhealth;
                                if (int.TryParse(value, out newhealth)) {
                                    Program.warframeArray[i].maxHealth = newhealth;
                                }
                                await Context.Channel.SendMessageAsync(Program.warframeArray[i].name + " Maximum health updated.");
                                break;
                            case "MAXSHIELD":
                            case "SHIELD":
                                int newshield;
                                if (int.TryParse(value, out newshield)) {
                                    Program.warframeArray[i].maxShield = newshield;
                                }
                                await Context.Channel.SendMessageAsync(Program.warframeArray[i].name + " Maximum shields updated.");
                                break;
                        }
                        Program.WriteCodex();
                        foundItem = true;
                        break;
                    }
                }
            }
            else if (!foundItem) {
                //Third search companions
                for (int i = 0; i < Program.companionArray.Length; i++) {
                    if (name == Program.companionArray[i].name) {
                        switch (property.ToUpper()) {
                            case "NAME":
                                string oldName = Program.companionArray[i].name;
                                Program.companionArray[i].name = value;
                                await Context.Channel.SendMessageAsync("Changed name of " + oldName + " to " + Program.companionArray[i].name + ".");
                                break;
                            case "DESCRIPTION":
                            case "DESC":
                                Program.companionArray[i].description = value;
                                await Context.Channel.SendMessageAsync(Program.companionArray[i].name + " description updated.");
                                break;
                            case "HEALTH":
                            case "MAXHEALTH":
                                int newhealth;
                                if (int.TryParse(value, out newhealth)) {
                                    Program.companionArray[i].maxHealth = newhealth;
                                }
                                await Context.Channel.SendMessageAsync(Program.companionArray[i].name + " Maximum health updated.");
                                break;
                        }
                        Program.WriteCodex();
                        foundItem = true;
                        break;
                    }
                }
            }
            else if (!foundItem) {
                //Fourth search mods
                for (int i = 0; i < Program.modArray.Length; i++) {
                    if (name == Program.modArray[i].name + "]") {
                        switch (property.ToUpper()) {
                            case "NAME":
                                string oldName = Program.modArray[i].name;
                                Program.modArray[i].name = value;
                                await Context.Channel.SendMessageAsync("Changed name of " + oldName + " to " + Program.modArray[i].name + ".");
                                break;
                            case "DESCRIPTION":
                                Program.modArray[i].description = value;
                                await Context.Channel.SendMessageAsync(Program.modArray[i].name + " description updated.");
                                break;
                        }
                        Program.WriteCodex();
                        foundItem = true;
                        break;
                    }
                }
            }
            else if (!foundItem) {
                await Context.Channel.SendMessageAsync("No existing entry found.");
                return;
            }
        }

        public async Task DeleteEntry(string name) {

        }

        async Task CreateWeapon(string name, string iconURL = null) {
            for (int i = 0; i < Program.weaponArray.Length; i++) {
                if (name == Program.weaponArray[i].name) {
                    await Context.Channel.SendMessageAsync("Similarly named entry exists. Use \"[contrib] edit\" instead.");
                    return;
                }
            }
            Weapon nWeapon = new Weapon();
            nWeapon.name = name;
            if (iconURL == null) {
                await Context.Channel.SendMessageAsync("Creating new weapon entry " + nWeapon.name + ", please provide icon URL.");
            }
            while (iconURL == null) {
                var response = await WaitForMessage(Context.Message.Author, Context.Channel);
                if (response.Content != null || response.Content != "c") {
                    if (Uri.IsWellFormedUriString(response.Content, UriKind.Absolute)) {
                        iconURL = response.Content;
                    }
                    else {
                        await Context.Channel.SendMessageAsync("Invalid URL.");
                    }
                }
                else {
                    await Context.Channel.SendMessageAsync("Creation cancelled.");
                    return;
                }
            }
            nWeapon.iconURL = iconURL;
            await Context.Channel.SendMessageAsync("Weapon " + nWeapon.name + " created with icon. Use \"[contrib] edit\" to add further details.");
            if (Program.weaponList == null) {
                Program.weaponList = new List<Weapon>();
            }
            Program.weaponList.Add(nWeapon);
            Program.WriteCodex();
            foreach (Weapon wep in Program.weaponList) {
                Program.LogLine(wep.name);
            }
        }

        async Task CreateWarframe(string name, string iconURL = null) {
            for (int i = 0; i < Program.warframeArray.Length; i++) {
                if (name == Program.warframeArray[i].name) {
                    await Context.Channel.SendMessageAsync("Similarly named entry exists. Use \"[contrib] edit\" instead.");
                    return;
                }
            }
            Warframe nWarframe = new Warframe();
            nWarframe.name = name;
            if (iconURL == null) {
                await Context.Channel.SendMessageAsync("Creating new weapon entry " + nWarframe.name + ", please provide icon URL.");
            }
            while (iconURL == null) {
                var response = await WaitForMessage(Context.Message.Author, Context.Channel);
                if (response.Content != null || response.Content != "c") {
                    if (Uri.IsWellFormedUriString(response.Content, UriKind.Absolute)) {
                        iconURL = response.Content;
                    }
                    else {
                        await Context.Channel.SendMessageAsync("Invalid URL.");
                    }
                }
                else {
                    await Context.Channel.SendMessageAsync("Creation cancelled.");
                    return;
                }
            }
            nWarframe.iconURL = iconURL;
            await Context.Channel.SendMessageAsync("Weapon " + nWarframe.name + " created with icon. Use \"[contrib] edit\" to add further details.");
            if (Program.weaponList == null) {
                Program.weaponList = new List<Weapon>();
            }
            Program.warframeList.Add(nWarframe);
            Program.WriteCodex();
            foreach (Weapon wep in Program.weaponList) {
                Program.LogLine(wep.name + " entry created.");
            }
        }

    }
} //END
