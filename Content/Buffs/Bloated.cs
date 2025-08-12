using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Bloated : ModBuff
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
            player.moveSpeed -= 0.4f; // movespeed reduced by 40%
        }
    }
}
