using System;
using System.IO;
using System.Windows.Forms;

namespace Калькулятор_посчитайкин
{
    public partial class FHelp : Form
    {
        public FHelp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Загрузка справки из файла.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Load(object sender, EventArgs e)
        {
            try
            {
                var instr = File.OpenText("Readme.txt");

                // Загружаем из текстового документа, чтобы и установщик его подхватывал и менять было легче.
                tb_manual.Text = instr.ReadToEnd();
                instr.Close();
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить файл справки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
