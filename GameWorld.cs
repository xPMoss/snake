using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    public class GameWorld
    {
        public List<int> highscore = new List<int>(); // <- High score list

        public int score = 0; // <- Total score per game
        static public int timerStart = 300; // <- Default 300
        public int timer = timerStart;

        public int spawnTimer = 25; // <- When this value spawn F or Bomb

        public int W = 90; // <- Game area height
        public int H = 40; // <- Game area width

        public List<GameObject> gameObjects = new List<GameObject>(); // <- All gameobjects

        public GameWorld()
        {
            



        }

        public void Update()
        {
            // TODO
            spawnTimer++;
            timer--;

            if (spawnTimer >= 25)
            {
                Spawn();

            }

            // Update all Gameobjects logic //
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject obj = gameObjects[i];

                obj.Update();

            }


        }

        public void ResetTimer() {
            timer = timerStart;

        }

        /// <summary>
        /// Spawn food or bomb
        /// </summary>
        public void Spawn()
        {
            var rand = new Random();
            int numX = rand.Next(1, W);
            int numY = rand.Next(1, H);

            bool collision = true;

            // CHECK IF EMPTY SPACE //
            while (collision)
            {
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    GameObject other = gameObjects[i];

                    if (numX == other.position.X && numY == other.position.Y)
                    {
                        numX = rand.Next(2, W);
                        numY = rand.Next(1, H);

                    }
                    else
                    {
                        collision = false;

                    }

                }

            }

            rand = new Random();
            int randBomb = rand.Next(1, 10);

            if (randBomb < 4)
            {
                Bomb bood = new Bomb(numX, numY, this);
                gameObjects.Add(bood);
            }
            else
            {
                Food food = new Food(numX, numY, this);
                gameObjects.Add(food);
            }


            spawnTimer = 0;

        }

        /// <summary>
        /// Create wall obsticles
        /// </summary>
        public void CreateWalls()
        {

            int x = 15;
            int y = 10;
            int c = 10;

            for (int i = y; i < c+y; i++)
            {
                Wall wall = new Wall(x, i, '|');
                gameObjects.Add(wall);

            }


            for (int j = x; j < x+20; j++)
            {
                Wall wall = new Wall(j, y+c, '-');
                gameObjects.Add(wall);
            }


            for (int j = 30; j < 30 + 15; j++)
            {
                Wall wall = new Wall(j, 35, '-');
                gameObjects.Add(wall);
            }

            for (int j = 40; j < 40 + 30; j++)
            {
                Wall wall = new Wall(j, 6, '-');
                gameObjects.Add(wall);
            }

            x = 50;
            y = 15;
            c = 15;

            for (int i = y; i < c + y; i++)
            {
                Wall wall = new Wall(x, i, '|');
                gameObjects.Add(wall);

            }


            for (int j = x; j < x + 25; j++)
            {
                Wall wall = new Wall(j, y+c, '-');
                gameObjects.Add(wall);
            }


        }

        /// <summary>
        /// Resets variabels for new game
        /// </summary>
        public void ResetGameWorld() {
            score = 0;
            timer = timerStart;
            spawnTimer = 25;

            gameObjects = new List<GameObject>();
            CreateWalls();


        }

    }

}
