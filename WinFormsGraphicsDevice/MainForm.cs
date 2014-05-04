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
        public static int score;
        public static int lives;
        public const int width = 800;
        public const int height = 600;
        public const int startingLives = 5;
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
            //TODO: Exit game if in the upper left corner.
            if (Row.allRows == null) { return; }
            for (int i = 0; i < Row.allRows.Count; i++)
            {
                Row.allRows[i].update(gameTime);
            }
            player.update(gameTime);
            //So here we should check if the player has reached a certain height
            if (player.getPosition().Y < height / 2)
            {
                //shift everything downward and create a new row
                //and delete the last row
                for (int i = 0; i < Row.allRows.Count; i++)
                {
                    Row.allRows[i].setPosition(Row.allRows[i].getPosition() + new Vector2(0, 64));
                }
                player.setPosition(player.getPosition() + new Vector2(0, 64));
                //randomly generate a row
                new Row(0, 1, Spawns.LOG);
            }
            if (player.getPosition().X > width)
            {
                //player just died
                player.playerReset();
            }
        }
    }
}
