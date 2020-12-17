using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;


namespace Pogram
{

    class PlayerCharacter
    {
        public bool gameOver = false;
    }
    class Invader
    {
        public bool killed = false;
        public float originalX;
        public float originalY;
        public float speed = 0.8f;
        public Rectangle enemyrect;

        public Invader(int x, int y)
        {
            this.originalX = x;
            this.originalY = y;
            this.enemyrect = new Rectangle(x, y, 30, 20);

        }
        //speed ska ändras lite grann varje gång den åker ned
        public void Update()
        {
            enemyrect.x += speed;


            if ((enemyrect.x > originalX + 170 || enemyrect.x < originalX - 200))
            {
                if (Math.Abs(speed) < 3)
                {
                    speed *= 1.1f;
                }
                speed = -speed;
                enemyrect.y += 20;

            }


            Raylib.DrawText(speed.ToString(), 500, 0, 32, Color.WHITE);
        }


    }




    //gör så att när gameover = true så går man till game over, game over blir om invader kolliderar med player.y bla. 
    //Kom ihåg att göra så att när invaders.count = 0 så ska det bli en boss fight eller något.
    //gör ett gunship som följer efter spelarens x och skjuter en bullet med ett visst mellanrum, om man skjuter ned ett gunship så får man mycket points, men det respawnar lite senare.

    class Program
    {
        static void Main(string[] args)
        {

            Raylib.InitWindow(800, 600, "Spelgrej");
            float playerx = 375;
            float playery = 550;
            float playerspeed = 4f;
            float BulletX = playerx;
            float BulletY = playery;
            //score
            int InitialScore = 0;

            Rectangle Bullet;


            List<Rectangle> bullets = new List<Rectangle>();
            List<Invader> invaders = new List<Invader>();

            Font f1 = Raylib.LoadFont("PaladinsLaser-BERx.otf");
            Font f2 = Raylib.LoadFont("Revamped-X3q1a.ttf");





            for (int i = 0; i < 4; i++)
            {
                invaders.Add(new Invader(200, 20 + i * 40));
                invaders.Add(new Invader(250, 20 + i * 40));
                invaders.Add(new Invader(300, 20 + i * 40));
                invaders.Add(new Invader(350, 20 + i * 40));
                invaders.Add(new Invader(400, 20 + i * 40));
                invaders.Add(new Invader(450, 20 + i * 40));
                invaders.Add(new Invader(500, 20 + i * 40));
                invaders.Add(new Invader(550, 20 + i * 40));
                invaders.Add(new Invader(600, 20 + i * 40));

            }

            int currentRoom = 0;


            Raylib.SetTargetFPS(60);

            //laddar bagrunder

            Texture2D spaceImage = Raylib.LoadTexture(@"bilder/space.png");
            Texture2D starsImage = Raylib.LoadTexture(@"bilder/stars.png");

            //Image spaceImage = Raylib.LoadImage(@"bilder/space.png");
            //Image starsImage = Raylib.LoadImage(@"bilder/stars.png");

            //main gameloop
            while (!Raylib.WindowShouldClose())
            {


                Raylib.BeginDrawing();

                if (currentRoom == 0)
                {
                    //Bakgrunden ritas, är nu bakgrund 1
                    Raylib.DrawTexture(spaceImage, 0, 0, Color.WHITE);

                    //Mäter textens bredd och höjd
                    Vector2 titleSize = Raylib.MeasureTextEx(f1, "Astro Attackers", 50, 0);
                    //Text ritas centrerad
                    Raylib.DrawTextEx(f1, "Astro Attackers", new Vector2(400 - titleSize.X / 2, 140), 50, 0, Color.GREEN);

                    Vector2 textSize = Raylib.MeasureTextEx(f2, "press enter to start playing", 20, 0);
                    Raylib.DrawTextEx(f2, "press enter to start playing", new Vector2(400 - textSize.X / 2, 200), 20, 0, Color.GREEN);


                    if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                    {
                        currentRoom = 1;
                    }
                }

                //spelrum
                else if (currentRoom == 1)
                {
                    //bakgrunden ritas
                    Raylib.DrawTexture(starsImage, 0, 0, Color.WHITE);

                    //spelaren ritas
                    Rectangle Player = new Rectangle(playerx, playery, 50, 30);


                    //Varje gång update körs så rör invaders på sig
                    foreach (var invader in invaders)
                    {
                        invader.Update();
                    }


                    //Bullet physics
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
                        bullets[i] = new Rectangle(bullets[i].x, bullets[i].y - 6, 5, 5);

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
                        Raylib.DrawRectangleRec(bullets[i], Color.RED);

                    }

                    foreach (var invader in invaders)
                    {
                        Raylib.DrawRectangleRec(invader.enemyrect, Color.GREEN);
                    }

                    Raylib.DrawRectangleRec(Player, Color.RED);


                }
                //game over rum
                else if (currentRoom == 2)
                {

                }
                Raylib.EndDrawing();



            }
        }



    }
}