using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using yeetz.Content.Items;

namespace yeetz.Content.Projectiles;

public class bladeofyeetzswing : ModProjectile
{
    public override bool ShouldUpdatePosition() => false;
    public override string Texture => "Terraria/Images/Item_" + ItemID.Excalibur;
    public override void SetDefaults()
    {
        Projectile.aiStyle = -1;
        Projectile.Size = new Vector2(48, 48);
        Projectile.DamageType = DamageClass.Melee;
        Projectile.friendly = true;
        Projectile.damage = 50;
        Projectile.knockBack = 6;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.ownerHitCheck = true;
    }

    public override void AI()
    {
        base.AI();
        Player player = Main.player[Projectile.owner];
        float offset = Utils.MultiLerp(MathHelper.SmoothStep(0, 1, Utils.GetLerpValue(55, 0, player.itemAnimation, true)), -3.4f, 2.4f, 2f, 2f, -3);
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4 + offset * player.direction;
        player.heldProj = Projectile.whoAmI;
        player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2 * 1.5f);
        Projectile.Center = player.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2 * 1.5f) + (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 27;
        if (MathF.Sign(Projectile.velocity.X) != player.direction)
            Projectile.velocity.X *= player.direction;
        if (!player.ItemTimeIsZero && player.active && !player.dead && !player.CCed && player.HeldItem.type == ModContent.ItemType<bladeofyeetz>())
        {
            Projectile.timeLeft = 2;
            
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex0 = TextureAssets.Projectile[Type].Value;
        Main.EntitySpriteDraw(tex0, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + (Projectile.direction == -1 ? MathF.PI * 1.5f : 0), Projectile.Size / 2, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None);
        return false;
    }
}