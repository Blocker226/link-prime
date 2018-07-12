using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace Discord_Link_Prime {

    public class RootObject  {
        public List<Weapon> theWeaponList { get; set; }
        public List<Warframe> theWarframeList { get; set; }
        public List<Companion> theCompanionList { get; set; }
        public List<Mod> theModList { get; set; }
    }

    public class Weapon {
        public string name { get; set; }
        public string iconURL { get; set; }
        public string description { get; set; }
        public WeaponType type { get; set; }
        public WeaponDamageType[] damage { get; set; }
        public Polarity[] polarity { get; set; }
    }

    public class Warframe {
        public string name { get; set; }
        public string iconURL { get; set; }
        public string description { get; set; }
        public int maxArmour { get; set; }
        public int maxHealth { get; set; }
        public int maxPower { get; set; }
        public int maxShield { get; set; }
        public int sprintSpeed { get; set; }
        public Polarity[] polarity { get; set; }
    }

    public class Companion {
        public string name { get; set; }
        public string iconURL { get; set; }
        public string description { get; set; }
        public int maxArmour { get; set; }
        public int maxHealth { get; set; }
        public int maxShield { get; set; }
        public Polarity[] polarity { get; set; }
        public WeaponDamageType[] damage { get; set; }
    }

    public class Mod {
        public string name { get; set; }
        public string iconURL { get; set; }
        public ModRarity rarity { get; set; }
        public Polarity polarity { get; set; }
        public int maxDrain { get; set; }
        public int maxRanks { get; set; }
        public bool exilus { get; set; }
        public string description { get; set; }
        public string compat { get; set; }

    }

    public class Misc {
        public string name { get; set; }
        public string iconURL { get; set; }
        public string description { get; set; }
    }

    public enum WeaponType {
        None,
        //ranged
        Bow,
        Crossbow,
        [Description("Akimbo Pistols")]
        AkimboPistols,
        Launcher,
        Pistol,
        Rifle,
        Shotgun,
        Sniper,
        Speargun,
        Throwing,
        //melee
        [Description("Blade and Whip")]
        BladeWhip,
        Claws,
        Dagger,
        [Description("Dual Daggers")]
        DualDaggers,
        [Description("Dual Swords")]
        DualSwords,
        Fists,
        Glaive,
        Gunblade,
        Hammer,
        [Description("Heavy Blade")]
        HeavyBlade,
        Machete,
        Nikana,
        Nunchaku,
        Polearm,
        Rapier,
        Scythe,
        Sparring,
        Staff,
        Sword,
        [Description("Sword and Shield")]
        SwordShield,
        Tonfas,
        Whip
    }

    public enum WeaponDamageType {
        Impact,
        Puncture,
        Slash,
        Cold,
        Electricity,
        Heat,
        Toxin,
        Blast,
        Corrosive,
        Gas,
        Magnetic,
        Radiation,
        Viral
    }

    public enum Polarity {
        Madurai,
        Naramon,
        Penjaga,
        Vazarin,
        Unairu,
        Zenurik
    }

    public enum ModRarity {
        [Description("#FFFFFF")]
        Legendary,
        [Description("#ffe438")]
        Rare,
        [Description("#d9e4f7")]
        Uncommon,
        [Description("#ce6d12")]
        Common
    }
}
