// Em ChallengingTerrariaMod/Content/Systems/WarmthSystem.cs

using Terraria;
using Terraria.ModLoader;
using ChallengingTerrariaMod.Content.Systems.Players;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using Terraria.UI;
using Terraria.Localization;
using ChallengingTerrariaMod.Content.Systems.UI;
using Terraria.DataStructures;
using System;
using ChallengingTerrariaMod.Content.Buffs; // Esta linha é necessária novamente para verificar buffs personalizados

namespace ChallengingTerrariaMod.Content.Systems
{
    public class WarmthSystem : ModSystem
    {
        public const int TEMPERATURE_UPDATE_RATE = 60; // 60 ticks = 1 segundo
        private const float DETECTION_RADIUS_TILES = 15f;

        // Constantes de temperatura
        public const int ComfortableTemperature = 1000;
        public const int MinTemperature = 0;
        public const int MaxTemperature = 2000;

        // Fator de normalização 
        private const int NORMALIZATION_RATE = 10;

        // Nova variável para armazenar a temperatura anterior de CADA jogador
        // Usamos um array pois pode haver múltiplos jogadores.
        public static int[] PreviousTemperature;

        // Variáveis da UI
        public static UserInterface WarmthUserInterface;
        public static WarmthMeterUI WarmthUIState;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                WarmthUIState = new WarmthMeterUI();
                WarmthUserInterface = new UserInterface();
                WarmthUserInterface.SetState(WarmthUIState);
            }
            // Inicializa o array de temperaturas anteriores
            PreviousTemperature = new int[Main.maxPlayers];
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                PreviousTemperature[i] = ComfortableTemperature;
            }
        }

        public override void Unload()
        {
            if (!Main.dedServ)
            {
                WarmthUIState = null;
                WarmthUserInterface = null;
            }
            PreviousTemperature = null;
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (WarmthUserInterface?.CurrentState != null)
            {
                WarmthUserInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "ChallengingTerrariaMod: Warmth UI",
                    delegate
                    {
                        // Desenha a UI apenas se o jogador local estiver ativo e não morto/fantasma
                        if (Main.LocalPlayer.active && !Main.LocalPlayer.dead && !Main.LocalPlayer.ghost)
                        {
                            WarmthUserInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public override void PostUpdatePlayers()
        {
            if (PreviousTemperature == null || PreviousTemperature.Length < Main.maxPlayers)
            {
                PreviousTemperature = new int[Main.maxPlayers];
            }

            if (Main.GameUpdateCount % TEMPERATURE_UPDATE_RATE == 0)
            {
                foreach (Player player in Main.player)
                {
                    if (player.active && !player.dead && !player.ghost)
                    {
                        WarmthPlayer warmthPlayer = player.GetModPlayer<WarmthPlayer>();
                        if (warmthPlayer == null) continue;

                        int currentTemperatureIncrement = CalculateTemperatureIncrement(player, warmthPlayer.CurrentTemperature);
                        
                        // Aplica o incremento calculado das fontes ambientais/de buff
                        warmthPlayer.CurrentTemperature += currentTemperatureIncrement;

                        // --- Lógica de Morte por Temperatura Extrema ---
                        if (warmthPlayer.CurrentTemperature >= MaxTemperature) // 2000
                        {
                            player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral(player.name + " turned to ashes")), 9999, 0);
                            warmthPlayer.CurrentTemperature = ComfortableTemperature;
                            continue;
                        }
                        else if (warmthPlayer.CurrentTemperature <= MinTemperature) // 0
                        {
                            player.KillMe(PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral(player.name + "'s vital organs turned to ice")), 9999, 0);
                            warmthPlayer.CurrentTemperature = ComfortableTemperature;
                            continue;
                        }

                        // Aplica normalização APENAS se não houver incremento de fontes externas
                        if (currentTemperatureIncrement == 0)
                        {
                            if (warmthPlayer.CurrentTemperature > ComfortableTemperature)
                            {
                                warmthPlayer.CurrentTemperature -= NORMALIZATION_RATE;
                                if (warmthPlayer.CurrentTemperature < ComfortableTemperature)
                                {
                                    warmthPlayer.CurrentTemperature = ComfortableTemperature;
                                }
                            }
                            else if (warmthPlayer.CurrentTemperature < ComfortableTemperature)
                            {
                                warmthPlayer.CurrentTemperature += NORMALIZATION_RATE;
                                if (warmthPlayer.CurrentTemperature > ComfortableTemperature)
                                {
                                    warmthPlayer.CurrentTemperature = ComfortableTemperature;
                                }
                            }
                        }

                        warmthPlayer.CurrentTemperature = Utils.Clamp(warmthPlayer.CurrentTemperature, MinTemperature, MaxTemperature);

                        // --- ATUALIZAÇÃO DA MUDANÇA DE TEMPERATURA PARA O SPRITE ---
                        int currentTemperatureAfterUpdate = warmthPlayer.CurrentTemperature;
                        warmthPlayer.LastTemperatureChange = currentTemperatureAfterUpdate - PreviousTemperature[player.whoAmI];
                        PreviousTemperature[player.whoAmI] = currentTemperatureAfterUpdate;
                    }
                }
            }
        }

        private int CalculateTemperatureIncrement(Player player, int currentTemperature)
        {
            bool hotSourceDetected;
            bool inWaterLiquid;

            List<int> increments = new List<int>();

            Point playerTileCoords = player.Center.ToTileCoordinates();
            int detectionRadiusTiles = (int)DETECTION_RADIUS_TILES;

            // Fontes de Calor (Fogueira, Fornalhas, etc.) - apenas se currentTemperature < ComfortableTemperature
            
            hotSourceDetected = false;
            for (int x = playerTileCoords.X - detectionRadiusTiles; x <= playerTileCoords.X + detectionRadiusTiles; x++)
            {
                for (int y = playerTileCoords.Y - detectionRadiusTiles; y <= playerTileCoords.Y + detectionRadiusTiles; y++)
                {
                    if (!WorldGen.InWorld(x, y)) continue;

                    Tile tile = Main.tile[x, y];

                    if (tile.HasTile && (
                        tile.TileType == TileID.Furnaces ||
                        tile.TileType == TileID.Hellforge ||
                        tile.TileType == TileID.AdamantiteForge || // Inclui Adamantite e Titanium Forge
                        tile.TileType == TileID.Fireplace ||
                        tile.TileType == TileID.GlassKiln ||
                        tile.TileType == TileID.LihzahrdFurnace ||
                        tile.TileType == TileID.Campfire
                    ))
                    {
                        hotSourceDetected = true;
                        if (currentTemperature < ComfortableTemperature)
                        {
                            increments.Add(20);
                        }
                        break;
                    }
                }
                if (hotSourceDetected) {
                    break;
                }
            }

            // Dentro da Água - apenas se currentTemperature > ComfortableTemperature
            
            inWaterLiquid = false;

            if (player.wet && !player.lavaWet && !player.honeyWet)
            {
                inWaterLiquid = true;
                if (currentTemperature > ComfortableTemperature)
                {
                    increments.Add(-20);
                }  
            }
            
            // --- Fontes Primárias ---

            // A noite quando na superfície
            if (!Main.dayTime && player.ZoneOverworldHeight && !hotSourceDetected)
            {
                increments.Add(-10);
            }
            // Na tundra (qualquer camada)
            if (player.ZoneSnow && !hotSourceDetected)
            {
                increments.Add(-15);
            }
            // Durante Chuvas quando na superfície
                // Detecção de Tempestade: Chuva forte (Main.raining) + Cobertura de nuvens intensa (Main.cloudAlpha)
                if (Main.raining && Main.cloudAlpha > 0.7f && player.ZoneOverworldHeight && !hotSourceDetected)
                {
                    increments.Add(-10); // Tempestade
                }
                else if (Main.raining && player.ZoneOverworldHeight && !hotSourceDetected)
                {
                    increments.Add(-5); // Chuva normal
                }

            // Sob efeito de Buffs
            if (player.HasBuff(BuffID.OnFire))
            {
                increments.Add(10);
            }
            if (player.HasBuff(BuffID.Frostburn))
            {
                increments.Add(-10);
            }
            if (player.HasBuff(BuffID.Chilled)) 
            {
                increments.Add(-15);
            }

            // Na praia de dia
            if (player.ZoneBeach && Main.dayTime && !inWaterLiquid)
            {
                increments.Add(10);
            }
            // No deserto de dia
            if (player.ZoneDesert && Main.dayTime && !inWaterLiquid)
            {
                increments.Add(15);
            }
            // No Inferno
            if (player.ZoneUnderworldHeight)
            {
                increments.Add(20);
            }
            // No Espaço (ZoneSkyHeight é a altura do espaço)
            if (player.ZoneSkyHeight)
            {
                increments.Add(55);
            }

            return increments.Sum();
        }
    }
}