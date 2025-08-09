using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using ChallengingTerrariaMod.Content.Systems.Players;
using Terraria.ID;
using ChallengingTerrariaMod.Content.Buffs;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace ChallengingTerrariaMod.Content.Systems.Players
{
    public class HungerPlayer : ModPlayer
    {
        public float CurrentHunger; 
        private bool foodItemConsumedThisTick; 

        private const int RespawnHungerValue = 400; 

        public override void Initialize()
        {
            CurrentHunger = HungerSystem.MaxHungerNormal;
        }

        public override void PreUpdate()
        {
            if (Player.active && !Player.dead && !Player.ghost)
            {
                // Lógica de Vômito (saciedade extrema)
                if (CurrentHunger >= HungerSystem.AbsoluteMaxHunger)
                {
                    CurrentHunger = RespawnHungerValue;
                    Player.AddBuff(ModContent.BuffType<Nauseous>(), 3 * 10 * 60);
                    Player.AddBuff(ModContent.BuffType<ThrowingUp>(), 2 * 60);

                    Player.ClearBuff(BuffID.WellFed);
                    Player.ClearBuff(BuffID.WellFed2);
                    Player.ClearBuff(BuffID.WellFed3);

                    for (int i = 0; i < 20; i++)
                    {
                        Dust.NewDust(Player.position, Player.width, Player.height, DustID.Smoke,
                                     Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f),
                                     100, new Color(0, 255, 0, 100), Main.rand.NextFloat(1.5f, 2.5f));
                    }
                }

                if (Main.GameUpdateCount % HungerSystem.HungerTickRate == 0)
                {
                    CurrentHunger -= HungerSystem.HungerDecrementIdle;

                    if (Player.velocity.X != 0 || Player.velocity.Y != 0)
                    {
                        CurrentHunger -= HungerSystem.HungerDecrementMoving;
                    }

                    CurrentHunger = Utils.Clamp(CurrentHunger, 0, HungerSystem.AbsoluteMaxHunger);

                    ApplyHungerDebuffs();
                }
            }
        }

        public override void OnRespawn()
        {
            CurrentHunger = RespawnHungerValue;
        }

        public override bool CanUseItem(Item item)
        {
            bool isFoodBuffItem = item.buffType == BuffID.WellFed ||
                                  item.buffType == BuffID.WellFed2 ||
                                  item.buffType == BuffID.WellFed3;

            if (item.consumable && isFoodBuffItem)
            {
                if (Player.HasBuff(ModContent.BuffType<Nauseous>()) || Player.HasBuff(ModContent.BuffType<Fainted>()))
                {
                    return false; 
                }
                foodItemConsumedThisTick = true;
                return true; 
            }

            if (Player.HasBuff(ModContent.BuffType<Starved>()))
            {
                if (item.damage > 0 || item.pick > 0 || item.axe > 0 || item.hammer > 0)
                {
                    return false; 
                }
            }
            return base.CanUseItem(item);
        }

        public override void PostUpdateBuffs()
        {
            if (foodItemConsumedThisTick)
            {

                foodItemConsumedThisTick = false;

                int hungerRestored = 0;

                if (Player.HasBuff(BuffID.WellFed3) && Player.buffTime[Player.FindBuffIndex(BuffID.WellFed3)] > 1) // >1 para evitar buffs recém removidos
                {
                    hungerRestored = 350;
                }
                else if (Player.HasBuff(BuffID.WellFed2) && Player.buffTime[Player.FindBuffIndex(BuffID.WellFed2)] > 1)
                {
                    hungerRestored = 150;
                }
                else if (Player.HasBuff(BuffID.WellFed) && Player.buffTime[Player.FindBuffIndex(BuffID.WellFed)] > 1)
                {
                    hungerRestored = 50;
                }

                if (hungerRestored > 0)
                {
                    CurrentHunger += hungerRestored;
                    CurrentHunger = Utils.Clamp(CurrentHunger, 0, HungerSystem.AbsoluteMaxHunger);

                    if (CurrentHunger < HungerSystem.HungerDebuffThreshold_Peckish)
                    {
                        for (int i = 0; i < Player.buffType.Length; i++)
                        {
                            int buffType = Player.buffType[i];
                            if (buffType == BuffID.WellFed ||
                                buffType == BuffID.WellFed2 ||
                                buffType == BuffID.WellFed3)
                            {
                                Player.buffTime[i] = 10 * 60;
                            }
                        }
                    }
                }
            }
        }


        private void ApplyHungerDebuffs()
        {
            int buffDuration = 120; 

            Player.ClearBuff(ModContent.BuffType<Stuffed>());
            Player.ClearBuff(ModContent.BuffType<Full>());
            Player.ClearBuff(ModContent.BuffType<Peckish>());
            Player.ClearBuff(ModContent.BuffType<Hungry>());
            Player.ClearBuff(ModContent.BuffType<Famished>());
            Player.ClearBuff(ModContent.BuffType<Starved>());

            if (CurrentHunger >= HungerSystem.MaxHungerDebuffThreshold_Bloated)
            {
                Player.AddBuff(ModContent.BuffType<Bloated>(), buffDuration);
            }
            else if (CurrentHunger >= HungerSystem.MaxHungerDebuffThreshold_Stuffed)
            {
                Player.AddBuff(ModContent.BuffType<Stuffed>(), buffDuration);
            }
            else if (CurrentHunger >= HungerSystem.MaxHungerDebuffThreshold_Full)
            {
                Player.AddBuff(ModContent.BuffType<Full>(), buffDuration);

            }
            else if (CurrentHunger <= HungerSystem.HungerDebuffThreshold_Starved)
            {
                Player.AddBuff(ModContent.BuffType<Starved>(), buffDuration);
            }
            else if (CurrentHunger <= HungerSystem.HungerDebuffThreshold_Famished)
            {
                Player.AddBuff(ModContent.BuffType<Famished>(), buffDuration);
            }
            else if (CurrentHunger <= HungerSystem.HungerDebuffThreshold_Hungry)
            {
                Player.AddBuff(ModContent.BuffType<Hungry>(), buffDuration);
            }
            else if (CurrentHunger <= HungerSystem.HungerDebuffThreshold_Peckish)
            {
                Player.AddBuff(ModContent.BuffType<Peckish>(), buffDuration);
            }
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("CurrentHunger", CurrentHunger);
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("CurrentHunger"))
            {
                CurrentHunger = tag.GetFloat("CurrentHunger");
            }
        }
    }
}