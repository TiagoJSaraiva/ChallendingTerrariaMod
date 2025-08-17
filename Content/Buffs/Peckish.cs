// ChallengingTerrariaMod/Content/Buffs/Peckish.cs
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Peckish : ModBuff
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
            player.statDefense /= 1.2f;
            player.endurance -= 0.03f;
            player.pickSpeed += 0.25f;
        }
    }
}