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

#if __ANDROID__
using System;
using Android.Content;
using Android.Opengl;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.UI.Controls;
using Google.AR.Core;
using Google.AR.Core.Exceptions;
using Javax.Microedition.Khronos.Opengles;

namespace Esri.ArcGISRuntime.ARToolkit
{
    [Android.Runtime.Register("Esri.ArcGISRuntime.ARToolkit.ARSceneView")]
    public partial class ARSceneView : SceneView
    {
        private class Renderer : Java.Lang.Object, GLSurfaceView.IRenderer
        {
            private Action<IGL10> _onDraw;
            private Action<IGL10, int, int> _surfaceChanged;
            private Action<IGL10, Javax.Microedition.Khronos.Egl.EGLConfig> _onSurfaceCreated;

            public Renderer(Action<IGL10> onDraw, Action<IGL10, int, int> surfaceChanged, Action<IGL10, Javax.Microedition.Khronos.Egl.EGLConfig> onSurfaceCreated)
            {
                _onDraw = onDraw;
                _surfaceChanged = surfaceChanged;
                _onSurfaceCreated = onSurfaceCreated;
            }

            public void OnDrawFrame(IGL10 gl) => _onDraw(gl);

            public void OnSurfaceChanged(IGL10 gl, int width, int height) => _surfaceChanged(gl, width, height);

            public void OnSurfaceCreated(IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config) => _onSurfaceCreated(gl, config);
        }

        private DisplayRotationHelper _displayRotationHelper;
        private BackgroundRenderer _backgroundRenderer = new BackgroundRenderer();
        private Renderer _renderer;
        private CompassOrientationHelper _orientationHelper; //Used for getting heading, and orientation if ARCore is disabled
        private Google.AR.Core.Session _session;

        /// <summary>
        /// A value defining whether a request for ARCore has been made. Used when requesting installation of ARCore.
        /// </summary>
        private bool _arCoreInstallRequested;

        /// <summary>
        /// Initializes a new instance of the <see cref="ARSceneView"/> class.
        /// </summary>
        /// <param name="context">The Context the view is running in, through which it can access resources, themes, etc.</param>
        public ARSceneView(Android.Content.Context context)
            : base(context)
        {
            InitializeCommon();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ARSceneView"/> class.
        /// </summary>
        /// <param name="context"> The Context the view is running in, through which it can access resources, themes, etc.</param>
        /// <param name="attr">The attributes of the AXML element declaring the view.</param>
        public ARSceneView(Context context, Android.Util.IAttributeSet attr)
            : base(context, attr)
        {
            InitializeCommon();
        }

        /// <summary>
        /// Gets a reference to the ARCore Session
        /// </summary>
        public Session Session => _session;

        private bool IsDesignTime => IsInEditMode;
       
        private void Initialize()
        {
            if (IsDesignTime)
                return;
            _displayRotationHelper = new DisplayRotationHelper(Context);
            _orientationHelper = new CompassOrientationHelper(Context);
            _orientationHelper.OrientationChanged += OrientationHelper_OrientationChanged;
            _renderer = new Renderer(OnDrawFrame, OnSurfaceChanged, OnSurfaceCreated);
            GLSurfaceView mSurfaceView = new GLSurfaceView(Context);

            // Set up renderer.
            mSurfaceView.PreserveEGLContextOnPause = true;
            mSurfaceView.SetEGLContextClientVersion(2);
            mSurfaceView.SetEGLConfigChooser(8, 8, 8, 8, 16, 0); // Alpha used for plane blending.
            mSurfaceView.SetRenderer(_renderer);
            mSurfaceView.RenderMode = Rendermode.Continuously;
            AddViewInLayout(mSurfaceView, 0, new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent));
            IsManualRendering = true;
        }

        public bool UseARCore { get; set; } = true;

        private void InitARCore()
        {
            if (Context is Android.App.Activity activity)
            {
                if (ContextCompat.CheckSelfPermission(activity, Android.Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(activity, new string[] { Android.Manifest.Permission.Camera }, 0);
                    return;
                }

                if (ArCoreApk.Instance.RequestInstall(activity, !_arCoreInstallRequested) == ArCoreApk.InstallStatus.InstallRequested)
                {
                    _arCoreInstallRequested = true;
                    return;
                }
            }

            var availability = ArCoreApk.Instance.CheckAvailability(Context);
            if (availability != ArCoreApk.Availability.SupportedInstalled)
            {
                throw new NotSupportedException("This device does not support AR: " + availability.ToString());
            }

            string message = null;
            Exception exception = null;
            Session session = null;
            try
            {
                session = new Session(Context);
            }
            catch (UnavailableArcoreNotInstalledException e)
            {
                message = "Please install ARCore";
                exception = e;
            }
            catch (UnavailableApkTooOldException e)
            {
                message = "Please update ARCore";
                exception = e;
            }
            catch (UnavailableSdkTooOldException e)
            {
                message = "Please update this app";
                exception = e;
            }
            catch (Java.Lang.Exception e)
            {
                exception = e;
                message = "This device does not support AR";
            }

            if (message != null)
            {
                throw new Exception(message, exception);
            }

            // Create default config, check is supported, create session from that config.
            var config = new Google.AR.Core.Config(session);

            session.Configure(config);
            _session = session;
        }

        private void OnStopTracking()
        {
            _isTracking = false;
            _displayRotationHelper?.OnPause();
            _orientationHelper?.Pause();
            if (_session != null)
            {
                _session.Pause();
            }
        }

        private void OnStartTracking()
        {
            if (UseARCore)
            {
                if (_session == null)
                {
                    InitARCore();
                }

                if (_session != null)
                {
                    _isTracking = false;
                    IsTrackingStateChanged?.Invoke(this, false);
                    _session.Resume();
                }
                _displayRotationHelper?.OnResume();
            }
            _orientationHelper?.Resume();
        }

        /// <summary>
        /// Occurs before the Scene gets updated
        /// </summary>
        /// <param name="gl">GL Context</param>
        /// <param name="session">AR Core Session</param>
        /// <param name="frame">Frame</param>
        protected virtual void OnDrawBegin(IGL10 gl, Session session, Frame frame)
        {
            DrawBegin?.Invoke(this, new DrawEventArgs(gl, session, frame));
        }

        /// <summary>
        /// Occurs before the Scene gets updated
        /// </summary>
        public event EventHandler<DrawEventArgs> DrawBegin;

        /// <summary>
        /// Occurs after the Scene has rendered
        /// </summary>
        public event EventHandler<DrawEventArgs> DrawComplete;

        /// <summary>
        /// Occurs after the Scene has rendered
        /// </summary>
        /// <param name="gl">GL Context</param>
        /// <param name="session">AR Core Session</param>
        /// <param name="frame">Frame</param>
        protected virtual void OnDrawComplete(IGL10 gl, Session session, Frame frame)
        {
            DrawComplete?.Invoke(this, new DrawEventArgs(gl, session, frame));
        }

        private Frame _lastFrame;

        private void OnDrawFrame(IGL10 gl)
        {
            // Clear screen to notify driver it should not load any pixels from previous frame.
            GLES20.GlClear(GLES20.GlColorBufferBit | GLES20.GlDepthBufferBit);

            if (_session != null && UseARCore)
            {
                // Notify ARCore session that the view size changed so that the perspective matrix and
                // the video background can be properly adjusted.
                _displayRotationHelper.UpdateSessionIfNeeded(_session);
                try
                {
                    _session.SetCameraTextureName(_backgroundRenderer.TextureId);

                    // Obtain the current frame from ARSession. When the configuration is set to
                    // UpdateMode.BLOCKING (it is by default), this will throttle the rendering to the
                    // camera framerate.
                    Frame frame = _session.Update();
                    var camera = frame.Camera;

                    // Draw background.
                    if (_renderVideoFeed)
                    {
                        _backgroundRenderer.Draw(frame);
                    }

                    OnDrawBegin(gl, _session, frame);

                    // If not tracking, don't draw 3d objects.
                    if (camera.TrackingState == TrackingState.Paused)
                    {
                        return;
                    }

                    // No tracking error at this point. If we detected any plane, then hide the
                    // message UI, otherwise show searchingPlane message.
                    bool tracking = HasTrackingPlane();
                    if (_isTracking != tracking)
                    {
                        _isTracking = tracking;
                        IsTrackingStateChanged?.Invoke(this, tracking);
                    }

                    // Get projection matrix.
                    float[] projmtx = new float[16];
                    camera.GetProjectionMatrix(projmtx, 0, 0.1f, 100.0f);

                    // Get camera matrix and draw.
                    // float[] viewmtx = new float[16];
                    // camera.GetViewMatrix(viewmtx, 0);
                    if (tracking)
                    {
                        var pose = camera.DisplayOrientedPose;
                        var c = Camera;
                        if (c != null)
                        {
                            if (_controller.OriginCamera == null)
                            {
                                _controller.OriginCamera = new Esri.ArcGISRuntime.Mapping.Camera(c.Location, c.Heading, 90, 0);
                                OriginCameraChanged?.Invoke(this, EventArgs.Empty);
                            }

                            _controller.TransformationMatrix = InitialTransformation + TransformationMatrix.Create(pose.Qx(), pose.Qy(), pose.Qz(), pose.Qw(), pose.Tx(), pose.Ty(), pose.Tz());
                            var intrinsics = camera.ImageIntrinsics;
                            float[] fl = intrinsics.GetFocalLength();
                            float[] pp = intrinsics.GetPrincipalPoint();
                            int[] size = intrinsics.GetImageDimensions();
                            SetFieldOfView(fl[0], fl[1], pp[0], pp[1], size[0], size[1], GetDeviceOrientation());
                        }
                    }
                    if (IsManualRendering)
                    {
                        RenderFrame();
                    }

                    OnDrawComplete(gl, _session, frame);
                    _lastFrame = frame;
                }
                catch (System.Exception)
                {
                }
            }
            else
            {
                if(RenderVideoFeed)
                {
                    // TODO: Render the camera on the background
                }

                if (IsManualRendering)
                {
                    RenderFrame();
                }
            }
        }

        private void OrientationHelper_OrientationChanged(object sender, CompassOrientationEventArgs e)
        {
            if (!UseARCore)
            {
                // Use orientation sensor instead of ARCore
                var m = e.Transformation;
                var qw = Math.Sqrt(1 + m[0] + m[4] + m[8]) / 2;
                var qx = (m[7] - m[5]) / (4 * qw);
                var qy = (m[2] - m[6]) / (4 * qw);
                var qz = (m[3] - m[1]) / (4 * qw);
                _controller.TransformationMatrix = InitialTransformation + TransformationMatrix.Create(qx, qz, -qy, qw, 0, 0, 0);
            }
        }

        private UI.DeviceOrientation GetDeviceOrientation()
        {
            var windowManager = Context.GetSystemService(Context.WindowService).JavaCast<Android.Views.IWindowManager>();
            switch (windowManager.DefaultDisplay.Rotation)
            {
                case Android.Views.SurfaceOrientation.Rotation0: return UI.DeviceOrientation.Portrait;
                case Android.Views.SurfaceOrientation.Rotation90: return UI.DeviceOrientation.LandscapeLeft;
                case Android.Views.SurfaceOrientation.Rotation180: return UI.DeviceOrientation.ReversePortrait;
                case Android.Views.SurfaceOrientation.Rotation270:
                default: return UI.DeviceOrientation.LandscapeRight;
            }
        }

        /// <summary>
        /// Records a change in surface dimensions.
        /// </summary>
        /// <param name="gl">GL context</param>
        /// <param name="width">The updated width of the surface.</param>
        /// <param name="height">The updated height of the surface.</param>
        protected virtual void OnSurfaceChanged(IGL10 gl, int width, int height)
        {
            _displayRotationHelper.OnSurfaceChanged(width, height);
            GLES20.GlViewport(0, 0, width, height);
        }

        /// <summary>
        /// Triggered when the GL surface has been created
        /// </summary>
        /// <param name="gl">GL context</param>
        /// <param name="config">GL Configuration</param>
        protected virtual void OnSurfaceCreated(IGL10 gl, Javax.Microedition.Khronos.Egl.EGLConfig config)
        {
            GLES20.GlClearColor(0.1f, 0.1f, 0.1f, 1.0f);

            // Create the texture and pass it to ARCore session to be filled during update().
            _backgroundRenderer.CreateOnGlThread(/*context=*/Context);
            if (_session != null)
            {
                _session.SetCameraTextureName(_backgroundRenderer.TextureId);
            }
        }

        /// <summary>
        /// Checks if we detected at least one plane.
        /// </summary>
        /// <returns>True if at least one plane is detected</returns>
        private bool HasTrackingPlane()
        {
            foreach (var plane in _session.GetAllTrackables(Java.Lang.Class.FromType(typeof(Plane))))
            {
                if (((Plane)plane).TrackingState == TrackingState.Tracking)
                {
                    return true;
                }
            }

            return false;
        }

        private bool _isTracking;

        /// <summary>
        /// Raised if the tracking state changes
        /// </summary>
        /// <seealso cref="IsTracking"/>
        /// <seealso cref="StartTrackingAsync"/>
        /// <seealso cref="StopTracking"/>
        public event EventHandler<bool> IsTrackingStateChanged;

        private TransformationMatrix HitTest(Android.Graphics.PointF screenPoint)
        {
            var frame = _lastFrame;
            if (frame != null && frame.Camera.TrackingState == TrackingState.Tracking)
            {
                var hitResults = _lastFrame.HitTest(screenPoint.X, screenPoint.Y);
                var hitResult = hitResults.Count > 0 ? hitResults[0] : null;
                if (hitResult != null)
                {
                    var q = hitResult.HitPose.GetRotationQuaternion();
                    var t = hitResult.HitPose.GetTranslation();
                    return TransformationMatrix.Create(q[0], q[1], q[2], q[3], t[0], t[1], t[2]);
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Event args used for the <see cref="ARSceneView.DrawBegin"/> and <see cref="ARSceneView.DrawComplete"/> events.
    /// </summary>
    /// <seealso cref="ARSceneView.DrawBegin"/>
    /// <seealso cref="ARSceneView.DrawComplete"/>
#pragma warning disable SA1402
    public sealed class DrawEventArgs : EventArgs
#pragma warning restore SA1402
    {
        internal DrawEventArgs(IGL10 gl, Session session, Frame frame)
        {
            GL = gl;
            Session = session;
            Frame = frame;
        }

        /// <summary>
        /// Gets the current frame
        /// </summary>
        public Frame Frame { get; }

        /// <summary>
        /// Gets the session
        /// </summary>
        public Session Session { get; }

        /// <summary>
        /// Gets the GL context
        /// </summary>
        public IGL10 GL { get; }
    }
}
#endif