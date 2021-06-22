# ArcGIS Runtime SDK for .NET - Toolkit

<a href="https://developers.arcgis.com"><img src="https://img.shields.io/badge/ArcGIS%20Developers%20Home-633b9b?style=flat-square" /></a> <a href="https://developers.arcgis.com/net/"><img src="https://img.shields.io/badge/Documentation-633b9b?style=flat-square" /></a>
<a href="https://developers.arcgis.com/documentation/mapping-apis-and-services/tutorials/"><img src="https://img.shields.io/badge/Tutorials-633b9b?style=flat-square" /></a>
<a href="https://developers.arcgis.com/net/wpf/sample-code/"><img src="https://img.shields.io/badge/Samples-633b9b?style=flat-square" /></a>
<a href="https://github.com/Esri/arcgis-runtime-demos-dotnet"><img src="https://img.shields.io/badge/Demos-633b9b?style=flat-square" /></a>
<a href=""><img src="https://img.shields.io/badge/Toolkit-black?style=flat-square" /></a>
<a href="https://github.com/Esri/arcgis-runtime-templates-dotnet"><img src="https://img.shields.io/badge/Templates-633b9b?style=flat-square&logo=visualstudio&labelColor=gray" /></a>
<a href="https://www.nuget.org/profiles/Esri_Inc"><img src="https://img.shields.io/badge/NuGet-633b9b?style=flat-square&logo=nuget&labelColor=gray" /></a> <a href="https://community.esri.com/t5/arcgis-runtime-sdks/ct-p/arcgis-runtime-sdks"><img src="https://img.shields.io/badge/🙋-Get%20help%20in%20Esri%20Community-633b9b?style=flat-square" /></a>

<a href="https://esri.github.io/arcgis-toolkit-dotnet/controls.html"><img src="https://img.shields.io/badge/%F0%9F%93%84-Toolkit%20Guide-633b9b?style=flat-square" /></a>
<a href="https://esri.github.io/arcgis-toolkit-dotnet/api/index.html"><img src="https://img.shields.io/badge/Toolkit%20API%20Reference:-fff?style=flat-square" /></a> <a href="https://esri.github.io/arcgis-toolkit-dotnet/api/android/index.html"><img src="https://img.shields.io/badge/Xamarin.Android-3ddc84?style=flat-square&labelColor=gray&logo=android" /></a> <a href="https://esri.github.io/arcgis-toolkit-dotnet/api/ios/index.html"><img src="https://img.shields.io/badge/Xamarin.iOS-black?style=flat-square&labelColor=gray&logo=ios" /></a>
<a href="https://esri.github.io/arcgis-toolkit-dotnet/api/netstd/index.html"><img src="https://img.shields.io/badge/Xamarin.Forms-3498db?style=flat-square&labelColor=gray&logo=Xamarin" /></a>
<a href="https://esri.github.io/arcgis-toolkit-dotnet/api/netfx/index.html"><img src="https://img.shields.io/badge/WPF-0078d6?style=flat-square&labelColor=gray&logo=windowsxp" /></a>
<a href="https://esri.github.io/arcgis-toolkit-dotnet/api/uwp/index.html"><img src="https://img.shields.io/badge/UWP-0078d6?style=flat-square&labelColor=gray&logo=windows" /></a>

**Toolkit controls accelerate application development with ArcGIS Runtime**, including...

<!-- Options: Centered image, gallery, side image -->
<!-- Use gallery for multi-projects (e.g. Demos repo), centered image for desktop/iPad apps, side image for phone apps -->

<div align="center">

| <img src="./docs/images/readme_compass.png" height="64"> | <img src="./docs/images/readme_legend.png" height="64" /> | <img src="./docs/images/readme_popup.png" height="64"> | <img src="./docs/images/readme_scaleline.png" height="64" /> |
|----|---|---|---|
| Compass | Legend | Popup viewer | Scale line |

</div>

...and many more!

<hr />

## Get started

The simplest way to get started is to add the NuGet package(s) to your projects:

<a href="https://www.nuget.org/packages/Esri.ArcGISRuntime.Toolkit/"><img src="https://img.shields.io/badge/Toolkit-007ac2?style=flat-square&labelColor=gray&logo=nuget" /></a>
<a href="https://www.nuget.org/packages/Esri.ArcGISRuntime.ARToolkit/"><img src="https://img.shields.io/badge/AR%20Toolkit-007ac2?style=flat-square&labelColor=gray&logo=nuget" /></a>
<a href="https://www.nuget.org/packages/Esri.ArcGISRuntime.Toolkit.Xamarin.Forms/"><img src="https://img.shields.io/badge/Xamarin.Forms%20Toolkit-007ac2?style=flat-square&labelColor=gray&logo=nuget" /></a>
<a href="https://www.nuget.org/packages/Esri.ArcGISRuntime.ARToolkit.Forms/"><img src="https://img.shields.io/badge/Xamarin.Forms%20AR%20Toolkit-007ac2?style=flat-square&labelColor=gray&logo=nuget" /></a>

If you need more control, you can fork the repo and build it yourself. [See the docs](https://esri.github.io/arcgis-toolkit-dotnet/buildingtoolkit.html) for full instructions.

Building the toolkit requires the latest version of Visual Studio 2019 running on Windows.

## Features

|Component |UWP  |WPF  |Xamarin.Android  |Xamarin.iOS  |Xamarin.Forms |
|---|---|---|---|---|---|
|[ARSceneView](ar.md)   | N/A | N/A | ✔ | ✔ | ✔ |
|[BookmarksView](bookmarks-view.md)   | ✔ | ✔ | ✔ | ✔ | ✔ |
|Compass   | ✔ | ✔ | ✔ | ✔ | ✔ |
|FeatureDataField   | ✔ | ✔ | [![GitHub Issue State](https://img.shields.io/github/issues/detail/s/Esri/arcgis-toolkit-dotnet/198.svg)](https://github.com/Esri/arcgis-toolkit-dotnet/issues/198) | [![GitHub Issue State](https://img.shields.io/github/issues/detail/s/Esri/arcgis-toolkit-dotnet/198.svg)](https://github.com/Esri/arcgis-toolkit-dotnet/issues/198) | [![GitHub Issue State](https://img.shields.io/github/issues/detail/s/Esri/arcgis-toolkit-dotnet/198.svg)](https://github.com/Esri/arcgis-toolkit-dotnet/issues/198) |
|Legend   | ✔ | ✔ | ✔ | ✔ | ✔ |
|LayerLegend   | ✔ | ✔ | ✔ | ✔ | ✔ |
|MeasureToolbar   | ✔ | ✔ | [![GitHub Issue State](https://img.shields.io/github/issues/detail/s/Esri/arcgis-toolkit-dotnet/199.svg)](https://github.com/Esri/arcgis-toolkit-dotnet/issues/199) | [![GitHub Issue State](https://img.shields.io/github/issues/detail/s/Esri/arcgis-toolkit-dotnet/199.svg)](https://github.com/Esri/arcgis-toolkit-dotnet/issues/199) | [![GitHub Issue State](https://img.shields.io/github/issues/detail/s/Esri/arcgis-toolkit-dotnet/199.svg)](https://github.com/Esri/arcgis-toolkit-dotnet/issues/199) |
|PopupViewer | ✔ | ✔ | ✔ | ✔ | ✔ |
|ScaleLine   | ✔ | ✔ | ✔ | ✔ | ✔ |
|SignInForm   |   | Preview |   |   |   |
|SymbolDisplay   | ✔ | ✔ | ✔ | ✔ | ✔ |
|TableOfContents   | N/A | Preview | N/A  | N/A | N/A |
|TimeSlider   | ✔ | ✔ | ✔ | ✔ | ✔ |

## Contributing

Contributions are welcome. Please see the [contribution guide](CONTRIBUTING.md) for more details.

Find a bug or want to request a new feature? Please let us know by [submitting an issue](https://github.com/Esri/arcgis-toolkit-dotnet/issues/new).

## License

Copyright © 2014-2021 Esri.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   https://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

A copy of the license is available in the repository's [license.txt](/license.txt) file.