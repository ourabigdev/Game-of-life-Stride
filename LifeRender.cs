using Stride.CommunityToolkit.Engine;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Games;
using Stride.Graphics;
using Stride.Input;
using Stride.Rendering;
using Stride.Rendering.Compositing;

namespace Game_of_life
{
    public class LifeRender : SceneRendererBase
    {
        private SpriteBatch? spriteBatch;
        private Texture? cellTexture;
        private Texture? lineTexture;

        private readonly GridManager gridManager = new GridManager(GameSettings.GridWidth, GameSettings.GridHeight);
        private int counter;
        private bool isSpaceKeyPressed;
        private bool isPaused = true;
        private bool texturesInitialized;

        protected override void InitializeCore()
        {
            gridManager.InitializeGrid();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cellTexture = Texture.New2D(GraphicsDevice, 1, 1, PixelFormat.R8G8B8A8_UNorm);
            lineTexture = Texture.New2D(GraphicsDevice, 1, 1, PixelFormat.R8G8B8A8_UNorm);
        }

        public void HandleInput(Game game)
        {
            var input = game.Input;

            if (input.IsKeyPressed(Keys.Space))
                isPaused = !isPaused;

            if (input.IsKeyPressed(Keys.R))
                gridManager.InitializeGrid();

            if (input.IsMouseButtonDown(MouseButton.Left))
            {
                var mouse = input.MousePosition;
                int gridX = (int)(mouse.X * GameSettings.WindowWidth) / GameSettings.CellSize;
                int gridY = (int)(mouse.Y * GameSettings.WindowHeight) / GameSettings.CellSize;
                gridManager.ToggleCell(gridX, gridY, true);
            }

            if (input.IsMouseButtonDown(MouseButton.Right))
            {
                var mouse = input.MousePosition;
                int gridX = (int)(mouse.X * GameSettings.WindowWidth) / GameSettings.CellSize;
                int gridY = (int)(mouse.Y * GameSettings.WindowHeight) / GameSettings.CellSize;
                gridManager.ToggleCell(gridX, gridY, false);
            }
        }

        protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
        {
            // Initialize textures once
            if (!texturesInitialized && cellTexture != null && lineTexture != null)
            {
                cellTexture.SetData(drawContext.CommandList, new Color[] { Color.White });
                lineTexture.SetData(drawContext.CommandList, new Color[] { Color.White });
                texturesInitialized = true;
            }

            // Update grid logic
            if (!isPaused)
            {
                counter++;
                if (counter > GameSettings.UpdateDelay)
                {
                    counter = 0;
                    gridManager.UpdateGrid();
                }
            }

            // Don't call Clear here — SetupBase2D owns the render target
            // Just draw on top of whatever background exists
            if (spriteBatch == null || cellTexture == null || lineTexture == null) return;

            spriteBatch.Begin(drawContext.GraphicsContext);

            // Draw cells
            for (int x = 0; x < GameSettings.GridWidth; x++)
            {
                for (int y = 0; y < GameSettings.GridHeight; y++)
                {
                    var color = gridManager.CurrentGrid[x, y]
                        ? Color.CornflowerBlue
                        : new Color(40, 40, 40, 255);

                    spriteBatch.Draw(cellTexture,
                        new Rectangle(
                            x * GameSettings.CellSize,
                            y * GameSettings.CellSize,
                            GameSettings.CellSize,
                            GameSettings.CellSize),
                        color);
                }
            }

            // Draw grid lines
            for (int x = 0; x <= GameSettings.GridWidth; x++)
            {
                spriteBatch.Draw(lineTexture,
                    new Rectangle(x * GameSettings.CellSize, 0, 1, GameSettings.GridHeight * GameSettings.CellSize),
                    new Color(50, 50, 50, 80));
            }
            for (int y = 0; y <= GameSettings.GridHeight; y++)
            {
                spriteBatch.Draw(lineTexture,
                    new Rectangle(0, y * GameSettings.CellSize, GameSettings.GridWidth * GameSettings.CellSize, 1),
                    new Color(50, 50, 50, 80));
            }

            spriteBatch.End();
        }
    }
}