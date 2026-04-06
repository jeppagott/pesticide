using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace yeetz.Content.Projectiles;

public class homingfire : ModProjectile
{
    public override string Texture => "yeetz/Content/Projectiles/phfire";
    
    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(44, 44);
        Projectile.hostile = true;
        Projectile.aiStyle = -1;
        Projectile.tileCollide = true;
        Projectile.timeLeft = 250;
    }

    public override void AI()
    {
        base.AI();
        Player player = Main.player[(int)Projectile.ai[2]];
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        Projectile.ai[1]++;
        if (player.active && !player.dead)
        {
            if (Projectile.ai[1] == 30)
            {
                Projectile.velocity *= 0.7f;
            }
            if (Projectile.ai[1] >= 60 && Projectile.ai[1] < 130)
            {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(player.Center) * 24, 0.06f);
            }

            if (Projectile.ai[1] > 130)
            {
                if (Projectile.velocity.Length() <= 10)
                {
                    Projectile.velocity *= 1.5f;
                }
            }
        }
    }
    
    public override void OnSpawn(IEntitySource source)
    {
        SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);
        base.OnSpawn(source);
    }
    
    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.Center);
        for (int i = 0; i < 22; i++)
        {
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * -1.3f, Projectile.velocity.Y * -1.3f, Scale: 1.9f).noGravity = true;
        }
        base.OnKill(timeLeft);
    }
    
    
    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 5;
        ProjectileID.Sets.TrailingMode[Type] = 2;
    }
    private static Asset<Texture2D> tex1 => ModContent.Request<Texture2D>("yeetz/Content/Projectiles/spirit");
    
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex0 = TextureAssets.Projectile[Type].Value;
        SpriteBatch sb = Main.spriteBatch;
        float fadeMult = 1f / Projectile.oldPos.Length;
        for (int i = 0; i < Projectile.oldPos.Length; i++)
        {
            float mult = (1f - fadeMult * i);
            sb.Draw(tex0, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.Lerp(Color.White with{A = 170}, Color.Red with{A = 170}, 0.4f) * mult * 0.12f, Projectile.rotation, tex0.Size() / 2 + new Vector2(0, -28), Projectile.scale, SpriteEffects.None, 0);
            sb.Draw(tex1.Value, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.Lerp(Color.White with{A = 0}, Color.Red with{A = 0}, 0.4f) * mult * 0.12f, Projectile.rotation - MathHelper.PiOver2, tex1.Size() / 2 + new Vector2(18, -6), Projectile.scale, SpriteEffects.None, 0);
        }
        for (int i = 0; i < Projectile.oldPos.Length; i++)
        {
            float mult = (1f - fadeMult * i);
            sb.Draw(tex0, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.Black * mult * 0.1f, Projectile.rotation, tex0.Size() / 2 + new Vector2(0, -28), Projectile.scale, SpriteEffects.None, 0);
            sb.Draw(tex1.Value, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, Color.Black * mult * 0.1f, Projectile.rotation - MathHelper.PiOver2, tex1.Size() / 2 + new Vector2(18, -6), Projectile.scale, SpriteEffects.None, 0);
        }
        
        Main.EntitySpriteDraw(tex0, Projectile.Center - Main.screenPosition, null, Color.Black, Projectile.rotation, tex0.Size() / 2 + new Vector2(0, -28), Projectile.scale, SpriteEffects.None);
        Main.EntitySpriteDraw(tex1.Value, Projectile.Center - Main.screenPosition, null, Color.Black, Projectile.rotation - MathHelper.PiOver2, tex1.Size() / 2 + new Vector2(18, -6), Projectile.scale, SpriteEffects.None);
        
        Main.EntitySpriteDraw(tex0, Projectile.Center - Main.screenPosition, null, Color.White with{A = 170}, Projectile.rotation, tex0.Size() / 2 + new Vector2(0, -28), Projectile.scale, SpriteEffects.None);
        Main.EntitySpriteDraw(tex1.Value, Projectile.Center - Main.screenPosition, null, Color.White with{A = 0}, Projectile.rotation - MathHelper.PiOver2, tex1.Size() / 2 + new Vector2(18, -6), Projectile.scale, SpriteEffects.None);
        return false;
    }
}