// Em ChallengingTerrariaMod/Content/Buffs/Exhausted.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using ChallengingTerrariaMod.Content.Systems;
using ChallengingTerrariaMod.Content.Systems.Players; // Para BuffID.Confused, BuffID.Drunk

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Exhausted : ModBuff
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
            player.manaRegenBonus -= 50;
            player.statLifeMax2 = RestSystem.RoundValue(player.statLifeMax2, 1.6f);
            player.statManaMax2 = RestSystem.RoundValue(player.statManaMax2, 1.6f);
        }
    }

    public class ExhaustedPlayer : ModPlayer
    {
        public override void UpdateBadLifeRegen()
        {
            if ((Player.HasBuff(ModContent.BuffType<SleepDeprived>()) || Player.HasBuff(ModContent.BuffType<Exhausted>())) && Player.lifeRegen > 0)
            {
                Player.lifeRegen = (int)(Player.lifeRegen * 0.4f);
            }
        }
    }
}