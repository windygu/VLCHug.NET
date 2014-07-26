﻿using System;
using System.Runtime.InteropServices;

using VLCInterface.Bridge;
using VLCInterface.Enumerations;

namespace VLCInterface
{
    public class Media : IDisposable, IVLCObject
    {
        public VLCMediaState State
        {
            get
            {
                return VLCAPI.Media.GetState(this);
            }
        }

        public VLCMediaState PlayerState
        {
            get
            {
                return Player.State;
            }
        }

        public Boolean IsDisposed
        {
            get;
            private set;
        }

        public IntPtr Handle
        {
            get;
            private set;
        }

        private VLCInstance Parent;

        private Player Player;

        public Media(VLCInstance VLCInstance, String PlayString, Boolean IsFilePath = false)
        {
            IsDisposed = false;

            Parent = VLCInstance;

            Handle = IsFilePath ?
                VLCAPI.Media.FromPath(VLCInstance, PlayString) :
                VLCAPI.Media.FromMRL(VLCInstance, PlayString);

            Player = new Player(this);
        }

        public Boolean Play()
        {
            return Player.Play();
        }

        public void Stop()
        {
            Player.Stop();
        }

        public void AddOption(String Option)
        {
            Boolean Restart = false;

            if (
                Player.State != VLCMediaState.Stopped && 
                Player.State != VLCMediaState.Ended && 
                Player.State != VLCMediaState.NothingSpecial
                )
            {
                Restart = true;
                Stop();
            }

            VLCAPI.Media.AddOption(this, Option);
            
            if (Restart) Play();
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                VLCAPI.Media.Release(this);
            }
        }

        ~Media()
        {
            Dispose();
        }
    }
}
