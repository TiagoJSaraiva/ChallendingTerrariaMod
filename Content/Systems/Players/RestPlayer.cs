using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ID;
using ChallengingTerrariaMod.Content.Systems.Projectiles;
using ChallengingTerrariaMod.Content.Buffs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameInput;

namespace ChallengingTerrariaMod.Content.Systems.Players
{
    public class RestPlayer : ModPlayer
    {
        public float CurrentRest;
        public bool isFainted = false;
        private int faintedTimer = 0;

        public override void Initialize()
        {
            CurrentRest = 0;
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
            Player.GetModPlayer<RestPlayer>().Player.statManaMax2 += 0;
            Player.GetDamage(DamageClass.Magic) -= 0f;
            Player.GetDamage(DamageClass.Ranged) -= 0f;
        }

        public override void PreUpdate()
        {
            if (Player.active && !Player.dead && !Player.ghost)
            {
                if (isFainted)
                {
                    faintedTimer++;
                    if (faintedTimer >= 10 * 60)
                    {
                        isFainted = false;
                        faintedTimer = 0;
                        CurrentRest = Utils.Clamp(CurrentRest - 180, 0, 1000);
                        Player.ClearBuff(ModContent.BuffType<Fainted>());
                    }
                }
            }
        }

        public override bool CanUseItem(Item item)
        {
            if (Player.HasBuff(ModContent.BuffType<Fainted>())) return false;
            return base.CanUseItem(item);
        }

        public override void PostUpdateBuffs()
        {
            if (isFainted)
            {
                FaintedVFXDrawing();
                Player.controlRight = false;
                Player.controlLeft = false;
                Player.controlDown = false;
                Player.immuneAlpha = 220;
                Player.controlJump = false;
                Player.controlHook = false;
                Player.controlMount = false;
                Player.controlTorch = false;
            }
            else
            {
                Player.immuneAlpha = 0;
            }

            if (Main.GameUpdateCount % RestSystem.REST_UPDATE_RATE == 0)
            {
                if (Player.active && !Player.dead && !Player.ghost && !isFainted)
                {
                    ApplyRestDebuffs();
                }
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentRest", CurrentRest);
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
        }

        private void ApplyRestDebuffs()
        {
            if (Player.HasBuff(ModContent.BuffType<Exhausted>()) && Main.rand.NextFloat() < 0.2f && !Player.HasBuff(BuffID.Confused))
            {
                Player.AddBuff(BuffID.Confused, 3 * 60);
            }
            
            Player.ClearBuff(ModContent.BuffType<Tired>());
            Player.ClearBuff(ModContent.BuffType<Sleepy>());
            Player.ClearBuff(ModContent.BuffType<Exhausted>());
            Player.ClearBuff(ModContent.BuffType<Fainted>());

            if (CurrentRest >= 1000)
            {
                if (!isFainted)
                {
                    CurrentRest = 1000;
                    Player.AddBuff(ModContent.BuffType<Fainted>(), 10 * 60);
                    isFainted = true;
                    faintedTimer = 0;
                }
            }
            else if (CurrentRest >= 801)
            {
                Player.AddBuff(ModContent.BuffType<Exhausted>(), 61);
            }
            else if (CurrentRest >= 501)
            {
                Player.AddBuff(ModContent.BuffType<Sleepy>(), 61);
            }
            else if (CurrentRest >= 201)
            {
                Player.AddBuff(ModContent.BuffType<Tired>(), 61);
            }
        }
    }
}
