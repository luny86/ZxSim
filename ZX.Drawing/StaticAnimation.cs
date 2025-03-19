
using System.Drawing;

namespace ZX.Drawing
{
    /// <summary>
    /// Basic animated sprite that does not move.
    /// </summary>
    internal class StaticAnimation : IAnimation
    {
        /// <summary>
        /// Invoked when a non-looping animation has reached
        /// the end.
        /// </summary>
        public event EventHandler? AnimationComplete;

        private int _startFrame = 0;

        private int _frame = 0;

        private int _startFreq;
        private int _freqCount = 0;
        private int _hold = 0;

        public StaticAnimation(string name)
        {
            Name = name;
            Enabled = true;
        }

        public string Name { get; init; }

        /// <summary>
        /// Determines if the animation has been completed.
        /// </summary>
        /// <remarks>
        /// Looping animations never end.
        /// Non=looping will complete once the last frame has
        /// been reached and Hold is zero.
        public bool Completed 
        { 
            get
            {
                 return Loop == false && Frame == LastFrame && Hold == 0;
            }
        }

        public bool Enabled { get; set; }

        public bool Loop { get; set; }

        public int Frame 
        { 
            get { return _frame; } 
            init { _frame = value; } 
        }
        
        public int StartFrame 
        { 
            get { return _startFrame; }
            init 
            { 
                _frame = value; 
                _startFrame = value; 
            } 
        }

        public int LastFrame { get; init; }
        
        public int Frequency 
        { 
            get { return _startFreq; }
            init 
            {
                _startFreq = value;
                _freqCount = value;
            }
        }

        /// <summary>
        /// Number of frames the last image is displayed
        /// once a single shot animation has occurred.
        /// </summary>
        public int Hold
        {
            get { return _hold; }
            set
            {
                _hold = value;
            }
        }

        public Point Position { get; set; }

        public void Update()
        {
            if(_freqCount-- < 1)
            {
                _freqCount = _startFreq;

                if(_frame < LastFrame)
                {
                    _frame++;
                }

                if(_frame == LastFrame)
                {
                    if(Loop)
                    {
                        _frame = _startFrame;
                    }
                    else
                    {
                        OnAnimationComplete();
                    }

                    if(_hold > 0)
                    {
                        _hold--;
                    }
                }
            }
        }

        private void OnAnimationComplete()
        {
            AnimationComplete?.Invoke(this, EventArgs.Empty);
        }
    }
}