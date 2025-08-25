using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using ChallengingTerrariaMod.Content.Consumables;

namespace ChallengingTerrariaMod.Content
{
    public class MeuModPlayer : ModPlayer
{
    public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
    {
        List<Item> items = new List<Item>();

        Item coffe = new Item();
        coffe.SetDefaults(ModContent.ItemType<CaffeinePill>());
        coffe.stack = 1;
        items.Add(coffe);

        Item pocao = new Item();
        pocao.SetDefaults(ItemID.Apple);
        pocao.stack = 10;
        items.Add(pocao);

        return items; // Retorna a lista de itens
    }
}
}