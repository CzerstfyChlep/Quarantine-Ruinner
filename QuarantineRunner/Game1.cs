using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace QuarantineRunner
{
   
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Vector2 PlayerHitBoxPos = new Vector2(14 * 2,9 * 2);
        public static Vector2 PlayerHitBoxSize = new Vector2(36 * 2, 50 * 2);
        public static List<UIWindow> UIWindows = new List<UIWindow>();
        public static UIWindow Grabbed = null;
        public static int ScrollValue = 0;
        public static Vector2 ScreenResolution = new Vector2();
        public static TextInput FocusedText = null;
        public static Vector2 LastMousePosition = new Vector2();
        KeyboardControl KeyboardC = new KeyboardControl();
        public static Label PointCounter = null;
        public static Label PaperCounter = null;
        public static bool HelpIsOn = true;

        public static Texture2D LeftShelf0, RightShelf0, LeftShelf1, RightShelf1, Runner0, Runner1, Virus, Mask, ToiletPaper,EmptyHeart, BaseTile, Heart;
        public static SpriteFont Font;
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 896;
            graphics.PreferredBackBufferWidth = 896;
            ScreenResolution = new Vector2(896, 896);
            IsMouseVisible = true;
            Window.Title = "Quarantine Ru(i)nner by CzerstfyChlep";           
        }

        public static List<Tile> Tiles = new List<Tile>();
        public static List<Object> Objects = new List<Object>();

        protected override void Initialize()
        {          
            base.Initialize();
        }

        public static UIWindow HelpWindow = null;
       
        protected override void LoadContent()
        {
        
            spriteBatch = new SpriteBatch(GraphicsDevice);
            BaseTile = Content.Load<Texture2D>("Base Tile");
            LeftShelf0 = Content.Load<Texture2D>("Left Shelf 0");
            RightShelf0 = Content.Load<Texture2D>("Right Shelf 0");
            LeftShelf1 = Content.Load<Texture2D>("Left Shelf 1");
            RightShelf1 = Content.Load<Texture2D>("Right Shelf 1");
            Runner0 = Content.Load<Texture2D>("Runner 0");
            Runner1 = Content.Load<Texture2D>("Runner 1");
            Virus = Content.Load<Texture2D>("Virus");
            Mask = Content.Load<Texture2D>("Mask");
            ToiletPaper = Content.Load<Texture2D>("Toilet Paper");
            Font = Content.Load<SpriteFont>("Font");
            Heart = Content.Load<Texture2D>("Heart");
            EmptyHeart = Content.Load<Texture2D>("Empty Heart");

            for (int a = 0; a < 7; a++)
            {
                Tiles.Add(new Tile(new Vector2(0, 128 * a), LeftShelf0));
                Tiles.Add(new Tile(new Vector2(128 * 1, 128 * a), BaseTile));
                Tiles.Add(new Tile(new Vector2(128 * 2, 128 * a), BaseTile));
                Tiles.Add(new Tile(new Vector2(128 * 3, 128 * a), BaseTile));
                Tiles.Add(new Tile(new Vector2(128 * 4, 128 * a), BaseTile));
                Tiles.Add(new Tile(new Vector2(128 * 5, 128 * a), BaseTile));
                Tiles.Add(new Tile(new Vector2(128 * 6, 128 * a), RightShelf0));

            }
            Tiles.Add(new Tile(new Vector2(0, -128), LeftShelf0));
            Tiles.Add(new Tile(new Vector2(128 * 1, -128), BaseTile));
            Tiles.Add(new Tile(new Vector2(128 * 2, -128), BaseTile));
            Tiles.Add(new Tile(new Vector2(128 * 3, -128), BaseTile));
            Tiles.Add(new Tile(new Vector2(128 * 4, -128), BaseTile));
            Tiles.Add(new Tile(new Vector2(128 * 5, -128), BaseTile));
            Tiles.Add(new Tile(new Vector2(128 * 6, -128), RightShelf1));



            UIWindow Score = new UIWindow()
            {
                Position = new Vector2(806, 0),
                Size = new Vector2(100, 90),
            };
            Label Paper = new Label("pap", "0", Color.Black, new Vector2(40, 10), Font);
            Paper.Size = new Vector2(60, 28);
            Paper.Alignment = TextAlignemt.Center;
            Score.AddControl(Paper);
            PaperCounter = Paper;

            Label ScoreLabel = new Label("score", "0", Color.Black, new Vector2(0, 65), new Vector2(100, 0), Font);
            ScoreLabel.Alignment = TextAlignemt.Center;
            Score.AddControl(ScoreLabel);
            PointCounter = ScoreLabel;
            UIWindows.Add(Score);

            UIWindow Help = new UIWindow()
            {
                Title = "Manual",
                Movable = false,
                Size = new Vector2(600,450),
                Position = new Vector2(ScreenResolution.X / 2 - 600/2, ScreenResolution.Y / 2 - 450 / 2)
            };

            Button close = new Button("", "Begin the hunt", Color.Black, new Vector2(200, 400), Font, new Vector2(200,30), new Action<object>(CloseHelp));
            Label HelpText = new Label("", "A viral disease has struck our society.", Color.Black, new Vector2(0, 70), new Vector2(600, 0), Font);
            Label HelpText1 = new Label("", "You, a brave hero, have decided", Color.Black, new Vector2(0, 95), new Vector2(600, 0), Font);
            Label HelpText2 = new Label("", "to prepare yourself for the quarantine.", Color.Black, new Vector2(0, 120), new Vector2(600, 0), Font);
            Label HelpText3 = new Label("", "Your mission is simple: GET. THE. TOILET PAPER.", Color.Black, new Vector2(0, 145), new Vector2(600, 0), Font);

            Label HelpText4 = new Label("", "Collect precious toilet paper!", Color.Black, new Vector2(0, 170 + 30), new Vector2(600, 0), Font);
            Label HelpText5 = new Label("", "Avoid deadly virus!", Color.Black, new Vector2(0, 195 + 60), new Vector2(600, 0), Font);
            Label HelpText6 = new Label("", "Protect yourself with masks!", Color.Black, new Vector2(0, 220 + 90), new Vector2(600, 0), Font);

            Label HelpText7 = new Label("", "Use arrow keys or A/D to move", Color.Black, new Vector2(0, 360), new Vector2(600, 0), Font);

            //Label HelpText2 = new Label("", "A viral disease has struck our society.\nYou, a brave hero, decided to prepare yourself for the quarantine.\nYour mission is simple: GET. THE. TOILET PAPER.", Color.Black, new Vector2(0, 40), new Vector2(300, 0), Font);
            HelpText.Alignment = TextAlignemt.Center;
            HelpText1.Alignment = TextAlignemt.Center;
            HelpText2.Alignment = TextAlignemt.Center;
            HelpText3.Alignment = TextAlignemt.Center;
            HelpText4.Alignment = TextAlignemt.Center;
            HelpText5.Alignment = TextAlignemt.Center;
            HelpText6.Alignment = TextAlignemt.Center;
            HelpText7.Alignment = TextAlignemt.Center;

            Help.AddControl(HelpText);
            Help.AddControl(HelpText1);
            Help.AddControl(HelpText2);
            Help.AddControl(HelpText3);
            Help.AddControl(HelpText4);
            Help.AddControl(HelpText5);
            Help.AddControl(HelpText6);
            Help.AddControl(HelpText7);
            Help.AddControl(close);
            HelpWindow = Help;
            UIWindows.Add(Help);
        }

        public static void Reset()
        {
            HelpWindow.Visible = true;
            HelpIsOn = true;
           
            Health = 5;          
            PlayerSpeed = 2;
            Row = 0;
            Created = false;
            PlayerPosition = new Vector2(0, 128 * 6);
            Invincibility = 0;
            Objects.Clear();
            Tiles.Clear();
            for (int a = 0; a < 7; a++)
            {
                Tiles.Add(new Tile(new Vector2(0, 128 * a), LeftShelf0));
                Tiles.Add(new Tile(new Vector2(128 * 1, 128 * a), BaseTile));
                Tiles.Add(new Tile(new Vector2(128 * 2, 128 * a), BaseTile));
                Tiles.Add(new Tile(new Vector2(128 * 3, 128 * a), BaseTile));
                Tiles.Add(new Tile(new Vector2(128 * 4, 128 * a), BaseTile));
                Tiles.Add(new Tile(new Vector2(128 * 5, 128 * a), BaseTile));
                Tiles.Add(new Tile(new Vector2(128 * 6, 128 * a), RightShelf0));

            }
            Tiles.Add(new Tile(new Vector2(0, -128), LeftShelf0));
            Tiles.Add(new Tile(new Vector2(128 * 1, -128), BaseTile));
            Tiles.Add(new Tile(new Vector2(128 * 2, -128), BaseTile));
            Tiles.Add(new Tile(new Vector2(128 * 3, -128), BaseTile));
            Tiles.Add(new Tile(new Vector2(128 * 4, -128), BaseTile));
            Tiles.Add(new Tile(new Vector2(128 * 5, -128), BaseTile));
            Tiles.Add(new Tile(new Vector2(128 * 6, -128), RightShelf1));


        } 
        public static void CloseHelp(object arg)
        {
            HelpWindow.Visible = false;
            HelpIsOn = false;
            ToiletPapers = 0;
            Points = 0;
        }

        protected override void UnloadContent()
        {
           
        }

      
        

        public static float PlayerSpeed = 2;
        public static float Row = 0;
        public static Vector2 PlayerPosition = new Vector2(0, 128 * 6);
        public static Random GlobalRandom = new Random();
        public static bool Created = false;
        public static int PlayerAnimation = 0;
        public static int AnimationCool = 0;
        public static int Points = 0;
        public static int Health = 5;
        public static int ToiletPapers = 0;

        public static int Invincibility = 0;
        public static bool InvFade = false;

  

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            

            if (KeyboardC.KeyDelay < 4)
            {
                if (FocusedText != null)
                {
                    FocusedText.Text += KeyboardC.GetTextKey();
                    Keys k = KeyboardC.GetControlKey();
                    switch (k)
                    {
                        case Keys.Back:
                            if (FocusedText.Text.Length > 0)
                                FocusedText.Text = FocusedText.Text.Remove(FocusedText.Text.Length - 1);
                            break;                                                          
                    }
                }               
            }

            if (KeyboardC.KeyDelay > 0)
                KeyboardC.KeyDelay--;


            List<Object> ObjectsToRemove = new List<Object>();

            HandleMouse.Handle(Mouse.GetState());
            LastMousePosition.X = Mouse.GetState().Position.X;
            LastMousePosition.Y = Mouse.GetState().Position.Y;

            if (Invincibility > 0)
                Invincibility--;

            if (Invincibility % 15 == 0)
                InvFade = !InvFade;

            if (Invincibility == 0)
                InvFade = false;




            foreach (Object o in Objects)
            {
                if (RectangleIntersect(o.HitBoxPos + o.Position, o.HitBoxSize, PlayerHitBoxPos + PlayerPosition + new Vector2(128, 0), PlayerHitBoxSize))
                {
                    ObjectsToRemove.Add(o);
                    if (o.Type == 1)
                    {
                        ToiletPapers++;
                        Points += 100;
                    }
                    else if (o.Type == 2 && Invincibility == 0) {
                        if (PlayerSpeed > 14)
                            PlayerSpeed -= 12;
                        else
                            PlayerSpeed = 2;
                        Health--;
                        Invincibility = 250;
                        if (Points > 500)
                            Points -= 500;
                        else
                            Points = 0;
                        if (Health == 0)
                            Reset();

                    }
                    else if(o.Type == 3)
                    {
                        if (Health < 5)
                            Health++;
                        else
                        {
                            Points += 1000;
                        }
                    }
                }
            }
            Objects.RemoveAll(x => ObjectsToRemove.Contains(x));

            if (!HelpIsOn)
            {
                if (AnimationCool == 8)
                {
                    if (PlayerAnimation == 0)
                        PlayerAnimation = 1;
                    else
                        PlayerAnimation = 0;
                    AnimationCool = 0;
                }
                else
                    AnimationCool++;

                KeyboardState keyboardstate = Keyboard.GetState();
                if (keyboardstate.IsKeyDown(Keys.Left) || keyboardstate.IsKeyDown(Keys.A))
                {
                    if (PlayerPosition.X > -22 + PlayerSpeed / 1.5f)
                        PlayerPosition -= new Vector2(4 + PlayerSpeed / 1.5f, 0);
                    else
                        PlayerPosition.X = -22;
                }
                else if (keyboardstate.IsKeyDown(Keys.Right) || keyboardstate.IsKeyDown(Keys.D))
                {
                    if (PlayerPosition.X < 128 * 4 + 22)
                        PlayerPosition += new Vector2(4 + PlayerSpeed / 1.5f, 0);
                    else
                        PlayerPosition.X = 128 * 4 + 22;
                }
            }
            bool crt = false;
            bool rls = false;
            List<Tile> ToRemove = new List<Tile>();

            if (!HelpIsOn)
            {
                PlayerPosition.Y -= PlayerSpeed;
            }

            foreach (Tile t in Tiles)
            {              
                if (t.Position.Y >= PlayerPosition.Y + 32)
                {
                    crt = true;                   
                }          
                if(t.Position.Y >= PlayerPosition.Y + 128)
                {
                    ToRemove.Add(t);
                    rls = true;
                }
            }
            Tiles.RemoveAll(x => ToRemove.Contains(x));
            if (rls)
                Created = false;





            if (crt && !Created)
            {
                Row++;
                if(GlobalRandom.Next(0,8) == 1)
                    Tiles.Add(new Tile(new Vector2(0, Row * -128), LeftShelf1));
                else
                    Tiles.Add(new Tile(new Vector2(0, Row * -128), LeftShelf0)); 
                
                Tiles.Add(new Tile(new Vector2(128 * 1, Row * -128), BaseTile));                             
                Tiles.Add(new Tile(new Vector2(128 * 2, Row * -128), BaseTile));                              
                Tiles.Add(new Tile(new Vector2(128 * 3, Row * -128), BaseTile));                               
                Tiles.Add(new Tile(new Vector2(128 * 4, Row * -128), BaseTile));
                Tiles.Add(new Tile(new Vector2(128 * 5, Row * -128), BaseTile));


                int VirusChance = 30;
                int MaxVirus = 2;

                int PaperChance = 50;

                int MaskChance = 1000;


                if(Row < 100)
                {
                    VirusChance = 25;
                    MaxVirus = 3;
                    PaperChance = 45;
                }
                else if(Row < 250)
                {
                    VirusChance = 20;
                    MaxVirus = 3;
                    PaperChance = 35;
                }
                else if(Row < 500)
                {
                    VirusChance = 15;
                    MaxVirus = 4;
                    PaperChance = 30;
                    MaskChance = 750;
                }
                else
                {
                    VirusChance = 12;
                    MaxVirus = 4;
                    PaperChance = 25;
                    MaskChance = 666;
                }


                int[] order = new int[5] {-1, -1, -1, -1, -1};
                for(int a = 0; a < 5; a++)
                {
                    int place = -1;
                    do
                    {
                        place = GlobalRandom.Next(0, 5);
                    }
                    while (order[place] != -1);
                    order[place] = a;
                }


                int viruscount = 0;
                foreach(int a in order)
                {
                    
                    if (GlobalRandom.Next(0, VirusChance) == 1)
                    {
                        if (viruscount < MaxVirus)
                        {
                            Objects.Add(new Object(new Vector2(128 * (a + 1), Row * -128), 2));
                        }
                    }
                    else if (GlobalRandom.Next(0, PaperChance) == 1)
                    {
                        Objects.Add(new Object(new Vector2(128 * (a + 1) -32, Row * -128 - 32), 1));
                    }
                    else if (GlobalRandom.Next(0, MaskChance) == 1)
                    {
                        Objects.Add(new Object(new Vector2(128 * (a + 1) -32, Row * -128 - 32), 3));
                    }
                    
                    

                }


                if (GlobalRandom.Next(0, 8) == 1)
                    Tiles.Add(new Tile(new Vector2(128 * 6, Row * -128), RightShelf1));
                else
                    Tiles.Add(new Tile(new Vector2(128 * 6, Row * -128), RightShelf0));
                Created = true;
            }

            if (!HelpIsOn)
            {
                if (PlayerSpeed < 20)
                    PlayerSpeed += 0.05f;
            }
            PaperCounter.Text = ToiletPapers + "";


            if(PlayerSpeed > 15)
                Points += (int)(PlayerSpeed * 0.1f);


            PointCounter.Text = Points + "";
           

            base.Update(gameTime);
        }

       
        public bool RectangleIntersect(Vector2 rec1pos, Vector2 rec1siz, Vector2 rec2pos, Vector2 rec2siz)
        {
            if ((rec2pos.X + rec2siz.X >= rec1pos.X && rec2pos.X <= rec1pos.X + rec1siz.X) && (rec2siz.Y + rec2pos.Y >= rec1pos.Y && rec2pos.Y <= rec1pos.Y + rec1siz.Y))
            {
                return true;
            }
            else
                return false;          
        }


        readonly RasterizerState _rasterizerState = new RasterizerState() { ScissorTestEnable = true };

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            bool crt = false;
           
            foreach(Tile t in Tiles)
            {
                spriteBatch.Draw(t.Texture, t.Position - new Vector2(0, PlayerPosition.Y) + new Vector2(0, 128 * 6), color: Color.White, scale: new Vector2(2, 2));               
            }

            foreach(Object o in Objects)
            {
                switch (o.Type)
                {
                    case 1:
                        spriteBatch.Draw(ToiletPaper, o.Position - new Vector2(0, PlayerPosition.Y) + new Vector2(64, 128 * 6 + 64), color: Color.White, scale: new Vector2(1, 1));
                        break;
                    case 2:                        
                        spriteBatch.Draw(Virus, o.Position - new Vector2(0, PlayerPosition.Y) + new Vector2(0, 128 * 6), color: Color.White, scale: new Vector2(2, 2));                        
                        break;
                    case 3:
                        spriteBatch.Draw(Mask, o.Position - new Vector2(0, PlayerPosition.Y) + new Vector2(64, 128 * 6 + 64), color: Color.White, scale: new Vector2(1, 1));
                        break;
                }
            }
            
                //spriteBatch.DrawString(Font, Row + "", new Vector2(), Color.Black);

            for (int a = 0; a < 5; a++)
            {
                spriteBatch.Draw(EmptyHeart, new Vector2(a * 64, 0), color: Color.White, scale: new Vector2(1, 1));
            }
            for (int a = 0; a< Health; a++)
            {
                spriteBatch.Draw(Heart, new Vector2(a * 64,0), color: Color.White, scale: new Vector2(1, 1));
            }
            /*
            for (int a = 0; a < 7; a++)
            {
                spriteBatch.Draw(LeftShelf0, new Vector2(0, 128 * a), color: Color.White, scale: new Vector2(2, 2));
                spriteBatch.Draw(BaseTile, new Vector2(128 * 1, 128 * a), color: Color.White, scale: new Vector2(2, 2));
                spriteBatch.Draw(BaseTile, new Vector2(128 * 2, 128 * a), color: Color.White, scale: new Vector2(2, 2));
                spriteBatch.Draw(BaseTile, new Vector2(128 * 3, 128 * a), color: Color.White, scale: new Vector2(2, 2));
                spriteBatch.Draw(BaseTile, new Vector2(128 * 4, 128 * a), color: Color.White, scale: new Vector2(2, 2));
                spriteBatch.Draw(BaseTile, new Vector2(128 * 5, 128 * a), color: Color.White, scale: new Vector2(2, 2));
                spriteBatch.Draw(RightShelf1, new Vector2(128 * 6, 128 * a), color: Color.White, scale: new Vector2(2, 2));

            }
            */
            if (!InvFade)
            {
                if (PlayerAnimation == 0)
                    spriteBatch.Draw(Runner0, PlayerPosition * new Vector2(1, 0) + new Vector2(128, 6 * 128), color: Color.White, scale: new Vector2(2, 2));
                else
                    spriteBatch.Draw(Runner1, PlayerPosition * new Vector2(1, 0) + new Vector2(128, 6 * 128), color: Color.White, scale: new Vector2(2, 2));
            }

            spriteBatch.End();
            foreach (UIWindow window in UIWindows)
            {

                if (window.Visible)
                {
                    spriteBatch.Begin(samplerState: SamplerState.PointClamp, rasterizerState: _rasterizerState);
                    DLine.DrawLine(spriteBatch, new Vector2(window.Position.X, window.Position.Y + window.Size.Y / 2), new Vector2(window.Position.X + window.Size.X, window.Position.Y + window.Size.Y / 2), Color.White, window.Size.Y);
                    DLine.DrawLine(spriteBatch, window.Position, new Vector2(window.Position.X + window.Size.X, window.Position.Y), Color.Black, 6);
                    DLine.DrawLine(spriteBatch, new Vector2(window.Position.X + window.Size.X - 3, window.Position.Y), new Vector2(window.Position.X + window.Size.X - 3, window.Position.Y + window.Size.Y), Color.Black, 6);
                    DLine.DrawLine(spriteBatch, new Vector2(window.Position.X + window.Size.X, window.Position.Y + window.Size.Y), new Vector2(window.Position.X, window.Position.Y + window.Size.Y), Color.Black, 6);
                    DLine.DrawLine(spriteBatch, new Vector2(window.Position.X + 3, window.Position.Y + window.Size.Y), window.Position + new Vector2(3, 0), Color.Black, 6);


                    //Title

                    if (window.Title != "")
                    {
                        Vector2 size = Font.MeasureString(window.Title);
                        spriteBatch.DrawString(Font, window.Title, new Vector2(window.Position.X + window.Size.X / 2 - size.X / 2, window.Position.Y + 25 - size.Y / 2), Color.Black);
                        DLine.DrawLine(spriteBatch, new Vector2(window.Position.X + 3, window.Position.Y + 50), new Vector2(window.Position.X + window.Size.X - 3, window.Position.Y + 50), Color.Black, 6);
                    }

                    if (window.Closable)
                    {
                        DLine.DrawLine(spriteBatch, new Vector2(window.Position.X + window.Size.X - 40, window.Position.Y + 10), new Vector2(window.Position.X + window.Size.X - 10, window.Position.Y + 40), Color.Black, 2);
                        DLine.DrawLine(spriteBatch, new Vector2(window.Position.X + window.Size.X - 10, window.Position.Y + 10), new Vector2(window.Position.X + window.Size.X - 40, window.Position.Y + 40), Color.Black, 2);
                    }

                    spriteBatch.End();
                    foreach (Control c in window.GetAllChildren())
                    {
                        spriteBatch.Begin(samplerState: SamplerState.PointClamp, rasterizerState: _rasterizerState);
                        //spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(new Point((int)c.GetAbsolutePosition().X, (int)c.GetAbsolutePosition().Y), new Point((int)c.Size.X, (int)c.Size.Y));
                        if (window.Title != "")
                        {
                            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(new Point((int)window.Position.X, (int)window.Position.Y + 52), new Point((int)window.Size.X, (int)window.Size.Y - 50));
                        }
                        else
                        {
                            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(new Point((int)window.Position.X, (int)window.Position.Y), new Point((int)window.Size.X, (int)window.Size.Y));
                        }
                        if (c.GetAllParents().Any(x => x is Container))
                        {
                            Control w = c.GetAllParents().Find(x => x is Container);
                            spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(new Point((int)w.GetAbsolutePosition().X, (int)w.GetAbsolutePosition().Y), new Point((int)w.Size.X, (int)w.Size.Y));
                        }

                        if (c is Label l)
                        {
                            if (l.Size.X == 0 && l.Size.Y == 0)
                            {
                                int line = 0;
                                foreach (string s in l.Text.Split('\n'))
                                {
                                    Vector2 NewPosition = l.Position + new Vector2(0, l.Font.LineSpacing * line);
                                    string displayedstring = s;
                                    spriteBatch.DrawString(l.Font, displayedstring, l.GetAbsolutePosition() + new Vector2(0, l.Font.LineSpacing * line), l.Color);
                                    line++;
                                }

                            }
                            else
                            {
                                int line = 0;
                                string[] split = l.Text.Split('\n');
                                int totalsizey = split.Length * l.Font.LineSpacing;
                                foreach (string s in split)
                                {
                                    Vector2 PositionToWrite = new Vector2();
                                    Vector2 size = l.Font.MeasureString(s);
                                    Vector2 AbsolutePosition = l.GetAbsolutePosition();

                                    if (l.Alignment == TextAlignemt.TopLeft || l.Alignment == TextAlignemt.Top || l.Alignment == TextAlignemt.TopRight)
                                    {
                                        PositionToWrite.Y = AbsolutePosition.Y;
                                    }
                                    else if (l.Alignment == TextAlignemt.MiddleLeft || l.Alignment == TextAlignemt.Center || l.Alignment == TextAlignemt.MiddleRight)
                                    {
                                        PositionToWrite.Y = AbsolutePosition.Y + l.Size.Y / 2 - totalsizey / 2;
                                    }
                                    else if (l.Alignment == TextAlignemt.BottomLeft || l.Alignment == TextAlignemt.Bottom || l.Alignment == TextAlignemt.BottomRight)
                                    {
                                        PositionToWrite.Y = AbsolutePosition.Y + l.Size.Y - totalsizey;
                                    }

                                    if (l.Alignment == TextAlignemt.TopLeft || l.Alignment == TextAlignemt.MiddleLeft || l.Alignment == TextAlignemt.BottomLeft)
                                    {
                                        PositionToWrite.X = AbsolutePosition.X;
                                    }
                                    else if (l.Alignment == TextAlignemt.Top || l.Alignment == TextAlignemt.Center || l.Alignment == TextAlignemt.Bottom)
                                    {
                                        PositionToWrite.X = AbsolutePosition.X + l.Size.X / 2 - size.X / 2;
                                    }
                                    else if (l.Alignment == TextAlignemt.TopRight || l.Alignment == TextAlignemt.MiddleRight || l.Alignment == TextAlignemt.BottomRight)
                                    {
                                        PositionToWrite.X = AbsolutePosition.X + l.Size.X - size.X;
                                    }
                                    PositionToWrite.Y += l.Font.LineSpacing * line;
                                    spriteBatch.DrawString(l.Font, s, PositionToWrite, l.Color);
                                    line++;

                                }
                            }
                        }
                        else if (c is Scroll s)
                        {
                            DLine.DrawLine(spriteBatch, s.GetAbsolutePosition(), s.GetAbsolutePosition() + new Vector2(s.Size.X, 0), Color.Black, 2);
                            DLine.DrawLine(spriteBatch, s.GetAbsolutePosition() + s.Size, s.GetAbsolutePosition() + new Vector2(s.Size.X, 0), Color.Black, 2);
                            DLine.DrawLine(spriteBatch, s.GetAbsolutePosition() + s.Size, s.GetAbsolutePosition() + new Vector2(0, s.Size.Y), Color.Black, 2);
                            DLine.DrawLine(spriteBatch, s.GetAbsolutePosition(), s.GetAbsolutePosition() + new Vector2(0, s.Size.Y), Color.Black, 2);
                            float size = s.Size.Y / (s.MaxScroll + 1 + s.ScrollShown);
                            DLine.DrawLine(spriteBatch, s.GetAbsolutePosition() + new Vector2(10, size * s.ScrollPosition + 3), s.GetAbsolutePosition() + new Vector2(10, size * (s.ScrollPosition + 1 + s.ScrollShown) - 3), Color.Black, 12);
                        }
                        else if (c is ProgressBar pb)
                        {
                            DLine.DrawLine(spriteBatch, pb.GetAbsolutePosition() + pb.Size * new Vector2(0, 0.5f), pb.GetAbsolutePosition() + pb.Size * new Vector2(1, 0.5f), Color.Red, pb.Size.Y);
                            float leng = pb.Size.X * (pb.Value / pb.Maximum);
                            DLine.DrawLine(spriteBatch, pb.GetAbsolutePosition() + pb.Size * new Vector2(0, 0.5f), pb.GetAbsolutePosition() + pb.Size * new Vector2(0, 0.5f) + new Vector2(leng, 0), Color.Blue, pb.Size.Y);

                            DLine.DrawLine(spriteBatch, pb.GetAbsolutePosition() - new Vector2(2, 0), pb.GetAbsolutePosition() + new Vector2(pb.Size.X + 2, 0), Color.Black, 4);
                            DLine.DrawLine(spriteBatch, pb.GetAbsolutePosition() + pb.Size, pb.GetAbsolutePosition() + new Vector2(pb.Size.X, 0), Color.Black, 4);
                            DLine.DrawLine(spriteBatch, (pb.GetAbsolutePosition() + pb.Size) + new Vector2(2, 0), pb.GetAbsolutePosition() + new Vector2(-2, pb.Size.Y), Color.Black, 4);
                            DLine.DrawLine(spriteBatch, pb.GetAbsolutePosition(), pb.GetAbsolutePosition() + new Vector2(0, pb.Size.Y), Color.Black, 4);

                            int line = 0;
                            string[] split = pb.Text.Split('\n');
                            int totalsizey = split.Length * pb.Font.LineSpacing;
                            foreach (string ss in split)
                            {
                                Vector2 PositionToWrite = new Vector2();
                                Vector2 size = pb.Font.MeasureString(ss);
                                Vector2 AbsolutePosition = pb.GetAbsolutePosition();
                                PositionToWrite.Y = AbsolutePosition.Y + pb.Size.Y / 2 - totalsizey / 2;
                                PositionToWrite.X = AbsolutePosition.X + pb.Size.X / 2 - size.X / 2;
                                PositionToWrite.Y += pb.Font.LineSpacing * line;
                                spriteBatch.DrawString(pb.Font, ss, PositionToWrite, Color.Black);
                                line++;
                            }
                        }
                        else if (c is ExtraLine el)
                        {
                            DLine.DrawLine(spriteBatch, el.GetAbsolutePosition(), el.GetAbsolutePosition(el.SecondPosition), el.Color, el.LineWidth);
                        }
                        else if (c is TextInput t)
                        {
                            Vector2 size = t.Font.MeasureString(t.Text + " ");
                            DLine.DrawLine(spriteBatch, new Vector2(t.GetAbsolutePosition().X, t.GetAbsolutePosition().Y + size.Y / 2), new Vector2(t.GetAbsolutePosition().X + t.Width, t.GetAbsolutePosition().Y + size.Y / 2), Color.White, size.Y);

                            DLine.DrawLine(spriteBatch, window.Position + t.Position, new Vector2(t.GetAbsolutePosition().X + t.Width, t.GetAbsolutePosition().Y), Color.Black, 4);
                            DLine.DrawLine(spriteBatch, new Vector2(t.GetAbsolutePosition().X + t.Width - 2, t.GetAbsolutePosition().Y), new Vector2(t.GetAbsolutePosition().X + t.Width - 2, t.GetAbsolutePosition().Y + size.Y), Color.Black, 4);
                            DLine.DrawLine(spriteBatch, new Vector2(t.GetAbsolutePosition().X + t.Width, t.GetAbsolutePosition().Y + size.Y), new Vector2(t.GetAbsolutePosition().X, t.GetAbsolutePosition().Y + size.Y), Color.Black, 4);
                            DLine.DrawLine(spriteBatch, new Vector2(t.GetAbsolutePosition().X + 2, t.GetAbsolutePosition().Y + size.Y), t.GetAbsolutePosition() + new Vector2(2, 0), Color.Black, 4);

                            string finaltext = t.Text;
                            
                            if (size.X < t.Width - 20)
                                spriteBatch.DrawString(t.Font, finaltext, t.GetAbsolutePosition() + new Vector2(10, 0), Color.Black);
                            else
                            {
                                string newstring = finaltext;
                                Vector2 newsize = new Vector2();
                                do
                                {
                                    newstring = newstring.Remove(0, 1);
                                    newsize = t.Font.MeasureString(newstring);
                                }
                                while (newsize.X > t.Width - 20);
                                spriteBatch.DrawString(t.Font, newstring, t.GetAbsolutePosition() + new Vector2(10, 0), Color.Black);
                            }
                        }
                        else if (c is Button b)
                        {

                            DLine.DrawLine(spriteBatch, new Vector2(b.GetAbsolutePosition().X, b.GetAbsolutePosition().Y + b.Size.Y / 2), new Vector2(b.GetAbsolutePosition().X + b.Size.X, b.GetAbsolutePosition().Y + b.Size.Y / 2), b.Backcolor, b.Size.Y);
                            if (b.Shadow || b.Clicked)
                                DLine.DrawLine(spriteBatch, new Vector2(b.Position.X + window.Position.X, b.Position.Y + window.Position.Y + b.Size.Y / 2), new Vector2(b.GetAbsolutePosition().X + b.Size.X, b.GetAbsolutePosition().Y + b.Size.Y / 2), new Color(0, 0, 0, 128), b.Size.Y);
                            DLine.DrawLine(spriteBatch, b.GetAbsolutePosition(), new Vector2(b.GetAbsolutePosition().X + b.Size.X, b.GetAbsolutePosition().Y), Color.Black, 6);
                            DLine.DrawLine(spriteBatch, new Vector2(b.GetAbsolutePosition().X + b.Size.X - 3, b.GetAbsolutePosition().Y), new Vector2(b.GetAbsolutePosition().X + b.Size.X - 3, b.GetAbsolutePosition().Y + b.Size.Y), Color.Black, 6);
                            DLine.DrawLine(spriteBatch, new Vector2(b.GetAbsolutePosition().X + b.Size.X, b.GetAbsolutePosition().Y + b.Size.Y), new Vector2(b.GetAbsolutePosition().X, b.GetAbsolutePosition().Y + b.Size.Y), Color.Black, 6);
                            DLine.DrawLine(spriteBatch, new Vector2(b.GetAbsolutePosition().X + 3, b.Position.Y + window.Position.Y + b.Size.Y), b.GetAbsolutePosition() + new Vector2(3, 0), Color.Black, 6);
                            if (b.Texture != null)
                                spriteBatch.Draw(b.Texture, b.GetAbsolutePosition(), Color.White);
                            Vector2 size = Font.MeasureString(b.Text);
                            spriteBatch.DrawString(Font, b.Text, new Vector2(b.GetAbsolutePosition().X + b.Size.X / 2 - size.X / 2, b.GetAbsolutePosition().Y + b.Size.Y / 2 - size.Y / 2), b.Color);
                        }
                        spriteBatch.End();
                        spriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, (int)ScreenResolution.X, (int)ScreenResolution.Y);

                    }

                }
            }

            spriteBatch.Begin();
            spriteBatch.Draw(ToiletPaper, new Vector2(806, 0), color: Color.White, scale: new Vector2(0.75f, 0.75f));

            if (HelpIsOn)
            {
                //Label HelpText4 = new Label("", "Collect precious toilet paper!", Color.Black, new Vector2(0, 170 + 30), new Vector2(600, 0), Font);
                //Label HelpText5 = new Label("", "Avoid deadly virus!", Color.Black, new Vector2(0, 195 + 60), new Vector2(600, 0), Font);
                //Label HelpText6 = new Label("", "Protect yourself with masks!", Color.Black, new Vector2(0, 220 + 90), new Vector2(600, 0), Font);
                spriteBatch.Draw(ToiletPaper, new Vector2(245, 423 - 25), color: Color.White, scale: new Vector2(0.75f, 0.75f));
                spriteBatch.Draw(ToiletPaper, new Vector2(595, 423 - 25), color: Color.White, scale: new Vector2(0.75f, 0.75f));

                spriteBatch.Draw(Virus, new Vector2(300, 476 - 25), color: Color.White, scale: new Vector2(0.75f, 0.75f));
                spriteBatch.Draw(Virus, new Vector2(545, 476 - 25), color: Color.White, scale: new Vector2(0.75f, 0.75f));

                spriteBatch.Draw(Mask, new Vector2(245, 532 - 25), color: Color.White, scale: new Vector2(0.75f, 0.75f));
                spriteBatch.Draw(Mask, new Vector2(600, 532 - 25), color: Color.White, scale: new Vector2(0.75f, 0.75f));



            }
            spriteBatch.End();          
            base.Draw(gameTime);
        }

      
    }
    public static class DLine
    {
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }

        public static Texture2D _texture;
        private static Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _texture.SetData(new[] { Color.White });
            }
            return _texture;
        }
    }
    public class Tile
    {
        public Vector2 Position = new Vector2();
        public Texture2D Texture = null;
        public Tile(Vector2 pos, Texture2D tex)
        {
            Position = pos;
            Texture = tex;
        }
    }

    public class Object
    {
        public Vector2 Position = new Vector2();
        public Vector2 HitBoxPos = new Vector2();
        public Vector2 HitBoxSize = new Vector2();
        public int Type = 0;

        public Object(Vector2 pos, int typ)
        {
            Position = pos;
            Type = typ;
            switch (typ)
            {
                case 1:
                    HitBoxPos = new Vector2(17 + 64, 12 + 64);
                    HitBoxSize = new Vector2(39, 39);
                    break;
                case 2:
                    HitBoxPos = new Vector2(18 * 2, 19 * 2);
                    HitBoxSize = new Vector2((46 - 18)*2, (50-19)*2);
                    break;
                case 3:
                    HitBoxPos = new Vector2(8 + 64, 15 + 64);
                    HitBoxSize = new Vector2(49, 39);
                    break;
            }
        }

    }


}
