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

using Esri.ArcGISRuntime.UtilityNetworks;
#if !XAMARIN
using System.Collections.Generic;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
#else
using System.Windows;
using System.Windows.Controls;
#endif

namespace Esri.ArcGISRuntime.Toolkit.UI.Controls
{
    [TemplatePart(Name = "List", Type = typeof(ListView))]
    public partial class TraceConfigurationsView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TraceConfigurationsView"/> class.
        /// </summary>
        public TraceConfigurationsView()
        {
            DefaultStyleKey = typeof(TraceConfigurationsView);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
#if NETFX_CORE
        protected override void OnApplyTemplate()
#else
        public override void OnApplyTemplate()
#endif
        {
            base.OnApplyTemplate();

            ListView = GetTemplateChild("List") as ListView;

            if (ListView != null)
            {
                ListView.ItemsSource = _dataSource;
            }
        }

        private ListView _listView;

        private ListView ListView
        {
            get => _listView;
            set
            {
                if (value != _listView)
                {
                    if (_listView != null)
                    {
                        _listView.SelectionChanged -= ListSelectionChanged;
                    }

                    _listView = value;

                    if (_listView != null)
                    {
                        _listView.SelectionChanged += ListSelectionChanged;
                    }
                }
            }
        }

        private void ListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is UtilityNamedTraceConfiguration bm)
            {
                SelectAndNavigateToTraceConfiguration(bm);
            }

            ((ListView)sender).SelectedItem = null;
        }

        private GeoView GeoViewImpl
        {
            get { return (GeoView)GetValue(GeoViewProperty); }
            set { SetValue(GeoViewProperty, value); }
        }

        private IEnumerable<UtilityNamedTraceConfiguration> TraceConfigurationsOverrideImpl
        {
            get { return (IEnumerable<UtilityNamedTraceConfiguration>)GetValue(TraceConfigurationsOverrideProperty); }
            set { SetValue(TraceConfigurationsOverrideProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="GeoView" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty GeoViewProperty =
            DependencyProperty.Register(nameof(GeoView), typeof(GeoView), typeof(TraceConfigurationsView), new PropertyMetadata(null, OnGeoViewPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="TraceConfigurationsOverride" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty TraceConfigurationsOverrideProperty =
            DependencyProperty.Register(nameof(TraceConfigurationsOverride), typeof(IList<UtilityNamedTraceConfiguration>), typeof(TraceConfigurationsView), new PropertyMetadata(null, OnTraceConfigurationsOverridePropertyChanged));

        private static void OnGeoViewPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TraceConfigurationsView)d)._dataSource.SetGeoView(e.NewValue as GeoView);
        }

        private static void OnTraceConfigurationsOverridePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TraceConfigurationsView)d)._dataSource.SetOverrideList(e.NewValue as IEnumerable<UtilityNamedTraceConfiguration>);
        }

        /// <summary>
        /// Gets or sets the item template used to render TraceConfiguration entries in the list.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ItemTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(TraceConfigurationsView), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the style used by the list view items in the underlying list view control.
        /// </summary>
        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ItemContainerStyle"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemContainerStyleProperty =
            DependencyProperty.Register(nameof(ItemContainerStyle), typeof(Style), typeof(TraceConfigurationsView), null);
    }
}
#endif