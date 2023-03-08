using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{

    public enum Direction { up, down, left, right, none }

    interface IRenderable {

        char MyChar();

        void Render();

        // Write " " on GameObjects position //
        void WriteEmpty();


    }

    interface IMovable
    {
        public void MyDirection(Direction KeyPress);

        void Clear();
    }

    public struct Position {

        public int X;
        public int Y;


    }

    public abstract class GameObject
    {
        // TODO
        public Position position = new Position();

        public abstract void Update();


    }

    /// <summary>
    /// Tail object, nees an X- and Y-cordinate
    /// </summary>
    public class Tail : GameObject, IRenderable
    {


        public Tail(int _X, int _Y)
        {

            position.X = _X;
            position.Y = _Y;

        }

        public char MyChar()
        {
            return 'X';
        }

        public override void Update()
        {



        }

        public void Render()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(MyChar());


        }

        public void WriteEmpty()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;

        }

    }

    /// <summary>
    /// Player object. Needs an X- and Y- cordinate and a GameWorld object
    /// </summary>
    public class Player : GameObject, IRenderable, IMovable
    {
        private GameWorld world;

        public Direction direction = Direction.up;
        public Direction lastDirection;
        public int velocity = 1;

        public List<GameObject> tail = new List<GameObject>();

        public Player(GameWorld _gameWorld)
        {
            world = _gameWorld;

            // Set position to middle of game area //
            position.X = world.W / 2;
            position.Y = world.H / 2;

        }

        /// <summary>
        /// Char to print on screen
        /// </summary>
        /// <returns>Char to print on screen</returns>
        public char MyChar()
        {
            return 'X';
        }

        /// <summary>
        /// Set direction of player based on key pressed by user
        /// </summary>
        /// <param name="_direction">0 1 2 3 4 // up, down, left, right, none</param>
        public void MyDirection(Direction _direction)
        {

            // if player has tail -> can't reverse direction //
            if (tail.Count > 0)
            {
                if (_direction == Direction.up && lastDirection != Direction.down)
                {
                    direction = _direction;

                }
                else if (_direction == Direction.down && lastDirection != Direction.up)
                {
                    direction = _direction;

                }
                else if (_direction == Direction.left && lastDirection != Direction.right)
                {
                    direction = _direction;

                }
                else if (_direction == Direction.right && lastDirection != Direction.left)
                {
                    direction = _direction;

                }

            }
            else {
                direction = _direction;

            }


        }

        public override void Update() {

            if (direction != Direction.none)
            {
                lastDirection = direction;
            }
            

            if (tail.Count > 0 && direction != Direction.none)
            {
                Tail tailObject = new Tail(position.X, position.Y);
                tail.Add(tailObject);

            }

            MovePlayer();
            

            if (tail.Count > 0 && direction != Direction.none)
            {
                tail.RemoveAt(0);


            }

            CollisionDetection();

        }

        /// <summary>
        /// Moves player 1 unit in player direction. 
        /// If player exits gamearea teleport to oposite place of gameareas
        /// </summary>
        public void MovePlayer() {
            
            if (direction == Direction.up)
            {
                position.Y -= velocity;

            }
            else if (direction == Direction.down)
            {
                position.Y += velocity;
            }
            else if (direction == Direction.left)
            {
                position.X -= velocity;
            }
            else if (direction == Direction.right)
            {
                position.X += velocity;

            }



        }

        /// <summary>
        /// Move player back to previous position and set direction to none(no speeed)
        /// </summary>
        public void MovePlayerBack() {

            if (direction == Direction.up)
            {
                for (int j = 0; j < tail.Count; j++)
                {
                    tail[j].position.Y += velocity;

                }
                position.Y += velocity;

            }
            else if (direction == Direction.down)
            {
                for (int j = 0; j < tail.Count; j++)
                {
                    tail[j].position.Y -= velocity;

                }
                position.Y -= velocity;
            }
            else if (direction == Direction.left)
            {
                for (int j = 0; j < tail.Count; j++)
                {
                    tail[j].position.X += velocity;

                }
                position.X += velocity;
            }
            else if (direction == Direction.right)
            {
                for (int j = 0; j < tail.Count; j++)
                {
                    tail[j].position.X -= velocity;

                }
                position.X -= velocity;

            }

            direction = Direction.none;

        }


        /// <summary>
        /// Checks if player is colliding with object
        /// </summary>
        public string CollisionDetection()
        {
            string collider = "";

            // Check if outside game area //
            if (position.X < 1 || position.X > world.W || position.Y < 1 || position.Y > world.H)
            {
                collider = "Border";
                MovePlayerBack();

            }

            // Check if colliding with tail //
            for (int i = 0; i < tail.Count; i++)
            {
                GameObject other = tail[i];

                if (position.X == other.position.X && position.Y == other.position.Y)
                {
                    collider = "Tail";
                    Program.GameEnd("YOU HIT YOUR TAIL!");

                }


            }

            // Check if colliding with F, | or bomb //
            for (int i = 0; i < world.gameObjects.Count; i++)
            {
                GameObject other = world.gameObjects[i];

                // Check if same X and Y position //
                if (position.X == other.position.X && position.Y == other.position.Y)
                {
                    if (other is Food)
                    {
                        collider = "Food";
                        // Add a tail object //
                        Tail tailObject = new Tail(position.X, position.Y);
                        tail.Add(tailObject);

                        world.gameObjects.RemoveAt(i); // Remove eaten F
                        world.score++; // Add to score
                        world.ResetTimer();
                    }

                    if (other is Wall)
                    {
                        collider = "Wall";
                        ((Wall)other).SetColliding(true);
                        MovePlayerBack();

                    }

                    if (other is Bomb)
                    {
                        collider = "Bomb";
                        Program.GameEnd("YOU HIT A BOMB!");

                    }

                }

            }

            return collider;

        }

        /// <summary>
        /// Render object on screen
        /// </summary>
        public void Render()
        {
            RenderTail();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(MyChar());

        }

        public void RenderTail()
        {

            for (int i = 0; i < tail.Count; i++)
            {
                ((IRenderable)tail[i]).Render();

            }
           

        }

        public void Clear() {
            ClearTail();
            WriteEmpty();
        }

        public void WriteEmpty()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;

        }

        public void ClearTail()
        {

            for (int i = 0; i < tail.Count; i++)
            {
               ((IRenderable)tail[i]).WriteEmpty();

            }


        }


    }

    /// <summary>
    /// Wall object, nees an X- and Y-cordinate
    /// </summary>
    public class Wall : GameObject, IRenderable
    {
        char wallType;
        public bool colliding = false;

        /// <summary>
        /// Wall object, nees an X- and Y-cordinate
        /// </summary>
        /// <param name="_x">X-cordinate</param>
        /// <param name="_y">Y-cordinate</param>
        public Wall(int _x, int _y, char _wallType)
        {
            position.X = _x;
            position.Y = _y;
            wallType = _wallType;
        }

        public char MyChar()
        {

            return wallType;

        }

        public override void Update()
        {
            colliding = false;

        }

        public void SetColliding(bool _colliding)
        {
            colliding = _colliding;

        }


        public void Render()
        {
            if (colliding == false)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;

            }


            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(MyChar());

        }

        public void WriteEmpty()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;

        }

    }

    /// <summary>
    /// Food object. Needs an X- and Y- cordinate and a GameWorld object
    /// </summary>
    public class Food : GameObject, IRenderable
    {
        private GameWorld world;

        private int timer;

        Random randomNr = new Random();
        int randomTimer;

        /// <summary>
        /// Food object. Needs an X- and Y- cordinate and a GameWorld object
        /// </summary>
        /// <param name="_x">X Cordinate</param>
        /// <param name="_y">Y Cordinate</param>
        /// <param name="_gameWorld">GameWorld object</param>
        public Food(int _x, int _y, GameWorld _gameWorld)
        {
            world = _gameWorld;
            randomTimer = randomNr.Next(10, 30);
            timer = randomTimer * 5;

            position.X = _x;
            position.Y = _y;

        }

        public char MyChar()
        {
            return 'F';

        }

        public override void Update()
        {
            timer--;

            if (timer < 1)
            {
                RandomMove();
            }

        }

        /// <summary>
        /// Moves this object to a new random position
        /// </summary>
        public void RandomMove()
        {
            var rand = new Random();
            int numX = rand.Next(1, world.W);
            int numY = rand.Next(1, world.H);

            bool collision = true;

            // CHECK IF EMPTY SPACE //
            while (collision)
            {
                for (int i = 0; i < world.gameObjects.Count; i++)
                {
                    GameObject other = world.gameObjects[i];

                    if (numX == other.position.X && numY == other.position.Y)
                    {
                        numX = rand.Next(2, world.W);
                        numY = rand.Next(1, world.H);

                    }
                    else
                    {
                        collision = false;

                    }

                }


            }

            WriteEmpty();

            position.X = numX;
            position.Y = numY;

            timer = randomTimer * 5;


        }

        public void Render()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(MyChar());


        }

        public void Clear()
        {
            

        }

        public void WriteEmpty()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;

        }


    }

    public class Blast : GameObject, IRenderable
    {
        private bool hit = false;

        public Blast(int _x, int _y)
        {
            
            position.X = _x;
            position.Y = _y;

        }

        public char MyChar()
        {
            return 'o';

        }

        public override void Update()
        {
            // If hit player Game Over!
            if (hit)
            {
                Program.GameEnd("YOU WHERE BLOWN UP!");
            }

            CollisionDetection();



        }

        private void CollisionDetection()
        {
            GameObject player = Program.player;

            // Check if same X and Y position as Player //
            if (position.X == player.position.X && position.Y == player.position.Y)
            {
                if (Program.GameOver == false)
                {
                    hit = true;

                }

            }

        }

        public void Render()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(MyChar());

        }

        public void Clear()
        {


        }

        public void WriteEmpty()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;

        }

    }

    /// <summary>
    /// Bomb object. Needs an X- and Y- cordinate and a GameWorld object
    /// </summary>
    public class Bomb : GameObject, IRenderable
    {
        private GameWorld world;

        private int blastTimer = 10; // <- How long should the blast last
        private int timer; // <- Countdown to blast
        public List<GameObject> blasts = new List<GameObject>();

        Random randomNr = new Random();
        int randomTimer;

        /// <summary>
        /// Bomb object. Needs an X- and Y- cordinate and a GameWorld object
        /// </summary>
        /// <param name="_x">X cordinate</param>
        /// <param name="_y">Y cordinate</param>
        /// <param name="_gameworld"></param>
        public Bomb(int _x, int _y, GameWorld _gameworld)
        {
            world = _gameworld;

            randomTimer = randomNr.Next(5, 10);
            timer = randomTimer * 5;

            position.X = _x;
            position.Y = _y;
        }

        public char MyChar()
        {
            return 'O';

        }

        public override void Update()
        {
            timer--;

            if (timer < 1 && blastTimer == 10)
            {
                CreateBlast();
            }

            if (blastTimer < 10 && blastTimer > 0)
            {
                blastTimer--;
            }

            if (blastTimer < 1) {
                // Find this object and remove //
                for (int i = 0; i < world.gameObjects.Count; i++)
                {
                    GameObject other = world.gameObjects[i];

                    // Check if same X and Y position //
                    if (position.X == other.position.X && position.Y == other.position.Y)
                    {
                        world.gameObjects.RemoveAt(i); // Remove bomb

                        WriteEmpty();
                    }

                }

            }

            if (timer < 1)
            {
                for (int i = 0; i < blasts.Count; i++)
                {
                    blasts[i].Update();

                }

            }
        }


        public void CreateBlast() {

            int minX = -2;
            int maxX = 3;
            int minY = -2;
            int maxY = 3;

            // Check position and set correct blast size
            if (position.X < 3)
            {
                minX = -position.X +1;

            }

            if (position.X > Program.world.W -2)
            {
                maxX = 2;

                if (position.X > Program.world.W - 1)
                {
                    maxX = 1;

                }
            }

            if (position.Y < 3)
            {
                minY = -position.Y +1;

            }

            if (position.Y > Program.world.H -2)
            {
                maxY = 2;

                if (position.Y > Program.world.H - 2)
                {
                    maxY = 1;

                }


            }

            // Create blast objects and add to list
            for (int i = minY; i < maxY; i++) // Y-size
            {
                for (int j = minX; j < maxX; j++) // X-size
                {
                    Blast blast = new Blast(position.X + j, position.Y + i);
                    blasts.Add(blast);

                }

            }

            blastTimer--; // <- Start countdown until removed

        }

        public void Render()
        {
            RenderBlast();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(MyChar());


        }

        public void RenderBlast()
        {
            for (int i = 0; i < blasts.Count; i++)
            {
                ((IRenderable)blasts[i]).Render();

            }


        }

        public void Clear()
        {


        }

        public void WriteEmpty()
        {
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;

            for (int i = 0; i < blasts.Count; i++)
            {
                ((IRenderable)blasts[i]).WriteEmpty();

            }

        }



    }






}
