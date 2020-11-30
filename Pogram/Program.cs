using System;
using System.Collections.Generic;
using Raylib_cs;


namespace Pogram
{

    class Invader
    {
        public bool killed = false;
        public float originalX;
        public float speed = 1;
        public Rectangle enemyrect;

        public Invader(int x, int y)
        {
            this.originalX = x;
            this.enemyrect = new Rectangle(x, y, 30, 20);

        }
        //speed ska ändras lite grann varje gång den åker ned
        public void Update()
        {
            enemyrect.x += speed;

            if (enemyrect.x > originalX + 170 || enemyrect.x < originalX - 200)
            {

                speed = -speed;
                enemyrect.y += 10;
            }


        }
    }


    //Få så att en hel lista med invaders kommer att bli till ett mönster så som i originalet. G
    //Kom ihåg att göra så att när invaders.count = 0 så ska det bli en boss fight eller något.
    class Program
    {
        static void Main(string[] args)
        {

            Raylib.InitWindow(800, 600, "Spelgrej");
            float playerx = 375;
            float playery = 550;
            float playerspeed = 7f;
            float BulletX = playerx;
            float BulletY = playery;
            //score
            int InitialScore = 0;

            Rectangle Bullet;


            List<Rectangle> bullets = new List<Rectangle>();
            List<Invader> invaders = new List<Invader>();

            invaders.Add(new Invader(200, 20));
            invaders.Add(new Invader(250, 20));
            invaders.Add(new Invader(300, 20));
            invaders.Add(new Invader(350, 20));
            invaders.Add(new Invader(400, 20));
            invaders.Add(new Invader(450, 20));
            invaders.Add(new Invader(500, 20));
            invaders.Add(new Invader(550, 20));
            invaders.Add(new Invader(600, 20));






            Raylib.SetTargetFPS(60);

            //main gameloop
            while (!Raylib.WindowShouldClose())
            {
                Raylib.ClearBackground(Color.GRAY);

                //spelaren ritas
                Rectangle Player = new Rectangle(playerx, playery, 50, 30);

                //Invaders rör på sig
                foreach (var invader in invaders)
                {
                    invader.Update();
                }

                //detta gör att när en bullet nuddar en enemy så går den till listan bulletstoremove där den tas bort. Det gör även att enemyn försvinner. 
                List<Rectangle> bulletsToRemove = new List<Rectangle>();

                foreach (var bullet in bullets)
                {
                    foreach (var invader in invaders)
                    {
                        if (Raylib.CheckCollisionRecs(bullet, invader.enemyrect))
                        {
                            invader.killed = true;

                            //bullets.Remove(bullet);
                            bulletsToRemove.Add(bullet);
                            InitialScore += 100;

                        }

                    }


                }

                foreach (Rectangle bullet in bulletsToRemove)
                {
                    bullets.Remove(bullet);
                }
                bulletsToRemove.Clear();




                //Tar bort alla invaders där killed är true
                invaders.RemoveAll(invader => invader.killed == true);





                //Bullet physics



                //Player movement



                if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
                {
                    playerx += playerspeed;
                }

                if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
                {
                    playerx -= playerspeed;
                }




                if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE) && bullets.Count < 2)
                {
                    Bullet = new Rectangle(Player.x + 22, Player.y, 5, 5);

                    bullets.Add(Bullet);
                }

                for (int i = 0; i < bullets.Count; i++)
                {
                    bullets[i] = new Rectangle(bullets[i].x, bullets[i].y - 5, 5, 5);

                }

                //Tar bort alla bullets där y är mindre än noll
                bullets.RemoveAll(b => b.y < 0);

                //Raylib.DrawText($"{bullets.Count}", 8, 8, 21, Color.WHITE);

                //score count
                Raylib.DrawText($"{InitialScore}", 8, 8, 21, Color.WHITE);



                if (playerx < 0) playerx = 0;
                if (playerx > 800 - Player.width) playerx = 800 - Player.width;
                if (playery < 0) playery = 0;
                if (playery > 600 - Player.height) playery = 600 - Player.height;

                for (int i = 0; i < bullets.Count; i++)
                {
                    Raylib.DrawRectangleRec(bullets[i], Color.BLACK);

                }

                foreach (var invader in invaders)
                {
                    Raylib.DrawRectangleRec(invader.enemyrect, Color.GREEN);
                }

                Raylib.DrawRectangleRec(Player, Color.RED);

                Raylib.EndDrawing();


            }
        }



    }
}