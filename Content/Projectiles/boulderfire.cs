using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace yeetz.Content.Projectiles;

public class boulderfire : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.Size = new Vector2(16, 16);
        Projectile.aiStyle = -1;
        Projectile.hostile = true;
        Projectile.tileCollide = true;
        Projectile.timeLeft = 100;
        Projectile.penetrate = -1;
    }
}