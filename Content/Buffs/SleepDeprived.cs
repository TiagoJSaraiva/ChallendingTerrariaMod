// ChallengingTerrariaMod/Content/Buffs/Nauseous.cs
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChallengingTerrariaMod.Content.Systems;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class SleepDeprived : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.manaRegenBonus -= 50;
            player.statLifeMax2 = RestSystem.RoundValue(player.statLifeMax2, 1.6f);
            player.statManaMax2 = RestSystem.RoundValue(player.statManaMax2, 1.6f);
        }
    }
}