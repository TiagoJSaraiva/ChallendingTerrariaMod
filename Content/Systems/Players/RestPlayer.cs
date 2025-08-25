using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using ChallengingTerrariaMod.Content.Projectiles;
using ChallengingTerrariaMod.Content.Buffs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameInput;
using System.Collections.Generic;
using Terraria.UI;

namespace ChallengingTerrariaMod.Content.Systems.Players
{
    public class RestPlayer : ModPlayer
    {
        public float CurrentRest;
        public int timeNoSleep;
        public int faintedThreshold = 0;
        public int exhaustedThreshold = 300;
        public int sleepyThreshold = 600;
        public int tiredThreshold = 900;


        public override void Initialize()
        {
            CurrentRest = 1200;
            timeNoSleep = 0;
        }

        public void FaintedVFXDrawing()
        {
            if (Main.myPlayer != Player.whoAmI) return; // Só no cliente local

            Vector2 spawnPos = Player.Center + new Vector2(0, -40); // Acima da cabeça

            int type = ModContent.ProjectileType<FaintedVFX>();

            // Garante que só haja 1 projétil do tipo por jogador
            bool alreadyExists = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == type)
                {
                    alreadyExists = true;
                    proj.Center = spawnPos;
                    proj.timeLeft = 2; // Mantém vivo
                }
            }

            if (!alreadyExists)
            {
                Projectile.NewProjectile(
                Player.GetSource_FromThis(),
                spawnPos,
                Vector2.Zero,
                type,
                0,
                0,
                Player.whoAmI
                );
            }
        }

        public override void ResetEffects()
        {

        }

        public override bool CanUseItem(Item item)
        {
            if (Player.HasBuff(ModContent.BuffType<Fainted>())) return false;
            return base.CanUseItem(item);
        }

        public override void PostUpdateBuffs()
        {
            if (!Player.HasBuff(ModContent.BuffType<Fainted>()))
            {
                Player.immuneAlpha = 0;
            }

            if (Main.GameUpdateCount % RestSystem.restUpdateRate == 0)
            {
                if (Player.active && !Player.dead && !Player.ghost && !Player.HasBuff(ModContent.BuffType<Fainted>()))
                {
                    timeNoSleep++;
                    ApplyRestDebuffs();
                }
            }
        }


        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentRest", CurrentRest);
            tag.Add("timeNoSleep", timeNoSleep);
        }

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
            if (tag.ContainsKey("timeNoSleep"))
            {
                timeNoSleep = tag.GetInt("timeNoSleep");
            }
            else
            {
                timeNoSleep = 0; // Valor padrão se não houver dados salvos
            }
        }

        private void ApplyRestDebuffs()
        {
            if (timeNoSleep >= 2400) // MUDAR PARA 2400
            {
                Main.NewText("Your sleep routine has been terrible. Your body is starting to feel the consequences.", Color.LightBlue);
                Player.AddBuff(ModContent.BuffType<SleepDeprived>(), 600 * 60);
                timeNoSleep = 0;
            }

            if (Player.HasBuff(ModContent.BuffType<Exhausted>()) && Main.rand.NextFloat() < 0.05f && !Player.HasBuff(BuffID.Confused))
            {
                Player.AddBuff(BuffID.Confused, 2 * 60);
            }

            Player.ClearBuff(ModContent.BuffType<Tired>());
            Player.ClearBuff(ModContent.BuffType<Sleepy>());
            Player.ClearBuff(ModContent.BuffType<Exhausted>());

            if (CurrentRest <= faintedThreshold)
            {
                CurrentRest = exhaustedThreshold;
                Player.AddBuff(ModContent.BuffType<Fainted>(), 10 * 60);
            }
            else if (CurrentRest <= exhaustedThreshold)
            {
                Player.AddBuff(ModContent.BuffType<Exhausted>(), 61);
            }
            else if (CurrentRest <= sleepyThreshold)
            {
                Player.AddBuff(ModContent.BuffType<Sleepy>(), 61);
            }
            else if (CurrentRest <= tiredThreshold)
            {
                Player.AddBuff(ModContent.BuffType<Tired>(), 61);
            }
        }
    }
}
