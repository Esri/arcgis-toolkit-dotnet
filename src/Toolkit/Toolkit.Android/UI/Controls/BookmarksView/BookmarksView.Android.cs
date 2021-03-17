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
using Android.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Esri.ArcGISRuntime.Mapping;

namespace Esri.ArcGISRuntime.Toolkit.UI.Controls
{
    public partial class BookmarksView
    {
        private RecyclerView _internalListView;
        private BookmarksAdapter _adapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarksView"/> class.
        /// </summary>
        /// <param name="context">The Context the view is running in, through which it can access resources, themes, etc.</param>
#pragma warning disable CS8618 // _internalListView and  _adapter set in Initialize
        public BookmarksView(Context? context)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(context)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BookmarksView"/> class.
        /// </summary>
        /// <param name="context">The Context the view is running in, through which it can access resources, themes, etc.</param>
        /// <param name="attr">The attributes of the AXML element declaring the view.</param>
#pragma warning disable CS8618 // _internalListView and  _adapter set in Initialize
        public BookmarksView(Context? context, IAttributeSet? attr)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(context, attr)
        {
            Initialize();
        }

        internal void Initialize()
        {
            _internalListView = new RecyclerView(Context);
            _internalListView.SetLayoutManager(new LinearLayoutManager(Context));
            _adapter = new BookmarksAdapter(Context, _dataSource);
            _internalListView.SetAdapter(_adapter);
            AddView(_internalListView);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            if (_adapter != null)
            {
                _adapter.BookmarkSelected += ListView_ItemClick;
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();

            if (_adapter != null)
            {
                _adapter.BookmarkSelected -= ListView_ItemClick;
            }
        }

        private void ListView_ItemClick(object sender, Bookmark bookmark)
        {
            SelectAndNavigateToBookmark(bookmark);
        }
    }
}