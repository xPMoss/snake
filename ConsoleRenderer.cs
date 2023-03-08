using System;
using System.Collections.Generic;
using System.Text;
using ConsoleExtender;

namespace Snake
{
    class ConsoleRenderer
    {
        private GameWorld world;

        private int bkgColor = 8;

        public ConsoleRenderer(GameWorld _gameWorld)
        {
            world = _gameWorld;

            // Set console size to world size //
            Console.SetWindowSize(world.W+36, world.H+10);
            Console.SetBufferSize(world.W+36, world.H+10);

            // Set to more square font-type //f
            ConsoleHelper.SetCurrentFont("Lucida Console", 12);

            // Hide text cursor //
            Console.CursorVisible = false;

            RenderFrame();
        }

        public void Clear() {

            // Write empty on moving objects //
            for (int i = 0; i < world.gameObjects.Count; i++)
            {
                GameObject obj = world.gameObjects[i];

                if (obj is IMovable)
                {
                    Console.BackgroundColor = (ConsoleColor)bkgColor;
                    ((IMovable)obj).Clear();

                }

            }

            // Reset colors
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

        }

        public void Render()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            

            //ClearGameArea();
            RenderScore(world.score, world.timer);

            // Render gameobjects //
            for (int i = 0; i < world.gameObjects.Count; i++)
            {
                GameObject obj = world.gameObjects[i];

                if (obj is IRenderable)
                {
                    Console.BackgroundColor = (ConsoleColor)bkgColor;
                    ((IRenderable)obj).Render();

                }

            }

            // Reset colors
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;


            //DebugRender();

        }

        
        public void ClearScoreArea()
        {

            Console.BackgroundColor = ConsoleColor.Black;

            List<string> scoreText = new List<string>();

            for (int i = 1; i < 10; i++)
            {
                //Console.SetCursorPosition(world.W + 3, i);
                //Console.Write("                          ");

                Console.SetCursorPosition(world.W + 3, i);
                Console.Write(new string(' ', Console.WindowWidth - world.W-4));
            }

        }
        

        /// <summary>
        /// Renders information area (Score, timer, etc...)
        /// </summary>
        public void RenderScore(int _score, int _timer) {

            Console.BackgroundColor = ConsoleColor.Black;

            List<string> scoreText = new List<string>();

            scoreText.Add("Score: " + _score);
            scoreText.Add("Timer: " + _timer / 5);
            scoreText.Add("");
            scoreText.Add($"Collect as many 'F' as possible");
            scoreText.Add($"before the time runs out.");
            scoreText.Add($"Watch out for Bombs!");
            scoreText.Add("");
            scoreText.Add($"Use 'WASD' to play");
            scoreText.Add($"Press 'Q' to quit");

            for (int i = 0; i < scoreText.Count; i++)
            {
                Console.SetCursorPosition(world.W + 3, i+1);
                if (i == 1)
                {
                    Console.Write("          ");
                    Console.SetCursorPosition(world.W + 3, i + 1);
                }
                Console.WriteLine(scoreText[i]);

            }

        }

        /// <summary>
        /// Overload - information area
        /// </summary>
        /// <param name="_score"></param>
        /// <param name="_timer"></param>
        public void RenderScore(char _score, char _timer)
        {

            Console.BackgroundColor = ConsoleColor.Black;

            List<string> scoreText = new List<string>();

            scoreText.Add("Score: " + _score);
            scoreText.Add("Timer: " + _timer);
            scoreText.Add("");
            scoreText.Add($"Collect as many 'F' as possible");
            scoreText.Add($"before the time runs out.");
            scoreText.Add($"Watch out for Bombs!");
            scoreText.Add("");
            scoreText.Add($"Use 'WASD' to play");
            scoreText.Add($"Press 'Q' to quit");

            for (int i = 0; i < scoreText.Count; i++)
            {
                Console.SetCursorPosition(world.W + 3, i + 1);
                if (i == 1)
                {
                    Console.Write("          ");
                    Console.SetCursorPosition(world.W + 3, i + 1);
                }
                Console.WriteLine(scoreText[i]);

            }

        }

        /// <summary>
        /// Overload - information area
        /// </summary>
        /// <param name="_score"></param>
        /// <param name="_timer"></param>
        public void RenderScore()
        {

            Console.BackgroundColor = ConsoleColor.Black;

            List<string> scoreText = new List<string>();

            scoreText.Add($"Collect as many 'F' as possible");
            scoreText.Add($"before the time runs out.");
            scoreText.Add($"Watch out for Bombs!");
            scoreText.Add("");
            scoreText.Add($"Use 'WASD' to play");
            scoreText.Add($"Press 'Q' to quit");

            for (int i = 0; i < scoreText.Count; i++)
            {
                Console.SetCursorPosition(world.W + 3, i + 1);
                Console.WriteLine(scoreText[i]);

            }

        }

        public void ClearGameArea()
        {
            Console.BackgroundColor = (ConsoleColor)bkgColor;

            // CLEAR GAME AREA //
            for (int i = 1; i < world.H + 1; i++)
            {
                Console.SetCursorPosition(1, i);
                Console.Write(new string(' ', world.W));

            }

        }

        /// <summary>
        /// Renders game area border
        /// </summary>
        public void RenderFrame()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);
            Console.Write("o");

            // DRAW TOP LINE //
            for (int i = 1; i < Console.BufferWidth-2; i++)
            {
                Console.SetCursorPosition(i, 0);
                if (i == world.W + 1)
                {
                    Console.Write("o");
                }
                else
                {
                    Console.Write("-");
                }
            }

            Console.SetCursorPosition(Console.BufferWidth - 1, 0);
            Console.Write("o");

            // DRAW L LINE //
            for (int i = 1; i < world.H+1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("|");
            }

            // DRAW R LINE //
            for (int i = 1; i < world.H+1; i++)
            {
                Console.SetCursorPosition(world.W+1, i);
                Console.Write("|");
            }

            // DRAW R LINE 2 //
            for (int i = 1; i < world.H + 1; i++)
            {
                Console.SetCursorPosition(Console.BufferWidth-1, i);
                Console.Write("|");
            }

            Console.SetCursorPosition(0, world.H + 1);
            Console.Write("o");

            // DRAW BOTTOM LINE //
            for (int i = 1; i < Console.BufferWidth - 1; i++)
            {
                Console.SetCursorPosition(i, world.H+1);
                if (i == world.W+1)
                {
                    Console.Write("o");
                }
                else
                {
                    Console.Write("-");
                }
                
            }

            Console.SetCursorPosition(Console.BufferWidth - 1, world.H + 1);
            Console.Write("o");

        }

        /// <summary>
        /// For debugging purpose
        /// </summary>
        public void DebugRender()
        {

            // CLEAR DEBUG AREA //
            for (int i = world.H + 2; i < world.H + 6; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.BufferWidth));

            }

            /*
            // RENDER SCORE //
            Console.SetCursorPosition(0, world.H + 2);
            Console.WriteLine("\n [INFO:]");
            Console.WriteLine(" Spawn Timer: " + world.spawnTimer);
            Console.WriteLine(" Tails: " + Program.player.tail.Count);
            */
            
            Console.WriteLine("\n [GAMEOBJECTS:]");

            for (int i = 0; i < world.gameObjects.Count; i++)
            {
                GameObject obj = world.gameObjects[i];

                if (obj is Bomb || obj is Food)
                {
                    Console.WriteLine(" " + i + ". " + obj);

                }
                
            }
            



        }
    }

}
