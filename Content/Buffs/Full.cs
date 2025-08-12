// ChallengingTerrariaMod/Content/Buffs/Full.cs
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Full : ModBuff
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
            player.moveSpeed -= 0.25f; // movespeed reduced by 25%
        }
    }
}