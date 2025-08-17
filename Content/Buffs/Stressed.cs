// ChallengingTerrariaMod/Content/Buffs/Stuffed.cs
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Stressed : ModBuff
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
            player.GetDamage(DamageClass.Generic) -= 0.10f; // 10% reduction in player damage
            player.GetCritChance(DamageClass.Generic) -= 5; // 5% reduction in player critchance
        }
    }
}