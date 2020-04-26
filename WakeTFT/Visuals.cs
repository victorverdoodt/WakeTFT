using GameOverlay.Drawing;
using GameOverlay.Windows;
using System;
using System.Diagnostics;
using System.Numerics;
using Color = System.Drawing.Color;

namespace WakeTFT
{
    class Visuals
    {
        #region things
        private readonly GraphicsWindow _window;

        public Visuals()
        {
            // initialize a new Graphics object
            // GraphicsWindow will do the remaining initialization
            var graphics = new Graphics
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true,
                UseMultiThreadedFactories = false,
                VSync = true,
                WindowHandle = IntPtr.Zero
            };

            // it is important to set the window to visible (and topmost) if you want to see it!
            _window = new GraphicsWindow(0, 0, 800, 600, graphics)
            //_window = new StickyWindow(Memory.Process.MainWindowHandle, graphics)
            {
                IsTopmost = true,
                IsVisible = true,
                FPS = 144,
                X = 0,
                Y = 0,
                Width = 1920,
                Height = 1080
            };

            _window.DrawGraphics += _window_DrawGraphics;
        }

        public void Run()
        {
            // creates the window and setups the graphics
            _window.Create();
            _window.Join();
        }

        #endregion
        private void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            var gfx = e.Graphics;
            gfx.ClearScene();

            DrawTextWithOutline("WakeTFT" + Form1.testi.ToString(), 10, 5, 25, Color.DeepSkyBlue, Color.Black, true, true);

            
            #region drawing functions
            void DrawBoxEdge(float x, float y, float width, float height, Color color, float thiccness = 2.0f)
            {
                gfx.DrawRectangleEdges(GetBrushColor(color), x, y, x + width, y + height, thiccness);
            }

            void DrawText(string text, float x, float y, int size, Color color, bool bold = false, bool italic = false)
            {
                //if (Tools.InScreenPos(x, y))
                //{
                    gfx.DrawText(gfx.CreateFont("Arial", size, bold, italic), GetBrushColor(color), x, y, text);
                //}
            }

            void DrawTextWithOutline(string text, float x, float y, int size, Color color, Color outlinecolor, bool bold = true, bool italic = false)
            {
                DrawText(text, x - 1, y + 1, size, outlinecolor, bold, italic);
                DrawText(text, x + 1, y + 1, size, outlinecolor, bold, italic);
                DrawText(text, x, y, size, color, bold, italic);
            }

            void DrawTextWithBackground(string text, float x, float y, int size, Color color, Color backcolor, bool bold = false, bool italic = false)
            {
                //if (Tools.InScreenPos(x, y))
                //{
                    gfx.DrawTextWithBackground(gfx.CreateFont("Arial", size, bold, italic), GetBrushColor(color), GetBrushColor(backcolor), x, y, text);
                //}
            }

            void DrawLine(float fromx, float fromy, float tox, float toy, Color color, float thiccness = 2.0f)
            {
                gfx.DrawLine(GetBrushColor(color), fromx, fromy, tox, toy, thiccness);
            }

            void DrawFilledBox(float x, float y, float width, float height, Color color)
            {
                gfx.FillRectangle(GetBrushColor(color), x, y, x + width, y + height);
            }

            void DrawCircle(float x, float y, float radius, Color color, float thiccness = 1)
            {
                gfx.DrawCircle(GetBrushColor(color), x, y, radius, thiccness);
            }

            void DrawCrosshair(CrosshairStyle style, float x, float y, float size, float thiccness, Color color)
            {
                gfx.DrawCrosshair(GetBrushColor(color), x, y, size, thiccness, style);
            }

            void DrawFillOutlineBox(float x, float y, float width, float height, Color color, Color fillcolor, float thiccness = 1.0f)
            {
                gfx.OutlineFillRectangle(GetBrushColor(color), GetBrushColor(fillcolor), x, y, x + width, y + height, thiccness);
            }

            void DrawBox(float x, float y, float width, float height, Color color, float thiccness = 2.0f)
            {
                gfx.DrawRectangle(GetBrushColor(color), x, y, x + width, y + height, thiccness);
            }

            void DrawOutlineBox(float x, float y, float width, float height, Color color, float thiccness = 2.0f)
            {
                gfx.OutlineRectangle(GetBrushColor(Color.FromArgb(0, 0, 0)), GetBrushColor(color), x, y, x + width, y + height, thiccness);
            }

            void DrawRoundedBox(float x, float y, float width, float height, float radius, Color color, float thiccness = 2.0f)
            {
                gfx.DrawRoundedRectangle(GetBrushColor(color), x, y, x + width, y + height, radius, thiccness);
            }

            SolidBrush GetBrushColor(Color color)
            {
                return gfx.CreateSolidBrush(color.R, color.G, color.B, color.A);
            }
            #endregion
        }
    }
}
