namespace AddPost
{
    partial class AddPost
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
            bDataSet = new Button();
            textBox7 = new TextBox();
            tbNeuralNetworkResult = new TextBox();
            bImageLeft = new Button();
            bImageRight = new Button();
            tbImageIndex = new TextBox();
            bImageDelete = new Button();
            bTbTagFix = new Button();
            index = new DataGridViewTextBoxColumn();
            tag = new DataGridViewTextBoxColumn();
            gelbooru = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)pbImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDictionary).BeginInit();
            SuspendLayout();
            // 
            // pbImage
            // 
            pbImage.BorderStyle = BorderStyle.FixedSingle;
            pbImage.Location = new Point(12, 39);
            pbImage.Name = "pbImage";
            pbImage.Size = new Size(600, 530);
            pbImage.SizeMode = PictureBoxSizeMode.Zoom;
            pbImage.TabIndex = 0;
            pbImage.TabStop = false;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(623, 39);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(110, 23);
            textBox1.TabIndex = 1;
            textBox1.Text = "Ссылка";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(623, 11);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(110, 23);
            textBox2.TabIndex = 2;
            textBox2.Text = "Время";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(623, 94);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(110, 23);
            textBox3.TabIndex = 3;
            textBox3.Text = "Теги";
            // 
            // tbUrl
            // 
            tbUrl.Location = new Point(748, 39);
            tbUrl.Multiline = true;
            tbUrl.Name = "tbUrl";
            tbUrl.Size = new Size(719, 46);
            tbUrl.TabIndex = 6;
            // 
            // tbDate
            // 
            tbDate.Location = new Point(748, 11);
            tbDate.Name = "tbDate";
            tbDate.ReadOnly = true;
            tbDate.Size = new Size(142, 23);
            tbDate.TabIndex = 5;
            // 
            // tbTag
            // 
            tbTag.Location = new Point(748, 94);
            tbTag.Multiline = true;
            tbTag.Name = "tbTag";
            tbTag.Size = new Size(603, 46);
            tbTag.TabIndex = 4;
            tbTag.KeyUp += tbTag_KeyUp;
            // 
            // bSend
            // 
            bSend.Location = new Point(623, 574);
            bSend.Name = "bSend";
            bSend.Size = new Size(110, 38);
            bSend.TabIndex = 7;
            bSend.Text = "Отправка";
            bSend.UseVisualStyleBackColor = true;
            bSend.Click += bSend_Click;
            // 
            // textBox8
            // 
            textBox8.Location = new Point(623, 145);
            textBox8.Name = "textBox8";
            textBox8.ReadOnly = true;
            textBox8.Size = new Size(110, 23);
            textBox8.TabIndex = 9;
            textBox8.Text = "Словарь";
            // 
            // bBuff
            // 
            bBuff.Location = new Point(623, 530);
            bBuff.Name = "bBuff";
            bBuff.Size = new Size(110, 38);
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
            cbClear1.Location = new Point(623, 70);
            cbClear1.Name = "cbClear1";
            cbClear1.Size = new Size(72, 19);
            cbClear1.TabIndex = 11;
            cbClear1.Text = "Очистка";
            cbClear1.UseVisualStyleBackColor = true;
            // 
            // cbClear2
            // 
            cbClear2.AutoSize = true;
            cbClear2.Checked = true;
            cbClear2.CheckState = CheckState.Checked;
            cbClear2.Location = new Point(623, 121);
            cbClear2.Name = "cbClear2";
            cbClear2.Size = new Size(72, 19);
            cbClear2.TabIndex = 13;
            cbClear2.Text = "Очистка";
            cbClear2.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(896, 11);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(142, 23);
            textBox4.TabIndex = 14;
            textBox4.Text = "Время между постами";
            // 
            // cbTimeBetweenPost
            // 
            cbTimeBetweenPost.FormattingEnabled = true;
            cbTimeBetweenPost.Items.AddRange(new object[] { "1ч", "2ч", "3ч", "4ч", "5ч", "6ч", "7ч", "8ч", "9ч", "10ч", "11ч", "12ч", "13ч", "14ч", "15ч", "16ч", "17ч", "18ч", "19ч", "20ч", "21ч", "22ч", "23ч", "24ч" });
            cbTimeBetweenPost.Location = new Point(1044, 11);
            cbTimeBetweenPost.Name = "cbTimeBetweenPost";
            cbTimeBetweenPost.Size = new Size(142, 23);
            cbTimeBetweenPost.TabIndex = 15;
            cbTimeBetweenPost.SelectedIndexChanged += cbTimeBetweenPost_SelectedIndexChanged;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(1192, 11);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(91, 23);
            textBox5.TabIndex = 16;
            textBox5.Text = "Id группы";
            // 
            // tbGroupId
            // 
            tbGroupId.Location = new Point(1289, 11);
            tbGroupId.Name = "tbGroupId";
            tbGroupId.Size = new Size(91, 23);
            tbGroupId.TabIndex = 17;
            tbGroupId.Text = "220199532";
            tbGroupId.Leave += tbGroupId_Leave;
            // 
            // dgvDictionary
            // 
            dgvDictionary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDictionary.Columns.AddRange(new DataGridViewColumn[] { index, tag, gelbooru });
            dgvDictionary.Location = new Point(748, 145);
            dgvDictionary.Name = "dgvDictionary";
            dgvDictionary.RowHeadersWidth = 45;
            dgvDictionary.Size = new Size(719, 467);
            dgvDictionary.TabIndex = 18;
            dgvDictionary.CellMouseClick += dgvDictionary_CellMouseClick;
            // 
            // bDataSet
            // 
            bDataSet.Location = new Point(623, 487);
            bDataSet.Name = "bDataSet";
            bDataSet.Size = new Size(110, 38);
            bDataSet.TabIndex = 19;
            bDataSet.Text = "Отправить в дата сет";
            bDataSet.UseVisualStyleBackColor = true;
            bDataSet.Click += bDataSet_Click;
            // 
            // textBox7
            // 
            textBox7.Location = new Point(12, 11);
            textBox7.Name = "textBox7";
            textBox7.ReadOnly = true;
            textBox7.Size = new Size(130, 23);
            textBox7.TabIndex = 30;
            textBox7.Text = "Результат нейронки";
            // 
            // tbNeuralNetworkResult
            // 
            tbNeuralNetworkResult.Location = new Point(148, 11);
            tbNeuralNetworkResult.Name = "tbNeuralNetworkResult";
            tbNeuralNetworkResult.ReadOnly = true;
            tbNeuralNetworkResult.Size = new Size(464, 23);
            tbNeuralNetworkResult.TabIndex = 31;
            // 
            // bImageLeft
            // 
            bImageLeft.Font = new Font("Segoe UI", 20F);
            bImageLeft.Location = new Point(177, 574);
            bImageLeft.Name = "bImageLeft";
            bImageLeft.Size = new Size(110, 38);
            bImageLeft.TabIndex = 32;
            bImageLeft.Text = "<-----";
            bImageLeft.UseVisualStyleBackColor = true;
            bImageLeft.Click += bImageLeft_Click;
            // 
            // bImageRight
            // 
            bImageRight.Font = new Font("Segoe UI", 20F);
            bImageRight.Location = new Point(349, 574);
            bImageRight.Name = "bImageRight";
            bImageRight.Size = new Size(110, 38);
            bImageRight.TabIndex = 33;
            bImageRight.Text = "----->\r\n";
            bImageRight.UseVisualStyleBackColor = true;
            bImageRight.Click += bImageRight_Click;
            // 
            // tbImageIndex
            // 
            tbImageIndex.Location = new Point(293, 582);
            tbImageIndex.Name = "tbImageIndex";
            tbImageIndex.ReadOnly = true;
            tbImageIndex.Size = new Size(50, 23);
            tbImageIndex.TabIndex = 34;
            tbImageIndex.TextAlign = HorizontalAlignment.Center;
            // 
            // bImageDelete
            // 
            bImageDelete.Font = new Font("Segoe UI", 8.830189F);
            bImageDelete.Location = new Point(12, 574);
            bImageDelete.Name = "bImageDelete";
            bImageDelete.Size = new Size(110, 38);
            bImageDelete.TabIndex = 35;
            bImageDelete.Text = "Удалить ";
            bImageDelete.UseVisualStyleBackColor = true;
            bImageDelete.Click += bImageDelete_Click;
            // 
            // bTbTagFix
            // 
            bTbTagFix.Location = new Point(1357, 94);
            bTbTagFix.Name = "bTbTagFix";
            bTbTagFix.Size = new Size(110, 46);
            bTbTagFix.TabIndex = 36;
            bTbTagFix.Text = "Исправление";
            bTbTagFix.UseVisualStyleBackColor = true;
            bTbTagFix.Click += bTbTagFix_Click;
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
            // AddPost
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(1478, 620);
            Controls.Add(bTbTagFix);
            Controls.Add(bImageDelete);
            Controls.Add(tbImageIndex);
            Controls.Add(bImageRight);
            Controls.Add(bImageLeft);
            Controls.Add(tbNeuralNetworkResult);
            Controls.Add(textBox7);
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
            Name = "AddPost";
            Text = "AddPost";
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
        private TextBox textBox7;
        private TextBox tbNeuralNetworkResult;
        private Button bImageLeft;
        private Button bImageRight;
        private TextBox tbImageIndex;
        private Button bImageDelete;
        private Button bTbTagFix;
        private DataGridViewTextBoxColumn index;
        private DataGridViewTextBoxColumn tag;
        private DataGridViewTextBoxColumn gelbooru;
    }
}