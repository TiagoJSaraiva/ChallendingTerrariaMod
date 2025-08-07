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

        // Sleep gain and loss of the player
        private const float SLEEP_GAIN_PER_SECOND = 3; // Tiredness gained per second at night while not sleeping
        private const float SLEEP_LOSS_PER_SECOND = 3; // Tiredness loss per second when the time isn't accelerated (in case of there being more than one player in the same world, and only one of them sleeping)

        private const float SLEEP_LOSS_PER_SECOND_ACCELERATED = 36f; // Tiredness loss per second when the time is accelerated

        // Removeremos as constantes de horário específicas, pois não serão mais usadas para a lógica de ganho.
        // private const double START_SLEEP_GAIN_TIME = 19.5; 
        // private const double END_SLEEP_GAIN_TIME = 4.5; 

        // UI do sono
        public static UserInterface RestUserInterface;
        public static RestMeterUI RestUIState; 

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

                        // Lógica de atualização do sono
                        if (player.sleeping.isSleeping)
                        {
                            restPlayer.timeNoSleep -= 10;
                            restPlayer.timeNoSleep = Utils.Clamp(restPlayer.timeNoSleep, 0, 0);
                            if (Main.dayRate > 1)
                            {
                                restPlayer.CurrentRest -= SLEEP_LOSS_PER_SECOND_ACCELERATED;
                            }
                            else
                            {
                                restPlayer.CurrentRest -= SLEEP_LOSS_PER_SECOND;
                            }
                        } else if (!Main.dayTime) // AGORA VERIFICA APENAS SE É NOITE GERAL
                        {
                            // Se NÃO estiver dormindo na cama E for noite (qualquer hora da noite), ele ganha sono.
                            restPlayer.CurrentRest += SLEEP_GAIN_PER_SECOND;
                        }
                        // Se não estiver dormindo na cama E for dia (Main.dayTime é true), o sono não muda.
                        
                        // Garante que o sono esteja dentro dos limites
                        restPlayer.CurrentRest = Utils.Clamp(restPlayer.CurrentRest, 0, 1008);
                    }
                }
            }
        }
    }
}