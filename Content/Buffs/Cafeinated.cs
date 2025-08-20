// ChallengingTerrariaMod/Content/Buffs/Full.cs
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent;
using ChallengingTerrariaMod.Content.Systems.Players;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Cafeinated : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}