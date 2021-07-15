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

using System;
using System.Collections.Generic;
using Esri.ArcGISRuntime.UtilityNetworks;

namespace Esri.ArcGISRuntime.Toolkit.Xamarin.Forms
{
    /// <summary>
    /// Event argument used by <see cref="TraceConfigurationsView.TraceConfigurationsChanged"/> event.
    /// </summary>
    public sealed class TraceConfigurationsChangedEventArgs : EventArgs
    {
        internal TraceConfigurationsChangedEventArgs(IEnumerable<UtilityNamedTraceConfiguration> traceConfigurations)
        {
            TraceConfigurations = traceConfigurations;
        }

        /// <summary>
        /// Gets the new value for <see cref="IEnumerable{UtilityNamedTraceConfiguration>"/>.
        /// </summary>
        /// <value>The new value for <see cref="IEnumerable{UtilityNamedTraceConfiguration>"/>.</value>
        public IEnumerable<UtilityNamedTraceConfiguration> TraceConfigurations { get; }
    }
}