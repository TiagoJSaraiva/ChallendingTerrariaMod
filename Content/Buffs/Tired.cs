// Em ChallengingTerrariaMod/Content/Buffs/Tired.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.Localization; // Para LocalizedText

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Tired : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // Nome e descrição do buff
            // DisplayName.SetDefault("Tired");
            // Description.SetDefault("You are tired and can't focus. Magic and Ranged damage slightly reduced. Mana regen reduced.");

            // Propriedades do buff
            Main.debuff[Type] = true;  // É um debuff
            Main.pvpBuff[Type] = true; // Afeta em PvP
            Main.buffNoSave[Type] = true; // Não salva ao sair do jogo
            Main.buffNoTimeDisplay[Type] = false; // Exibe o tempo restante
            // Não é necessário Main.vanityPet[Type], Main.lightPet[Type], etc.
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Redução de dano
            player.GetDamage(DamageClass.Magic) -= 0.10f; // -10% de dano mágico
            player.GetDamage(DamageClass.Ranged) -= 0.10f; // -10% de dano ranged

            // Redução da regeneração de mana
            // A regeneração de mana é um pouco complexa no Terraria.
            // Uma forma de reduzi-la é diminuir o bônus de regeneração passiva ou aumentar o tempo de espera.
            // Para uma "redução leve", podemos diminuir o multiplicador de regeneração.
            // player.manaRegenDelayMax += 5; // Aumenta o delay para começar a regenerar
            // player.manaRegenBonus -= 1; // Reduz o bônus de regeneração
            // Ou, uma abordagem mais direta é modificar o player.manaRegen
            player.manaRegenCount -= (int)(player.manaRegenCount * 0.25f); // Reduz em 25% a contagem de regeneração
            // Isso fará com que a mana regenere mais lentamente. Ajuste o 0.25f conforme o desejado para "levemente".
        }
    }
}