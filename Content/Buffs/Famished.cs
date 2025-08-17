// ChallengingTerrariaMod/Content/Buffs/Famished.cs
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Famished : ModBuff
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
            player.statDefense /= 1.6f;
            player.endurance -= 0.1f;
            player.pickSpeed += 0.45f;
        }
    }
}