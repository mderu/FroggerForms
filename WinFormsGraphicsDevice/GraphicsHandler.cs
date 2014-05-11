#region File Description
//-----------------------------------------------------------------------------
// SpinningTriangleControl.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using frogger;
#endregion

namespace frogger
{
    /// <summary>
    /// Example control inherits from GraphicsDeviceControl, which allows it to
    /// render using a GraphicsDevice. This control shows how to draw animating
    /// 3D graphics inside a WinForms application. It hooks the Application.Idle
    /// event, using this to invalidate the control, which will cause the animation
    /// to constantly redraw.
    /// </summary>
    class GraphicsHandler : GraphicsDeviceControl
    {
        //initialize some varaibles here

        SpriteBatch spriteBatch;
        public static Dictionary<string, Texture2D> sprites;
        SpriteFont font;
        public ContentManager Content;
        BasicEffect effect;
        Stopwatch timer;

        protected override void Initialize()
        {
            // Create our effect.
            effect = new BasicEffect(GraphicsDevice);
            effect.VertexColorEnabled = true;

            // Start the animation timer.
            timer = Stopwatch.StartNew();

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };

            ServiceContainer services = new ServiceContainer();

            sprites = new Dictionary<string, Texture2D>();

            frogger.Object.allObjects = new List<frogger.Object>();
            Row.allRows = new List<Row>();
            new Row(MainForm.TileSize * 0, 2.5f/30f);
            new Row(MainForm.TileSize * 1, 2 / 30f);
            new Row(MainForm.TileSize * 2, 1.5f / 30f);
            new Row(MainForm.TileSize * 3, 1 / 30f, Spawns.LOG);
            MainForm.score = 0;
            MainForm.lives = MainForm.startingLives;
            //put the player at the bottom of the screen
            MainForm.player = new Player(new Vector2(MainForm.startingX, MainForm.startingY), MainForm.startingLives);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            Content = new ContentManager(Services, "Content");

            font = this.Content.Load<SpriteFont>("Segoe");
            sprites.Add("placeholder", this.Content.Load<Texture2D>("placeholder"));
            sprites.Add("frog", this.Content.Load<Texture2D>("frog"));
            sprites.Add("road", this.Content.Load<Texture2D>("road"));
            sprites.Add("water", this.Content.Load<Texture2D>("water"));
            sprites.Add("blueCarBen", this.Content.Load<Texture2D>("blueCarBen"));
            sprites.Add("redCarBen", this.Content.Load<Texture2D>("redCarBen"));
            sprites.Add("blueCar", this.Content.Load<Texture2D>("blueCar"));
            sprites.Add("redCar", this.Content.Load<Texture2D>("redCar"));
            sprites.Add("greyCar", this.Content.Load<Texture2D>("greyCar"));
            MainForm.player.setSprite("frog");

            timer = Stopwatch.StartNew();

            MainForm.updater.Start();

        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            float aspect = GraphicsDevice.Viewport.AspectRatio;
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 10);
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            for (int i = 0; i < Row.allRows.Count; i++)
            {
                Row.allRows[i].drawRow(this.spriteBatch);
            }
            MainForm.player.draw(this.spriteBatch);
            //draw score and lives
            //use the difference at the bottom of the screen for this
            spriteBatch.DrawString(font, "Score: " + MainForm.score, new Vector2(0, (MainForm.height - 30)), Color.Red);
            spriteBatch.DrawString(font, "Lives Remaining: " + MainForm.lives, new Vector2(200, (MainForm.height - 30)), Color.Red);
            spriteBatch.End();
        }
        public static Texture2D getSprite(string s)
        {
            return sprites[s];
        }
    }
}
