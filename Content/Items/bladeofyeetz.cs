using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using yeetz.Content.Projectiles;

namespace yeetz.Content.Items;

public class bladeofyeetz : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 10;
        Item.DamageType = DamageClass.Melee;
        Item.width = 48;
        Item.height = 48;
        Item.knockBack = 6;
        Item.useTime = 55;
        Item.useAnimation = 55;
        Item.useStyle = ItemUseStyleID.HiddenAnimation;
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.shoot = ModContent.ProjectileType<bladeofyeetzswing>();
        Item.shootSpeed = 1;
        Item.useTurn = false;
    }

    public override bool CanUseItem(Player player)
    {
        return player.ownedProjectileCounts[Item.shoot] == 0;
    }
}