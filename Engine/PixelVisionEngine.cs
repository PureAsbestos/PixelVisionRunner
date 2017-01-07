﻿//  
// Copyright (c) Jesse Freeman. All rights reserved.  
// 
// Licensed under the Microsoft Public License (MS-PL) License. 
// See LICENSE file in the project root for full license information. 
// 
// Contributors
// --------------------------------------------------------
// This is the official list of Pixel Vision 8 contributors:
//  
// Jesse Freeman
// 

using System;
using System.Collections.Generic;
using System.Text;
using PixelVisionSDK.Engine.Chips;
using PixelVisionSDK.Engine.Chips.Audio;
using PixelVisionSDK.Engine.Chips.Game;
using PixelVisionSDK.Engine.Chips.Graphics.Colors;
using PixelVisionSDK.Engine.Chips.Graphics.Display;
using PixelVisionSDK.Engine.Chips.Graphics.Sprites;
using PixelVisionSDK.Engine.Chips.IO.Controller;

namespace PixelVisionSDK.Engine
{
    /// <summary>
    ///     This is the default engine class for Pixel Vision 8. It manages the
    ///     state of all chips, the game itself and helps with communication between
    ///     the two.
    /// </summary>
    public class PixelVisionEngine : IEngine
    {
        protected string[] defaultChips;

        protected Dictionary<string, string> metaData = new Dictionary<string, string>
        {
            {"name", "untitled"}
        };

        /// <summary>
        ///     The PixelVisionEngine constructor requires a render target and an
        ///     optional list of <paramref name="chips" /> to be properly configured.
        /// </summary>
        /// <param name="chips"></param>
        /// <param name="name"></param>
        /// <tocexclude />
        public PixelVisionEngine(string[] chips = null, string name = "Engine")
        {
            if (chips != null)
                defaultChips = chips;

            this.name = name;

            Init();
        }

        /// <summary>
        /// </summary>
        /// <tocexclude />
        public string name { get; set; }

        /// <summary>
        ///     Access to the ChipManager.
        ///     <tocexclude />
        /// </summary>
        public ChipManager chipManager { get; set; }

        /// <summary>
        ///     Access to the ColorChip.
        /// </summary>
        /// <tocexclude />
        public ColorChip colorChip { get; set; }

        /// <summary>
        ///     Access to the ColorMapChip.
        /// </summary>
        /// <tocexclude />
        public ColorMapChip colorMapChip { get; set; }

        /// <summary>
        ///     Access to the ControllerChip.
        /// </summary>
        /// <tocexclude />
        public ControllerChip controllerChip { get; set; }

        /// <summary>
        ///     Access to the DisplayChip.
        /// </summary>
        /// <tocexclude />
        public DisplayChip displayChip { get; set; }

        /// <summary>
        ///     Access to the ScreenBufferChip.
        /// </summary>
        /// <tocexclude />
        public ScreenBufferChip screenBufferChip { get; set; }

        /// <summary>
        ///     Access to the SoundChip.
        /// </summary>
        /// <tocexclude />
        public SoundChip soundChip { get; set; }

        /// <summary>
        ///     Access to the SpriteChip.
        /// </summary>
        /// <tocexclude />
        public SpriteChip spriteChip { get; set; }

        /// <summary>
        ///     Access to the TileMapChip.
        /// </summary>
        /// <tocexclude />
        public TileMapChip tileMapChip { get; set; }

        /// <summary>
        ///     Access to the FontChip.
        /// </summary>
        /// <tocexclude />
        public FontChip fontChip { get; set; }

        /// <summary>
        ///     Access to the MusicChip.
        /// </summary>
        /// <tocexclude />
        public MusicChip musicChip { get; set; }

        /// <summary>
        ///     Access to the APIBridge.
        /// </summary>
        /// <tocexclude />
        public IAPIBridge apiBridge { get; set; }

        /// <summary>
        ///     Access to the current game in memory.
        /// </summary>
        /// <tocexclude />
        public GameChip currentGame { get; set; }

        /// <summary>
        ///     Flag if the engine is <see cref="running" /> or not.
        /// </summary>
        /// <tocexclude />
        public bool running { get; private set; }

        /// <summary>
        ///     The PixelVisionEngine Init() method creates the
        ///     <see cref="ChipManager" /> and <see cref="APIBridge" /> as well as any
        ///     additional chips supplied in the <see cref="defaultChips" /> array.
        /// </summary>
        /// <tocexclude />
        public virtual void Init()
        {
            chipManager = new ChipManager(this);
            apiBridge = new APIBridge(this);
            if (defaultChips != null)
            {
                foreach (var chip in defaultChips)
                {
                    chipManager.GetChip(chip);
                }
            }
        }

        /// <summary>
        ///     This method allows you to load a <paramref name="game" /> into the
        ///     Engine's memory. Loading a <paramref name="game" /> doesn't run it.
        ///     It simply sets it up to be run by calling the RunGame() method. This
        ///     allows you to perform additonal tasks once a <paramref name="game" />
        ///     is loaded before it plays.
        /// </summary>
        /// <param name="game"></param>
        /// <tocexclude />
        public virtual void LoadGame(GameChip game)
        {
            running = false;
            chipManager.ActivateChip(game.GetType().FullName, game);
        }

        /// <summary>
        ///     Attempts to run a game that has been loaded into memory via the
        ///     LoadGame() method. It resets the display and game as well as calling
        ///     init on the game itself. It also toggles the <see cref="running" />
        ///     flag to true.
        /// </summary>
        /// <tocexclude />
        public virtual void RunGame()
        {
            if (currentGame == null)
                return;

            // Reset the game state
            currentGame.Reset();

            // Run the game
            currentGame.Init();

            running = true;
        }

        /// <summary>
        ///     This method is called in order to update the business logic of the
        ///     engine, its chips and any loaded game. This method only executes if
        ///     the engine is running.
        /// </summary>
        /// <param name="timeDelta"></param>
        /// <tocexclude />
        public virtual void Update(float timeDelta)
        {
            if (!running)
                return;

            chipManager.Update(timeDelta);
        }

        /// <summary>
        ///     This method is called in order to update the display logic of the
        ///     engine, its chips and any loaded game. This method only executes if
        ///     the engine is running.
        /// </summary>
        /// <tocexclude />
        public virtual void Draw()
        {
            if (!running)
                return;

            chipManager.Draw();
        }

        /// <summary>
        ///     This method resets the engine. This method only executes if the
        ///     engine is running.
        /// </summary>
        /// <tocexclude />
        public virtual void Reset()
        {
            if (!running)
                return;

            chipManager.Reset();
        }

        /// <summary>
        ///     This method is called when shutting down the engine
        /// </summary>
        /// <tocexclude />
        public virtual void Shutdown()
        {
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <tocexclude />
        public string SerializeData()
        {
            return chipManager.SerializeData();
        }

        /// <summary>
        /// </summary>
        /// <param name="sb"></param>
        /// <tocexclude />
        public void CustomSerializedData(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="data"></param>
        /// <tocexclude />
        public void DeserializeData(Dictionary<string, object> data)
        {
            chipManager.DeserializeData(data);
        }
    }
}