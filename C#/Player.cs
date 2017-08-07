using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BCA
{
    /// <summary>
    /// Handler for Time Changed Event. Fired by Player Class
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e" 
    ///     <see cref="TimeChangedEventArgs"/> 
    ///     <seealso cref="EventArgs"/>
    ///     >Time Args</param>
    public delegate void TimeChangedHandler(object sender, TimeChangedEventArgs e);
    /// <summary>
    /// Handler for Player State Changed Event. Fired by Player Class
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e" 
    ///     <see cref="PlayerStateChangedEventArgs"/> 
    ///     <seealso cref="EventArgs"/>
    ///     >PlayerStateArgs</param>
    public delegate void PlayerStateChangedHandler(object sender, PlayerStateChangedEventArgs e);

    /// <summary>
    /// TimeChangedEventArgs - 
    /// Event Arguments for the TimeChanged Event
    /// <list type="bullet">
    ///     <listheader>
    ///         <term>Contained Member:</term>
    ///     </listheader>
    ///     <item>
    ///         <description>Time: Actual Player Time</description>
    ///     </item>
    ///     <item>
    ///         <description>Duration: Duration of the actual played animation</description>
    ///         <see cref="Animations.Animation.AnimationDuration"/>
    ///     </item>
    ///     <item>
    ///         <description>Frame: Last played frame as 2 dimensional Color array</description>
    ///         <see cref="Corale.Colore.Core.Color"/>
    ///     </item>
    /// </list>
    /// </summary>
    public class TimeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Current position
        /// </summary>
        public int Position { get; private set; }
        /// <summary>
        /// Whole duration of the loaded animation
        /// </summary>
        public int Duration { get; private set; }
        /// <summary>
        /// Last played frame
        /// </summary>
        public Corale.Colore.Core.Color[][] Frame { get; private set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="position">Current position</param>
        /// <param name="duration">Whole duration of the loaded animation</param>
        /// <param name="frame">Last played frame</param>
        public TimeChangedEventArgs(int position, int duration, Corale.Colore.Core.Color[][] frame)
        {
            this.Position = position;
            this.Duration = duration;
            this.Frame = frame;
        }
    }
    /// <summary>
    /// PlayerStateChangedEventArgs - 
    /// Event Arguments for the PlayerStateChanged Event
    /// <list type="bullet">
    ///     <listheader>
    ///         <term>Contained Member:</term>
    ///     </listheader>
    ///     <item>
    ///         <description>State: Actual state of the player</description>
    ///         <see cref="PlayerStateChangedEventArgs.PlayerState"/>
    ///     </item>
    /// </list>
    /// </summary>
    public class PlayerStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Used to represent the player state and within <see cref="PlayerStateChangedEventArgs"/>
        /// </summary>
        public enum PlayerState
        {
            Idle,
            Initialized,
            Playing,
            Paused,
            Ended
        }
        /// <summary>
        /// New state
        /// </summary>
        public PlayerState State { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="state">New player state</param>
        public PlayerStateChangedEventArgs(PlayerState state)
        {
            this.State = state;
        }
    }

    public class Player
    {
        /// <summary>
        /// Constructor. Needs to be private, because singleton pattern
        /// </summary>
        private Player()
        {
            //////////////////////////////////
            // Initialize Chroma SDK and stuff
            if (!Corale.Colore.Core.Chroma.IsSdkAvailable())
                return;
            if (!Corale.Colore.Core.Chroma.Instance.Initialized)
                Corale.Colore.Core.Chroma.Instance.Initialize();
            while (!Corale.Colore.Core.Chroma.Instance.Initialized) ;
            //////////////////////////////////

            // Register Handler to refresh player state
            PlayerStateChanged += Player_PlayerStateChanged;
        }
        /// <summary>
        /// Inner Player Singleton Instance
        /// </summary>
        private static Player _Instance = null;
        /// <summary>
        /// Returns the player instance
        /// </summary>
        public static Player Instance { get { if (_Instance == null) _Instance = new Player(); return _Instance; } }
        
        /// <summary>
        /// Fired when either <see cref="Player.GoToTime(int)"/> is called or every frame when playing animation
        /// </summary>
        public event TimeChangedHandler TimeChanged;
        /// <summary>
        /// Fired when Player state changes
        /// </summary>
        public event PlayerStateChangedHandler PlayerStateChanged;
        
        /// <summary>
        /// Actual Player State
        /// </summary>
        public PlayerStateChangedEventArgs.PlayerState PlayerState { get; private set; } = PlayerStateChangedEventArgs.PlayerState.Idle;
        /// <summary>
        /// Refreshing the player state
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">PlayerStateChangedEventArgs
        /// <see cref="PlayerStateChangedEventArgs"/></param>
        private void Player_PlayerStateChanged(object sender, PlayerStateChangedEventArgs e)
        {
            this.PlayerState = e.State;
        }

        public void LoadAnimation(string filepath)
        {
            using (Stream fileStream = File.OpenRead(filepath),
              zippedStream = new GZipStream(fileStream, CompressionMode.Decompress))
            {
                LoadAnimation(zippedStream);
            }
        }
        public void LoadAnimation(System.IO.Stream s)
        {
            LoadAnimation(Binarys.Utils.ReadBCA(s));
        }
        public void LoadAnimation(Binarys.BinaryFile bca)
        {
            LoadAnimation(Animations.Animation.FromBCA(bca));
        }
        public void LoadAnimation(Animations.Animation animation)
        {
            Initialize();

            if(PlayerState != PlayerStateChangedEventArgs.PlayerState.Idle)
                Stop();
            this.animation = animation;
            if (PlayerStateChanged != null)
                PlayerStateChanged(this, new PlayerStateChangedEventArgs(PlayerStateChangedEventArgs.PlayerState.Initialized));
        }

        private void Initialize()
        {
            // If the player has already loaded an animation, kill it first
            if (PlayerThread != null)
            {
                PlayerThread.Abort();
            }
            
            // create a new player thread
            PlayerThread = new Thread(playerThredDoWork);
            PlayerThread.Name = "Chroma Animation Player Thread";
            PlayerThread.Start();

        }

        /// <summary>
        /// Start player
        /// </summary>
        public void Start()
        {
            if (PlayerStateChanged != null)
                PlayerStateChanged(this, new PlayerStateChangedEventArgs(PlayerStateChangedEventArgs.PlayerState.Playing));
        }
        /// <summary>
        /// Pause player
        /// </summary>
        public void Pause()
        {
            if (PlayerStateChanged != null)
                PlayerStateChanged(this, new PlayerStateChangedEventArgs(PlayerStateChangedEventArgs.PlayerState.Paused));
        }
        /// <summary>
        /// Jump to passed possition.
        /// Only jumps, if player isn't playing.
        /// </summary>
        /// <param name="time">Position to go to</param>
        public void GoToTime(int time)
        {
            if ((PlayerState != PlayerStateChangedEventArgs.PlayerState.Playing) && // Don't allow jumping, when playing
                (PlayerState != PlayerStateChangedEventArgs.PlayerState.Idle)) // This means, no animation is loaded
            {
            }
        }
        /// <summary>
        /// Stop player and go to time position 0
        /// </summary>
        public void Stop()
        {
            if (PlayerStateChanged != null)
                PlayerStateChanged(this, new PlayerStateChangedEventArgs(PlayerStateChangedEventArgs.PlayerState.Ended));

            GoToTime(0);
        }



        #region Player Thread 
        /// <summary>
        /// Actual player position.
        /// </summary>
        public int CurrentPlayerTime { get; private set; } = 0;
        /// <summary>
        /// Set to play animation in endless repeat
        /// </summary>
        public bool EndlessRepeat { get; set; } = false;
        /// <summary>
        /// Main Thread, running all the time, when Instance is created
        /// </summary>
        private Thread PlayerThread;
        /// <summary>
        /// Currently loaded Animation
        /// </summary>
        private Animations.Animation animation;
        /// <summary>
        /// Worker method for player thread
        /// </summary>
        private void playerThredDoWork()
        {
            while (true)
            {
                while (PlayerState == PlayerStateChangedEventArgs.PlayerState.Playing)
                {
                    int frameDelay = 1000 / animation.FPS;
                    int nextFrame = CurrentPlayerTime / frameDelay;

                    if (nextFrame >= animation.Frames.Count)
                    {
                        if (EndlessRepeat)
                        {
                            CurrentPlayerTime = 0;
                        }
                        else
                        {
                            Stop();
                        }
                        break;
                    }

                    Corale.Colore.Core.Chroma.Instance.Keyboard.SetGrid(
                        animation.Frames[nextFrame].FrameData[Binarys.BinaryFile.DeviceType.Keyboard]);

                    Thread.Sleep(frameDelay);
                    CurrentPlayerTime += frameDelay;
                    if (TimeChanged != null)
                        TimeChanged(this, new TimeChangedEventArgs(CurrentPlayerTime, frameDelay * animation.Frames.Count, null));
                }
            }
        }
        #endregion

    }
}
