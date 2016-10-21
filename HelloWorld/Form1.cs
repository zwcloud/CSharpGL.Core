using System;
using System.Diagnostics;
using System.Windows.Forms;
using CSharpGL.Objects.RenderContexts;
using GLM;
using System.Drawing;
using OpenGL = CSharpGL.GL;

namespace HelloWorld
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //  Set the user draw styles.
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            CreateRenderContext();
        }

        private readonly Stopwatch stopWatch = new Stopwatch();
        protected RenderContext renderContext;

        protected void CreateRenderContext()
        {
            // Initialises OpenGL.
            renderContext = new FBORenderContext();

            //  Create the render context.
            renderContext.Create(OpenGLVersion, Width, Height, 32, null);
            renderContext.MakeCurrent();

            string version = CSharpGL.GL.GetString(CSharpGL.GL.GL_VERSION);
            this.Text = version;

            //  Set the most basic OpenGL styles.
            OpenGL.ShadeModel(OpenGL.GL_SMOOTH);
            OpenGL.ClearDepth(1.0f);
            OpenGL.Enable(OpenGL.GL_DEPTH_TEST);
            OpenGL.DepthFunc(OpenGL.GL_LEQUAL);
            OpenGL.Hint(OpenGL.GL_PERSPECTIVE_CORRECTION_HINT, OpenGL.GL_NICEST);
            ResizeGL(this.Width, this.Height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (renderContext == null)
            {
                base.OnPaint(e);
                return;
            }

            stopWatch.Reset();
            stopWatch.Start();

            //	Make sure it's our instance of openSharpGL that's active.
            renderContext.MakeCurrent();

            Clear();

            //	If there is a draw handler, then call it.
            DoOpenGLDraw(e);

            //	Blit our offscreen bitmap.
            Graphics graphics = e.Graphics;
            IntPtr deviceContext = graphics.GetHdc();
            renderContext.Blit(deviceContext);
            graphics.ReleaseHdc(deviceContext);

            stopWatch.Stop();

            this.FPS = 1000.0 / stopWatch.Elapsed.TotalMilliseconds;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void Clear()
        {
            // Sky blue fore background.
            OpenGL.ClearColor(0x87 / 255.0f, 0xce / 255.0f, 0xeb / 255.0f, 0xff / 255.0f);

            //  Clear the color and depth buffer.
            OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        private void redrawTimer_Tick_1(object sender, EventArgs e)
        {
            //this.renderingRequired = true;
            this.Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (renderContext != null)
            {
                renderContext.MakeCurrent();

                renderContext.SetDimensions(this.Width, this.Height);

                OpenGL.Viewport(0, 0, this.Width, this.Height);

                ResizeGL(this.Width, this.Height);

                this.Invalidate();
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            DestroyRenderContext();

            base.OnHandleDestroyed(e);
        }

        private void DestroyRenderContext()
        {
            if (renderContext != null)
            {
                renderContext.Dispose();
                renderContext = null;
            }
        }

        public double FPS { get; private set; }

        /// <summary>
        /// Gets or sets the desired OpenGL version.
        /// </summary>
        /// <value>
        /// The desired OpenGL version.
        /// </value>
        public GLVersion OpenGLVersion
        {
            get { return openGLVersion; }
        }

        /// <summary>
        /// The default desired OpenGL version.
        /// </summary>
        private GLVersion openGLVersion = GLVersion.OpenGL2_1;

        /// <summary>
        /// Call this function in derived classes to do the OpenGL Draw event.
        /// </summary>
        private void DoOpenGLDraw(PaintEventArgs e)
        {
            var handler = OpenGLDraw;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Occurs when OpenGL drawing should be performed.
        /// </summary>
        public event EventHandler<PaintEventArgs> OpenGLDraw;

        ////  Use the 'look at' helper function to position and aim the camera.
        static readonly mat4 viewMatrix = glm.lookAt(new vec3(0, 0, 2), new vec3(0, 0, 0), new vec3(0, 1, 0));
        public static void ResizeGL(double width, double height)
        {
            //  Set the projection matrix.
            OpenGL.MatrixMode(OpenGL.GL_PROJECTION);

            //  Load the identity.
            OpenGL.LoadIdentity();
            ////  Create a perspective transformation.
            //OpenGL.gluPerspective(60.0f, width / height, 0.01, 100.0);
            mat4 projectionMatrix = glm.perspective(glm.radians(60.0f), (float)(width / height), 0.01f, 100.0f);
            OpenGL.MultMatrixf((projectionMatrix * viewMatrix).to_array());

            //  Set the modelview matrix.
            OpenGL.MatrixMode(OpenGL.GL_MODELVIEW);
        }
    }
}
