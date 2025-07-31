// Em ChallengingTerrariaMod/Content/Systems/Players/RestPlayer.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO; // Necessário para SaveData e LoadData
using Terraria.ID; // Necessário para BuffID
using ChallengingTerrariaMod.Content.Systems; // Para acessar RestSystem e suas constantes
using ChallengingTerrariaMod.Content.Buffs; // Para os buffs customizados
using Terraria.DataStructures; // Necessário para PlayerDeathReason (embora não usado diretamente aqui, é comum em ModPlayer)
using Microsoft.Xna.Framework; // Necessário para Color (se for usar para mensagens, etc.)

namespace ChallengingTerrariaMod.Content.Systems.Players
{
    public class RestPlayer : ModPlayer
    {
        public float CurrentRest; // Medidor de sono, de 0 a 1000

        // Flags para controlar o estado do jogador quando "Fainted"
        public bool isFainted = false;
        private int faintedTimer = 0; // Contagem de ticks para o debuff Fainted

        public override void Initialize()
        {
            CurrentRest = 0; // Começa com 0 de sono
        }

        public override void ResetEffects()
        {
            // Resetar modificadores de estatísticas de buffs personalizados no início de cada tick
            // (Os buffs em si controlam a duração e aplicação, aqui apenas garantimos que os efeitos são aplicados se o buff estiver ativo)
            Player.GetModPlayer<RestPlayer>().Player.statManaMax2 += 0; // Exemplo de reset, os buffs farão as mudanças reais
            Player.GetDamage(DamageClass.Magic) -= 0f; // Exemplo
            Player.GetDamage(DamageClass.Ranged) -= 0f; // Exemplo
        }

        public override void PreUpdate()
        {
            // Garante que o jogador está ativo e não morto/fantasma para processar o sono
            if (Player.active && !Player.dead && !Player.ghost)
            {
                // Se o jogador estiver desmaiado, forçamos que ele não se mova ou use itens
                if (isFainted)
                {
                    Player.controlLeft = false;
                    Player.controlRight = false;
                    Player.controlUp = false;
                    Player.controlDown = false;
                    Player.controlJump = false;
                    Player.controlHook = false;
                    Player.controlUseItem = false;
                    Player.controlUseTile = false;
                    Player.controlThrow = false;
                    Player.gravDir = 1f; // Garante que ele caia
                    Player.velocity.X = 0f; // Impede movimento horizontal

                    // Controla a duração do debuff Fainted
                    faintedTimer++;
                    if (faintedTimer >= 10 * 60) // 10 segundos * 60 ticks/segundo
                    {
                        isFainted = false;
                        faintedTimer = 0;
                        CurrentRest = Utils.Clamp(CurrentRest - 180, 0, 1000); // Reduz 180 pontos de sono
                        Player.ClearBuff(ModContent.BuffType<Fainted>()); // Remove o buff Fainted
                        // O jogador recupera o controle automaticamente quando isFainted volta a ser false
                    }
                }
            }
        }

        public override void PostUpdateBuffs()
        {
            // Aplica debuffs de sono uma vez por segundo
            if (Main.GameUpdateCount % RestSystem.REST_UPDATE_RATE == 0)
            {
                if (Player.active && !Player.dead && !Player.ghost && !isFainted) // Não aplica outros debuffs se estiver desmaiado
                {
                    ApplyRestDebuffs();
                }
            }
        }

        // Método para salvar os dados do jogador
        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentRest", CurrentRest);
        }

        // Método para carregar os dados do jogador
        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("CurrentRest"))
            {
                CurrentRest = tag.GetFloat("CurrentRest");
            }
            else
            {
                CurrentRest = 0; // Valor padrão se não houver dados salvos
            }
        }

        // Método privado para aplicar os buffs de sono
        private void ApplyRestDebuffs()
        {
            // Limpa todos os buffs de sono personalizados para garantir que apenas o correto seja aplicado
            Player.ClearBuff(ModContent.BuffType<Tired>());
            Player.ClearBuff(ModContent.BuffType<Sleepy>());
            Player.ClearBuff(ModContent.BuffType<Exhausted>());
            Player.ClearBuff(ModContent.BuffType<Fainted>()); // Embora Fainted tenha sua própria lógica, é bom limpar aqui também

            // Lógica de aplicação dos buffs
            if (CurrentRest >= 1000)
            {
                // Se o jogador atingir 1000, e não estiver já desmaiado
                if (!isFainted)
                {
                    CurrentRest = 1000; // Garante que o valor não exceda o máximo
                    Player.AddBuff(ModContent.BuffType<Fainted>(), 10 * 60); // Aplica Fainted por 10 segundos
                    isFainted = true;
                    faintedTimer = 0; // Reinicia o timer do desmaio
                    // A lógica de controle e perda de sono após o desmaio está no PreUpdate
                }
            }
            else if (CurrentRest >= 801)
            {
                Player.AddBuff(ModContent.BuffType<Exhausted>(), 60); // Dura 1 segundo, atualizado a cada segundo

     
                if (Main.rand.NextFloat() < 0.10f) // 10% de chance
                {
                    if (!Player.HasBuff(BuffID.Confused)) 
                    {
                        Player.AddBuff(BuffID.Confused, 10 * 60); // 10 segundos
                    }
                }
            }
            else if (CurrentRest >= 501)
            {
                Player.AddBuff(ModContent.BuffType<Sleepy>(), 60);
            }
            else if (CurrentRest >= 201)
            {
                Player.AddBuff(ModContent.BuffType<Tired>(), 60);
            }
            // Se estiver entre 0 e 200, os buffs são limpos, então nenhum é adicionado.
        }
    }
}