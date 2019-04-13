﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ColorMine.ColorSpaces;
using Reloaded.WPF.Utilities.Animation.Manual.Utilities;

namespace Reloaded.WPF.Utilities.Animation.Manual
{
    public class ManualAnimations
    {
        /// <summary>
        /// Enables hue cycling of the drop shadow for the window.
        /// </summary>
        /// <param name="setColorFunction">Function that applies a colour to a variable.</param>
        /// <param name="token">A token generated by a <see cref="CancellationTokenSource"/> allowing for peaceful abort(ion) of the task.</param>
        /// <param name="framesPerSecond">The amount of frames per second.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="chroma">Range 0 to 100. The quality of a color's purity, intensity or saturation. </param>
        /// <param name="lightness">Range 0 to 100. The quality (chroma) lightness or darkness.</param>
        /// <remarks>https://www.harding.edu/gclayton/color/topics/001_huevaluechroma.html</remarks>
        public static async Task HueCycleColor(Action<Color> setColorFunction, CancellationToken token, int framesPerSecond = 30, int duration = 6000, float chroma = 50F, float lightness = 50F)
        {
            await Task.Run(() =>
            {
                bool executionMethod(Color color)
                {
                    // Exit if requested.
                    if (token.IsCancellationRequested)
                        return false;

                    setColorFunction(color);
                    return true;
                }

                var animation = new ManualAnimation<Color>(duration, framesPerSecond,
                    time => ColorInterpolator.GetRainbowColor(chroma, lightness, time).ToColor(),
                    executionMethod);

                animation.Repeat = ulong.MaxValue;
                animation.Animate();
            });
        }

        /// <summary>
        /// Animates a transition between one colour to another.
        /// </summary>
        /// <param name="setColorFunction">Function that applies a colour to a variable.</param>
        /// <param name="framesPerSecond">The amount of frames per second.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="token">A token generated by a <see cref="CancellationTokenSource"/> allowing for peaceful abort(ion) of the task.</param>
        /// <param name="sourceColor">The colour to animate from. </param>
        /// <param name="targetColor">The colour to animate to.</param>
        /// <remarks>https://www.harding.edu/gclayton/color/topics/001_huevaluechroma.html</remarks>
        public static async Task ColorAnimate(Action<Color> setColorFunction, CancellationToken token, Color sourceColor, Color targetColor, int framesPerSecond = 30, int duration = 6000)
        {
            await Task.Run(() =>
            {
                bool executionMethod(Color color)
                {
                    // Exit if requested.
                    if (token.IsCancellationRequested)
                        return false;

                    setColorFunction(color);
                    return true;
                }

                var lchSourceColor = sourceColor.ToLch();
                var lchTargetColor = targetColor.ToLch();

                var animation = new ManualAnimation<Color>(duration, framesPerSecond,
                    time => ColorInterpolator.CalculateIntermediateColour(lchSourceColor, lchTargetColor, time).ToColor(),
                    executionMethod);

                animation.Animate();
            });
        }
    }
}
