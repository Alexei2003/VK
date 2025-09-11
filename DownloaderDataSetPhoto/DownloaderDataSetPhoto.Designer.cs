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
            pGelbooru = new Panel();
            tbGelbooruUrl = new TextBox();
            textBox4 = new TextBox();
            bDownloadPhotosGelbooru = new Button();
            textBox2 = new TextBox();
            tBackgroundImageCopy = new System.Windows.Forms.Timer(components);
            bBackgroundImageCopy = new Button();
            bGelbooru = new Button();
            dgvDictionary = new DataGridView();
            index = new DataGridViewTextBoxColumn();
            tag = new DataGridViewTextBoxColumn();
            gelbooru = new DataGridViewTextBoxColumn();
            bBackgroundImageCopyOff = new Button();
            bBackgroundImageCopyOn = new Button();
            textBox1 = new TextBox();
            pBackgroundImageCopy = new Panel();
            bSaveTag = new Button();
            textBox5 = new TextBox();
            textBox6 = new TextBox();
            tbGelbooru = new TextBox();
            bDownloadAll = new Button();
            pGelbooru.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDictionary).BeginInit();
            pBackgroundImageCopy.SuspendLayout();
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
            // pGelbooru
            // 
            pGelbooru.BorderStyle = BorderStyle.FixedSingle;
            pGelbooru.Controls.Add(tbGelbooruUrl);
            pGelbooru.Controls.Add(textBox4);
            pGelbooru.Controls.Add(bDownloadPhotosGelbooru);
            pGelbooru.Controls.Add(textBox2);
            pGelbooru.Location = new Point(186, 174);
            pGelbooru.Name = "pGelbooru";
            pGelbooru.Size = new Size(158, 166);
            pGelbooru.TabIndex = 41;
            // 
            // tbGelbooruUrl
            // 
            tbGelbooruUrl.Location = new Point(3, 57);
            tbGelbooruUrl.Name = "tbGelbooruUrl";
            tbGelbooruUrl.Size = new Size(150, 23);
            tbGelbooruUrl.TabIndex = 46;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(3, 30);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(150, 23);
            textBox4.TabIndex = 45;
            textBox4.Text = "Url";
            // 
            // bDownloadPhotosGelbooru
            // 
            bDownloadPhotosGelbooru.Location = new Point(3, 123);
            bDownloadPhotosGelbooru.Name = "bDownloadPhotosGelbooru";
            bDownloadPhotosGelbooru.Size = new Size(150, 38);
            bDownloadPhotosGelbooru.TabIndex = 44;
            bDownloadPhotosGelbooru.Text = "Скачать";
            bDownloadPhotosGelbooru.UseVisualStyleBackColor = true;
            bDownloadPhotosGelbooru.Click += bDownloadPhotosGelbooru_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(3, 3);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(150, 23);
            textBox2.TabIndex = 36;
            textBox2.Text = "Gelbooru";
            // 
            // tBackgroundImageCopy
            // 
            tBackgroundImageCopy.Interval = 1000;
            tBackgroundImageCopy.Tick += tBackgroundImageCopy_Tick;
            // 
            // bBackgroundImageCopy
            // 
            bBackgroundImageCopy.Location = new Point(12, 251);
            bBackgroundImageCopy.Name = "bBackgroundImageCopy";
            bBackgroundImageCopy.Size = new Size(150, 38);
            bBackgroundImageCopy.TabIndex = 39;
            bBackgroundImageCopy.Text = "Фоновое копирование";
            bBackgroundImageCopy.UseVisualStyleBackColor = true;
            bBackgroundImageCopy.Click += bBackgroundImageCopy_Click;
            // 
            // bGelbooru
            // 
            bGelbooru.Location = new Point(12, 295);
            bGelbooru.Name = "bGelbooru";
            bGelbooru.Size = new Size(150, 38);
            bGelbooru.TabIndex = 43;
            bGelbooru.Text = "Gelbooru";
            bGelbooru.UseVisualStyleBackColor = true;
            bGelbooru.Click += bGelbooru_Click;
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
            bBackgroundImageCopyOff.Location = new Point(3, 120);
            bBackgroundImageCopyOff.Name = "bBackgroundImageCopyOff";
            bBackgroundImageCopyOff.Size = new Size(150, 38);
            bBackgroundImageCopyOff.TabIndex = 37;
            bBackgroundImageCopyOff.Text = "Выключить";
            bBackgroundImageCopyOff.UseVisualStyleBackColor = true;
            bBackgroundImageCopyOff.Click += bBackgroundImageCopyOff_Click;
            // 
            // bBackgroundImageCopyOn
            // 
            bBackgroundImageCopyOn.Location = new Point(3, 76);
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
            pBackgroundImageCopy.Location = new Point(186, 174);
            pBackgroundImageCopy.Name = "pBackgroundImageCopy";
            pBackgroundImageCopy.Size = new Size(158, 166);
            pBackgroundImageCopy.TabIndex = 40;
            // 
            // bSaveTag
            // 
            bSaveTag.Location = new Point(12, 178);
            bSaveTag.Name = "bSaveTag";
            bSaveTag.Size = new Size(150, 38);
            bSaveTag.TabIndex = 45;
            bSaveTag.Text = "Сохранить тег";
            bSaveTag.UseVisualStyleBackColor = true;
            bSaveTag.Click += bSaveTag_Click;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(12, 222);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(150, 23);
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
            bDownloadAll.Location = new Point(12, 339);
            bDownloadAll.Name = "bDownloadAll";
            bDownloadAll.Size = new Size(150, 38);
            bDownloadAll.TabIndex = 49;
            bDownloadAll.Text = "Скачать всё";
            bDownloadAll.UseVisualStyleBackColor = true;
            bDownloadAll.Click += bDownloadAll_Click;
            // 
            // DownloaderDataSetPhoto
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(1097, 493);
            Controls.Add(bDownloadAll);
            Controls.Add(textBox6);
            Controls.Add(tbGelbooru);
            Controls.Add(textBox5);
            Controls.Add(bSaveTag);
            Controls.Add(dgvDictionary);
            Controls.Add(bGelbooru);
            Controls.Add(bBackgroundImageCopy);
            Controls.Add(pGelbooru);
            Controls.Add(textBox3);
            Controls.Add(tbTag);
            Controls.Add(pBackgroundImageCopy);
            Name = "DownloaderDataSetPhoto";
            Text = "DownloaderDataSetPhoto";
            FormClosing += Form1_FormClosing;
            pGelbooru.ResumeLayout(false);
            pGelbooru.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDictionary).EndInit();
            pBackgroundImageCopy.ResumeLayout(false);
            pBackgroundImageCopy.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbTag;
        private TextBox textBox3;
        private Panel pGelbooru;
        private TextBox textBox2;
        private System.Windows.Forms.Timer tBackgroundImageCopy;
        private Button bBackgroundImageCopy;
        private Button bGelbooru;
        private Button bDownloadPhotosGelbooru;
        private TextBox textBox4;
        private TextBox tbGelbooruUrl;
        private DataGridView dgvDictionary;
        private Button bBackgroundImageCopyOff;
        private Button bBackgroundImageCopyOn;
        private TextBox textBox1;
        private Panel pBackgroundImageCopy;
        private Button bSaveTag;
        private TextBox textBox5;
        private DataGridViewTextBoxColumn index;
        private DataGridViewTextBoxColumn tag;
        private DataGridViewTextBoxColumn gelbooru;
        private TextBox textBox6;
        private TextBox tbGelbooru;
        private Button bDownloadAll;
    }
}
