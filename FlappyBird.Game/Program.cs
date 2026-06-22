using Raylib_cs;

class Program
{

    static bool CheckCollision(Bird bird , Pipe pipe , float tubeDownY , float tubeDownHeight)
    {
        Rectangle birdHitBox = new Rectangle(
        
            bird.x - 45,
            bird.y - 30,
            55,
            30
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

        Pipe[] pipes = new Pipe[3];

        Random random = new Random();

        Game game = new Game();

        string[] menuItems = {
            "Play",
            "Difficulty",
            "Settings",
            "Leaderboard",
            "Exit"
        };

        string[] difficultyItems = {
            "Easy",
            "Medium",
            "Hard",
            "Dynamic",
            "Back"
        };

        int selectedIndex = 0;
        int selectedDifficultyIndex = 0;
        

        const int width = 1280;
        const int height = 720;


        float jumpfoce = -300f;
        
        float tubeUpY = 0f;

        // Setting variables

        Raylib.InitWindow(width , height , "Flappy Bird");
        Raylib.SetTargetFPS(60);

        Texture2D background = Raylib.LoadTexture("Assets/Sprites/Background.png");
        Texture2D title = Raylib.LoadTexture("Assets/Sprites/GameTitle.png");



        Raylib.InitAudioDevice();
        Music ambient = Raylib.LoadMusicStream("Assets/ambient.mp3");
        Raylib.PlayMusicStream(ambient);
        Raylib.SetMusicVolume(ambient , 0.5f);



        Bird bird = new Bird();


        float bgX1 = 0;
        float bgX2 = background.Width;
        float bgSpeed = 50f;



            for(int i = 0; i < pipes.Length ; i++)
            {
                pipes[i] = new Pipe();

                pipes[i].x = width + i * 400;
            }

        while (!Raylib.WindowShouldClose())
        {

            Raylib.UpdateMusicStream(ambient);


            game.Start(game);


            float dt = Raylib.GetFrameTime();

            bgX1 -= bgSpeed * dt;
            bgX2 -= bgSpeed * dt;
        

            if(bgX1 <= -background.Width)
            {
                bgX1 = bgX2 + background.Width;
            }

            if(bgX2 <= -background.Width)
            {
                bgX2 = bgX1 + background.Width;
            }


        
            if (game.state == GameState.Playing)
            {
                game.ApplyDifficulty();

                bird.Update(game.gravity);

                foreach(Pipe pipe in pipes)
            {
                pipe.Update(game.speed , width , random , game.pipeGap);
                game.ScoreSystem(pipe , bird);
            }
            }

            switch (game.state)
            {
                case GameState.Menu :

                if (Raylib.IsKeyPressed(KeyboardKey.Up))
                {
                    selectedIndex--;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.Down))
                {
                    selectedIndex++;
                }
                if(selectedIndex < 0)
                {
                    selectedIndex = menuItems.Length - 1;
                }
                if(selectedIndex >= menuItems.Length)
                {
                    selectedIndex = 0;
                }



            if(game.state == GameState.Menu && Raylib.IsKeyPressed(KeyboardKey.Enter))
            {
                switch (selectedIndex)
                {
                    case 0:
                        game.state = GameState.Playing;
                    break;

                    case 1:
                        game.state = GameState.DifficultyMenu;
                    break;

                    case 4:
                        return;
                }
            }

                break;

                case GameState.DifficultyMenu :

                if (Raylib.IsKeyPressed(KeyboardKey.Up))
                {
                    selectedDifficultyIndex--;
                }
                if (Raylib.IsKeyPressed(KeyboardKey.Down))
                {
                    selectedDifficultyIndex++;
                }
                if(selectedDifficultyIndex < 0)
                {
                    selectedDifficultyIndex = difficultyItems.Length - 1;
                }
                if(selectedDifficultyIndex >= difficultyItems.Length)
                {
                    selectedDifficultyIndex = 0;
                }
            

            if(game.state == GameState.DifficultyMenu && Raylib.IsKeyPressed(KeyboardKey.Enter))
            {
                switch(selectedDifficultyIndex)
                {
                    case 0:
                       game.difficulty = Difficulty.Easy;
                       game.state = GameState.Menu;
                    break;

                    case 1:
                        game.difficulty = Difficulty.Medium;
                        game.state = GameState.Menu;
                    break;

                    case 2:
                        game.difficulty = Difficulty.Hard;
                        game.state = GameState.Menu;
                    break;

                    case 3:
                        game.difficulty = Difficulty.Dynamic;
                        game.state = GameState.Menu;
                    break;

                    case 4:
                        Console.WriteLine("Back is passed");
                        game.state = GameState.Menu;
                    break;
                }
            }
                
                break;
            }



            game.Reset(game, bird , pipes , width , random);

            if (game.state == GameState.Playing && Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                bird.velocity = jumpfoce;
            }

            game.CheckPause();

            //Input



            if(bird.y > height + 10)
            {
               game.state = GameState.GameOver;
            }


            foreach(Pipe pipe in pipes)
            {
                float tubeDownY = pipe.gapY + pipe.gapSize;
                float tubeDownHeight = height - tubeDownY;

                if (CheckCollision(bird , pipe , tubeDownY , tubeDownHeight))
                {
                    game.state = GameState.GameOver;
                }
            }
        

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);


            Raylib.DrawTexture(background , (int)bgX1 , 0 , Color.White);
            Raylib.DrawTexture(background , (int)bgX2 , 0 , Color.White);


            foreach(Pipe pipe in pipes)
            {
                float tubeDownY = pipe.gapY + pipe.gapSize;
                float tubeDownHeight = height - tubeDownY;

                pipe.Draw(pipe , tubeDownHeight , tubeDownY , tubeUpY);
            }

            //Drawing pipes! 

           if(game.state == GameState.Playing || game.state == GameState.Paused || game.state == GameState.GameOver)
            {
                 bird.Draw();
            }

            //Draw bird

            if (game.state == GameState.Menu)
            {

                Raylib.DrawTexture(title , 440 , 60 , Color.White);

               for (int i = 0 ; i < menuItems.Length; i++)
                {
                    Color color =
                    i == selectedIndex ? Color.Yellow : Color.White;

                int menuStartY = 350;

                Raylib.DrawText(menuItems[i] , 500 , menuStartY + i * 60 , 40 , color);

                }
            }

            

            if(game.state == GameState.DifficultyMenu)
            {
                for(int i = 0 ; i < difficultyItems.Length; i++)
                {
                    Color color =
                    i == selectedDifficultyIndex ? Color.Yellow : Color.White;

                    Raylib.DrawText(difficultyItems[i] , 500 , 250 + i * 60 , 40 , color);
                }
            }

            if (game.state == GameState.GameOver)
            {
               DrawCenteredText("Game over!" , Color.Red);
            }

            if(game.state == GameState.Paused)
            {
                DrawCenteredText("Pause" , Color.White);
            }

            Raylib.DrawText("Flappy Rocket" , 20 , 20 , 25 , Color.White);
            Raylib.DrawText($"Score: {game.score}" , 20 , 60 , 30 , Color.White);

        //debug panel:

            Raylib.DrawText($"DifficultyIndex: {selectedDifficultyIndex}",20,100,20,Color.Red);

            Raylib.DrawText($"State: {game.state}",20,130,20,Color.Red);

            Raylib.DrawText($"Selected Difficulty: {game.difficulty}",20,160,20,Color.Red);


        //Drawing text

            Raylib.EndDrawing();
        }

        Raylib.UnloadTexture(title);
        Raylib.UnloadTexture(background);
        bird.UnLoad();

        Raylib.UnloadMusicStream(ambient);
        Raylib.CloseAudioDevice();

        Raylib.CloseWindow();
    }
}