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

using Esri.ArcGISRuntime.Mapping;
using Xamarin.Forms;

namespace Esri.ArcGISRuntime.Toolkit.Xamarin.Forms
{
    /// <summary>
    /// Determines which DataTemplate to use for a given layer content item in a Legend control.
    /// </summary>

    internal class LegendItemTemplateSelector : DataTemplateSelector
    {
        private Legend _owner;

        public LegendItemTemplateSelector(Legend owner)
        {
            _owner = owner;
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is Layer)
            {
                return LayerTemplate;
            }

            if (item is ILayerContent)
            {
                return SublayerTemplate;
            }

            if (item is LegendInfo)
            {
                return LegendInfoTemplate;
            }

            return null;
        }

        public DataTemplate LayerTemplate => _owner.LayerItemTemplate;

        public DataTemplate SublayerTemplate => _owner.SublayerItemTemplate;

        public DataTemplate LegendInfoTemplate => _owner.LegendInfoItemTemplate;
    }
}