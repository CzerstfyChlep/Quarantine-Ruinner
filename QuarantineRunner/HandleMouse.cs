using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuarantineRunner
{
    public static class HandleMouse
    {
        public static void Handle(MouseState ms)
        {
            Point MousePosition = ms.Position;
            if (MousePosition.X > 0 && MousePosition.Y > 0 && MousePosition.X < Game.ScreenResolution.X && MousePosition.Y < Game.ScreenResolution.Y)
            {

               
                if (ms.LeftButton == ButtonState.Released)
                {
                    if (Game.Grabbed != null)
                        Game.Grabbed = null;
                }





                foreach (UIWindow window in Game.UIWindows)
                {
                    if ((MousePosition.X > window.Position.X && MousePosition.X < window.Position.X + window.Size.X) && (MousePosition.Y > window.Position.Y && MousePosition.Y < window.Position.Y + window.Size.Y))
                    {
                        bool scrolled = false;
                        if (Game.ScrollValue != ms.ScrollWheelValue)
                        {
                            foreach (Control c in window.GetAllChildren())
                            {
                                if (c.Scroll != null)
                                {
                                    if ((MousePosition.X > c.GetAbsolutePosition().X && MousePosition.X < c.GetAbsolutePosition().X + c.Size.X) && (MousePosition.Y > c.GetAbsolutePosition().Y && MousePosition.Y < c.GetAbsolutePosition().Y + c.Size.Y))
                                    {
                                        c.ScrollHandle((ms.ScrollWheelValue - Game.ScrollValue) / -120);
                                    }
                                }
                            }

                            if (window.Scroll != null && !scrolled)
                                window.ScrollHandle((ms.ScrollWheelValue - Game.ScrollValue) / -120);
                        }
                        foreach (Button b in window.Controls.Where(x => x is Button))
                        {
                            if (MousePosition.X > window.Position.X + b.Position.X && MousePosition.X < window.Position.X + b.Position.X + b.Size.X && MousePosition.Y > window.Position.Y + b.Position.Y && MousePosition.Y < window.Position.Y + b.Position.Y + b.Size.Y)
                                b.Shadow = true;
                            else
                                b.Shadow = false;
                        }

                    }
                    
                }
                Game.ScrollValue = ms.ScrollWheelValue;
                Vector2 TrueMousePosition = new Vector2(ms.X, ms.Y);
                if (ms.LeftButton == ButtonState.Pressed || ms.RightButton == ButtonState.Pressed)
                {
                    Game.FocusedText = null;
                    //MouseDelay = 10;
                    bool ui = false;
                    UIWindow toRemove = null;
                    if (Game.Grabbed == null)
                    {
                        foreach (UIWindow window in Game.UIWindows)
                        {
                            if (!window.Visible)
                                continue;
                            if ((MousePosition.X > window.Position.X && MousePosition.X < window.Position.X + window.Size.X) && (MousePosition.Y > window.Position.Y && MousePosition.Y < window.Position.Y + window.Size.Y))
                            {
                                ui = true;
                                if (window.Closable)
                                {
                                    if ((MousePosition.X > window.Position.X + window.Size.X - 41 && MousePosition.X < window.Position.X + window.Size.X - 9) && (MousePosition.Y > window.Position.Y + 9 && MousePosition.Y < window.Position.Y + 41))
                                    {
                                        if (window.PermaClose)
                                            toRemove = window;
                                        else
                                            window.Visible = false;
                                    }
                                }

                                if (window.Movable)
                                {
                                    if ((MousePosition.X > window.Position.X && MousePosition.X < window.Position.X + window.Size.X) && (MousePosition.Y > window.Position.Y && MousePosition.Y < window.Position.Y + 50))
                                    {
                                        Game.Grabbed = window;
                                    }
                                }

                                foreach (Button b in window.Controls.Where(x => x is Button))
                                {
                                    if (MousePosition.X > window.Position.X + b.Position.X && MousePosition.X < window.Position.X + b.Position.X + b.Size.X && MousePosition.Y > window.Position.Y + b.Position.Y && MousePosition.Y < window.Position.Y + b.Position.Y + b.Size.Y)
                                    {
                                        b.Method.Invoke(b.Argument);
                                        break;
                                    }
                                }
                                foreach (Control c in window.GetAllChildren())
                                {
                                    if (c is TextInput t)
                                    {
                                        Vector2 size = t.Font.MeasureString("S");
                                        if (MousePosition.X > t.GetAbsolutePosition().X && MousePosition.X < t.GetAbsolutePosition().X + t.Width && MousePosition.Y > t.GetAbsolutePosition().Y && MousePosition.Y < t.GetAbsolutePosition().Y + size.Y)
                                        {
                                            Game.FocusedText = t;
                                            break;
                                        }
                                    }
                                    if (c is Scroll s)
                                    {
                                        if (MousePosition.X > s.GetAbsolutePosition().X && MousePosition.X < s.GetAbsolutePosition().X + s.Size.X && MousePosition.Y > s.GetAbsolutePosition().Y && MousePosition.Y < s.GetAbsolutePosition().Y + s.Size.Y)
                                        {
                                            float size = s.Size.Y / (s.MaxScroll + 1 + s.ScrollShown);
                                            float p = MousePosition.Y - s.GetAbsolutePosition().Y;
                                            s.Parent.ScrollHandle(-1000);
                                            s.Parent.ScrollHandle((int)(p / size / 2));
                                        }
                                    }
                                }



                                break;
                            }
                        }
                    }
                    Game.UIWindows.Remove(toRemove);

                    if (Game.Grabbed != null)
                    {
                        float xchange = Game.LastMousePosition.X - MousePosition.X;
                        float ychange = Game.LastMousePosition.Y - MousePosition.Y;
                        xchange *= -1; ychange *= -1;
                        Game.Grabbed.Position.X += (int)xchange;
                        Game.Grabbed.Position.Y += (int)ychange;
                        if (Game.Grabbed.Position.X < 0)
                            Game.Grabbed.Position.X = 0;
                        else if (Game.Grabbed.Position.X > Game.ScreenResolution.X - 100)
                            Game.Grabbed.Position.X = Game.ScreenResolution.X - 100;
                        if (Game.Grabbed.Position.Y < 0)
                            Game.Grabbed.Position.Y = 0;
                        else if (Game.Grabbed.Position.Y > Game.ScreenResolution.Y - 60)
                            Game.Grabbed.Position.Y = Game.ScreenResolution.Y - 60;


                        if (Game.UIWindows.First() != Game.Grabbed && Game.Grabbed.Visible)
                        {
                            Game.UIWindows.Remove(Game.Grabbed);
                            Game.UIWindows.Insert(0, Game.Grabbed);
                        }
                    }

                    
                }

              
            }

        }
    }
}
