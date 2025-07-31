using Terraria;
using Terraria.ModLoader;
using Terraria.ID; 
using ChallengingTerrariaMod.Content.Systems.Players;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.UI;
using Terraria.Localization;
using ChallengingTerrariaMod.Content.Systems.UI; 
using System;

namespace ChallengingTerrariaMod.Content.Systems
{
    public class RestSystem : ModSystem
    {
        public const int REST_UPDATE_RATE = 60; // Atualiza a cada 60 ticks (1 segundo real)

        // Taxas de sono (já calculadas para "por segundo real")
        private const float SLEEP_GAIN_PER_SECOND = 3f;   // 180 pontos / 60 segundos
        private const float SLEEP_LOSS_PER_SECOND = 36f; // 180 pontos / 5 segundos (tempo acelerado)

        // Horários do Terraria (em ticks, 1 minuto real = 3600 ticks do jogo)
        // 7:30 PM (19:30): Início do ganho de sono
        private const double START_SLEEP_GAIN_TIME = (19.5 * 3600); // 19h30 * 3600 ticks/hora
        // 4:30 AM (4:30): Fim do ganho de sono
        private const double END_SLEEP_GAIN_TIME = (4.5 * 3600);   // 4h30 * 3600 ticks/hora

        // UI do sono
        public static UserInterface RestUserInterface;
        public static RestMeterUI RestUIState; // Precisaremos criar esta classe

        public override void Load()
        {
            if (!Main.dedServ)
            {
                RestUIState = new RestMeterUI();
                RestUIState.Activate();
                RestUserInterface = new UserInterface();
                RestUserInterface.SetState(RestUIState);
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ)
            {
                RestUIState = null;
                RestUserInterface = null;
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (RestUserInterface?.CurrentState != null)
            {
                RestUserInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "ChallengingTerrariaMod: Rest UI",
                    delegate
                    {
                        // Desenha a UI apenas se o jogador local estiver ativo e não morto/fantasma
                        if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
                        {
                            RestUserInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void PostUpdatePlayers()
        {
            // A lógica de atualização do sono só acontece a cada segundo (60 ticks)
            if (Main.GameUpdateCount % REST_UPDATE_RATE == 0)
            {
                foreach (Player player in Main.player)
                {
                    if (player.active && !player.dead && !player.ghost)
                    {
                        RestPlayer restPlayer = player.GetModPlayer<RestPlayer>();
                        if (restPlayer == null) continue;

                        // Lógica de ganho de sono
                        double currentAbsoluteGameTimeInTicks = Main.time;
                        if (!Main.dayTime)
                        {
                            currentAbsoluteGameTimeInTicks += Main.dayLength;
                        }
                        
                        double currentHour = currentAbsoluteGameTimeInTicks / 3600.0;

                        bool isSleepTime = false;
                        if (currentHour >= 19.5 || currentHour < 4.5)
                        {
                            isSleepTime = true;
                        }

                        if (isSleepTime)
                        {
                            restPlayer.CurrentRest += SLEEP_GAIN_PER_SECOND;
                        }

                        // Lógica de perda de sono (dormir na cama)
                        // CORREÇÃO: Usar player.sleeping.isSleeping
                        if (player.sleeping.isSleeping) // Verifica proximidade da cama
                        {
                            restPlayer.CurrentRest -= SLEEP_LOSS_PER_SECOND;
                        }
                        
                        // Garante que o sono esteja dentro dos limites
                        restPlayer.CurrentRest = Utils.Clamp(restPlayer.CurrentRest, 0, 1000);
                    }
                }
            }
        }
    }
}