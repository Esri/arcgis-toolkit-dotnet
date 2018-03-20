﻿// /*******************************************************************************
//  * Copyright 2012-2018 Esri
//  *
//  *  Licensed under the Apache License, Version 2.0 (the "License");
//  *  you may not use this file except in compliance with the License.
//  *  You may obtain a copy of the License at
//  *
//  *  http://www.apache.org/licenses/LICENSE-2.0
//  *
//  *   Unless required by applicable law or agreed to in writing, software
//  *   distributed under the License is distributed on an "AS IS" BASIS,
//  *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  *   See the License for the specific language governing permissions and
//  *   limitations under the License.
//  ******************************************************************************/

#if __IOS__

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Esri.ArcGISRuntime.Toolkit.Internal;
#if __IOS__
using Color = UIKit.UIColor;
#endif

namespace Esri.ArcGISRuntime.Toolkit.UI.Controls
{
    public partial class TimeSlider
    {
        private string _defaultFullExtentLabelFormat = "M/d/yyyy";
        private string _defaultCurrentExtentLabelFormat = "M/d/yyyy";

        #region Properties

        private TimeExtent _currentExtent;

        /// <summary>
        /// Gets or sets the <see cref="TimeExtent" /> associated with the visual thumbs(s) displayed on the TimeSlider.
        /// </summary>
        private TimeExtent CurrentExtentImpl
        {
            get => _currentExtent;
            set
            {
                var oldValue = value;
                _currentExtent = value;
                ApplyCurrentExtentLabelFormat();
                OnCurrentExtentPropertyChanged(oldValue, _currentExtent);
            }
        }

        private TimeExtent _fullExtent;

        /// <summary>
        /// Gets or sets the <see cref="TimeExtent" /> that specifies the overall start and end time of the time slider instance
        /// </summary>
        private TimeExtent FullExtentImpl
        {
            get => _fullExtent;
            set
            {
                _fullExtent = value;
                ApplyFullExtentLabelFormat();
                OnFullExtentPropertyChanged();
            }
        }

        private TimeValue _timeStepInterval;

        /// <summary>
        /// Gets or sets the time step intervals for the time slider.  The slider thumbs will snap to and tick marks will be shown at this interval.
        /// </summary>
        private TimeValue TimeStepIntervalImpl
        {
            get => _timeStepInterval;
            set
            {
                _timeStepInterval = value;
                OnTimeStepIntervalPropertyChanged();
            }
        }

        private IEnumerable<DateTimeOffset> _timeSteps;

        /// <summary>
        /// Gets or sets the time steps that can be used to set the slider instance's current extent
        /// </summary>
        private IEnumerable<DateTimeOffset> TimeStepsImpl
        {
            get => _timeSteps;
            set
            {
                _timeSteps = value;
                OnTimeStepsPropertyChanged();
            }
        }

        private TimeSpan _playbackInterval = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Gets or sets the interval at which the time slider's current extent will move to the next or previous time step
        /// </summary>
        private TimeSpan PlaybackIntervalImpl
        {
            get => _playbackInterval;
            set
            {
                _playbackInterval = value;
                OnPlaybackIntervalPropertyChanged(_playbackInterval);
            }
        }

        private PlaybackDirection _playbackDirection = PlaybackDirection.Forward;

        /// <summary>
        /// Gets or sets whether the current extent will move to the next or the previous time step during playback
        /// </summary>
        private PlaybackDirection PlaybackDirectionImpl
        {
            get => _playbackDirection;
            set => _playbackDirection = value;
        }

        private LoopMode _playbackLoopMode = LoopMode.None;

        /// <summary>
        /// Gets or sets the behavior when the current extent reaches the end of the slider during playback
        /// </summary>
        private LoopMode PlaybackLoopModeImpl
        {
            get => _playbackLoopMode;
            set => _playbackLoopMode = value;
        }

        private bool _isStartTimePinned = false;

        /// <summary>
        /// Gets or sets a value indicating whether the start time of the <see cref="CurrentExtent"/> is locked into place
        /// </summary>
        private bool IsStartTimePinnedImpl
        {
            get => _isStartTimePinned;
            set
            {
                _isStartTimePinned = value;
                OnIsStartTimePinnedChanged(_isStartTimePinned);
            }
        }

        private bool _isEndTimePinned = false;

        /// <summary>
        /// Gets or sets a value indicating whether the end time of the <see cref="CurrentExtent"/> is locked into place
        /// </summary>
        private bool IsEndTimePinnedImpl
        {
            get => _isEndTimePinned;
            set
            {
                _isEndTimePinned = value;
                OnIsEndTimePinnedChanged(_isEndTimePinned);
            }
        }

        private bool _isPlaying = false;

        /// <summary>
        /// Gets or sets a value indicating whether the time slider is animating playback
        /// </summary>
        private bool IsPlayingImpl
        {
            get => IsPlaying;
            set
            {
                _isPlaying = value;
                OnIsPlayingPropertyChanged(_isPlaying);
            }
        }

        #region Appearance Properties

        private string _fullExtentLabelFormat;

        /// <summary>
        /// Gets or sets the string format to use for displaying the start and end labels for the <see cref="FullExtent"/>
        /// </summary>
        private string FullExtentLabelFormatImpl
        {
            get => _fullExtentLabelFormat;
            set
            {
                _fullExtentLabelFormat = value;
                ApplyFullExtentLabelFormat();
            }
        }

        private string _currentExtentLabelFormat;

        /// <summary>
        /// Gets or sets the string format to use for displaying the start and end labels for the <see cref="CurrentExtent"/>
        /// </summary>
        private string CurrentExtentLabelFormatImpl
        {
            get => _currentExtentLabelFormat;
            set
            {
                _currentExtentLabelFormat = value;
                ApplyCurrentExtentLabelFormat();
                OnCurrentExtentLabelFormatPropertyChanged(_currentExtentLabelFormat);
            }
        }

        private string _timeStepIntervalLabelFormat;

        /// <summary>
        /// Gets or sets the string format to use for displaying the labels for the tick marks representing each time step interval
        /// </summary>
        private string TimeStepIntervalLabelFormatImpl
        {
            get => _timeStepIntervalLabelFormat;
            set
            {
                _timeStepIntervalLabelFormat = value;
                OnTimeStepIntervalLabelFormatPropertyChanged(TimeStepIntervalLabelFormat);
            }
        }

        private TimeSliderLabelMode _labelMode = TimeSliderLabelMode.CurrentExtent;

        /// <summary>
        /// Gets or sets the mode to use for labels along the time slider
        /// </summary>
        private TimeSliderLabelMode LabelModeImpl
        {
            get => _labelMode;
            set
            {
                _labelMode = value;
                OnLabelModePropertyChanged(_labelMode);
            }
        }

        private Color _thumbStroke = Color.LightGray;

        /// <summary>
        /// Gets or sets the border color of the thumbs
        /// </summary>
        private Color ThumbStrokeImpl
        {
            get => _thumbStroke;
            set
            {
                _thumbStroke = value;
                MinimumThumb?.SetBorderColor(value);
                MaximumThumb?.SetBorderColor(value);
            }
        }

        private Color _thumbFill = Color.White;

        /// <summary>
        /// Gets or sets the fill color of the thumbs
        /// </summary>
        private Color ThumbFillImpl
        {
            get => _thumbFill;
            set
            {
                _thumbFill = value;
                MinimumThumb?.SetBackgroundColor(value);
                MaximumThumb?.SetBackgroundColor(value);
            }
        }

        private Color _currentExtentFill = Color.FromRGBA(0, 111, 255, 255);

        /// <summary>
        /// Gets or sets the fill color of the area on the slider track that indicates the <see cref="CurrentExtent"/>
        /// </summary>
        private Color CurrentExtentFillImpl
        {
            get => _currentExtentFill;
            set
            {
                _currentExtentFill = value;
                HorizontalTrackThumb?.SetBackgroundColor(value);
            }
        }

        private Color _fullExtentFill = Color.FromRGBA(170, 169, 170, 255);

        /// <summary>
        /// Gets or sets the fill color of the area on the slider track that indicates the <see cref="FullExtent"/>
        /// </summary>
        private Color FullExtentFillImpl
        {
            get => _fullExtentFill;
            set
            {
                _fullExtentFill = value;
                SliderTrack?.SetBackgroundColor(value);
            }
        }

        private Color _fullExtentStroke = Color.FromRGBA(170, 169, 170, 255);

        /// <summary>
        /// Gets or sets the border color of the area on the slider track that indicates the <see cref="FullExtent"/>
        /// </summary>
        private Color FullExtentStrokeImpl
        {
            get => _fullExtentStroke;
            set
            {
                _fullExtentStroke = value;
                SliderTrack?.SetBorderColor(value);
            }
        }

        private double _fullExtentBorderWidth = 0;

        /// <summary>
        /// Gets or sets the border width of the area on the slider track that indicates the <see cref="FullExtent"/>
        /// </summary>
        /// <value>The full width of the extent border.</value>
        public double FullExtentBorderWidth
        {
            get => _fullExtentBorderWidth;
            set
            {
                _fullExtentBorderWidth = value;
                SliderTrack?.SetBorderWidth(value);
            }
        }

        private Color _timeStepIntervalTickFill = Color.FromRGBA(170, 169, 170, 255);

        /// <summary>
        /// Gets or sets the color of the slider's tickmarks
        /// </summary>
        private Color TimeStepIntervalTickFillImpl
        {
            get => _timeStepIntervalTickFill;
            set
            {
                _timeStepIntervalTickFill = value;
                if (Tickmarks != null)
                {
                    Tickmarks.TickFill = value;
                }
            }
        }

        private Color _playbackButtonsFill = Color.FromRGBA(230, 230, 230, 255);

        /// <summary>
        /// Gets or sets the fill color of the playback buttons
        /// </summary>
        private Color PlaybackButtonsFillImpl
        {
            get => _playbackButtonsFill;
            set
            {
                _playbackButtonsFill = value;
                PreviousButton?.SetBackgroundColor(value);
                NextButton?.SetBackgroundColor(value);
                PlayPauseButton.SetBackgroundColor(value);
            }
        }

        private Color _playbackButtonsStroke = Color.FromRGBA(170, 169, 170, 255);

        /// <summary>
        /// Gets or sets the border color of the playback buttons
        /// </summary>
        private Color PlaybackButtonsStrokeImpl
        {
            get => _playbackButtonsStroke;
            set
            {
                _playbackButtonsStroke = value;
                PreviousButton?.SetBorderColor(value);
                NextButton?.SetBorderColor(value);
                PlayPauseButton.SetBorderColor(value);
            }
        }

        private Color _fullExtentLabelColor;

        /// <summary>
        /// Gets or sets the color of the full extent labels
        /// </summary>
        private Color FullExtentLabelColorImpl
        {
            get => _fullExtentLabelColor;
            set
            {
                _fullExtentLabelColor = value;
                FullExtentStartTimeLabel?.SetTextColor(value);
            }
        }

        private Color _currentExtentLabelColor;

        /// <summary>
        /// Gets or sets the color of the current extent labels
        /// </summary>
        private Color CurrentExtentLabelColorImpl
        {
            get => _currentExtentLabelColor;
            set
            {
                _currentExtentLabelColor = value;
                MinimumThumbLabel?.SetTextColor(value);
                MaximumThumbLabel?.SetTextColor(value);
            }
        }

        private Color _timeStepIntervalLabelColor;

        /// <summary>
        /// Gets or sets the color of the time step interval labels
        /// </summary>
        private Color TimeStepIntervalLabelColorImpl
        {
            get => _timeStepIntervalLabelColor;
            set
            {
                _timeStepIntervalLabelColor = value;
                if (Tickmarks != null)
                {
                    Tickmarks.TickLabelColor = value;
                }
            }
        }

        #endregion // Appearance Properties

        #endregion // Properties

        private void ApplyFullExtentLabelFormat()
        {
            var fullExtentLabelFormat = string.IsNullOrEmpty(FullExtentLabelFormat) ? _defaultFullExtentLabelFormat : FullExtentLabelFormat;
            FullExtentStartTimeLabel.Text = FullExtent?.StartTime.ToString(fullExtentLabelFormat) ?? string.Empty;
            FullExtentEndTimeLabel.Text = FullExtent?.EndTime.ToString(fullExtentLabelFormat) ?? string.Empty;
        }

        private void ApplyCurrentExtentLabelFormat()
        {
            var currentExtentLabelFormat = string.IsNullOrEmpty(CurrentExtentLabelFormat) ? _defaultCurrentExtentLabelFormat : CurrentExtentLabelFormat;
            MinimumThumbLabel.Text = CurrentExtent?.StartTime.ToString(currentExtentLabelFormat) ?? string.Empty;
            MaximumThumbLabel.Text = CurrentExtent?.EndTime.ToString(currentExtentLabelFormat) ?? string.Empty;
        }
    }
}

#endif