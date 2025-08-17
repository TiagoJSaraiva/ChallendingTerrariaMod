// ChallengingTerrariaMod/Content/Buffs/Nauseous.cs
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChallengingTerrariaMod.Content.Buffs
{
    public class Nauseous : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {

        }
    }
}