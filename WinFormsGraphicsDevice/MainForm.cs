#region File Description
//-----------------------------------------------------------------------------
// MainForm.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System.Windows.Forms;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace frogger
{
    using GdiColor = System.Drawing.Color;
    using XnaColor = Microsoft.Xna.Framework.Color;
    using System.Diagnostics;
    using System;
    using System.Timers;
    using frogger;
    public partial class MainForm : System.Windows.Forms.Form
    {
        public static Player player;
        public const int width = 1280;
        public const int height = 600;
        public const int startingLives = 5;
        public const int startingX = 200;
        public const int startingY = 258;
        public const double MAX_SPEED = .75;
        public const int TileSize = 128;
        private int top;

        public static int score;
        public static int lives;
        public const int gameTime = 16;
        public static Random rand = new Random();

        public static System.Timers.Timer updater;

        public MainForm()
        {
            InitializeComponent();
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            WindowState = System.Windows.Forms.FormWindowState.Maximized;

            updater = new System.Timers.Timer(gameTime);
            updater.Elapsed += new ElapsedEventHandler(Update);
        }
        void Update(object sender, ElapsedEventArgs e)
        {

            // TODO: Add your update logic here
            for (int i = 0; i < Row.allRows.Count; i++)
            {
                Row.allRows[i].update(gameTime);
            }

            player.update(gameTime);
            //So here we should check if the player has reached a certain height
            checkHeight();

            if (player.getPosition().X + TileSize < 0 || player.getPosition().X > width || player.isDead())
            {

                //player just died
                reset();
            }
        }
        protected void reset()
        {
            player.playerReset();
            generateNewLevel();
        }
        /// <summary>
        /// Checks the height to see if the view needs to be moved up
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected void checkHeight()
        {
            if (player.getPosition().Y < height / 2)
            {
                //shift everything downward and create a new row
                //and delete the last row
                for (int i = 0; i < Row.allRows.Count; i++)
                {
                    Row.allRows[i].setPosition(Row.allRows[i].getPosition() + new Vector2(0, MainForm.TileSize));
                }
                player.setPosition(player.getPosition() + new Vector2(0, MainForm.TileSize));
                //randomly generate a row
                //new Row(0, 1, Spawns.LOG);
                top++;
                if (top > 0)
                {
                    generateChunk();
                }
            }
        }
        public void generateNewLevel()
        {

            //generate a new starting level
            Row.allRows = new List<Row>();
            //generateChunk();
            new Row(TileSize * 0, 2.5f, Spawns.CAR);
            new Row(TileSize * 1, 0.5f, Spawns.CAR);
            new Row(TileSize * 2, -2.5f, Spawns.LOG);
            new Row(TileSize * 3, 1.0f, Spawns.CAR);
            top = 0;
        }

        public float getRandomSpeed()
        {
            float speed = (float)((rand.NextDouble() * (MAX_SPEED * 2)) - MAX_SPEED);
            return speed;
        }

        public void generateChunk()
        {
            Random rnd = new Random();
            int type = rnd.Next(0, 3);
            if (type == 0) //Generate stream
            {
                new Row(TileSize * (top - 2), getRandomSpeed(), Spawns.LOG);
                new Row(TileSize * (top - 1), getRandomSpeed(), Spawns.LOG);
                new Row(TileSize * top, getRandomSpeed(), Spawns.LOG);
                top = -2;
            }
            else if (type == 1) //Generate Road
            {
                new Row(TileSize * (top - 2), getRandomSpeed(), Spawns.CAR);
                new Row(TileSize * (top - 1), getRandomSpeed(), Spawns.CAR);
                new Row(TileSize * 0, getRandomSpeed(), Spawns.CAR);
                top = -2;
            }
            else if (type == 2) //Generate Safe spot
            {
                new Row(TileSize * 0, 0f, Spawns.FREESPACE);
                top = 0;
            }

        }

        /// <summary>
        /// Used for giving several classes access to the width
        /// of the screen.
        /// </summary>
        /// <returns>Returns the width of the screen</returns>
        public static int getWidth()
        {
            return width;
        }
        /// <summary>
        /// The bread and butter of our graphics drawings. Instead of
        /// letting each object load in it's own Texture2D, all of them
        /// are saved in the static "sprites" map. When objects need to
        /// be drawn they get their sprite from here.
        /// </summary>
        /// <param name="s">The key to the mapped sprite.</param>
        /// <returns>The sprite mapped at the key.</returns>
        public static Texture2D getSprite(string s)
        {
            return GraphicsHandler.sprites[s];
        }
    }

}
