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

        private bool normalized = false;

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
            bool inColdEnvironment = false;
            bool inWarmEnvironment = false;

            List<int> increments = new List<int>();

            
            if (player.wet && !player.lavaWet && !player.honeyWet)
            {
                inColdEnvironment = true;
            }

            if (!Main.dayTime && player.ZoneOverworldHeight)
            {
                increments.Add(-3);
                inColdEnvironment = true;
            }
            if (player.ZoneSnow)
            {
                increments.Add(-8);
                inColdEnvironment = true;
            }
            if (Main.raining && Main.cloudAlpha > 0.7f && player.ZoneOverworldHeight)
            {
                increments.Add(-5);
                inColdEnvironment = true;
            }
            else if (Main.raining && player.ZoneOverworldHeight)
            {
                increments.Add(-3);
                inColdEnvironment = true;
            }
        

            
            Point playerTileCoords = player.Center.ToTileCoordinates();
            int detectionRadiusTiles = (int)DETECTION_RADIUS_TILES;
            
            for (int x = playerTileCoords.X - detectionRadiusTiles; x <= playerTileCoords.X + detectionRadiusTiles; x++)
            {
                for (int y = playerTileCoords.Y - detectionRadiusTiles; y <= playerTileCoords.Y + detectionRadiusTiles; y++)
                {
                    if (!WorldGen.InWorld(x, y)) continue;

                    Tile tile = Main.tile[x, y];

                    if (tile.HasTile && (
                        tile.TileType == TileID.Furnaces ||
                        tile.TileType == TileID.Hellforge ||
                        tile.TileType == TileID.AdamantiteForge ||
                        tile.TileType == TileID.Fireplace ||
                        tile.TileType == TileID.GlassKiln ||
                        tile.TileType == TileID.LihzahrdFurnace ||
                        tile.TileType == TileID.Campfire
                    ))
                    {
                        inWarmEnvironment = true;
                        break;
                    }
                }
                if (inWarmEnvironment)
                {
                    break;
                }
            }

            if (player.ZoneBeach && Main.dayTime)
            {
                increments.Add(3);
                inWarmEnvironment = true;
            }
            if (player.ZoneDesert && Main.dayTime)
            {
                increments.Add(5); 
                inWarmEnvironment = true;
            }
            if (player.ZoneUnderworldHeight)
            {
                increments.Add(8);
                inWarmEnvironment = true;
            }
            if (player.ZoneSkyHeight)
            {
                increments.Add(40);
                inWarmEnvironment = true;
            }
        

            if (inColdEnvironment)
            {
                if (currentTemperature > ComfortableTemperature)
                {
                    if (player.HasBuff(BuffID.Warmth)) increments.Add(-4);
                    increments.Add(-20);
                }
            }

            if (inWarmEnvironment)
            {
                if (currentTemperature < ComfortableTemperature)
                {
                    if (player.HasBuff(BuffID.ObsidianSkin)) increments.Add(-3);
                    increments.Add(20);
                }
            }

            return increments.Sum();
        }
    }
}