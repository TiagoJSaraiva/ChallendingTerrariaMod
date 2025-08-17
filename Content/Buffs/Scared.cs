// ChallengingTerrariaMod/Content/Buffs/Stuffed.cs
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Scared : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) -= 0.20f; // 20% reduction in player damage
            player.GetCritChance(DamageClass.Generic) -= 15; // 15% reduction in player critchance
        }
    }
}