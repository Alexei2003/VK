namespace NeuralNetworkAnalyzer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pbImage = new PictureBox();
            dvgPercent = new DataGridView();
            tag = new DataGridViewTextBoxColumn();
            percent = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)pbImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dvgPercent).BeginInit();
            SuspendLayout();
            // 
            // pbImage
            // 
            pbImage.BorderStyle = BorderStyle.FixedSingle;
            pbImage.Location = new Point(12, 12);
            pbImage.Name = "pbImage";
            pbImage.Size = new Size(600, 600);
            pbImage.SizeMode = PictureBoxSizeMode.Zoom;
            pbImage.TabIndex = 0;
            pbImage.TabStop = false;
            pbImage.Click += pbImage_Click;
            // 
            // dvgPercent
            // 
            dvgPercent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dvgPercent.Columns.AddRange(new DataGridViewColumn[] { tag, percent });
            dvgPercent.Location = new Point(618, 12);
            dvgPercent.Name = "dvgPercent";
            dvgPercent.Size = new Size(600, 600);
            dvgPercent.TabIndex = 1;
            // 
            // tag
            // 
            tag.HeaderText = "Тег";
            tag.Name = "tag";
            tag.ReadOnly = true;
            tag.Width = 490;
            // 
            // percent
            // 
            percent.HeaderText = "Процент";
            percent.Name = "percent";
            percent.ReadOnly = true;
            percent.Width = 60;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 626);
            Controls.Add(dvgPercent);
            Controls.Add(pbImage);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pbImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)dvgPercent).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pbImage;
        private DataGridView dvgPercent;
        private DataGridViewTextBoxColumn tag;
        private DataGridViewTextBoxColumn percent;
    }
}
