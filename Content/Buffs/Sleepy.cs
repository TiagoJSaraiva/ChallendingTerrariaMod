// Em ChallengingTerrariaMod/Content/Buffs/Sleepy.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using ChallengingTerrariaMod.Content.Systems;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Sleepy : ModBuff
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
            player.manaRegenBonus -= 35;
            player.statLifeMax2 = RestSystem.RoundValue(player.statLifeMax2, 1.4f);
            player.statManaMax2 = RestSystem.RoundValue(player.statManaMax2, 1.4f);
        }
    }

    public class SleepyPlayer : ModPlayer
    {
        public override void UpdateBadLifeRegen()
        {
            if (Player.HasBuff(ModContent.BuffType<Sleepy>()) && Player.lifeRegen > 0)
            {
                Player.lifeRegen = (int)(Player.lifeRegen * 0.6f);
            }
        }
    }
}