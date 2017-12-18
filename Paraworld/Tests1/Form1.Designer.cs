namespace Tests1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.elementHostMeshViewer = new System.Windows.Forms.Integration.ElementHost();
            this.buttonReadGSFPack = new System.Windows.Forms.Button();
            this.labelStreamPos = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBoxLastError = new System.Windows.Forms.RichTextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.elementHostMeshViewer);
            this.panel1.Location = new System.Drawing.Point(213, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(664, 403);
            this.panel1.TabIndex = 14;
            // 
            // elementHostMeshViewer
            // 
            this.elementHostMeshViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHostMeshViewer.Location = new System.Drawing.Point(0, 0);
            this.elementHostMeshViewer.Name = "elementHostMeshViewer";
            this.elementHostMeshViewer.Size = new System.Drawing.Size(660, 399);
            this.elementHostMeshViewer.TabIndex = 8;
            this.elementHostMeshViewer.Text = "elementHost1";
            this.elementHostMeshViewer.Child = null;
            // 
            // buttonReadGSFPack
            // 
            this.buttonReadGSFPack.Location = new System.Drawing.Point(22, 31);
            this.buttonReadGSFPack.Name = "buttonReadGSFPack";
            this.buttonReadGSFPack.Size = new System.Drawing.Size(126, 37);
            this.buttonReadGSFPack.TabIndex = 13;
            this.buttonReadGSFPack.Text = "Open GSF Pack";
            this.buttonReadGSFPack.UseVisualStyleBackColor = true;
            this.buttonReadGSFPack.Click += new System.EventHandler(this.buttonReadGSFPack_Click);
            // 
            // labelStreamPos
            // 
            this.labelStreamPos.AutoSize = true;
            this.labelStreamPos.Location = new System.Drawing.Point(368, 239);
            this.labelStreamPos.Name = "labelStreamPos";
            this.labelStreamPos.Size = new System.Drawing.Size(0, 17);
            this.labelStreamPos.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(281, 472);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Stream pos:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 472);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Output:";
            // 
            // richTextBoxLastError
            // 
            this.richTextBoxLastError.Location = new System.Drawing.Point(22, 509);
            this.richTextBoxLastError.Name = "richTextBoxLastError";
            this.richTextBoxLastError.Size = new System.Drawing.Size(609, 96);
            this.richTextBoxLastError.TabIndex = 9;
            this.richTextBoxLastError.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(899, 635);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonReadGSFPack);
            this.Controls.Add(this.labelStreamPos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBoxLastError);
            this.Name = "Form1";
            this.Text = "View a mesh inside a Gsf";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Integration.ElementHost elementHostMeshViewer;
        private System.Windows.Forms.Button buttonReadGSFPack;
        private System.Windows.Forms.Label labelStreamPos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBoxLastError;
    }
}

