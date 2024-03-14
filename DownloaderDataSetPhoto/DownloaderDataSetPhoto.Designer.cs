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
            bDownloadPhotosVK = new Button();
            textBox11 = new TextBox();
            textBox10 = new TextBox();
            tbCountDownload = new TextBox();
            tbShiftDownload = new TextBox();
            textBox6 = new TextBox();
            bBackgroundImageCopyOff = new Button();
            bBackgroundImageCopyOn = new Button();
            textBox1 = new TextBox();
            pVK = new Panel();
            pBackgroundImageCopy = new Panel();
            pDanbooru = new Panel();
            textBox2 = new TextBox();
            tBackgroundImageCopy = new System.Windows.Forms.Timer(components);
            bBackgroundImageCopy = new Button();
            bVK = new Button();
            bDanbooru = new Button();
            pVK.SuspendLayout();
            pBackgroundImageCopy.SuspendLayout();
            pDanbooru.SuspendLayout();
            SuspendLayout();
            // 
            // tbTag
            // 
            tbTag.Location = new Point(12, 43);
            tbTag.Multiline = true;
            tbTag.Name = "tbTag";
            tbTag.Size = new Size(332, 52);
            tbTag.TabIndex = 5;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(12, 12);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(110, 25);
            textBox3.TabIndex = 6;
            textBox3.Text = "Тэги";
            // 
            // bDownloadPhotosVK
            // 
            bDownloadPhotosVK.Location = new Point(3, 158);
            bDownloadPhotosVK.Name = "bDownloadPhotosVK";
            bDownloadPhotosVK.Size = new Size(150, 25);
            bDownloadPhotosVK.TabIndex = 35;
            bDownloadPhotosVK.Text = "Скачать";
            bDownloadPhotosVK.UseVisualStyleBackColor = true;
            bDownloadPhotosVK.Click += bDownloadPhotosVK_Click;
            // 
            // textBox11
            // 
            textBox11.Location = new Point(3, 96);
            textBox11.Name = "textBox11";
            textBox11.ReadOnly = true;
            textBox11.Size = new Size(150, 25);
            textBox11.TabIndex = 34;
            textBox11.Text = "Количество скачиваний";
            // 
            // textBox10
            // 
            textBox10.Location = new Point(3, 34);
            textBox10.Name = "textBox10";
            textBox10.ReadOnly = true;
            textBox10.Size = new Size(150, 25);
            textBox10.TabIndex = 33;
            textBox10.Text = "Отступ до скачки";
            // 
            // tbCountDownload
            // 
            tbCountDownload.Location = new Point(3, 127);
            tbCountDownload.Name = "tbCountDownload";
            tbCountDownload.Size = new Size(150, 25);
            tbCountDownload.TabIndex = 32;
            tbCountDownload.Text = "0";
            // 
            // tbShiftDownload
            // 
            tbShiftDownload.Location = new Point(3, 65);
            tbShiftDownload.Name = "tbShiftDownload";
            tbShiftDownload.Size = new Size(150, 25);
            tbShiftDownload.TabIndex = 31;
            tbShiftDownload.Text = "0";
            // 
            // textBox6
            // 
            textBox6.Location = new Point(3, 3);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(150, 25);
            textBox6.TabIndex = 30;
            textBox6.Text = "VK";
            // 
            // bBackgroundImageCopyOff
            // 
            bBackgroundImageCopyOff.Enabled = false;
            bBackgroundImageCopyOff.Location = new Point(3, 86);
            bBackgroundImageCopyOff.Name = "bBackgroundImageCopyOff";
            bBackgroundImageCopyOff.Size = new Size(150, 43);
            bBackgroundImageCopyOff.TabIndex = 37;
            bBackgroundImageCopyOff.Text = "Выключить";
            bBackgroundImageCopyOff.UseVisualStyleBackColor = true;
            bBackgroundImageCopyOff.Click += bBackgroundImageCopyOff_Click;
            // 
            // bBackgroundImageCopyOn
            // 
            bBackgroundImageCopyOn.Location = new Point(3, 34);
            bBackgroundImageCopyOn.Name = "bBackgroundImageCopyOn";
            bBackgroundImageCopyOn.Size = new Size(150, 43);
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
            textBox1.Size = new Size(150, 25);
            textBox1.TabIndex = 38;
            textBox1.Text = "Фоновое копирование";
            // 
            // pVK
            // 
            pVK.Controls.Add(textBox6);
            pVK.Controls.Add(tbShiftDownload);
            pVK.Controls.Add(tbCountDownload);
            pVK.Controls.Add(textBox10);
            pVK.Controls.Add(textBox11);
            pVK.Controls.Add(bDownloadPhotosVK);
            pVK.Location = new Point(188, 101);
            pVK.Name = "pVK";
            pVK.Size = new Size(156, 200);
            pVK.TabIndex = 39;
            // 
            // pBackgroundImageCopy
            // 
            pBackgroundImageCopy.Controls.Add(textBox1);
            pBackgroundImageCopy.Controls.Add(bBackgroundImageCopyOn);
            pBackgroundImageCopy.Controls.Add(bBackgroundImageCopyOff);
            pBackgroundImageCopy.Location = new Point(188, 101);
            pBackgroundImageCopy.Name = "pBackgroundImageCopy";
            pBackgroundImageCopy.Size = new Size(156, 200);
            pBackgroundImageCopy.TabIndex = 40;
            // 
            // pDanbooru
            // 
            pDanbooru.Controls.Add(textBox2);
            pDanbooru.Location = new Point(188, 101);
            pDanbooru.Name = "pDanbooru";
            pDanbooru.Size = new Size(156, 200);
            pDanbooru.TabIndex = 41;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(3, 3);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(150, 25);
            textBox2.TabIndex = 36;
            textBox2.Text = "Danbooru";
            // 
            // tBackgroundImageCopy
            // 
            tBackgroundImageCopy.Interval = 1000;
            tBackgroundImageCopy.Tick += tBackgroundImageCopy_Tick;
            // 
            // bBackgroundImageCopy
            // 
            bBackgroundImageCopy.Location = new Point(12, 101);
            bBackgroundImageCopy.Name = "bBackgroundImageCopy";
            bBackgroundImageCopy.Size = new Size(150, 43);
            bBackgroundImageCopy.TabIndex = 39;
            bBackgroundImageCopy.Text = "Фоновое копирование";
            bBackgroundImageCopy.UseVisualStyleBackColor = true;
            bBackgroundImageCopy.Click += bBackgroundImageCopy_Click;
            // 
            // bVK
            // 
            bVK.Location = new Point(12, 150);
            bVK.Name = "bVK";
            bVK.Size = new Size(150, 43);
            bVK.TabIndex = 42;
            bVK.Text = "VK";
            bVK.UseVisualStyleBackColor = true;
            bVK.Click += bVK_Click;
            // 
            // bDanbooru
            // 
            bDanbooru.Location = new Point(12, 199);
            bDanbooru.Name = "bDanbooru";
            bDanbooru.Size = new Size(150, 43);
            bDanbooru.TabIndex = 43;
            bDanbooru.Text = "Danbooru";
            bDanbooru.UseVisualStyleBackColor = true;
            bDanbooru.Click += bDanbooru_Click;
            // 
            // DownloaderDataSetPhoto
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(360, 365);
            Controls.Add(bDanbooru);
            Controls.Add(bVK);
            Controls.Add(bBackgroundImageCopy);
            Controls.Add(pDanbooru);
            Controls.Add(pBackgroundImageCopy);
            Controls.Add(pVK);
            Controls.Add(textBox3);
            Controls.Add(tbTag);
            Name = "DownloaderDataSetPhoto";
            Text = "DownloaderDataSetPhoto";
            pVK.ResumeLayout(false);
            pVK.PerformLayout();
            pBackgroundImageCopy.ResumeLayout(false);
            pBackgroundImageCopy.PerformLayout();
            pDanbooru.ResumeLayout(false);
            pDanbooru.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbTag;
        private TextBox textBox3;
        private Button bDownloadPhotosVK;
        private TextBox textBox11;
        private TextBox textBox10;
        private TextBox tbCountDownload;
        private TextBox tbShiftDownload;
        private TextBox textBox6;
        private Button bBackgroundImageCopyOff;
        private Button bBackgroundImageCopyOn;
        private TextBox textBox1;
        private Panel pVK;
        private Panel pBackgroundImageCopy;
        private Panel pDanbooru;
        private TextBox textBox2;
        private System.Windows.Forms.Timer tBackgroundImageCopy;
        private Button bBackgroundImageCopy;
        private Button bVK;
        private Button bDanbooru;
    }
}
