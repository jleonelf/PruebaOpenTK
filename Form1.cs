using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace PruebaOpenTK
{
    public partial class Form1 : Form
    {
        private GLControl glControl;

        // 8 vértices del cubo

        private readonly Vector3[] verts =
        {
            new Vector3(-1, -1,  1), // 0
            new Vector3( 1, -1,  1), // 1
            new Vector3( 1,  1,  1), // 2
            new Vector3(-1,  1,  1), // 3
            new Vector3(-1, -1, -1), // 4
            new Vector3( 1, -1, -1), // 5
            new Vector3( 1,  1, -1), // 6
            new Vector3(-1,  1, -1), // 7
        };

        // 12 aristas
        private readonly int[,] edges =
        {
            {0,1},{1,2},{2,3},{3,0}, // frente
            {4,5},{5,6},{6,7},{7,4}, // atrás
            {0,4},{1,5},{2,6},{3,7}  // uniones
        };

        public Form1()
        {
            InitializeComponent();

            glControl = new GLControl(new GraphicsMode(32, 24, 0, 4));
            glControl.Dock = DockStyle.Fill;
            glControl.Load += GlControl_Load;
            glControl.Paint += GlControl_Paint;
            glControl.Resize += GlControl_Resize;
            Controls.Add(glControl);
        }

        private void GlControl_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.White); 
            GL.Enable(EnableCap.DepthTest);
            GL.LineWidth(2f);

            SetupProjection();
        }

        private void GlControl_Resize(object sender, EventArgs e)
        {
            SetupProjection();
            glControl.Invalidate();
        }

        private void SetupProjection()
        {
            if (glControl.ClientSize.Height == 0) return;

            GL.Viewport(0, 0, glControl.Width, glControl.Height);

            Matrix4 proj = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(60f),
                glControl.Width / (float)glControl.Height,
                0.1f,
                100f
            );
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref proj);
        }

        private void GlControl_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Vista con cámara fija
            Matrix4 view = Matrix4.LookAt(new Vector3(3, 3, 6), Vector3.Zero, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref view);

            GL.Color3(Color.Brown); // Cafe
            GL.Begin(PrimitiveType.Lines);
            for (int i = 0; i < edges.GetLength(0); i++)
            {
                Vector3 a = verts[edges[i, 0]];
                Vector3 b = verts[edges[i, 1]];
                GL.Vertex3(a);
                GL.Vertex3(b);
            }
            GL.End();

            glControl.SwapBuffers();
        }

        private void Form1_Load(object sender, EventArgs e) { }
    }
}
