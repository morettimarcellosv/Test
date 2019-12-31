using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Test.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class GamePage : ContentPage
    {
        Timer timer;

        // Some drawing parameters.
        private const int BallWidth = 50;
        private const int BallHeight = 50;
        private int BallX, BallY;   // Position.
        private int BallVx, BallVy; // Velocity.

        private void Form1_Load(object sender, EventArgs e)
        {
            timer = new Timer(new TimerCallback(tmrMoveBall_Tick), this, 10, 10);
            Debug.WriteLine("Test");


            // Use double buffering to reduce flicker.
            //this.SetStyle(
            //    ControlStyles.AllPaintingInWmPaint |
            //    ControlStyles.UserPaint |
            //    ControlStyles.DoubleBuffer,
            //    true);
            //this.UpdateStyles();
        }

        private void Boing()
        {
            showFill ^= true;
            //using (SoundPlayer player = new SoundPlayer(
            //    Properties.Resources.boing))
            //{
            //    player.Play();
            //}
        }

        private void tmrMoveBall_Tick(object sender)
        {
            BallX += BallVx;
            if (BallX < 0)
            {
                BallVx = -BallVx;
                Boing();
            }
            else if (BallX > surface.CanvasSize.Width)
            {
                BallVx = -BallVx;
                Boing();
            }

            BallY += BallVy;
            if (BallY < 0)
            {
                BallVy = -BallVy;
                Boing();
            }
            else if (BallY > surface.CanvasSize.Height)
            {
                BallVy = -BallVy;
                Boing();
            }

            (sender as ContentPage).Dispatcher.BeginInvokeOnMainThread(() =>
            {
                surface.InvalidateSurface();
            });
//            Refresh();
        }

        public GamePage()
        {
            InitializeComponent();
            this.SizeChanged += GamePage_SizeChanged;

            Form1_Load(null, null);
        }

        private void GamePage_SizeChanged(object sender, EventArgs e)
        {
            if (surface.CanvasSize.Width <= BallWidth || surface.CanvasSize.Height <= BallHeight)
                return;

            // Pick a random start position and velocity.
            Random rnd = new Random();
            BallVx = rnd.Next(1, 4);
            BallVy = rnd.Next(1, 4);
            BallX = rnd.Next(0, (int)(surface.CanvasSize.Width - BallWidth));
            BallY = rnd.Next(0, (int)(surface.CanvasSize.Height - BallHeight));
        }

        bool showFill = true;
        void OnCanvasViewTapped(object sender, EventArgs args)
        {
            showFill ^= true;
            (sender as SKCanvasView).InvalidateSurface();
        }

        private void Form1_Paint(object sender, SKPaintSurfaceEventArgs e)
        {
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Blue.ToSKColor(),
                StrokeWidth = 10
            };
            //            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Surface.Canvas.Clear(SKColor.Empty);

            if (showFill)
            {
                paint.Style = SKPaintStyle.Fill;
                paint.Color = SKColors.Yellow;
            }

            e.Surface.Canvas.DrawCircle(BallX, BallY, BallWidth, paint);
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 50
            };
            canvas.DrawCircle(info.Width / 2, info.Height / 2, 100, paint);

            if (showFill)
            {
                paint.Style = SKPaintStyle.Fill;
                paint.Color = SKColors.Blue;
                canvas.DrawCircle(info.Width / 2, info.Height / 2, 100, paint);
            }

            Form1_Paint(sender, args);
        }
    }
}