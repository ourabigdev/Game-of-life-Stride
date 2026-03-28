using System;
using System.Collections.Generic;
using System.Text;

namespace Game_of_life
{
    public static class GameSettings
    {
        public const int UpdateDelay = 3; // Delay in frames between updates
        public const int WindowWidth = 1280;
        public const int WindowHeight = 720;
        public const int CellSize = 28;
        public const int GridWidth = WindowWidth/CellSize;
        public const int GridHeight = WindowHeight/CellSize;
        public static readonly int[] BirthRules = { 3 };
        public static readonly int[] SurvivalRules = { 2, 3 };
    }
}
