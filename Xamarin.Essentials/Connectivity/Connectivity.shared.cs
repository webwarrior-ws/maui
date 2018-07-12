﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Essentials
{
    public static partial class Connectivity
    {
        static event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanagedInternal;

        // a cache so that events aren't fired unnecessarily
        // this is mainly an issue on Android, but we can stiil do this everywhere
        static NetworkAccess currentAccess;
        static List<ConnectionProfile> currentProfiles;

        public static NetworkAccess NetworkAccess => PlatformNetworkAccess;

        public static IEnumerable<ConnectionProfile> Profiles => PlatformProfiles;

        public static event EventHandler<ConnectivityChangedEventArgs> ConnectivityChanged
        {
            add
            {
                var wasRunning = ConnectivityChanagedInternal != null;

                ConnectivityChanagedInternal += value;

                if (!wasRunning && ConnectivityChanagedInternal != null)
                {
                    SetCurrent();
                    StartListeners();
                }
            }

            remove
            {
                var wasRunning = ConnectivityChanagedInternal != null;

                ConnectivityChanagedInternal -= value;

                if (wasRunning && ConnectivityChanagedInternal == null)
                    StopListeners();
            }
        }

        static void SetCurrent()
        {
            currentAccess = NetworkAccess;
            currentProfiles = new List<ConnectionProfile>(Profiles);
        }

        static void OnConnectivityChanged(NetworkAccess access, IEnumerable<ConnectionProfile> profiles)
            => OnConnectivityChanged(new ConnectivityChangedEventArgs(access, profiles));

        static void OnConnectivityChanged()
            => OnConnectivityChanged(NetworkAccess, Profiles);

        static void OnConnectivityChanged(ConnectivityChangedEventArgs e)
        {
            if (currentAccess != e.NetworkAccess || !currentProfiles.SequenceEqual(e.Profiles))
            {
                SetCurrent();
                MainThread.BeginInvokeOnMainThread(() => ConnectivityChanagedInternal?.Invoke(null, e));
            }
        }
    }

    public class ConnectivityChangedEventArgs : EventArgs
    {
        internal ConnectivityChangedEventArgs(NetworkAccess access, IEnumerable<ConnectionProfile> profiles)
        {
            NetworkAccess = access;
            Profiles = profiles;
        }

        public NetworkAccess NetworkAccess { get; }

        public IEnumerable<ConnectionProfile> Profiles { get; }
    }
}
