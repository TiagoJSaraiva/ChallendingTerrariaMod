// ChallengingTerrariaMod/Content/Buffs/Nauseous.cs
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ChallengingTerrariaMod.Content.Systems;
using ChallengingTerrariaMod.Content.Systems.Players;
using Terraria.DataStructures;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class SleepDeprived : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
    }

    public class SleepDeprivedPlayer : ModPlayer
    {
        public int timeLeft_SleepDeprived = 0;

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            for (int i = 0; i < Player.buffType.Length; i++)
            {
                if (Player.buffType[i] == ModContent.BuffType<SleepDeprived>())
                {
                    timeLeft_SleepDeprived = Player.buffTime[i];
                    break;
                }
            }
        }
        public override void OnRespawn()
        {
            Player.AddBuff(ModContent.BuffType<SleepDeprived>(), timeLeft_SleepDeprived);
        }
    }
}