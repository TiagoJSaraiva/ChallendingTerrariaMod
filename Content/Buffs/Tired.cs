// Em ChallengingTerrariaMod/Content/Buffs/Tired.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using ChallengingTerrariaMod.Content.Systems;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Tired : ModBuff
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
            player.manaRegenBonus -= 25;
            player.statLifeMax2 = RestSystem.RoundValue(player.statLifeMax2, 1.2f);
            player.statManaMax2 = RestSystem.RoundValue(player.statManaMax2, 1.2f);
        }
    }

    public class TiredPlayer : ModPlayer
    {
        public override void UpdateBadLifeRegen()
        {
            if (Player.HasBuff(ModContent.BuffType<Tired>()) && Player.lifeRegen > 0)
            {
                Player.lifeRegen = (int)(Player.lifeRegen * 0.8f);
            }
        }
    }
}