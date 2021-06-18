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

#if !_XAMARIN_ANDROID_ && !_XAMARIN_IOS_
using System;
using System.Collections.Generic;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using System.Linq;
#if XAMARIN_FORMS
using Esri.ArcGISRuntime.Toolkit.Xamarin.Forms.Internal;
using Esri.ArcGISRuntime.Xamarin.Forms;
using Point = Xamarin.Forms.Point;
#elif NETFX_CORE
using Point = Windows.Foundation.Point;
#else
using Point = System.Windows.Point;
#endif

#if !XAMARIN_FORMS
using System.Collections.Generic;
using System.Linq;
using Esri.ArcGISRuntime.Toolkit.Internal;
using Esri.ArcGISRuntime.UI.Controls;
#endif

namespace Esri.ArcGISRuntime.Toolkit.UI.Controls.OverviewMap
{
    internal class OverviewMapController
    {
        private double _scaleFactor;
        private FillSymbol _extentSymbol;
        private GeoView _connectedView;
        private GraphicsOverlay _extentOverlay;
        private readonly GeoView _overview;

        public OverviewMapController(GeoView overview)
        {
            _overview = overview;
            _extentOverlay = new GraphicsOverlay();
            _overview.GraphicsOverlays.Add(_extentOverlay);

            // _overview.ViewpointChanged += OnViewpointChanged;
            var listener = new WeakEventListener<GeoView, object, EventArgs>(_overview);
            listener.OnEventAction = (instance, source, eventArgs) => OnViewpointChanged(source, eventArgs);
            listener.OnDetachAction = (instance, weakEventListener) => instance.ViewpointChanged -= weakEventListener.OnEvent;
            _overview.ViewpointChanged += listener.OnEvent;

            // _overview.NavigationCompleted += OnNavigationCompleted;
            var listener2 = new WeakEventListener<GeoView, object, EventArgs>(_overview);
            listener2.OnEventAction = (instance, source, eventArgs) => OnNavigationCompleted(source, eventArgs);
            listener2.OnDetachAction = (instance, weakEventListener) => instance.NavigationCompleted -= weakEventListener.OnEvent;
            _overview.NavigationCompleted += listener2.OnEvent;
        }

        public GeoView AttachedView
        {
            get => _connectedView;
            set
            {
                if (value == _connectedView)
                {
                    return;
                }

                if (_connectedView != null)
                {
                    _connectedView.ViewpointChanged -= OnViewpointChanged;
                    _connectedView.NavigationCompleted -= OnNavigationCompleted;
                }

                _connectedView = value;
                if (_connectedView != null)
                {
                    _connectedView.ViewpointChanged += OnViewpointChanged;
                    _connectedView.NavigationCompleted += OnNavigationCompleted;
                    ApplyViewpoint(_connectedView, _overview);
                }
            }
        }

        public double ScaleFactor
        {
            get => _scaleFactor;
            set
            {
                if (_scaleFactor != value)
                {
                    _scaleFactor = value;
                    ApplyViewpoint(_connectedView, _overview);
                }
            }
        }

        public FillSymbol ExtentSymbol
        {
            get => _extentSymbol;
            set
            {
                if (_extentSymbol != value)
                {
                    _extentSymbol = value;
                    _extentOverlay.Renderer = new SimpleRenderer(_extentSymbol);
                    UpdateGraphic();
                }
            }
        }

        private void UpdateGraphic()
        {
            if (_connectedView == null || _overview == null)
            {
                return;
            }

            _extentOverlay.Graphics.Clear();

            if (_connectedView is MapView mv && mv.VisibleArea is Polygon area)
            {
                _extentOverlay.Graphics.Add(new Graphic(area));
                return;
            }
            else if (_connectedView is SceneView)
            {
                if (_connectedView.GetCurrentViewpoint(ViewpointType.CenterAndScale) is Viewpoint extent && extent.TargetGeometry is MapPoint centerPoint)
                {
                    _extentOverlay.Graphics.Add(new Graphic(centerPoint, new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Cross, ExtentSymbol?.Outline?.Color ?? System.Drawing.Color.Red, 16)));
                }
            }
        }

        private void OnViewpointChanged(object sender, EventArgs e)
        {
            GeoView sendingView = (GeoView)sender;
            GeoView receivingView = sendingView == _overview ? _connectedView : _overview;

            // Check for IsNavigating is intended to ignore OnViewpointChanged calls triggered by the call to SetViewpoint.
            if (sendingView?.IsNavigating ?? false)
            {
                ApplyViewpoint(sendingView, receivingView);
            }
        }

        private void OnNavigationCompleted(object sender, EventArgs e)
        {
            GeoView sendingView = (GeoView)sender;
            GeoView receivingView = sendingView == _overview ? _connectedView : _overview;
            ApplyViewpoint(sendingView, receivingView);
        }

        private void ApplyViewpoint(GeoView sendingView, GeoView receivingView)
        {
            if (sendingView == null || receivingView == null)
            {
                return;
            }

            if (sendingView.GetCurrentViewpoint(ViewpointType.CenterAndScale) is Viewpoint existingViewpoint)
            {
                var scaleMultiplier = sendingView == _overview ? 1.0 / ScaleFactor : ScaleFactor;
                Viewpoint newViewpoint = new Viewpoint((MapPoint)existingViewpoint.TargetGeometry, existingViewpoint.TargetScale * scaleMultiplier);
                receivingView.SetViewpoint(newViewpoint);
                UpdateGraphic();
            }
        }
    }
}
#endif
