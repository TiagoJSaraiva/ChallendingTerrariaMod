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
            // DisplayName.SetDefault("Nauseous");
            // Description.SetDefault("You feel extremely nauseous. Armor is heavily reduced, and life/mana regeneration is halted. You cannot eat.");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= 10; // Reduz armadura em 10
            player.GetDamage(DamageClass.Default) *= 0.30f;
            player.lifeRegen = 0; // Impede regeneração de vida
        }
    }
}