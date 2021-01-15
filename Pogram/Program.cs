using System;
using System.Collections.Generic;
using Raylib_cs;
using System.Numerics;


namespace Pogram
{


    class Invader
    {
        public bool killed = false;
        public float originalX;
        public float originalY;
        public float speed = 0.8f;
        public Rectangle enemyrect;
        public bool gameOver = false;
        public Invader(int x, int y)
        {
            this.originalX = x;
            this.originalY = y;
            this.enemyrect = new Rectangle(x, y, 30, 20);

        }

        public void Update()
        {
            //invaders rör på sig på x axeln
            enemyrect.x += speed;

            //när invaders kolliderar med en vägg
            if ((enemyrect.x > originalX + 170 || enemyrect.x < originalX - 200))
            {
                //Kommer öka speed vid varje kollision så länge den inte är större än 3
                if (Math.Abs(speed) < 3)
                {
                    speed *= 1.1f;
                }
                //Den hoppar ned ett steg på y axeln och byter håll på x axeln
                speed = -speed;
                enemyrect.y += 20;

            }
            //För att testa om speed fungerar
            Raylib.DrawText(speed.ToString(), 500, 0, 32, Color.WHITE);
        }


    }




    //Fixa gunBullets
    //Kom ihåg att göra så att när invaders.count = 0 så ska det bli en boss fight eller något.
    //gör ett Gunship som följer efter spelarens x och skjuter en bullet med ett visst mellanrum, om man skjuter ned ett Gunship så får man mycket points, men det respawnar lite senare.

    class Program
    {
        static void Main(string[] args)
        {
            //fönstet ritas 800 x 600 stort
            Raylib.InitWindow(800, 600, "Spelgrej");


            //random generator
            Random generator = new Random();

            //Floats
            float playerx = 375;
            float playery = 550;
            float playerspeed = 4f;
            float BulletX = playerx;
            float BulletY = playery;
            float GunshipX = 350;
            float GunshipSpeed = 1f;
            float GunY = 70;
            float GunBulletSpeed = 6f;

            //Ints
            int InitialScore = 0;
            //Spelet ska alltid starta med startrummet (0)
            int currentRoom = 0;


            //Int gunShoot kommer att slumpas senare så den är separat från resten av alla Ints
            int gunShoot = 1;

            int gunshipHealth = 10;


            //Bools
            bool GunshipIsSpawned = false;

            //rectangles
            Rectangle Bullet;
            Rectangle GunBullet;

            //Listor
            List<Rectangle> bullets = new List<Rectangle>();
            List<Invader> invaders = new List<Invader>();
            List<Rectangle> gunBullets = new List<Rectangle>();

            //Fonts
            Font f1 = Raylib.LoadFont("PaladinsLaser-BERx.otf");
            Font f2 = Raylib.LoadFont("Revamped-X3q1a.ttf");


            //timer
            float timerMax = 1;
            float timerNow = timerMax;





            //Invaders position fastställs. Koden är skrivit på så sätt att man bara kan ändra siffran efter (i <_) för att ändra hur många rader av invaders det ska vara.
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


            //Detta sätter en fast framerate. Om man inte har specificerat någon kommer programmet att alltid försöka ha så hög framerate som möjligt
            //Det kan göra att saker som exempelvis har 1 speed rör sig 1, varje frame, men om frameraten ändras (exempelvis när man rör på muspekaren), så kommer hur snabbt den rör på sig ändras.
            Raylib.SetTargetFPS(60);

            //laddar bagrunder
            Texture2D spaceImage = Raylib.LoadTexture(@"bilder/space.png");
            Texture2D starsImage = Raylib.LoadTexture(@"bilder/stars.png");

            //main gameloop
            while (!Raylib.WindowShouldClose())
            {


                Raylib.BeginDrawing();




                //Startrum (0)
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

                //spelrum (1)
                else if (currentRoom == 1)
                {
                    //bakgrunden ritas
                    Raylib.DrawTexture(starsImage, 0, 0, Color.WHITE);

                    //spelarens dimensioner fastställs
                    Rectangle Player = new Rectangle(playerx, playery, 50, 30);

                    //Gunships dimensioner fastställs
                    Rectangle Gunship = new Rectangle(GunshipX, 50, 100, 20);

                    //gun 1, 2, 3, och 4, och bullet dimensioner fastställs
                    Rectangle Gun1 = new Rectangle(GunshipX, GunY, 10, 20);
                    Rectangle Gun2 = new Rectangle(GunshipX + 30, GunY, 10, 20);
                    Rectangle Gun3 = new Rectangle(GunshipX + 60, GunY, 10, 20);
                    Rectangle Gun4 = new Rectangle(GunshipX + 90, GunY, 10, 20);
                    //Rectangle gunBullet = new Rectangle(GunshipX + 3, GunY, 4, 8);

                    //timern börjar köras
                    timerNow -= Raylib.GetFrameTime();

                    if (timerNow < 0)
                    {
                        //slumpar ett tal mellan 1-4
                        gunShoot = generator.Next(1, 5);

                        timerNow = timerMax;
                    }




                    //Varje gång update körs så rör invaders på sig
                    foreach (var invader in invaders)
                    {
                        invader.Update();
                    }


                    //Bullet physics
                    //detta gör att när en bullet nuddar en enemy så går den till listan stuffToRemove där den tas bort. Det gör även att enemyn försvinner
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



                    //När invaders har kommit ned till player så går det över till game over rummet (2)
                    foreach (var invader in invaders)
                    {
                        if (invader.enemyrect.y > playery - Player.height)
                        {
                            currentRoom = 2;
                        }
                    }



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



                    //Spawnar en bullet på mellanslag så länge det inte är mer än 2 ute. 
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

                    Vector2 scoreSize = Raylib.MeasureTextEx(f2, "Score:", 20, 0);

                    Raylib.DrawTextEx(f2, "Score:", new Vector2(0, 0), 20, 0, Color.WHITE);
                    Raylib.DrawTextEx(f2, $"{InitialScore}", new Vector2(scoreSize.X, 0), 20, 0, Color.WHITE);



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



                    //Gunship kod
                    if (invaders.Count < 1)
                    {

                        //Gunship ritas
                        Raylib.DrawRectangleRec(Gunship, Color.GREEN);
                        Raylib.DrawRectangleRec(Gun1, Color.GREEN);
                        Raylib.DrawRectangleRec(Gun2, Color.GREEN);
                        Raylib.DrawRectangleRec(Gun3, Color.GREEN);
                        Raylib.DrawRectangleRec(Gun4, Color.GREEN);

                        GunshipIsSpawned = true;



                        if (GunshipIsSpawned == true)
                        {
                            Vector2 healthText = Raylib.MeasureTextEx(f2, $"Gunship health{gunshipHealth}", 20, 0);

                            Raylib.DrawTextEx(f2, $"Gunship health{gunshipHealth}", new Vector2(400 - healthText.X / 2, 0), 20, 0, Color.GREEN);


                            if ((GunshipX > 700 || GunshipX < 0))
                            {
                                GunshipSpeed = -GunshipSpeed;
                            }
                            GunshipX += GunshipSpeed;




                            if (gunShoot == 1)
                            {

                                GunBullet = new Rectangle(GunshipX + 5 / 2, GunY, 5, 10);

                                gunBullets.Add(GunBullet);
                                gunShoot = 0;


                            }
                            else if (gunShoot == 2)
                            {

                                GunBullet = new Rectangle(GunshipX + 30 + 5 / 2, GunY, 5, 10);

                                gunBullets.Add(GunBullet);
                                gunShoot = 0;
                            }
                            else if (gunShoot == 3)
                            {

                                GunBullet = new Rectangle(GunshipX + 60 + 5 / 2, GunY, 5, 10);

                                gunBullets.Add(GunBullet);

                                gunShoot = 0;
                            }

                            else if (gunShoot == 4)
                            {

                                GunBullet = new Rectangle(GunshipX + 90, GunY, 5, 10);

                                gunBullets.Add(GunBullet);
                                gunShoot = 0;
                            }

                            gunBullets.RemoveAll(b => b.y > 600);

                            for (int i = 0; i < gunBullets.Count; i++)
                            {
                                gunBullets[i] = new Rectangle(gunBullets[i].x, gunBullets[i].y + GunBulletSpeed, 5, 10);

                            }
                            for (int i = 0; i < gunBullets.Count; i++)
                            {
                                Raylib.DrawRectangleRec(gunBullets[i], Color.GREEN);

                            }



                            foreach (var gunBullet in gunBullets)
                            {
                                if (Raylib.CheckCollisionRecs(gunBullet, Player))
                                {
                                    currentRoom = 2;
                                }
                            }

                            foreach (var bullet in bullets)
                            {
                                if (Raylib.CheckCollisionRecs(bullet, Gunship))
                                {
                                    bulletsToRemove.Add(bullet);
                                    bulletsToRemove.Clear();
                                    gunshipHealth -= 1;
                                }


                            }


                            if (gunshipHealth < 1)
                            {
                                currentRoom = 3;

                            }



                        }



                    }


                    Raylib.DrawRectangleRec(Player, Color.RED);


                }

                //game over rum
                //Fixa detta
                else if (currentRoom == 2)
                {
                    Raylib.DrawTexture(spaceImage, 0, 0, Color.WHITE);

                    Vector2 gameOverSize = Raylib.MeasureTextEx(f2, "Game over", 100, 0);
                    Raylib.DrawTextEx(f2, "Game over", new Vector2(400 - gameOverSize.X / 2, 200), 100, 0, Color.RED);

                    Vector2 pressEnterSize = Raylib.MeasureTextEx(f2, "Press enter to exit", 20, 0);
                    Raylib.DrawTextEx(f2, "Press enter to exit", new Vector2(400 - pressEnterSize.X / 2, 300), 20, 0, Color.RED);
                    if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                    {
                        Raylib.EndDrawing();

                    }
                }

                else if (currentRoom == 3)
                {
                    Raylib.DrawTexture(spaceImage, 0, 0, Color.WHITE);

                    Vector2 scoreSize = Raylib.MeasureTextEx(f2, "Score:", 20, 0);

                    Raylib.DrawTextEx(f2, "Score:", new Vector2(0, 0), 20, 0, Color.GREEN);
                    Raylib.DrawTextEx(f2, $"{InitialScore}", new Vector2(scoreSize.X, 0), 20, 0, Color.GREEN);

                }


                Raylib.EndDrawing();



            }
        }



    }
}