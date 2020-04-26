using GameOverlay.Drawing;
using GameOverlay.Windows;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using static WakeTFT.D3DXMath;
using static WakeTFT.GameInstance;
using Color = System.Drawing.Color;

namespace WakeTFT
{
    class Engine
    {
        #region things
        private readonly GraphicsWindow _window;
        public static Driver driver = new Driver();
        public static GameInstance Game = new GameInstance();

        public Engine()
        {
            driver.connect();

            while (!Utility.IsOpen)
                return;

            Game.Address = driver.get_process_base_address(Utility.LeagueId);
            Game.ProcessId = Utility.LeagueId;
            Game.Time = driver.read<float>(Utility.LeagueId, Game.Address + (UInt64)GameOffsets.GamePointer.oGameTime);


            Game.Renderer = new LolRenderer();
            Game.Renderer.RendererBase = driver.read<UIntPtr>(Utility.LeagueId, Game.Address + (UInt64)GameOffsets.GamePointer.oRenderer);
            Game.Renderer.Width = driver.read<int>(Utility.LeagueId, (UInt64)Game.Renderer.RendererBase + (UInt64)GameOffsets.RenderOffsets.oScreenWidth);
            Game.Renderer.Height = driver.read<int>(Utility.LeagueId, (UInt64)Game.Renderer.RendererBase + (UInt64)GameOffsets.RenderOffsets.oScreenHeight);
            Game.Renderer.viewMatrix = driver.read<D3DXMATRIX>(Utility.LeagueId, (UInt64)Game.Renderer.RendererBase + (UInt64)GameOffsets.RenderOffsets.oViewMatrix);
            Game.Renderer.projMatrix = driver.read<D3DXMATRIX>(Utility.LeagueId, (UInt64)Game.Renderer.RendererBase + (UInt64)GameOffsets.RenderOffsets.oProjection);

            Game.LocalPlayer = new GameInstance.Object();
            Game.LocalPlayer.ObjectBase = driver.read<UIntPtr>(Utility.LeagueId, Game.Address + (UInt64)GameOffsets.GamePointer.oLocalPlayer);
            Game.LocalPlayer.Health = driver.read<float>(Utility.LeagueId, (UInt64)Game.LocalPlayer.ObjectBase + (UInt64)GameOffsets.GameObject.oObjHealth);
            Game.LocalPlayer.MaxHealth = driver.read<float>(Utility.LeagueId, (UInt64)Game.LocalPlayer.ObjectBase + (UInt64)GameOffsets.GameObject.oObjMaxHealth);
            Debug.WriteLine(Game.LocalPlayer.Health);
            Game.LocalPlayer.Position = driver.read<D3DXVECTOR3>(Utility.LeagueId, (UInt64)Game.LocalPlayer.ObjectBase + (UInt64)GameOffsets.GameObject.oObjPos);

            var graphics = new Graphics
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = true,
                TextAntiAliasing = true,
                UseMultiThreadedFactories = false,
                VSync = true,
                WindowHandle = IntPtr.Zero
            };

            _window = new GraphicsWindow(0, 0, Engine.Game.Renderer.Width, Engine.Game.Renderer.Height, graphics)
            {
                IsTopmost = true,
                IsVisible = true,
                FPS = 144,
            };

            _window.DrawGraphics += _window_DrawGraphics;
        }

        public void Run()
        {
            _window.Create();
            _window.Join();
        }

        #endregion

        private unsafe void _window_DrawGraphics(object sender, DrawGraphicsEventArgs e)
        {
            var gfx = e.Graphics;
            gfx.ClearScene();

            //Setup dos renders
            Game.Renderer.RendererBase = driver.read<UIntPtr>(Utility.LeagueId, Game.Address + (UInt64)GameOffsets.GamePointer.oRenderer);
            Game.Renderer.Width = driver.read<int>(Utility.LeagueId, (UInt64)Game.Renderer.RendererBase + (UInt64)GameOffsets.RenderOffsets.oScreenWidth);
            Game.Renderer.Height = driver.read<int>(Utility.LeagueId, (UInt64)Game.Renderer.RendererBase + (UInt64)GameOffsets.RenderOffsets.oScreenHeight);
            Game.Renderer.viewMatrix = driver.read<D3DXMATRIX>(Utility.LeagueId, (UInt64)Game.Renderer.RendererBase + (UInt64)GameOffsets.RenderOffsets.oViewMatrix);
            Game.Renderer.projMatrix = driver.read<D3DXMATRIX>(Utility.LeagueId, (UInt64)Game.Renderer.RendererBase + (UInt64)GameOffsets.RenderOffsets.oProjection);
            //Setup do LocalPlayer
            Game.LocalPlayer.ObjectBase = driver.read<UIntPtr>(Utility.LeagueId, Game.Address + (UInt64)GameOffsets.GamePointer.oLocalPlayer);
            Game.LocalPlayer.Health = driver.read<float>(Utility.LeagueId, (UInt64)Game.LocalPlayer.ObjectBase + (UInt64)GameOffsets.GameObject.oObjHealth);
            Game.LocalPlayer.MaxHealth = driver.read<float>(Utility.LeagueId, (UInt64)Game.LocalPlayer.ObjectBase + (UInt64)GameOffsets.GameObject.oObjMaxHealth);
            Game.LocalPlayer.Position = driver.read<D3DXVECTOR3>(Utility.LeagueId, (UInt64)Game.LocalPlayer.ObjectBase + (UInt64)GameOffsets.GameObject.oObjPos);
            var LocalPlayerName = driver.read<LoLString>(Utility.LeagueId, (UInt64)Game.LocalPlayer.ObjectBase + (UInt64)GameOffsets.GameObject.oObjName);
            var LocalPlayerNameMax = driver.read<int>(Utility.LeagueId, (UInt64)Game.LocalPlayer.ObjectBase + (UInt64)GameOffsets.GameObject.oObjName + 0x14);
            string LocalPlayerNameString = Marshal.PtrToStringAnsi((IntPtr)LocalPlayerName.c_str(LocalPlayerNameMax));
            Game.LocalPlayer.Name = LocalPlayerNameString;
            


            D3DXMATRIX vMatrix = Game.Renderer.viewMatrix;
            D3DXMATRIX pMatrix = Game.Renderer.projMatrix;
            D3DXMATRIX vpMatrix = new D3DXMATRIX();
            D3DXMatrixMultiply(out vpMatrix, ref vMatrix, ref pMatrix);

            var WorldPos = Utility.WorldToScreen(Game.LocalPlayer.Position, new D3DXVECTOR2(Game.Renderer.Width, Game.Renderer.Height), vpMatrix);

            DrawCircle(WorldPos.x, WorldPos.y, 300, Color.DeepSkyBlue, 5);
            DrawTextWithOutline("WakeTFT - " + Game.LocalPlayer.Name, 10, 5, 25, Color.DeepSkyBlue, Color.Black, true, true);


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
