using System;
using System.Threading;
using System.Text;
using ConsoleExtender;

namespace Snake
{
    

    class Program
    {
        /// <summary>
        /// Checks Console to see if a keyboard key has been pressed, if so returns it as uppercase, otherwise returns '\0'.
        /// </summary>
        static public char ReadKeyIfExists() => Console.KeyAvailable ? Console.ReadKey(intercept: true).Key.ToString().ToUpper()[0] : '\0';

        static public GameWorld world = new GameWorld();
        static public ConsoleRenderer renderer = new ConsoleRenderer(world);
        static public Meny meny = new Meny(world, renderer);
        static public Player player = new Player(world);

        static int frameRate = 10;

        static bool firstTime = true;
        static public bool GameOver = false;
        static public bool running = true;

        static public void NewGame() {
            world.ResetGameWorld();
            player = new Player(world);
            world.gameObjects.Add(player);

        }

        static public void GameEnd(string _msg)
        {
            // Add to highscore
            if (world.score > 0)
            {
                world.highscore.Add(world.score);
                world.highscore.Sort();
                world.highscore.Reverse();
                //world.gameObjects.RemoveAt(0); // Remove lowest highscore


            }

            meny.GameOver(_msg);

        }


        static public void Loop()
        {

            if (firstTime)
            {
                meny.StartMeny();
                firstTime = false;
            }
           

            while (running)
            {
                // Kom ihåg vad klockan var i början
                DateTime before = DateTime.Now;

                // Hantera knapptryckningar från användaren
                char key = ReadKeyIfExists();

                if (world.timer <= 0)
                {
                    GameEnd("TIME IS UP!");

                }

                switch (key)
                {
                    case 'W':
                        player.MyDirection(Direction.up);
                        break;
                    case 'A':
                        player.MyDirection(Direction.left);
                        break;
                    case 'S':
                        player.MyDirection(Direction.down);
                        break;
                    case 'D':
                        player.MyDirection(Direction.right);
                        break;
                    case 'Q':
                        meny.QuitMeny();
                        running = false;
                        break;

                }


                // Uppdatera världen
                renderer.Clear();
                world.Update();
                renderer.Render();

                // Mät hur lång tid det tog
                double frameTime = Math.Ceiling((1000.0 / frameRate) - (DateTime.Now - before).TotalMilliseconds);

                if (frameTime > 0)
                {
                    // Vänta rätt antal millisekunder innan loopens nästa varv
                    Thread.Sleep((int)frameTime);

                }
            }
        } // <- LOOP END!

        static void Main(string[] args)
        {
            
            NewGame();
            Loop();

        }
    }
}
