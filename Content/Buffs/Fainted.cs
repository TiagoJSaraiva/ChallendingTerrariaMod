// Em ChallengingTerrariaMod/Content/Buffs/Fainted.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using ChallengingTerrariaMod.Content.Systems.Players; // Para acessar RestPlayer

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Fainted : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            RestPlayer restPlayer = player.GetModPlayer<RestPlayer>();
            restPlayer.FaintedVFXDrawing();
            player.controlRight = false;
            player.controlLeft = false;
            player.controlDown = false;
            player.immuneAlpha = 110;
            player.controlJump = false;
            player.controlHook = false;
            player.controlMount = false;
            player.controlTorch = false;
        }
    }

    public class FaintedPlayer : ModPlayer
    {
        public override bool CanUseItem(Item item)
        {
            if (Player.HasBuff(ModContent.BuffType<Fainted>())) return false;
            return base.CanUseItem(item);
        }
    }
}