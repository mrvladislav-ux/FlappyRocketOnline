using Raylib_cs;

class Program
{

    static bool CheckCollision(Bird bird , Pipe pipe , float tubeDownY , float tubeDownHeight)
    {
        Rectangle birdHitBox = new Rectangle(
        
            bird.x - bird.radius,
            bird.y - bird.radius,
            bird.radius * 2,
            bird.radius * 2
        );

        Rectangle TopTubeHitBox = new Rectangle(

            pipe.x,
            0,
            70,
            pipe.gapY
        );

        Rectangle BottomTubeHistBox = new Rectangle(

            pipe.x,
            tubeDownY,
            70,
            tubeDownHeight
        );

        return
             Raylib.CheckCollisionRecs(birdHitBox , TopTubeHitBox) ||
             Raylib.CheckCollisionRecs(birdHitBox , BottomTubeHistBox);
    }

    static void ResetGame(
        ref bool gameStarted,
        ref bool gameOver,    
        Bird bird, 
        Pipe[] pipes, 
        float width, 
        ref float gravity, 
        ref float speed, 
        ref int score, 
        float hight,
        Random random)
    {
        if(gameOver == true && Raylib.IsKeyPressed(KeyboardKey.R))
            {
                gameStarted = false;
                gameOver = false;

                for(int i = 0; i < pipes.Length ; i++)
                {
                    pipes[i].x = width + random.Next(100 , 400) + i * 400;
                    pipes[i].gapY = random.Next(100 , 500);
                    pipes[i].passed = false;
                }

                bird.x = 200f;
                bird.y = 360f;
                bird.velocity = 0;
                gravity = 0;
                speed = 0;
                score = 0;
            }
    }

    static void StartGame(ref bool gameStarted , ref float gravity , ref float speed)
    {
        if(Raylib.IsKeyPressed(KeyboardKey.Space) && gameStarted == false)
            {
                gameStarted = true;
            }


            if(gameStarted)
            {
                gravity = 900f;
                speed = 260f;
            }
    }

    static void UpdateBird(Bird bird , float gravity)
    {
        bird.velocity += gravity * Raylib.GetFrameTime();
        bird.y += bird.velocity * Raylib.GetFrameTime();

        if(bird.y < 0)
            {
                bird.y = 0;
            }
    }

    static void UpdatePipe(Pipe pipe , float speed , int width , Random random)
    {
        pipe.x -= speed * Raylib.GetFrameTime();

        if(pipe.x < -80)
            {
                pipe.x = width;
                pipe.gapY = random.Next(100 , 440);
                pipe.passed = false;
            }
    }

    static void ScoreSystem(Pipe pipe , ref int score , Bird bird)
    {
        if(!pipe.passed && pipe.x + 70 < bird.x)
            {
                score++;
                pipe.passed = true;
            }
    }


    static void DrawBird(Bird bird)
    {
        Raylib.DrawCircle((int)bird.x , (int)bird.y , bird.radius , Color.Yellow);
    }

    static void DrawCenteredText(string text , Color color)
    {
        int fontsize = 50;
        
        int textWidth = Raylib.MeasureText(text , fontsize);

        int x = (Raylib.GetScreenWidth() - textWidth) / 2;
        int y = (Raylib.GetScreenHeight() - fontsize) / 2;

        Raylib.DrawText(text , x , y , fontsize , color);
    }


    static void DrawPipe(Pipe pipe , float tubeDownHight , float tubeDownY , float tubeUpY)
    {
        Raylib.DrawRectangle((int)pipe.x , (int)tubeUpY , 70 , (int)pipe.gapY , Color.Green);

        Raylib.DrawRectangle((int)pipe.x , (int)tubeDownY ,70 , (int)tubeDownHight , Color.Green);
    }
    

    static void Main()
    {

        Bird bird = new Bird();

        Pipe[] pipes = new Pipe[3];

        Random random = new Random();
        

        const int width = 1280;
        const int height = 720;


        float gravity = 0;
        float jumpfoce = -300f;
        
        bool gameStarted = false;
        bool gameOver = false;
        

        float tubeUpY = 0f;
 
        float speed = 0f;

        int score = 0;

        // Setting variables

        Raylib.InitWindow(width , height , "Flappy Bird");
        Raylib.SetTargetFPS(60);


            for(int i = 0; i < pipes.Length ; i++)
            {
                pipes[i] = new Pipe();

                pipes[i].x = width + i * 400;
            }

        while (!Raylib.WindowShouldClose())
        {

        
            StartGame(ref gameStarted , ref gravity , ref speed);
        
        
            if (!gameOver)
            {
                UpdateBird(bird , gravity);

                foreach(Pipe pipe in pipes)
            {
                UpdatePipe(pipe , speed , width , random);
                ScoreSystem(pipe , ref score , bird);
            }
            }


            ResetGame(ref gameStarted , ref gameOver , bird , pipes , width , ref gravity , ref speed , ref score , height , random);

            if (!gameOver && Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                bird.velocity = jumpfoce;
            }

            //Input



            if(bird.y > height + 10)
            {
                gameOver = true;
            }


            foreach(Pipe pipe in pipes)
            {
                float tubeDownY = pipe.gapY + pipe.gapSize;
                float tubeDownHeight = height - tubeDownY;

                if (CheckCollision(bird , pipe , tubeDownY , tubeDownHeight))
                {
                    gameOver = true;
                }
            }
            

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);






            foreach(Pipe pipe in pipes)
            {
                float tubeDownY = pipe.gapY + pipe.gapSize;
                float tubeDownHeight = height - tubeDownY;

                DrawPipe(pipe , tubeDownHeight , tubeDownY , tubeUpY);
            }

            //Drawing pipes! 

            DrawBird(bird);

            //Draw bird

            if (!gameStarted)
            {
                DrawCenteredText("Press 'Space'" , Color.Black);
            }

            if (gameOver)
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