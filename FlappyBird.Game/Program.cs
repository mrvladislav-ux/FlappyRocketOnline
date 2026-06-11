using Raylib_cs;

class Program
{

    static void DrawCenteredText(string text , Color color)
    {
        int fontsize = 50;
        
        int textWidth = Raylib.MeasureText(text , fontsize);

        int x = (Raylib.GetScreenWidth() - textWidth) / 2;
        int y = (Raylib.GetScreenHeight() - fontsize) / 2;

        Raylib.DrawText(text , x , y , fontsize , color);
    }

    static void Main()
    {

        Random random = new Random();
        

        int width = 1280;
        int hight = 720;


        float birdX = 200f;
        float birdY = 360f;

        float velocity = 0;
        float gravity = 0;
        float jumpfoce = -300f;
        int radius = 20;
        
        bool gameStarted = false;
        bool GameOver = false;

        float tubeX = 1000f;

        float gapY = 250;
        float gapSize = 180;
        

        float tubeUpY = 0f;

        float tubeDownY = gapY + gapSize;
        float tubeDownHight = hight - tubeDownY;
 
        float speed = 0f;

        int score = 0;

        bool tubeIsPased = false;


        // Setting variables

        Raylib.InitWindow(width , hight , "Flappy Bird");
        Raylib.SetTargetFPS(60);


        while (!Raylib.WindowShouldClose())
        {
            
            Rectangle birdHitBox = new Rectangle(
        
            birdX - radius,
            birdY - radius,
            radius * 2,
            radius * 2
        );

        Rectangle TopTubeHitBox = new Rectangle(

            tubeX,
            0,
            70,
            gapY
        );

        Rectangle BottomTubeHistBox = new Rectangle(

            tubeX,
            tubeDownY,
            70,
            tubeDownHight
        );

            if(Raylib.IsKeyPressed(KeyboardKey.Space) && gameStarted == false)
            {
                gameStarted = true;
            }


            if(gameStarted)
            {
                gravity = 900f;
                speed = 260f;
            }

            if (!GameOver)
            {
                velocity += gravity * Raylib.GetFrameTime();
                birdY += velocity * Raylib.GetFrameTime();

                tubeX -= speed * Raylib.GetFrameTime();
            }

            //Game starting logic



            if (!GameOver && Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                velocity = jumpfoce;
            }

            if(GameOver == true && Raylib.IsKeyPressed(KeyboardKey.R))
            {
                gameStarted = false;
                GameOver = false;

                birdX = 200f;
                birdY = 360f;
                tubeX = width + 300;
                velocity = 0;
                gravity = 0;
                speed = 0;
                score = 0;
                tubeIsPased = false;

                gapY = 250;
                tubeDownY = gapY + gapSize;
                tubeDownHight = hight - tubeDownY;

                continue;
            }

            //Input

            if(birdY < 0)
            {
                birdY = 0;
            }

            if(birdY > hight + 10)
            {
                GameOver = true;
            }

            //Physics

            if(tubeX < -80)
            {
                tubeX = width;
                gapY = random.Next(100 , 440);

                 tubeDownY = gapY + gapSize;
                 tubeDownHight = hight - tubeDownY;

                 tubeIsPased = false;
            }

            if(!tubeIsPased && tubeX + 70 < birdX)
            {
                score++;
                tubeIsPased = true;
            }

            if(Raylib.CheckCollisionRecs(birdHitBox , TopTubeHitBox)|| Raylib.CheckCollisionRecs(birdHitBox , BottomTubeHistBox))
            {
                GameOver = true;
            }


            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);


            Raylib.DrawRectangle((int)tubeX , (int)tubeUpY , 70 , (int)gapY , Color.Green);

            Raylib.DrawRectangle((int)tubeX , (int)tubeDownY ,70 , (int)tubeDownHight , Color.Green);


            //Drawing tubes! 

            Raylib.DrawCircle((int)birdX , (int)birdY , radius , Color.Yellow);

            //Draw bird

            if (!gameStarted)
            {
                DrawCenteredText("Press 'Space'" , Color.Black);
            }

            if (GameOver)
            {
               DrawCenteredText("Game over!" , Color.Red);
            }

            Raylib.DrawText("Flappy Bird" , 20 , 20 , 25 , Color.Black);
            Raylib.DrawText($"Score: {score}" , 20 , 60 , 30 , Color.Black);


        //Drawing text

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}