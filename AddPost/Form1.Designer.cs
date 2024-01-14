namespace AddPost
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
            components = new System.ComponentModel.Container();
            pbImage = new PictureBox();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            tbUrl = new TextBox();
            tbDate = new TextBox();
            tbTag = new TextBox();
            bSend = new Button();
            textBox8 = new TextBox();
            bBuff = new Button();
            cbClear1 = new CheckBox();
            cbClear2 = new CheckBox();
            textBox4 = new TextBox();
            cbTimeBetweenPost = new ComboBox();
            textBox5 = new TextBox();
            tbGroupId = new TextBox();
            dgvDictionary = new DataGridView();
            tags = new DataGridViewTextBoxColumn();
            bDataSet = new Button();
            bBackgroundImageCopyOn = new Button();
            bBackgroundImageCopyOff = new Button();
            tBackgroundImageCopy = new System.Windows.Forms.Timer(components);
            tbPercentOriginalTag = new TextBox();
            cbPercentOriginalTag = new ComboBox();
            textBox6 = new TextBox();
            tbShiftDownload = new TextBox();
            tbCountDownload = new TextBox();
            textBox10 = new TextBox();
            textBox11 = new TextBox();
            bDownloadPhotos = new Button();
            ((System.ComponentModel.ISupportInitialize)pbImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDictionary).BeginInit();
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
            // 
            // textBox1
            // 
            textBox1.Location = new Point(623, 44);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(110, 25);
            textBox1.TabIndex = 1;
            textBox1.Text = "Ссылка";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(623, 13);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(110, 25);
            textBox2.TabIndex = 2;
            textBox2.Text = "Время";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(623, 106);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(110, 25);
            textBox3.TabIndex = 3;
            textBox3.Text = "Тэги";
            // 
            // tbUrl
            // 
            tbUrl.Location = new Point(748, 44);
            tbUrl.Multiline = true;
            tbUrl.Name = "tbUrl";
            tbUrl.Size = new Size(719, 56);
            tbUrl.TabIndex = 6;
            // 
            // tbDate
            // 
            tbDate.Location = new Point(748, 13);
            tbDate.Name = "tbDate";
            tbDate.ReadOnly = true;
            tbDate.Size = new Size(132, 25);
            tbDate.TabIndex = 5;
            // 
            // tbTag
            // 
            tbTag.Location = new Point(748, 106);
            tbTag.Multiline = true;
            tbTag.Name = "tbTag";
            tbTag.Size = new Size(719, 87);
            tbTag.TabIndex = 4;
            tbTag.KeyUp += tbTag_KeyUp;
            // 
            // bSend
            // 
            bSend.Location = new Point(623, 569);
            bSend.Name = "bSend";
            bSend.Size = new Size(110, 43);
            bSend.TabIndex = 7;
            bSend.Text = "Отправка";
            bSend.UseVisualStyleBackColor = true;
            bSend.Click += bSend_Click;
            // 
            // textBox8
            // 
            textBox8.Location = new Point(623, 199);
            textBox8.Name = "textBox8";
            textBox8.ReadOnly = true;
            textBox8.Size = new Size(110, 25);
            textBox8.TabIndex = 9;
            textBox8.Text = "Словарь";
            // 
            // bBuff
            // 
            bBuff.Location = new Point(623, 520);
            bBuff.Name = "bBuff";
            bBuff.Size = new Size(110, 43);
            bBuff.TabIndex = 10;
            bBuff.Text = "Вставка из буфера";
            bBuff.UseVisualStyleBackColor = true;
            bBuff.Click += bBuff_Click;
            // 
            // cbClear1
            // 
            cbClear1.AutoSize = true;
            cbClear1.Checked = true;
            cbClear1.CheckState = CheckState.Checked;
            cbClear1.Location = new Point(623, 79);
            cbClear1.Name = "cbClear1";
            cbClear1.Size = new Size(75, 21);
            cbClear1.TabIndex = 11;
            cbClear1.Text = "Очистка";
            cbClear1.UseVisualStyleBackColor = true;
            // 
            // cbClear2
            // 
            cbClear2.AutoSize = true;
            cbClear2.Checked = true;
            cbClear2.CheckState = CheckState.Checked;
            cbClear2.Location = new Point(623, 137);
            cbClear2.Name = "cbClear2";
            cbClear2.Size = new Size(75, 21);
            cbClear2.TabIndex = 13;
            cbClear2.Text = "Очистка";
            cbClear2.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(886, 13);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(142, 25);
            textBox4.TabIndex = 14;
            textBox4.Text = "Время между постами";
            // 
            // cbTimeBetweenPost
            // 
            cbTimeBetweenPost.FormattingEnabled = true;
            cbTimeBetweenPost.Items.AddRange(new object[] { "1ч", "2ч", "3ч", "4ч", "5ч", "6ч", "7ч", "8ч", "9ч", "10ч", "11ч", "12ч", "13ч", "14ч", "15ч", "16ч", "17ч", "18ч", "19ч", "20ч", "21ч", "22ч", "23ч", "24ч" });
            cbTimeBetweenPost.Location = new Point(1034, 13);
            cbTimeBetweenPost.Name = "cbTimeBetweenPost";
            cbTimeBetweenPost.Size = new Size(134, 25);
            cbTimeBetweenPost.TabIndex = 15;
            cbTimeBetweenPost.SelectedIndexChanged += cbTimeBetweenPost_SelectedIndexChanged;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(1174, 12);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(69, 25);
            textBox5.TabIndex = 16;
            textBox5.Text = "Id группы";
            // 
            // tbGroupId
            // 
            tbGroupId.Location = new Point(1243, 13);
            tbGroupId.Name = "tbGroupId";
            tbGroupId.Size = new Size(86, 25);
            tbGroupId.TabIndex = 17;
            tbGroupId.Text = "220199532";
            tbGroupId.Leave += tbGroupId_Leave;
            // 
            // dgvDictionary
            // 
            dgvDictionary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDictionary.Columns.AddRange(new DataGridViewColumn[] { tags });
            dgvDictionary.Location = new Point(748, 199);
            dgvDictionary.Name = "dgvDictionary";
            dgvDictionary.RowHeadersWidth = 45;
            dgvDictionary.Size = new Size(719, 413);
            dgvDictionary.TabIndex = 18;
            dgvDictionary.CellContentClick += dgvDictionary_CellContentClick;
            dgvDictionary.CellMouseClick += dgvDictionary_CellMouseClick;
            // 
            // tags
            // 
            tags.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tags.HeaderText = "Теги";
            tags.MinimumWidth = 6;
            tags.Name = "tags";
            tags.ReadOnly = true;
            // 
            // bDataSet
            // 
            bDataSet.Location = new Point(623, 471);
            bDataSet.Name = "bDataSet";
            bDataSet.Size = new Size(110, 43);
            bDataSet.TabIndex = 19;
            bDataSet.Text = "Отправить в дата сет";
            bDataSet.UseVisualStyleBackColor = true;
            bDataSet.Click += bDataSet_Click;
            // 
            // bBackgroundImageCopyOn
            // 
            bBackgroundImageCopyOn.Location = new Point(623, 230);
            bBackgroundImageCopyOn.Name = "bBackgroundImageCopyOn";
            bBackgroundImageCopyOn.Size = new Size(110, 43);
            bBackgroundImageCopyOn.TabIndex = 20;
            bBackgroundImageCopyOn.Text = "Включить фоновое копирование";
            bBackgroundImageCopyOn.UseVisualStyleBackColor = true;
            bBackgroundImageCopyOn.Click += bBackgroundImageCopyOn_Click;
            // 
            // bBackgroundImageCopyOff
            // 
            bBackgroundImageCopyOff.Enabled = false;
            bBackgroundImageCopyOff.Location = new Point(623, 279);
            bBackgroundImageCopyOff.Name = "bBackgroundImageCopyOff";
            bBackgroundImageCopyOff.Size = new Size(110, 43);
            bBackgroundImageCopyOff.TabIndex = 21;
            bBackgroundImageCopyOff.Text = "Выключить фоновое копирование";
            bBackgroundImageCopyOff.UseVisualStyleBackColor = true;
            bBackgroundImageCopyOff.Click += bBackgroundImageCopyOff_Click;
            // 
            // tBackgroundImageCopy
            // 
            tBackgroundImageCopy.Interval = 1000;
            tBackgroundImageCopy.Tick += tBackgroundImageCopy_Tick;
            // 
            // tbPercentOriginalTag
            // 
            tbPercentOriginalTag.Location = new Point(623, 328);
            tbPercentOriginalTag.Multiline = true;
            tbPercentOriginalTag.Name = "tbPercentOriginalTag";
            tbPercentOriginalTag.Size = new Size(110, 39);
            tbPercentOriginalTag.TabIndex = 22;
            tbPercentOriginalTag.Text = "Сходство ниже #Original";
            tbPercentOriginalTag.TextAlign = HorizontalAlignment.Center;
            tbPercentOriginalTag.UseSystemPasswordChar = true;
            // 
            // cbPercentOriginalTag
            // 
            cbPercentOriginalTag.FormattingEnabled = true;
            cbPercentOriginalTag.Items.AddRange(new object[] { "10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%" });
            cbPercentOriginalTag.Location = new Point(623, 373);
            cbPercentOriginalTag.Name = "cbPercentOriginalTag";
            cbPercentOriginalTag.Size = new Size(110, 25);
            cbPercentOriginalTag.TabIndex = 23;
            cbPercentOriginalTag.SelectedIndexChanged += cbPercentOriginalTag_SelectedIndexChanged;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(1473, 13);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(150, 25);
            textBox6.TabIndex = 24;
            textBox6.Text = "VK";
            // 
            // tbShiftDownload
            // 
            tbShiftDownload.Location = new Point(1473, 75);
            tbShiftDownload.Name = "tbShiftDownload";
            tbShiftDownload.Size = new Size(150, 25);
            tbShiftDownload.TabIndex = 25;
            tbShiftDownload.Text = "0";
            // 
            // tbCountDownload
            // 
            tbCountDownload.Location = new Point(1473, 137);
            tbCountDownload.Name = "tbCountDownload";
            tbCountDownload.Size = new Size(150, 25);
            tbCountDownload.TabIndex = 26;
            tbCountDownload.Text = "0";
            // 
            // textBox10
            // 
            textBox10.Location = new Point(1473, 44);
            textBox10.Name = "textBox10";
            textBox10.ReadOnly = true;
            textBox10.Size = new Size(150, 25);
            textBox10.TabIndex = 27;
            textBox10.Text = "Отступ до скачки";
            // 
            // textBox11
            // 
            textBox11.Location = new Point(1473, 106);
            textBox11.Name = "textBox11";
            textBox11.ReadOnly = true;
            textBox11.Size = new Size(150, 25);
            textBox11.TabIndex = 28;
            textBox11.Text = "Количество скачиваний";
            // 
            // bDownloadPhotos
            // 
            bDownloadPhotos.Location = new Point(1473, 168);
            bDownloadPhotos.Name = "bDownloadPhotos";
            bDownloadPhotos.Size = new Size(150, 25);
            bDownloadPhotos.TabIndex = 29;
            bDownloadPhotos.Text = "Скачать";
            bDownloadPhotos.UseVisualStyleBackColor = true;
            bDownloadPhotos.Click += bDownloadPhotos_Click;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1632, 629);
            Controls.Add(bDownloadPhotos);
            Controls.Add(textBox11);
            Controls.Add(textBox10);
            Controls.Add(tbCountDownload);
            Controls.Add(tbShiftDownload);
            Controls.Add(textBox6);
            Controls.Add(cbPercentOriginalTag);
            Controls.Add(tbPercentOriginalTag);
            Controls.Add(bBackgroundImageCopyOff);
            Controls.Add(bBackgroundImageCopyOn);
            Controls.Add(bDataSet);
            Controls.Add(dgvDictionary);
            Controls.Add(tbGroupId);
            Controls.Add(textBox5);
            Controls.Add(cbTimeBetweenPost);
            Controls.Add(textBox4);
            Controls.Add(cbClear2);
            Controls.Add(cbClear1);
            Controls.Add(bBuff);
            Controls.Add(textBox8);
            Controls.Add(bSend);
            Controls.Add(tbUrl);
            Controls.Add(tbDate);
            Controls.Add(tbTag);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(pbImage);
            MaximizeBox = false;
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            ((System.ComponentModel.ISupportInitialize)pbImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvDictionary).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbImage;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox tbUrl;
        private TextBox tbDate;
        private TextBox tbTag;
        private Button bSend;
        private TextBox textBox8;
        private Button bBuff;
        private CheckBox cbClear1;
        private CheckBox cbClear2;
        private TextBox textBox4;
        private ComboBox cbTimeBetweenPost;
        private TextBox textBox5;
        private TextBox tbGroupId;
        private DataGridView dgvDictionary;
        private Button bDataSet;
        private DataGridViewTextBoxColumn tags;
        private Button bBackgroundImageCopyOn;
        private Button bBackgroundImageCopyOff;
        private System.Windows.Forms.Timer tBackgroundImageCopy;
        private TextBox tbPercentOriginalTag;
        private ComboBox cbPercentOriginalTag;
        private TextBox textBox6;
        private TextBox tbShiftDownload;
        private TextBox tbCountDownload;
        private TextBox textBox10;
        private TextBox textBox11;
        private Button bDownloadPhotos;
    }
}