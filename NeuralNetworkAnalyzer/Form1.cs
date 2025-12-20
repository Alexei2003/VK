using System.Windows.Forms;

using DataSet;

using NeuralNetwork;

namespace NeuralNetworkAnalyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pbImage_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                using var predImage = pbImage.Image;
                var imageBmp = (Bitmap)Clipboard.GetImage();
                pbImage.Image = imageBmp;

                var image = ConverterBmp.ConvertToImageSharp(imageBmp);

                Task.Run(() =>
                {
                    var labels = NeuralNetworkWorker.NeuralNetworkResultKTopCountAndPercent(image, 0, 100);

                    this.Invoke((MethodInvoker)(() =>
                    {
                        dvgPercent.Rows.Clear();
                        for (var i = 0; i < labels.Length; i++)
                        {
                            dvgPercent.Rows.Add(new DataGridViewRow { Cells = { new DataGridViewTextBoxCell() { Value = labels[i].Name }, new DataGridViewTextBoxCell() { Value = labels[i].Value.ToString("F2") + "%" } } });
                            switch (labels[i].Name) 
                            {
                                case "#nsfw":
                                    dvgPercent.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                                    break;
                                case "#original":
                                    dvgPercent.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                                    break;
                                case "#bad_drawing":
                                    dvgPercent.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                                    break;
                            }
                            
                        }
                    }));
                });
            }
        }
    }
}
