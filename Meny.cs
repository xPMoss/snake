using System;

namespace Snake
{
	class Meny
	{

		private GameWorld world;
		private ConsoleRenderer renderer;

		public Meny(GameWorld _gameWorld, ConsoleRenderer _renderer)
		{
			world = _gameWorld;
			renderer = _renderer;

		}


		/// <summary>
		/// Print start meny on screen
		/// </summary>
		public void StartMeny()
        {
			ConsoleKey userInput;
			bool loop = true;

			// Show until user presses 'ENTER' //
			do
			{
				renderer.ClearGameArea();

				Console.BackgroundColor = ConsoleColor.DarkGray;
				Console.SetCursorPosition(0, 1);
				Console.WriteLine(@"    _____ _   _          _  ________    ");
				Console.WriteLine(@"   / ____| \ | |   /\   | |/ /  ____| ");
				Console.WriteLine(@"  | (___ |  \| |  /  \  | ' /| |__   ");
				Console.WriteLine(@"   \___ \| . ` | / /\ \ |  < |  __| ");
				Console.WriteLine(@"   ____) | |\  |/ ____ \| . \| |____ ");
				Console.WriteLine(@"  |_____/|_| \_/_/    \_\_|\_\______|");
				Console.WriteLine(@"  ");
				Console.WriteLine(@"  PRESS 'ENTER' TO START A NEW GAME");

				renderer.RenderScore();
				renderer.RenderFrame();

				userInput = Console.ReadKey().Key;

				if (userInput == ConsoleKey.Enter)
				{
					loop = false;

				}


			} while (loop);

			renderer.ClearGameArea();
			renderer.ClearScoreArea();

		}

		public void QuitMeny()
		{

			ConsoleKey userInput;
			bool loop = true;
			Program.GameOver = true;

			// Show until user presses 'Q' or 'N'//
			do
			{
				renderer.ClearGameArea();

				Console.BackgroundColor = ConsoleColor.DarkGray;
				Console.SetCursorPosition(0, 2);

				Console.WriteLine("  ARE YOU SURE?");
				Console.WriteLine("");
				Console.WriteLine("  PRESS 'Q' KEY TO QUIT");
				Console.WriteLine("  PRESS 'N' TO START A NEW GAME");

				renderer.RenderFrame();

				userInput = Console.ReadKey().Key;

				if (userInput == ConsoleKey.Q)
				{
					Program.running = false;
					loop = false;

				}
				else if (userInput == ConsoleKey.N)
				{


					loop = false;

				}


			} while (loop);

			if (userInput == ConsoleKey.N)
			{
				Program.GameOver = false;
				renderer.ClearGameArea();
				Program.NewGame();
				Program.Loop();
			}



		}

		/// <summary>
		/// Print Game Over meny
		/// </summary>
		/// <param name="msg">End message to user</param>
		public void GameOver(string msg) {

			ConsoleKey userInput;
			bool loop = true;
			Program.GameOver = true;

			// Show until user presses 'Q' or 'N'//
			do
			{
				renderer.ClearGameArea();

				Console.BackgroundColor = ConsoleColor.DarkGray;
				Console.SetCursorPosition(0, 2);
				Console.WriteLine(@"  " + msg);
				Console.WriteLine(@"    _____          __  __ ______    ______      ________ _____   ");
				Console.WriteLine(@"   / ____|   /\   |  \/  |  ____|  / __ \ \    / /  ____|  __ \  ");
				Console.WriteLine(@"  | |  __   /  \  | \  / | |__    | |  | \ \  / /| |__  | |__) | ");
				Console.WriteLine(@"  | | |_ | / /\ \ | |\/| |  __|   | |  | |\ \/ / |  __| |  _  /  ");
				Console.WriteLine(@"  | |__| |/ ____ \| |  | | |____  | |__| | \  /  | |____| | \ \  ");
				Console.WriteLine(@"   \_____/_/    \_\_|  |_|______|  \____/   \/   |______|_|  \_\ ");
				Console.WriteLine(@"  ");
				Console.WriteLine(@"  Your score was: " + Program.world.score);

				// Print highscore //
				HighScore();

				Console.WriteLine(@"  ");
				Console.WriteLine(@"  PRESS 'Q' KEY TO QUIT");
				Console.WriteLine(@"  PRESS 'N' TO START A NEW GAME");

				renderer.RenderFrame();

				userInput = Console.ReadKey().Key;

				if (userInput == ConsoleKey.Q)
				{
					Program.running = false;
					loop = false;

				}
				else if (userInput == ConsoleKey.N) {

					
					loop = false;

				}


			} while (loop);

            if (userInput == ConsoleKey.N)
            {
				Program.GameOver = false;
				renderer.ClearGameArea();
				Program.NewGame();
				Program.Loop();
			}
			else
			{
				QuitMeny();

			}




		}

		/// <summary>
		/// Loops throug Highscore-list and prints highscore on screen
		/// </summary>
		private void HighScore()
        {
			Console.WriteLine("\n  HIGHSCORE:");

			int c = 5;

			// If highscores slots less than 5 //
            if (world.highscore.Count < 5)
            {
				c = world.highscore.Count;

			}

			// Print scores...
			for (int i = 0; i < c; i++)
            {	
				int hs = world.highscore[i];
				int nr = i + 1;

				Console.WriteLine("  " + nr + ". " + hs);

			}

			// If highscores slots less than 5 print "-" instead of score//
			if (world.highscore.Count < 5)
            {
				int r = 5 - c;

                for (int i = 0; i < r; i++)
                {
					int nr = i + c + 1;

					Console.WriteLine("  " + nr + ". -");
				}

            }


        }

	}

}
