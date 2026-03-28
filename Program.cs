using Stride.CommunityToolkit.Engine;
using Stride.Core.Mathematics;
using Stride.Engine;
using Game_of_life;

using var game = new Game();
game.Run(start: Start);

void Start(Scene rootScene)
{
    game.Window.AllowUserResizing = true;
    game.Window.Title = "Game of Life";
    game.Window.SetSize(new Int2(GameSettings.WindowWidth, GameSettings.WindowHeight));
    game.SetupBase2D();

    var lifeRender = new LifeRender();
    game.AddSceneRenderer(lifeRender);

    // Hook into update loop for input
    game.Services.GetService<Stride.Games.IGameSystemCollection>();
    game.Script.AddTask(async () =>
    {
        while (true)
        {
            lifeRender.HandleInput(game);
            await game.Script.NextFrame();
        }
    });
}