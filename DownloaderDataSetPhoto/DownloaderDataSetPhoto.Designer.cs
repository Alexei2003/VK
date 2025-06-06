﻿namespace DownloaderDataSetPhoto
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
            pGelbooru = new Panel();
            tbGelbooruUrl = new TextBox();
            textBox4 = new TextBox();
            bDownloadPhotosGelbooru = new Button();
            textBox2 = new TextBox();
            tBackgroundImageCopy = new System.Windows.Forms.Timer(components);
            bBackgroundImageCopy = new Button();
            bVK = new Button();
            bGelbooru = new Button();
            pVK.SuspendLayout();
            pBackgroundImageCopy.SuspendLayout();
            pGelbooru.SuspendLayout();
            SuspendLayout();
            // 
            // tbTag
            // 
            tbTag.Location = new Point(12, 38);
            tbTag.Multiline = true;
            tbTag.Name = "tbTag";
            tbTag.Size = new Size(332, 46);
            tbTag.TabIndex = 5;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(12, 11);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(55, 23);
            textBox3.TabIndex = 6;
            textBox3.Text = "Теги";
            // 
            // bDownloadPhotosVK
            // 
            bDownloadPhotosVK.Location = new Point(3, 139);
            bDownloadPhotosVK.Name = "bDownloadPhotosVK";
            bDownloadPhotosVK.Size = new Size(150, 22);
            bDownloadPhotosVK.TabIndex = 35;
            bDownloadPhotosVK.Text = "Скачать";
            bDownloadPhotosVK.UseVisualStyleBackColor = true;
            bDownloadPhotosVK.Click += bDownloadPhotosVK_Click;
            // 
            // textBox11
            // 
            textBox11.Location = new Point(3, 85);
            textBox11.Name = "textBox11";
            textBox11.ReadOnly = true;
            textBox11.Size = new Size(150, 23);
            textBox11.TabIndex = 34;
            textBox11.Text = "Количество скачиваний";
            // 
            // textBox10
            // 
            textBox10.Location = new Point(3, 30);
            textBox10.Name = "textBox10";
            textBox10.ReadOnly = true;
            textBox10.Size = new Size(150, 23);
            textBox10.TabIndex = 33;
            textBox10.Text = "Отступ до скачки";
            // 
            // tbCountDownload
            // 
            tbCountDownload.Location = new Point(3, 112);
            tbCountDownload.Name = "tbCountDownload";
            tbCountDownload.Size = new Size(150, 23);
            tbCountDownload.TabIndex = 32;
            tbCountDownload.Text = "0";
            // 
            // tbShiftDownload
            // 
            tbShiftDownload.Location = new Point(3, 57);
            tbShiftDownload.Name = "tbShiftDownload";
            tbShiftDownload.Size = new Size(150, 23);
            tbShiftDownload.TabIndex = 31;
            tbShiftDownload.Text = "0";
            // 
            // textBox6
            // 
            textBox6.Location = new Point(3, 3);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(150, 23);
            textBox6.TabIndex = 30;
            textBox6.Text = "VK";
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
            bBackgroundImageCopyOn.Location = new Point(3, 30);
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
            // pVK
            // 
            pVK.BorderStyle = BorderStyle.FixedSingle;
            pVK.Controls.Add(textBox6);
            pVK.Controls.Add(tbShiftDownload);
            pVK.Controls.Add(tbCountDownload);
            pVK.Controls.Add(textBox10);
            pVK.Controls.Add(textBox11);
            pVK.Controls.Add(bDownloadPhotosVK);
            pVK.Location = new Point(188, 89);
            pVK.Name = "pVK";
            pVK.Size = new Size(158, 177);
            pVK.TabIndex = 39;
            // 
            // pBackgroundImageCopy
            // 
            pBackgroundImageCopy.BorderStyle = BorderStyle.FixedSingle;
            pBackgroundImageCopy.Controls.Add(textBox1);
            pBackgroundImageCopy.Controls.Add(bBackgroundImageCopyOn);
            pBackgroundImageCopy.Controls.Add(bBackgroundImageCopyOff);
            pBackgroundImageCopy.Location = new Point(188, 89);
            pBackgroundImageCopy.Name = "pBackgroundImageCopy";
            pBackgroundImageCopy.Size = new Size(158, 177);
            pBackgroundImageCopy.TabIndex = 40;
            // 
            // pGelbooru
            // 
            pGelbooru.BorderStyle = BorderStyle.FixedSingle;
            pGelbooru.Controls.Add(tbGelbooruUrl);
            pGelbooru.Controls.Add(textBox4);
            pGelbooru.Controls.Add(bDownloadPhotosGelbooru);
            pGelbooru.Controls.Add(textBox2);
            pGelbooru.Location = new Point(188, 89);
            pGelbooru.Name = "pGelbooru";
            pGelbooru.Size = new Size(158, 177);
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
            bDownloadPhotosGelbooru.Location = new Point(3, 139);
            bDownloadPhotosGelbooru.Name = "bDownloadPhotosGelbooru";
            bDownloadPhotosGelbooru.Size = new Size(150, 22);
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
            bBackgroundImageCopy.Location = new Point(12, 89);
            bBackgroundImageCopy.Name = "bBackgroundImageCopy";
            bBackgroundImageCopy.Size = new Size(150, 38);
            bBackgroundImageCopy.TabIndex = 39;
            bBackgroundImageCopy.Text = "Фоновое копирование";
            bBackgroundImageCopy.UseVisualStyleBackColor = true;
            bBackgroundImageCopy.Click += bBackgroundImageCopy_Click;
            // 
            // bVK
            // 
            bVK.Location = new Point(12, 132);
            bVK.Name = "bVK";
            bVK.Size = new Size(150, 38);
            bVK.TabIndex = 42;
            bVK.Text = "VK";
            bVK.UseVisualStyleBackColor = true;
            bVK.Click += bVK_Click;
            // 
            // bGelbooru
            // 
            bGelbooru.Location = new Point(12, 176);
            bGelbooru.Name = "bGelbooru";
            bGelbooru.Size = new Size(150, 38);
            bGelbooru.TabIndex = 43;
            bGelbooru.Text = "Gelbooru";
            bGelbooru.UseVisualStyleBackColor = true;
            bGelbooru.Click += bGelbooru_Click;
            // 
            // DownloaderDataSetPhoto
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(360, 277);
            Controls.Add(bGelbooru);
            Controls.Add(bVK);
            Controls.Add(bBackgroundImageCopy);
            Controls.Add(textBox3);
            Controls.Add(tbTag);
            Controls.Add(pGelbooru);
            Controls.Add(pBackgroundImageCopy);
            Controls.Add(pVK);
            Name = "DownloaderDataSetPhoto";
            Text = "DownloaderDataSetPhoto";
            pVK.ResumeLayout(false);
            pVK.PerformLayout();
            pBackgroundImageCopy.ResumeLayout(false);
            pBackgroundImageCopy.PerformLayout();
            pGelbooru.ResumeLayout(false);
            pGelbooru.PerformLayout();
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
        private Panel pGelbooru;
        private TextBox textBox2;
        private System.Windows.Forms.Timer tBackgroundImageCopy;
        private Button bBackgroundImageCopy;
        private Button bVK;
        private Button bGelbooru;
        private Button bDownloadPhotosGelbooru;
        private TextBox textBox4;
        private TextBox tbGelbooruUrl;
    }
}
