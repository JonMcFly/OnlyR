﻿namespace OnlyR.Core.Recorder
{
    using System;

    /// <summary>
    /// Controls optional volume fading at end of a recording
    /// </summary>
    internal sealed class VolumeFader
    {
        public event EventHandler FadeComplete;

        private readonly int _sampleRate;
        private readonly int _fadeTimeSecs = 4;
        private int _sampleCountToModify;
        private int _sampleCountModified;

        public bool Active { get; private set; }

        public VolumeFader(int sampleRate)
        {
            _sampleRate = sampleRate;
        }

        /// <summary>
        /// Start to fade out
        /// </summary>
        public void Start()
        {
            _sampleCountToModify = _fadeTimeSecs * _sampleRate; // num samples to modify in order to fade over _fadeTimeSecs
            _sampleCountModified = 0;
            Active = true;
        }

        /// <summary>
        /// Modifies the audio buffer in accord with the current fading status
        /// </summary>
        /// <param name="buffer">The aduio samples</param>
        /// <param name="bytesInBuffer">The number of bytes in the audio buffer</param>
        public void FadeBuffer(byte[] buffer, int bytesInBuffer)
        {
            _sampleCountModified += bytesInBuffer;
            float volumeAdjustmentFraction = 1 - ((float)_sampleCountModified / _sampleCountToModify);

            for (int index = 0; index < bytesInBuffer; index += 2)
            {
                short sample = (short)((buffer[index + 1] << 8) | buffer[index + 0]);

                short modifiedSample = (short)(sample * volumeAdjustmentFraction);
                buffer[index + 1] = (byte)(modifiedSample >> 8);
                buffer[index + 0] = (byte)(modifiedSample & 0xFF);
            }

            if (volumeAdjustmentFraction <= 0)
            {
                OnFadeComplete();
            }
        }

        private void OnFadeComplete()
        {
            FadeComplete?.Invoke(this, EventArgs.Empty);
            Active = false;
        }
    }
}
