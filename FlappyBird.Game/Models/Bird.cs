using System.Numerics;
using Raylib_cs;

public class Bird
{
    public float x = 200f;
    public float y = 360f;
    public float width = 96f;
    public float height = 96f;
    public float velocity = 0;
    public int radius = 20;

    public Texture2D rocket;


    public Bird()
    {
        rocket = Raylib.LoadTexture("Assets/Sprites/rocket2.png");
    }


    public void Update(float gravity)
    {
        velocity += gravity * Raylib.GetFrameTime();
        y += velocity * Raylib.GetFrameTime();

        if(y < 0)
            {
                y = 0;
            }
    }

    public void Draw()
    {
         Rectangle dest = new Rectangle(x , y , 96 , 96);

         Vector2 origin = new Vector2(64 , 64);

         Rectangle source = new Rectangle(0 , 0 , rocket.Width , rocket.Height);

         float rotation = velocity * 0.05f;
         rotation = Math.Clamp(rotation, -30f , 60f);

        Raylib.DrawTexturePro(rocket , source , dest , origin , rotation , Color.White);
    }
    
    public void UnLoad()
    {
        Raylib.UnloadTexture(rocket);
    }
}