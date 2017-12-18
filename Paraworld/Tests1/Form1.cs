using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Paraworld.Resources;


namespace Tests1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            elementHostMeshViewer.HostContainer.MouseEnter += HostContainer_MouseEnter;
        }

        private void HostContainer_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            elementHostMeshViewer.Focus();
        }

        private void buttonReadGSFPack_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                GsfPackage gsfPackage = GsfPackage.Read(ofd.FileName);
                StringBuilder sb = new StringBuilder();
                sb.Append("GSF Pack: ").Append(gsfPackage.Name).Append("\r\n");
                int modelNumber = 0;
                int meshNumber = 0;
                int modelsAmount = gsfPackage.Models.Count;
                int meshesAmount = 0;
                Paraworld.Resources.Graphics.Model model;
                model = gsfPackage.Models[modelNumber];
                meshesAmount = model.meshes.Count;
                Paraworld.Resources.Graphics.Mesh mesh;
                mesh = model.meshes[meshNumber];
                string baseTextureFilename = @"C:\Program Files (x86)\Sunflowers\ParaWorld\Data\Base\Texture\";
                string textureFilename = baseTextureFilename + gsfPackage.Materials[model.materialIndices[0]].textureFilename1.Replace('/', '\\').Replace(".tga", "_(0256).dds");
                if (!File.Exists(textureFilename)) textureFilename = null;
                TestControls.MeshViewer mv = (TestControls.MeshViewer)elementHostMeshViewer.Child;
                mv.SetMesh(mesh.BBox, mesh.Vertices, mesh.Triangles, mesh.UVMap, textureFilename);
                sb.Append("Model: ").Append((modelNumber + 1).ToString()).Append("/").Append(modelsAmount.ToString()).Append("\r\n");
                sb.Append("Model Name: ").Append(model.name).Append("\r\n");
                sb.Append("Mesh: ").Append((meshNumber + 1).ToString()).Append("/").Append(meshesAmount.ToString()).Append("\r\n");
                richTextBoxLastError.Text = sb.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create the WPF MeshViewer UserControl.
            TestControls.MeshViewer mv = new TestControls.MeshViewer();

            // Populate the MeshViewer with data for a mesh to show


            // Assign the WPF UserControl to the ElementHost control's Child property.
            elementHostMeshViewer.Child = mv;
        }
    }
}
