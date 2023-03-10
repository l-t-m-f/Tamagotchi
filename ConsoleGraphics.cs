using System.Drawing;

namespace Prototype
{
    internal static class ConsoleGraphics
    {
        internal struct Animation
        {
            internal Point Origin;
            internal int[][,] Frames;
            internal int CurrentFrame = 0;

            public Animation(int[][,] frames)
            {
                Frames = frames;
            }
        }

        internal struct GfxEgg
        {
            internal Animation Animation;
            public GfxEgg(Point origin)
            {
                Animation = new Animation {
                    Origin = origin,
                    Frames = new int[2][,] {
                        new int[,] {
                            { 0, 0, 1, 1, 1, 0, 0 },
                            { 0, 1, 1, 1, 1, 1, 0 },
                            { 1, 1, 1, 1, 1, 1, 1 },
                            { 1, 1, 1, 1, 1, 1, 1 },
                            { 1, 1, 1, 1, 1, 1, 1 },
                            { 0, 1, 1, 1, 1, 1, 0 },
                            { 0, 0, 1, 1, 1, 0, 0 },
                        },
                        new int[,] {
                            { 0, 0, 0, 0, 0, 0, 0 },
                            { 0, 0, 1, 1, 1, 0, 0 },
                            { 0, 1, 1, 1, 1, 1, 0 },
                            { 1, 1, 1, 1, 1, 1, 1 },
                            { 1, 1, 1, 1, 1, 1, 1 },
                            { 0, 1, 1, 1, 1, 1, 0 },
                            { 0, 0, 1, 1, 1, 0, 0 },
                        },
                    }
                };
            }
        }

    }
}
