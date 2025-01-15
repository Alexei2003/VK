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
            index = new DataGridViewTextBoxColumn();
            tags = new DataGridViewTextBoxColumn();
            bDataSet = new Button();
            textBox7 = new TextBox();
            tbNeuralNetworkResult = new TextBox();
            bImageLeft = new Button();
            bImageRight = new Button();
            tbImageIndex = new TextBox();
            bImageDelete = new Button();
            bTbTagFix = new Button();
            ((System.ComponentModel.ISupportInitialize)pbImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvDictionary).BeginInit();
            SuspendLayout();
            // 
            // pbImage
            // 
            pbImage.BorderStyle = BorderStyle.FixedSingle;
            pbImage.Location = new Point(12, 44);
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
            textBox3.Text = "Теги";
            // 
            // tbUrl
            // 
            tbUrl.Location = new Point(748, 44);
            tbUrl.Multiline = true;
            tbUrl.Name = "tbUrl";
            tbUrl.Size = new Size(719, 52);
            tbUrl.TabIndex = 6;
            // 
            // tbDate
            // 
            tbDate.Location = new Point(748, 13);
            tbDate.Name = "tbDate";
            tbDate.ReadOnly = true;
            tbDate.Size = new Size(142, 25);
            tbDate.TabIndex = 5;
            // 
            // tbTag
            // 
            tbTag.Location = new Point(748, 106);
            tbTag.Multiline = true;
            tbTag.Name = "tbTag";
            tbTag.Size = new Size(603, 52);
            tbTag.TabIndex = 4;
            tbTag.KeyUp += tbTag_KeyUp;
            // 
            // bSend
            // 
            bSend.Location = new Point(623, 650);
            bSend.Name = "bSend";
            bSend.Size = new Size(110, 43);
            bSend.TabIndex = 7;
            bSend.Text = "Отправка";
            bSend.UseVisualStyleBackColor = true;
            bSend.Click += bSend_Click;
            // 
            // textBox8
            // 
            textBox8.Location = new Point(623, 164);
            textBox8.Name = "textBox8";
            textBox8.ReadOnly = true;
            textBox8.Size = new Size(110, 25);
            textBox8.TabIndex = 9;
            textBox8.Text = "Словарь";
            // 
            // bBuff
            // 
            bBuff.Location = new Point(623, 601);
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
            textBox4.Location = new Point(896, 13);
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
            cbTimeBetweenPost.Location = new Point(1044, 13);
            cbTimeBetweenPost.Name = "cbTimeBetweenPost";
            cbTimeBetweenPost.Size = new Size(142, 25);
            cbTimeBetweenPost.TabIndex = 15;
            cbTimeBetweenPost.SelectedIndexChanged += cbTimeBetweenPost_SelectedIndexChanged;
            // 
            // textBox5
            // 
            textBox5.Location = new Point(1192, 12);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(91, 25);
            textBox5.TabIndex = 16;
            textBox5.Text = "Id группы";
            // 
            // tbGroupId
            // 
            tbGroupId.Location = new Point(1289, 13);
            tbGroupId.Name = "tbGroupId";
            tbGroupId.Size = new Size(91, 25);
            tbGroupId.TabIndex = 17;
            tbGroupId.Text = "220199532";
            tbGroupId.Leave += tbGroupId_Leave;
            // 
            // dgvDictionary
            // 
            dgvDictionary.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDictionary.Columns.AddRange(new DataGridViewColumn[] { index, tags });
            dgvDictionary.Location = new Point(748, 164);
            dgvDictionary.Name = "dgvDictionary";
            dgvDictionary.RowHeadersWidth = 45;
            dgvDictionary.Size = new Size(719, 529);
            dgvDictionary.TabIndex = 18;
            dgvDictionary.CellContentClick += dgvDictionary_CellContentClick;
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
            bDataSet.Location = new Point(623, 552);
            bDataSet.Name = "bDataSet";
            bDataSet.Size = new Size(110, 43);
            bDataSet.TabIndex = 19;
            bDataSet.Text = "Отправить в дата сет";
            bDataSet.UseVisualStyleBackColor = true;
            bDataSet.Click += bDataSet_Click;
            // 
            // textBox7
            // 
            textBox7.Location = new Point(12, 13);
            textBox7.Name = "textBox7";
            textBox7.ReadOnly = true;
            textBox7.Size = new Size(130, 25);
            textBox7.TabIndex = 30;
            textBox7.Text = "Результат нейронки";
            // 
            // tbNeuralNetworkResult
            // 
            tbNeuralNetworkResult.Location = new Point(148, 13);
            tbNeuralNetworkResult.Name = "tbNeuralNetworkResult";
            tbNeuralNetworkResult.ReadOnly = true;
            tbNeuralNetworkResult.Size = new Size(464, 25);
            tbNeuralNetworkResult.TabIndex = 31;
            // 
            // bImageLeft
            // 
            bImageLeft.Font = new Font("Segoe UI", 20F);
            bImageLeft.Location = new Point(177, 650);
            bImageLeft.Name = "bImageLeft";
            bImageLeft.Size = new Size(110, 43);
            bImageLeft.TabIndex = 32;
            bImageLeft.Text = "<-----";
            bImageLeft.UseVisualStyleBackColor = true;
            bImageLeft.Click += bImageLeft_Click;
            // 
            // bImageRight
            // 
            bImageRight.Font = new Font("Segoe UI", 20F);
            bImageRight.Location = new Point(349, 650);
            bImageRight.Name = "bImageRight";
            bImageRight.Size = new Size(110, 43);
            bImageRight.TabIndex = 33;
            bImageRight.Text = "----->\r\n";
            bImageRight.UseVisualStyleBackColor = true;
            bImageRight.Click += bImageRight_Click;
            // 
            // tbImageIndex
            // 
            tbImageIndex.Location = new Point(293, 660);
            tbImageIndex.Name = "tbImageIndex";
            tbImageIndex.ReadOnly = true;
            tbImageIndex.Size = new Size(50, 25);
            tbImageIndex.TabIndex = 34;
            tbImageIndex.TextAlign = HorizontalAlignment.Center;
            // 
            // bImageDelete
            // 
            bImageDelete.Font = new Font("Segoe UI", 8.830189F);
            bImageDelete.Location = new Point(12, 650);
            bImageDelete.Name = "bImageDelete";
            bImageDelete.Size = new Size(110, 43);
            bImageDelete.TabIndex = 35;
            bImageDelete.Text = "Удалить ";
            bImageDelete.UseVisualStyleBackColor = true;
            bImageDelete.Click += bImageDelete_Click;
            // 
            // bTbTagFix
            // 
            bTbTagFix.Location = new Point(1357, 106);
            bTbTagFix.Name = "bTbTagFix";
            bTbTagFix.Size = new Size(110, 52);
            bTbTagFix.TabIndex = 36;
            bTbTagFix.Text = "Исправление";
            bTbTagFix.UseVisualStyleBackColor = true;
            bTbTagFix.Click += bTbTagFix_Click;
            // 
            // AddPost
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(1478, 703);
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
        private DataGridViewTextBoxColumn tags;
    }
}