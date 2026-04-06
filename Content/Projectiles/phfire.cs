using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace yeetz.Content.Projectiles;

public class phfire : ModProjectile
{
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex0 = TextureAssets.Projectile[Type].Value;
        Main.EntitySpriteDraw(tex0, Projectile.Center - Main.screenPosition, null, Color.Black, Projectile.rotation, tex0.Size() / 2 + new Vector2(0, -28), Projectile.scale, SpriteEffects.None);
        Main.EntitySpriteDraw(tex0, Projectile.Center - Main.screenPosition, null, Color.White with{A = 170}, Projectile.rotation, tex0.Size() / 2 + new Vector2(0, -28), Projectile.scale, SpriteEffects.None);
        return false;
    }

    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(40, 40);
        Projectile.aiStyle = -1;
        Projectile.hostile = true;
        Projectile.tileCollide = true;
        Projectile.timeLeft = 1800;
        Projectile.penetrate = -1;
    }

    public override void AI()
    {
        base.AI();
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        Projectile.ai[1]++;
        if (Projectile.ai[1] >= 10)
        {
            Projectile.velocity.Y += 0.08f;
            if (Projectile.ai[1] >= 20)
            {
                Projectile.velocity.Y += 0.13f;
            }
        }
        for (int i = 0; i < 2; i++)
        {
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * -0.8f, Projectile.velocity.Y * -0.8f, Scale: 1.2f).noGravity = true;
        }
    }

    public override void OnSpawn(IEntitySource source)
    {
        SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, Projectile.Center);
        base.OnSpawn(source);
    }

    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, Projectile.Center);
        for (int i = 0; i < 22; i++)
        {
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * -1.3f, Projectile.velocity.Y * -1.3f, Scale: 1.9f).noGravity = true;
        }
        base.OnKill(timeLeft);
    }
}