using TShockAPI;
using System.IO;
using System;
using Terraria;
using TerrariaApi.Server;
using System.ComponentModel;

namespace PlayerInfo
{
    [ApiVersion(1, 23)]
    public class PlayerInfo : TerrariaPlugin
    {
        public override void Initialize()
        {
            ServerApi.Hooks.ServerJoin.Register(this, OnJoin);
            Commands.Init();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.ServerJoin.Deregister(this, OnJoin); ;
                base.Dispose(disposing);
            }
        }
        void OnJoin(HandledEventArgs eventArgs)
        {
            if (!eventArgs.Handled)
            {
                int player = 0;
                TShock.Utils.Broadcast("Joining: " + Main.player[player].name +
                                        "(" + Main.player[player].statLifeMax.ToString() +
                                        "/" + Main.player[player].statManaMax.ToString() + ")", Color.Azure);
            }

        }
        public static void OnError(object sender,
                                    ErrorEventArgs error)
        {
            TShock.Log.Error("! PlayerInfo Error: " + error.ToString());
        }

        public override string Name
        {
            get { return "PlayerInfo"; }
        }

        public override string Author
        {
            get { return "Jewsus"; }
        }

        public override string Description
        {
            get { return "Provides player from commands (/pInfo)"; }
        }

        public override Version Version
        {
            get { return new Version(1, 0); }
        }

        public PlayerInfo(Main game) : base(game)
        {
            Order = 11;
        }
    }
}
