// ChallengingTerrariaMod/Content/Buffs/Peckish.cs
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Cancer : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense /= 2f;
            player.endurance -= 0.50f;
            player.pickSpeed += 0.90f;
            player.moveSpeed -= 0.95f;
            player.GetDamage(DamageClass.Generic) -= 0.50f; // 20% reduction in player damage
        }
    }
}