// Em ChallengingTerrariaMod/Content/Systems/Players/WarmthPlayer.cs

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using ChallengingTerrariaMod.Content.Systems;
using Terraria.DataStructures; 

// Adicione esta linha para acessar seus debuffs customizados
using ChallengingTerrariaMod.Content.Buffs; 

namespace ChallengingTerrariaMod.Content.Systems.Players
{
    public class WarmthPlayer : ModPlayer
    {
        public int CurrentTemperature;
        private int _temperatureBeforeDeath;
        public int LastTemperatureChange = 0;

        public override void Initialize()
        {
            CurrentTemperature = WarmthSystem.ComfortableTemperature;
            _temperatureBeforeDeath = WarmthSystem.ComfortableTemperature;
            LastTemperatureChange = 0;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageReason)
        {
            _temperatureBeforeDeath = CurrentTemperature;
        }

        public override void OnRespawn()
        {
            CurrentTemperature = WarmthSystem.ComfortableTemperature;
            LastTemperatureChange = 0;
            Main.NewText($"Sua temperatura foi restaurada para {WarmthSystem.ComfortableTemperature}.", Color.LightBlue);
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentTemperature", CurrentTemperature);
            tag.Add("TemperatureBeforeDeath", _temperatureBeforeDeath);
            tag.Add("LastTemperatureChange", LastTemperatureChange);
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("CurrentTemperature"))
            {
                CurrentTemperature = tag.GetInt("CurrentTemperature");
            }
            else
            {
                CurrentTemperature = WarmthSystem.ComfortableTemperature;
            }

            if (tag.ContainsKey("TemperatureBeforeDeath"))
            {
                _temperatureBeforeDeath = tag.GetInt("TemperatureBeforeDeath");
            }
            else
            {
                _temperatureBeforeDeath = WarmthSystem.ComfortableTemperature;
            }

            if (tag.ContainsKey("LastTemperatureChange"))
            {
                LastTemperatureChange = tag.GetInt("LastTemperatureChange");
            }
            else
            {
                LastTemperatureChange = 0;
            }
        }

        public override void PreUpdate()
        {
            WarmthSystem.PreviousTemperature[Player.whoAmI] = CurrentTemperature;
        }

        public override void PostUpdate()
        {
            // Aplica debuffs apenas na mesma taxa de atualização da temperatura
            if (Main.GameUpdateCount % WarmthSystem.TEMPERATURE_UPDATE_RATE == 0)
            {
                // Garante que o jogador está ativo e não morto/fantasma para aplicar debuffs
                if (Player.active && !Player.dead && !Player.ghost)
                {
                    ApplyWarmthDebuffs(); // Chama a nova função privada
                }
            }
        }

        // NOVO MÉTODO PRIVADO PARA APLICAÇÃO DE DEBUFFS DE TEMPERATURA
        private void ApplyWarmthDebuffs()
        {
            // --- APLICAÇÃO DE DEBUFFS CUSTOMIZADOS BASEADA NA TEMPERATURA ---

            // 1. Lógica para DEBUFFS DE FRIO
            // Primeiro, remova TODOS os buffs de frio para garantir que apenas o correto será aplicado
            Player.ClearBuff(ModContent.BuffType<Freezing>());
            Player.ClearBuff(ModContent.BuffType<VeryCold>());
            Player.ClearBuff(ModContent.BuffType<Cold>());

            if (CurrentTemperature <= 199) // 0 até 199: Freezing
            {
                Player.AddBuff(ModContent.BuffType<Freezing>(), 60);
            }
            else if (CurrentTemperature <= 499) // 200 até 499: Chilled
            {
                Player.AddBuff(ModContent.BuffType<VeryCold>(), 60);
            }
            else if (CurrentTemperature <= 799) // 500 até 799: Cold
            {
                Player.AddBuff(ModContent.BuffType<Cold>(), 60);
            }
            // Se a temperatura estiver acima de 799 (ou seja, fora de qualquer faixa de frio),
            // os buffs já terão sido limpos pelo ClearBuffs inicial e não serão reaplicados.


            // 2. Lógica para DEBUFFS DE CALOR
            // Primeiro, remova TODOS os buffs de calor para garantir que apenas o correto será aplicado
            Player.ClearBuff(ModContent.BuffType<Scorching>());
            Player.ClearBuff(ModContent.BuffType<Hot>());
            Player.ClearBuff(ModContent.BuffType<Warm>());

            if (CurrentTemperature >= 1800) // 1800 até 2000: Scorching
            {
                Player.AddBuff(ModContent.BuffType<Scorching>(), 60);
            }
            else if (CurrentTemperature >= 1500) // 1500 até 1799: Hot
            {
                Player.AddBuff(ModContent.BuffType<Hot>(), 60);
            }
            else if (CurrentTemperature >= 1200) // 1200 até 1499: Warm
            {
                Player.AddBuff(ModContent.BuffType<Warm>(), 60);
            }
            // Se a temperatura estiver abaixo de 1200 (ou seja, fora de qualquer faixa de calor),
            // os buffs já terão sido limpos pelo ClearBuffs inicial e não serão reaplicados.
        }
    }
}