using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace yeetz.Content.Projectiles;

public class stinkcloud : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ToxicCloud3;

    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(28, 30);
        Projectile.aiStyle = -1;
        Projectile.hostile = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.timeLeft = 200;
        Projectile.Opacity = 0;
    }

    public override void AI()
    {
        base.AI();
        Projectile.ai[1]++;
        Projectile.velocity *= 0.97f;
        Projectile.rotation += Projectile.ai[0];
        if (Projectile.ai[1] >= 0 && Projectile.ai[1] < 15)
        {
            if (Projectile.ai[1] % 1 == 0)
            {
                Projectile.Opacity += 0.04f;
            }
        }

        if (Projectile.ai[1] >= 15)
        {
            if (Projectile.ai[1] % 30 == 0)
            {
                Projectile.Opacity -= 0.05f;
            }
        }
        for (int i = 0; i < 1; i++)
        {
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, Projectile.velocity.X * -0.1f, Projectile.velocity.Y * -0.1f, Scale: 0.6f, newColor: Color.PaleGreen, Alpha: 210).noGravity = true;
        }
    }

    public override void OnSpawn(IEntitySource source)
    {
        Projectile.ai[0] = Main.rand.NextFloat(-0.01f, 0.01f);
        for (int i = 0; i < 8; i++)
        {
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BubbleBurst_White, Projectile.velocity.X * -1.3f, Projectile.velocity.Y * -1.3f, Scale: 1.3f, newColor: Color.PaleGreen, Alpha: 210).noGravity = true;
        }
        base.OnSpawn(source);
    }

    public override void OnKill(int timeLeft)
    {
        for (int i = 0; i < 10; i++)
        {
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BubbleBurst_White, Projectile.velocity.X * -1.3f, Projectile.velocity.Y * -1.3f, Scale: 1.3f, newColor: Color.PaleGreen, Alpha: 210).noGravity = true;
        }
        base.OnKill(timeLeft);
    }
}