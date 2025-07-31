// Em ChallengingTerrariaMod/Content/Buffs/Fainted.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using ChallengingTerrariaMod.Content.Systems.Players; // Para acessar RestPlayer

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Fainted : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fainted");
            // Description.SetDefault("You fainted."); // A descrição curta, já que o jogador não pode fazer nada

            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false; // Exibe o tempo restante
            // Este buff não deve ser removido pelo jogador (clicando com o botão direito)
            // Main.buffNoRemove[Type] = true; // Isso impede a remoção manual
        }

        public override void Update(Player player, ref int buffIndex)
        {
            RestPlayer restPlayer = player.GetModPlayer<RestPlayer>();

            if (restPlayer != null)
            {
                // Ativa a flag de desmaio no ModPlayer
                restPlayer.isFainted = true;

                // Força o sprite do jogador a ficar deitado (se possível)
                // Isso pode exigir um pouco mais de trabalho no ModPlayer ou em um DrawLayer.
                // Por enquanto, o player.dead ou player.frozen pode dar uma ideia, mas não é exatamente "deitado".
                // Para um sprite deitado, você precisaria de um custom DrawLayer ou modificar a animação.
                // player.immune = true; // Pode dar invulnerabilidade enquanto desmaiado (opcional)
                // player.immuneTime = 2; // Pequena invulnerabilidade a cada tick para evitar dano constante

                // A lógica de desativação de controles e redução de sono após 10 segundos
                // está sendo tratada no RestPlayer.PreUpdate() para garantir que
                // o controle seja retomado e o sono reduzido APÓS a duração do buff.
                // O buff em si apenas sinaliza o estado.

                // Se o buff for removido antes do tempo (ex: por outro mod ou comando),
                // garantir que o estado de desmaio seja resetado.
                if (player.buffTime[buffIndex] == 0)
                {
                    restPlayer.isFainted = false;
                }
            }
        }
    }
}