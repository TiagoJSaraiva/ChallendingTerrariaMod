// ChallengingTerrariaMod/Content/Buffs/Nauseous.cs
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            player.statDefense -= 10; // Reduz armadura em 10
            player.GetDamage(DamageClass.Default) *= 0.30f;
            player.lifeRegen = 0; // Impede regeneração de vida
        }
    }
}