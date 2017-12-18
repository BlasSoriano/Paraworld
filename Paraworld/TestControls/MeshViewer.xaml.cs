using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Paraworld.Resources.Graphics;

namespace TestControls
{
    /// <summary>
    /// Interaction logic for MeshViewer.xaml
    /// </summary>
    public partial class MeshViewer : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MeshViewer()
        {
            InitializeComponent();
        }

        private bool mDown = false;
        private Point mLastPos;

        private MeshGeometry3D _meshGeom;
        public MeshGeometry3D MeshGeom
        {
            get
            {
                return this._meshGeom;
            }
            set
            {
                if (value != this._meshGeom)
                {
                    this._meshGeom = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private GeometryModel3D _mGeometry;
        public GeometryModel3D MGeometry
        {
            get { return this._mGeometry; }
            set
            {
                if (value != this._mGeometry)
                {
                    this._mGeometry = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private PerspectiveCamera _camera;
        public PerspectiveCamera Cam
        {
            get
            {
                return this._camera;
            }
            set
            {
                if (value != this._camera)
                {
                    this._camera = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Viewport3D _viewport;
        public Viewport3D ViewPort
        {
            get { return this._viewport; }
            set
            {
                if (value != this._viewport)
                {
                    this._viewport = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void SetMesh(BoundingBox bBox, List<Vertex> vertices, List<Triangle> triangles, List<TextureCoords> textureVertices, string textureFilename = null)
        {
            Point3DCollection points = new Point3DCollection();
            for (int i = 0; i < vertices.Count; i++)
            {
                points.Add(new Point3D(vertices[i].X, vertices[i].Y, vertices[i].Z));
            }
            int[] indices = new int[triangles.Count * 3];
            for (int i = 0; i < triangles.Count; i++)
            {
                indices[i * 3 + 0] = triangles[i].P1;
                indices[i * 3 + 1] = triangles[i].P2;
                indices[i * 3 + 2] = triangles[i].P3;
            }
            PointCollection textVert = new PointCollection();
            for (int i = 0; i < textureVertices.Count; i++)
            {
                textVert.Add(new Point(textureVertices[i].U, textureVertices[i].V));
            }
            MeshGeometry3D meshGeom3D = new MeshGeometry3D();
            meshGeom3D.Positions = points;
            meshGeom3D.TriangleIndices = new Int32Collection(indices);
            meshGeom3D.TextureCoordinates = textVert;
            MeshGeom = meshGeom3D;

            // Prepare the geometry
            DiffuseMaterial material = null;
            if (textureFilename == null)
            {
                SolidColorBrush solidBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                material = new DiffuseMaterial(solidBrush);
            }
            else
            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri(textureFilename, UriKind.Relative));
                material = new DiffuseMaterial(imageBrush);
            }
            MGeometry = new GeometryModel3D(meshGeom3D, material);
            MGeometry.Transform = new Transform3DGroup();
            ModelVisual3D mv3dModel = new ModelVisual3D();
            mv3dModel.Content = MGeometry;

            // Prepare the light
            DirectionalLight dirLight = new DirectionalLight(Color.FromArgb(255, 255, 255, 255), new Vector3D(3, -4, 5));
            ModelVisual3D mv3dDirLight = new ModelVisual3D();
            mv3dDirLight.Content = dirLight;

            // Prepare the camera
            double sizeX = bBox.max.X - bBox.min.X;
            double sizeY = bBox.max.Y - bBox.min.Y;
            double sizeZ = bBox.max.Z - bBox.min.Z;
            double centerX = sizeX / 2 + bBox.min.X;
            double centerY = sizeY / 2 + bBox.min.Y;
            double centerZ = sizeZ / 2 + bBox.min.Z;
            double distanceZ = Math.Max(sizeY, sizeZ) + 0.25;
            double posZ = bBox.max.Z + distanceZ;
            Point3D camPosition = new Point3D(centerX, centerY, -posZ);
            Vector3D camLookDirection = new Vector3D(0, 0, 1);
            //Point3D camPosition = new Point3D(0, 0, 2);
            //Vector3D camLookDirection = new Vector3D(0, 0, -1);
            Vector3D camUpDirection = new Vector3D(0, 1, 0);
            double camFieldOfView = 70.0;
            PerspectiveCamera cam = new PerspectiveCamera(camPosition, camLookDirection, camUpDirection, camFieldOfView);
            cam.FarPlaneDistance = distanceZ + sizeZ + distanceZ;
            cam.NearPlaneDistance = 0;
            Cam = cam;

            // Group into a viewport3d the light and the geometry, and add the camera
            Viewport3D vp3d = new Viewport3D();
            vp3d.Children.Add(mv3dDirLight);
            vp3d.Children.Add(mv3dModel);
            vp3d.Camera = Cam;
            ViewPort = vp3d;

            // Insert the viewport as the only children of the main grid
            MainGrid.Children.Clear();
            MainGrid.Children.Add(ViewPort);
        }

        private void MainGrid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Cam.Position = new Point3D(
                Cam.Position.X,
                Cam.Position.Y,
                Cam.Position.Z - e.Delta / 250D
                );
        }

        private void MainGrid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mDown = false;
        }

        private void MainGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            mDown = true;
            Point pos = Mouse.GetPosition(ViewPort);
            mLastPos = new Point(
                    pos.X - ViewPort.ActualWidth / 2,
                    ViewPort.ActualHeight / 2 - pos.Y);
        }

        private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mDown) return;
            Point pos = Mouse.GetPosition(ViewPort);
            Point actualPos = new Point(
                    pos.X - ViewPort.ActualWidth / 2,
                    ViewPort.ActualHeight / 2 - pos.Y);
            double dx = actualPos.X - mLastPos.X;
            double dy = actualPos.Y - mLastPos.Y;
            double mouseAngle = 0;

            if (dx != 0 && dy != 0)
            {
                mouseAngle = Math.Asin(Math.Abs(dy) /
                    Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)));
                if (dx < 0 && dy > 0) mouseAngle += Math.PI / 2;
                else if (dx < 0 && dy < 0) mouseAngle += Math.PI;
                else if (dx > 0 && dy < 0) mouseAngle += Math.PI * 1.5;
            }
            else if (dx == 0 && dy != 0)
            {
                mouseAngle = Math.Sign(dy) > 0 ? Math.PI / 2 : Math.PI * 1.5;
            }
            else if (dx != 0 && dy == 0)
            {
                mouseAngle = Math.Sign(dx) > 0 ? 0 : Math.PI;
            }

            double axisAngle = mouseAngle + Math.PI / 2;

            Vector3D axis = new Vector3D(
                    Math.Cos(axisAngle) * 4,
                    Math.Sin(axisAngle) * 4, 0);

            double rotation = 0.02 *
                    Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

            Transform3DGroup group = MGeometry.Transform as Transform3DGroup;
            QuaternionRotation3D r =
                 new QuaternionRotation3D(
                 new Quaternion(axis, rotation * 180 / Math.PI));
            group.Children.Add(new RotateTransform3D(r));

            mLastPos = actualPos;
        }
    }
}
