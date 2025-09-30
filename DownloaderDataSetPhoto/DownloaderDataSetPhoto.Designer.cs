namespace DownloaderDataSetPhoto
{
    partial class DownloaderDataSetPhoto
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
            components = new System.ComponentModel.Container();
            tbTag = new TextBox();
            textBox3 = new TextBox();
            bDownloadPhotosGelbooru = new Button();
            tBackgroundImageCopy = new System.Windows.Forms.Timer(components);
            dgvDictionary = new DataGridView();
            index = new DataGridViewTextBoxColumn();
            tag = new DataGridViewTextBoxColumn();
            gelbooru = new DataGridViewTextBoxColumn();
            bBackgroundImageCopyOff = new Button();
            bBackgroundImageCopyOn = new Button();
            textBox1 = new TextBox();
            pBackgroundImageCopy = new Panel();
            textBox5 = new TextBox();
            textBox6 = new TextBox();
            tbGelbooru = new TextBox();
            bDownloadAll = new Button();
            panel1 = new Panel();
            textBox2 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvDictionary).BeginInit();
            pBackgroundImageCopy.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // tbTag
            // 
            tbTag.Location = new Point(12, 38);
            tbTag.Multiline = true;
            tbTag.Name = "tbTag";
            tbTag.Size = new Size(332, 46);
            tbTag.TabIndex = 5;
            tbTag.KeyUp += tbTag_KeyUp;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(12, 11);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(28, 23);
            textBox3.TabIndex = 6;
            textBox3.Text = "Тег";
            // 
            // bDownloadPhotosGelbooru
            // 
            bDownloadPhotosGelbooru.Location = new Point(3, 76);
            bDownloadPhotosGelbooru.Name = "bDownloadPhotosGelbooru";
            bDownloadPhotosGelbooru.Size = new Size(150, 38);
            bDownloadPhotosGelbooru.TabIndex = 44;
            bDownloadPhotosGelbooru.Text = "Скачать";
            bDownloadPhotosGelbooru.UseVisualStyleBackColor = true;
            bDownloadPhotosGelbooru.Click += bDownloadPhotosGelbooru_Click;
            // 
            // tBackgroundImageCopy
            // 
            tBackgroundImageCopy.Interval = 1000;
            tBackgroundImageCopy.Tick += tBackgroundImageCopy_Tick;
            // 
            // dgvDictionary
            // 
            dgvDictionary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDictionary.Columns.AddRange(new DataGridViewColumn[] { index, tag, gelbooru });
            dgvDictionary.Location = new Point(366, 12);
            dgvDictionary.Name = "dgvDictionary";
            dgvDictionary.RowHeadersWidth = 45;
            dgvDictionary.Size = new Size(719, 467);
            dgvDictionary.TabIndex = 44;
            dgvDictionary.CellMouseClick += dgvDictionary_CellMouseClick;
            // 
            // index
            // 
            index.HeaderText = "Номер";
            index.MinimumWidth = 6;
            index.Name = "index";
            index.ReadOnly = true;
            index.Width = 60;
            // 
            // tag
            // 
            tag.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tag.HeaderText = "Теги";
            tag.MinimumWidth = 6;
            tag.Name = "tag";
            tag.ReadOnly = true;
            // 
            // gelbooru
            // 
            gelbooru.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            gelbooru.HeaderText = "Gelbooru";
            gelbooru.Name = "gelbooru";
            // 
            // bBackgroundImageCopyOff
            // 
            bBackgroundImageCopyOff.Enabled = false;
            bBackgroundImageCopyOff.Location = new Point(3, 76);
            bBackgroundImageCopyOff.Name = "bBackgroundImageCopyOff";
            bBackgroundImageCopyOff.Size = new Size(150, 38);
            bBackgroundImageCopyOff.TabIndex = 37;
            bBackgroundImageCopyOff.Text = "Выключить";
            bBackgroundImageCopyOff.UseVisualStyleBackColor = true;
            bBackgroundImageCopyOff.Click += bBackgroundImageCopyOff_Click;
            // 
            // bBackgroundImageCopyOn
            // 
            bBackgroundImageCopyOn.Location = new Point(3, 32);
            bBackgroundImageCopyOn.Name = "bBackgroundImageCopyOn";
            bBackgroundImageCopyOn.Size = new Size(150, 38);
            bBackgroundImageCopyOn.TabIndex = 36;
            bBackgroundImageCopyOn.Text = "Включить ";
            bBackgroundImageCopyOn.UseVisualStyleBackColor = true;
            bBackgroundImageCopyOn.Click += bBackgroundImageCopyOn_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(3, 3);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(150, 23);
            textBox1.TabIndex = 38;
            textBox1.Text = "Фоновое копирование";
            // 
            // pBackgroundImageCopy
            // 
            pBackgroundImageCopy.BorderStyle = BorderStyle.FixedSingle;
            pBackgroundImageCopy.Controls.Add(textBox1);
            pBackgroundImageCopy.Controls.Add(bBackgroundImageCopyOn);
            pBackgroundImageCopy.Controls.Add(bBackgroundImageCopyOff);
            pBackgroundImageCopy.Location = new Point(186, 193);
            pBackgroundImageCopy.Name = "pBackgroundImageCopy";
            pBackgroundImageCopy.Size = new Size(158, 125);
            pBackgroundImageCopy.TabIndex = 40;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(12, 164);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(332, 23);
            textBox5.TabIndex = 46;
            textBox5.Text = "Способ";
            // 
            // textBox6
            // 
            textBox6.Location = new Point(12, 85);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(58, 23);
            textBox6.TabIndex = 48;
            textBox6.Text = "Gelbooru";
            // 
            // tbGelbooru
            // 
            tbGelbooru.Location = new Point(12, 112);
            tbGelbooru.Multiline = true;
            tbGelbooru.Name = "tbGelbooru";
            tbGelbooru.Size = new Size(332, 46);
            tbGelbooru.TabIndex = 47;
            // 
            // bDownloadAll
            // 
            bDownloadAll.Location = new Point(3, 32);
            bDownloadAll.Name = "bDownloadAll";
            bDownloadAll.Size = new Size(150, 38);
            bDownloadAll.TabIndex = 49;
            bDownloadAll.Text = "Скачать всё";
            bDownloadAll.UseVisualStyleBackColor = true;
            bDownloadAll.Click += bDownloadAll_Click;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(bDownloadPhotosGelbooru);
            panel1.Controls.Add(bDownloadAll);
            panel1.Location = new Point(12, 193);
            panel1.Name = "panel1";
            panel1.Size = new Size(158, 125);
            panel1.TabIndex = 50;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(3, 3);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(150, 23);
            textBox2.TabIndex = 38;
            textBox2.Text = "Gelbooru";
            // 
            // DownloaderDataSetPhoto
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(1097, 493);
            Controls.Add(panel1);
            Controls.Add(textBox6);
            Controls.Add(tbGelbooru);
            Controls.Add(textBox5);
            Controls.Add(dgvDictionary);
            Controls.Add(textBox3);
            Controls.Add(tbTag);
            Controls.Add(pBackgroundImageCopy);
            Name = "DownloaderDataSetPhoto";
            Text = "DownloaderDataSetPhoto";
            FormClosing += Form1_FormClosing;
            ((System.ComponentModel.ISupportInitialize)dgvDictionary).EndInit();
            pBackgroundImageCopy.ResumeLayout(false);
            pBackgroundImageCopy.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbTag;
        private TextBox textBox3;
        private System.Windows.Forms.Timer tBackgroundImageCopy;
        private Button bDownloadPhotosGelbooru;
        private DataGridView dgvDictionary;
        private Button bBackgroundImageCopyOff;
        private Button bBackgroundImageCopyOn;
        private TextBox textBox1;
        private Panel pBackgroundImageCopy;
        private TextBox textBox5;
        private DataGridViewTextBoxColumn index;
        private DataGridViewTextBoxColumn tag;
        private DataGridViewTextBoxColumn gelbooru;
        private TextBox textBox6;
        private TextBox tbGelbooru;
        private Button bDownloadAll;
        private Panel panel1;
        private TextBox textBox2;
    }
}
