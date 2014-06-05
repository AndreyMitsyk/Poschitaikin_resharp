using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Project2;
using Калькулятор_посчитайкин.Properties;

namespace Калькулятор_посчитайкин
{
    /// <summary>
    /// Калькулятор "Посчитайкин" 2.0!
    /// </summary>
    public partial class PoschitaikinMain : Form
    {
        #region Объявление
        /// <summary>
        /// Наш класс.
        /// </summary>
        static readonly Xryo Calc = new Xryo();

        /// <summary>
        /// Операция в строке сверху.
        /// </summary>
        private string _operStr;

        /// <summary>
        /// Для тригонометрических операций.
        /// </summary>
        bool _trig;

        /// <summary>
        /// Системы счисления.
        /// </summary>
        SystemOp _syop = SystemOp.Dec;

        /// <summary>
        /// Углы.
        /// </summary>
        DegreesOp _deop = DegreesOp.Rad;

        /// <summary>
        /// Значение из памяти и обратно.
        /// </summary>
        object _resn = 0;

        /// <summary>
        /// Операции.
        /// </summary>
        Operations _op;

        /// <summary>
        /// Операция, следующая за прошлой сразу.
        /// </summary>
        Operations _opn;

        /// <summary>
        /// Операнды.
        /// </summary>
        object[] _opera;

        /// <summary>
        /// Номер операнда.
        /// </summary>
        int _i;

        /// <summary>
        /// Операции с памятью.
        /// </summary>
        private MemActions _mem;

        /// <summary>
        /// Флаг (введен ли символ).
        /// </summary>
        bool _nn;

        /// <summary>
        /// Инициализация.
        /// </summary>
        public PoschitaikinMain()
        {
            InitializeComponent();
        }

        #endregion

        #region Ввод

        /// <summary>
        /// Цифры и буквы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            switch (btn.Text)
            {
                case "0":
                    if (!(tb1.Text.Length == 1 && tb1.Text[0] == 0) || (tb1.Text.Length != 0))
                        Charadd("0", ref _nn);
                    break;
                case ",":
                    if ((!tb1.Text.Contains(',')) && _nn)
                        Charadd(",", ref _nn);
                    else if ((!tb1.Text.Contains(',')) && (_nn == false))
                    {
                        _nn = true;
                        Charadd(",", ref _nn);
                    }
                    else if ((tb1.Text.Contains(';')) && (tb1.Text[tb1.Text.Length - 1] != ','))
                        Charadd(",", ref _nn);
                    break;
                default:
                    Charadd(btn.Text, ref _nn);
                    break;
            }
        }

        /// <summary>
        /// Кнопка +-.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnpm_Click(object sender, EventArgs e)
        {
            if (_op == Operations.SArifm)
            {
                int i = tb1.TextLength - 1;
                while ((i != 0) && (tb1.Text[i] != ';'))
                {
                    i--;
                }

                tb1.Text = tb1.Text[i + 1] != '-' ? tb1.Text.Insert(i + 1, "-") : tb1.Text.Remove(i + 1, 1);
            }
            else
                if (tb1.Text != Resources.PoschitaikinMain_btnpm_Click__0)
                    tb1.Text = tb1.Text[0] != '-' ? tb1.Text.Insert(0, "-") : tb1.Text.Remove(0, 1);
        }

        /// <summary>
        /// Кнопка с.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btncc_Click(object sender, EventArgs e)
        {
            Del();
            lblvvod.Text = "";
        }

        /// <summary>
        /// Кнопка ←.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnbs_Click(object sender, EventArgs e)
        {
            Bs();
        }

        /// <summary>
        /// Кнопка се
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnce_Click(object sender, EventArgs e)
        {
            Delstr();
        }

        #endregion

        #region Настройка
        /// <summary>
        /// Настройки строки операций.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSystemOperation_Click(object sender, EventArgs e)
        {
            mnuSystemOperation.Checked = !mnuSystemOperation.Checked;
        }

        #region Вид кальлятора

        /// <summary>
        /// Переключение вида калькулятора.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuExtendedClick_Click(object sender, EventArgs e)
        {
            if (mnuExtendedView.Checked)
            {
                mnuExtendedView.Checked = false;
                mnuSystemOperation.Visible = false;
                _syop = SystemOp.Dec;
                S10();
                cbsist.Text = Convert.ToString(cbsist.Items[2]);
                pnldop.Visible = false;
                pnlsit.Visible = false;
                tb1.Width = 180;
                Width = 216;
                Height = 330;
                pnlvvod.Height = 205;
                lbl_mem.Location = new Point(174, 55);
            }
            else
            {
                mnuExtendedView.Checked = true;
                mnuSystemOperation.Visible = true;
                pnldop.Visible = true;
                pnlsit.Visible = true;
                tb1.Width = 329;
                Width = 370;
                lbl_mem.Location = new Point(322, 55);
            }
        }
        #endregion

        #endregion

        #region Загрузка
        /// <summary>
        /// Действия при загрузке.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Poschitaikin_main_Load(object sender, EventArgs e)
        {
            // Загрузка настроек.
            try
            {
                mnuSystemOperation.Checked = Settings.Default.Radix;

                if (Settings.Default.View)
                {
                    mnuExtendedView.Checked = true;
                    mnuSystemOperation.Visible = true;
                }
                else
                {
                    mnuExtendedView.Checked = false;
                    mnuSystemOperation.Visible = false;
                }
            }
            catch
            {
                ErrorEvent("Нет доступа!");
            }

            // Действия после запуска.
            lblvvod.Text = "";
            _opera = new object[] { null };
            tb1.Text = Resources.PoschitaikinMain_Poschitaikin_main_Load__0;
            _operStr = "";
            _trig = false;
            cbsist.Text = Convert.ToString(cbsist.Items[2]);

            // Вид в соответствии с настройками.
            if (mnuExtendedView.Checked == false)
            {
                tb1.Width = 180;
                Width = 216;
                Height = 330;
                pnlvvod.Height = 205;
                lbl_mem.Location = new Point(174, 55);
            }
        }

        /// <summary>
        /// При отображении формы фокус на равно.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Poschitaikin_main_Shown(object sender, EventArgs e)
        {
            Activate();
            btnrav.Focus();
        }
        #endregion

        #region Закрытие
        /// <summary>
        /// Действие при закрытии формы (запись настроек).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Poschitaikin_main_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Settings.Default.View = mnuExtendedView.Checked;
                Settings.Default.Radix = mnuSystemOperation.Checked;
                Settings.Default.Save();
            }
            catch (Exception exc)
            {
                ErrorEvent(exc.Message);
            }
        }
        #endregion

        #region Строка ввода
        /// <summary>
        /// Ввод символа.
        /// </summary>
        /// <param name="n">Символ</param>
        /// <param name="nn">Проверка наличия введенного символа.</param>
        private void Charadd(string n, ref bool nn)
        {
            if (nn)
                tb1.Text += n;
            else
                if (n != "0")
                {
                    tb1.Text = n;
                    nn = true;
                }
                else
                {
                    tb1.Text = n;
                }
            btnrav.Focus();
        }

        /// <summary>
        /// Удаление символа.
        /// </summary>
        private void Bs()
        {
            if (tb1.Text.Length > 1)
                tb1.Text = tb1.Text.Remove(tb1.Text.Length - 1);
            else
            {
                tb1.Text = Resources.PoschitaikinMain_Bs__0;
                _nn = false;
            }
        }

        /// <summary>
        /// Очистка строки.
        /// </summary>
        private void Delstr()
        {
            tb1.Text = Resources.PoschitaikinMain_Delstr__0;
            _nn = false;
        }

        /// <summary>
        /// Очистка.
        /// </summary>
        private void Del()
        {
            tb1.Text = Resources.PoschitaikinMain_Del__0;
            _i = 0;
            _nn = false;
            _trig = false;
            _opera = new object[] { null };
        }
        #endregion

        #region Меню копирования

        private void m_copy_Click(object sender, EventArgs e)
        {
            try
            {
                // Записываем в буфер содержимое текстбокса.
                Clipboard.SetData(DataFormats.Text, tb1.Text);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch { }
        }

        private void m_paste_Click(object sender, EventArgs e)
        {
            try
            {
                if (Calc.SSCheck(Clipboard.GetData(DataFormats.Text).ToString(), _syop))

                // Извлекаем из буфера обмена содержимое.
                tb1.Text = Convert.ToString(Clipboard.GetData(DataFormats.Text));
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch { }
        }

        #endregion

        #region Трей и выход
        /// <summary>
        /// Сворачивание в трей.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuTrei_Click(object sender, EventArgs e)
        {
            Hide();
            notifyIcon1.Visible = true;
        }

        /// <summary>
        /// Разворачивание из трея на двойной клик.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnucLoad_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        /// <summary>
        /// Выход из программы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnucExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Разворачивание из трея через меню.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        /// <summary>
        /// Выход из программы.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Системы счисления

        /// <summary>
        /// Изменение системы счисления.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbsist_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var conv = tb1.Text;
            try
            {

                switch (cbsist.Text)
                {
                    case "Bin (2)":
                        tb1.Text = Convert.ToString(Calc.SSConvert(conv, _syop, SystemOp.Bin));
                        _syop = SystemOp.Bin;
                        S2();
                        break;
                    case "Oct (8)":
                        tb1.Text = Convert.ToString(Calc.SSConvert(conv, _syop, SystemOp.Oct));
                        _syop = SystemOp.Oct;
                        S8();
                        break;
                    case "Dec (10)":
                        tb1.Text = Convert.ToString(Calc.SSConvert(conv, _syop, SystemOp.Dec));
                        _syop = SystemOp.Dec;
                        S10();
                        break;
                    case "Hex (16)":
                        tb1.Text = Convert.ToString(Calc.SSConvert(conv, _syop, SystemOp.Hex));
                        _syop = SystemOp.Hex;
                        S16();
                        break;
                }
            }
            catch (Exception exc)
            {
                btnrav.Focus();
                cbsist.Text = Resources.PoschitaikinMain_cbsist_SelectionChangeCommitted_Dec__10_;
                ErrorEvent(exc.Message);
            }
        }
        #endregion

        #region Изменение углов
        private void rbrad_CheckedChanged(object sender, EventArgs e)
        {
            _deop = DegreesOp.Rad;
        }

        private void rbdeg_CheckedChanged(object sender, EventArgs e)
        {
            _deop = DegreesOp.Deg;
        }

        private void rbgrad_CheckedChanged(object sender, EventArgs e)
        {
            _deop = DegreesOp.Grad;
        }
        #endregion

        #region Размер и активация элементов

        /// <summary>
        /// Активация 16-ичной системы счисления.
        /// </summary>
        private void S16()
        {
            Height = 360;
            pnlvvod.Height = 242;
            btna.Visible = true;
            btnb.Visible = true;
            btnc.Visible = true;
            btnd.Visible = true;
            btne.Visible = true;
            btnf.Visible = true;
            btn2.Enabled = true;
            btn2.BackColor = Color.LightSteelBlue;
            btn3.Enabled = true;
            btn3.BackColor = Color.LightSteelBlue;
            btn4.Enabled = true;
            btn4.BackColor = Color.LightSteelBlue;
            btn5.Enabled = true;
            btn5.BackColor = Color.LightSteelBlue;
            btn6.Enabled = true;
            btn6.BackColor = Color.LightSteelBlue;
            btn7.Enabled = true;
            btn7.BackColor = Color.LightSteelBlue;
            btn8.Enabled = true;
            btn8.BackColor = Color.LightSteelBlue;
            btn9.Enabled = true;
            btn9.BackColor = Color.LightSteelBlue;
            Deakt();

        }

        /// <summary>
        /// Активация 10-ичной системы счисления.
        /// </summary>
        private void S10()
        {
            Height = 330;
            pnlvvod.Height = 205;
            btna.Visible = false;
            btnb.Visible = false;
            btnc.Visible = false;
            btnd.Visible = false;
            btne.Visible = false;
            btnf.Visible = false;
            btn2.Enabled = true;
            btn2.BackColor = Color.LightSteelBlue;
            btn3.Enabled = true;
            btn3.BackColor = Color.LightSteelBlue;
            btn4.Enabled = true;
            btn4.BackColor = Color.LightSteelBlue;
            btn5.Enabled = true;
            btn5.BackColor = Color.LightSteelBlue;
            btn6.Enabled = true;
            btn6.BackColor = Color.LightSteelBlue;
            btn7.Enabled = true;
            btn7.BackColor = Color.LightSteelBlue;
            btn8.Enabled = true;
            btn8.BackColor = Color.LightSteelBlue;
            btn9.Enabled = true;
            btn9.BackColor = Color.LightSteelBlue;
            btnrav.Focus();
            rbdeg.Enabled = true;
            rbgrad.Enabled = true;
            rbrad.Enabled = true;
            btnper.Enabled = true;
            btnper.BackColor = Color.DodgerBlue;
            btnsqrt.Enabled = true;
            btnsqrt.BackColor = Color.DodgerBlue;
            btnpm.Enabled = true;
            btnpm.BackColor = Color.DodgerBlue;
            btndot.Enabled = true;
            btndot.BackColor = Color.LightSteelBlue;
            btnsin.Enabled = true;
            btnsin.BackColor = Color.LightSteelBlue;
            btncos.Enabled = true;
            btncos.BackColor = Color.LightSteelBlue;
            btntan.Enabled = true;
            btntan.BackColor = Color.LightSteelBlue;
            btnctan.Enabled = true;
            btnctan.BackColor = Color.LightSteelBlue;
            btnsqr.Enabled = true;
            btnsqr.BackColor = Color.LightSteelBlue;
            btnmean.Enabled = true;
            btnmean.BackColor = Color.LightSteelBlue;
            btnfak.Enabled = true;
            btnfak.BackColor = Color.LightSteelBlue;
            btnpi.Enabled = true;
            btnpi.BackColor = Color.LightSteelBlue;
        }

        /// <summary>
        /// Активация 8-ичной системы счисления.
        /// </summary>
        private void S8()
        {
            Height = 330;
            pnlvvod.Height = 205;
            btna.Visible = false;
            btnb.Visible = false;
            btnc.Visible = false;
            btnd.Visible = false;
            btne.Visible = false;
            btnf.Visible = false;
            btn2.Enabled = true;
            btn2.BackColor = Color.LightSteelBlue;
            btn3.Enabled = true;
            btn3.BackColor = Color.LightSteelBlue;
            btn4.Enabled = true;
            btn4.BackColor = Color.LightSteelBlue;
            btn5.Enabled = true;
            btn5.BackColor = Color.LightSteelBlue;
            btn6.Enabled = true;
            btn6.BackColor = Color.LightSteelBlue;
            btn7.Enabled = true;
            btn7.BackColor = Color.LightSteelBlue;
            btn8.Enabled = false;
            btn8.BackColor = Color.SlateGray;
            btn9.Enabled = false;
            btn9.BackColor = Color.SlateGray;
            Deakt();
        }

        /// <summary>
        /// Активация 2-ичной системы счисления.
        /// </summary>
        private void S2()
        {
            Height = 330;
            pnlvvod.Height = 205;
            btna.Visible = false;
            btnb.Visible = false;
            btnc.Visible = false;
            btnd.Visible = false;
            btne.Visible = false;
            btnf.Visible = false;
            btn2.Enabled = false;
            btn2.BackColor = Color.SlateGray;
            btn3.Enabled = false;
            btn3.BackColor = Color.SlateGray;
            btn4.Enabled = false;
            btn4.BackColor = Color.SlateGray;
            btn5.Enabled = false;
            btn5.BackColor = Color.SlateGray;
            btn6.Enabled = false;
            btn6.BackColor = Color.SlateGray;
            btn7.Enabled = false;
            btn7.BackColor = Color.SlateGray;
            btn8.Enabled = false;
            btn8.BackColor = Color.SlateGray;
            btn9.Enabled = false;
            btn9.BackColor = Color.SlateGray;
            Deakt();
        }

        /// <summary>
        /// Деактивация элементов, общих для 2,8,16-ой систем.
        /// </summary>
        private void Deakt()
        {
            btnrav.Focus();
            rbdeg.Enabled = false;
            rbgrad.Enabled = false;
            rbrad.Enabled = false;
            btnper.Enabled = false;
            btnper.BackColor = Color.SlateGray;
            btnsqrt.Enabled = false;
            btnsqrt.BackColor = Color.SlateGray;
            btnsin.Enabled = false;
            btnsin.BackColor = Color.SlateGray;
            btncos.Enabled = false;
            btncos.BackColor = Color.SlateGray;
            btntan.Enabled = false;
            btntan.BackColor = Color.SlateGray;
            btnctan.Enabled = false;
            btnctan.BackColor = Color.SlateGray;
            btnsqr.Enabled = false;
            btnsqr.BackColor = Color.SlateGray;
            btnmean.Enabled = false;
            btnmean.BackColor = Color.SlateGray;
            btnfak.Enabled = false;
            btnfak.BackColor = Color.SlateGray;
            btnpi.Enabled = false;
            btnpi.BackColor = Color.SlateGray;
            btnpm.Enabled = false;
            btnpm.BackColor = Color.SlateGray;
        }
        #endregion

        #region Операции

        #region Разбор операции
        /// <summary>
        /// Операции без, с одним и двумя операндами. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnop_Click(object sender, EventArgs e)
        {
            btnrav.Focus();
            var btn = (Button)sender;
            switch (btn.Text)
            {
                case "+":
                    _op = Operations.Plus;
                    _operStr = "+";
                    Twooper();
                    break;
                case "-":
                    _op = Operations.Minus;
                    _operStr = "-";
                    Twooper();
                    break;
                case "*":
                    _op = Operations.Multiply;
                    _operStr = "*";
                    Twooper();
                    break;
                case "/":
                    _op = Operations.Divide;
                    _operStr = "/";
                    Twooper();
                    break;
                case "pi":
                    _op = Operations.Pi;
                    _operStr = "pi";
                    _trig = false;
                    lblvvod.Text = _operStr;
                    Resultat();
                    break;
                case "sin":
                    _op = Operations.Sin;
                    _operStr = "sin";
                    Oneoper();
                    break;
                case "cos":
                    _op = Operations.Cos;
                    _operStr = "cos";
                    Oneoper();
                    break;
                case "tan":
                    _op = Operations.Tan;
                    _operStr = "tan";
                    Oneoper();
                    break;
                case "ctan":
                    _op = Operations.Ctan;
                    _operStr = "ctan";
                    Oneoper();
                    break;
                case "x^2":
                    _op = Operations.SqrX;
                    _operStr = "sqr";
                    Oneoper();
                    break;
                case "√":
                    _op = Operations.SqrtX;
                    _operStr = "sqrt";
                    Oneoper();
                    break;
                case "n!":
                    _op = Operations.N;
                    _operStr = "!";
                    Oneoper();
                    break;
            }

        }

        /// <summary>
        /// Процент.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnper_Click(object sender, EventArgs e)
        {
            lblvvod.Text = tb1.Text + Resources.PoschitaikinMain_btnper_Click_;
            tb1.Text = Convert.ToString(Convert.ToDouble(tb1.Text) / 100);
            lblvvod.Text += tb1.Text;
        }

        /// <summary>
        /// Среднее арифметическое.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnmean_Click(object sender, EventArgs e)
        {
            _nn = true;
            _op = Operations.SArifm;
            Charadd(";", ref _nn);
            _i++;
        }
        #endregion

        /// <summary>
        /// Для одного операнда.
        /// </summary>
        private void Oneoper()
        {
            _opera = new object[] { tb1.Text };
            if (_op != Operations.N)
                _trig = true;
            Resultat();
        }

        /// <summary>
        /// Для 2х операндов.
        /// </summary>
        private void Twooper()
        {
            Operations opb = _op;
            if (_opera[0] == null)
            {
                _opn = _op;
                _opera = new object[] { tb1.Text, null };
                lblvvod.Text = tb1.Text + Resources.PoschitaikinMain_Twooper__ + _operStr + Resources.PoschitaikinMain_Twooper__;
                Delstr();
                _i = 1;
                _op = opb;
            }
            else
            {
                _op = _opn;
                lblvvod.Text = "";
                btnrav.PerformClick();
                _opera = new object[] { tb1.Text, null };
                lblvvod.Text = tb1.Text + Resources.PoschitaikinMain_Twooper__ + _operStr + Resources.PoschitaikinMain_Twooper__;
                Delstr();
                _i = 1;
                _op = opb;
            }
        }

        #endregion

        #region Результат
        /// <summary>
        /// Клик на равно.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnrav_Click(object sender, EventArgs e)
        {
            // Если операнды не пусты.
            if ((_op != Operations.SArifm) & (_opera[0] != null))
                Resultat();
            else
                // Если операция среднее арифметическое.
                if (_op == Operations.SArifm)
                {
// ReSharper disable once CoVariantArrayConversion
                    object[] operan = tb1.Text.Split(';');
                    if ((string) operan[operan.Length - 1] == "")
                    {
                        var operanx = new object[operan.Length - 1];
                        for (int j = 0; j < operanx.Length; j++)
                            operanx[j] = operan[j];
                        _opera = operanx;
                    }
                    else
                        _opera = operan;
                    Resultat();
                }

            _i = 0;
            _trig = false;
        }

        /// <summary>
        /// Результат.
        /// </summary>
        private void Resultat()
        {
            try
            {
                if (_op != Operations.SArifm)
                    _opera[_i] = tb1.Text;

                if ((_op != Operations.Pi) && (_op != Operations.SArifm))
                    lblvvod.Text += tb1.Text;
                else
                    if (_op == Operations.SArifm)
                        lblvvod.Text = Resources.PoschitaikinMain_Resultat_mean_ + tb1.Text + Resources.PoschitaikinMain_Resultat__;
                if (_trig)
                    lblvvod.Text = _operStr + Resources.PoschitaikinMain_Resultat__ + tb1.Text + Resources.PoschitaikinMain_Resultat__;
                if (_op == Operations.N)
                    lblvvod.Text = tb1.Text + _operStr;

                // Задаем параметры подсчета результата.
                Calc.SystemSch = _syop;
                Calc.DegOp = _deop;
                Calc.Operation = _op;
                Calc.Operands = _opera;

                #region Генерация ошибок и результат

                if (_syop == SystemOp.Dec)
                {
                    if (Double.IsInfinity(Convert.ToDouble(Calc.Result)))
                        throw new DivideByZeroException();
                    if (Double.IsNaN(Convert.ToDouble(Calc.Result)))
                        throw new NotSupportedException();
                }
                if (Calc.Result != null)
                {
                    // Если операция не тригонометрическая.
                    if (!_trig)
                        tb1.Text = Convert.ToString(Calc.Result);
                    else
                        // Иначе, если результат крайне мал (например, cos 90 должен быть равен 0, но иначе он имеет порядок -17)
                        if (Math.Abs(Convert.ToDouble(Calc.Result)) > Math.Pow(10, -16))
                            tb1.Text = Convert.ToString(Calc.Result);
                        else
                            tb1.Text = Convert.ToString(Math.Round(Convert.ToDecimal(Calc.Result)));

                    lblvvod.Text += Resources.PoschitaikinMain_Resultat_ + tb1.Text;
                }
                else throw new InvalidOperationException();
                #endregion

                if (mnuExtendedView.Checked && mnuSystemOperation.Checked)
                    lblvvod.Text += Resources.PoschitaikinMain_Resultat____ + Convert.ToString((int)_syop) + Resources.PoschitaikinMain_Resultat__;
            }

            #region Обработка ошибок результата

            catch (DivideByZeroException)
            {
                ErrorEvent(_op != Operations.Ctan ? "Деление на 0!" : "ctan(0) не существует!");
                lblvvod.Text = "";
                Del();
            }

            catch (NotSupportedException)
            {
                ErrorEvent("Ошибка операции!");
                lblvvod.Text = "";
                Del();
            }

            catch (InvalidOperationException)
            {
                ErrorEvent("Неверная операция!");
                lblvvod.Text = "";
                Del();
            }

            catch (Exception exc)
            {
                ErrorEvent(exc.Message);
                lblvvod.Text = "";
                Del();
            }
            #endregion

            _nn = false;
            _trig = false;
            _opera = new object[] { null };
        }
        #endregion

        #region Нажатие клавиатуры

        private void Poschitaikin_main_KeyDown(object sender, KeyEventArgs e)
        {
            btnrav.Focus();

            // Если нажали ctrl+c, то обращаемся к копированию.
            if (e.Control && e.KeyCode == Keys.C)
                m_copy_Click(sender, e);

            else

                // Если нажали ctrl+v, то обращаемся к вставке.
                if (e.Control && e.KeyCode == Keys.V)
                m_paste_Click(sender, e);

            else
            {
                // Если система счисления 16 или 10.
                if ((_syop == SystemOp.Hex) || (_syop == SystemOp.Dec))
                {
                    // Цифры сверху клавиатуры.
                    if ((e.KeyValue > 47) && (e.KeyValue < 58))
                        Charadd(Convert.ToString(e.KeyValue - 48), ref _nn);

                    // Цифры Num.
                    if ((e.KeyValue > 95) && (e.KeyValue < 106))
                        Charadd(Convert.ToString(e.KeyValue - 96), ref _nn);

                    // Буквы (A-F).
                    if ((e.KeyValue > 64) && (e.KeyValue < 71) && (_syop == SystemOp.Hex))
                        Charadd(Convert.ToString(e.KeyCode), ref _nn);
                }

                // Если система счисления 8.
                if (_syop == SystemOp.Oct)
                {
                    // Цифры сверху клавиатуры.
                    if ((e.KeyValue > 47) && (e.KeyValue < 56))
                        Charadd(Convert.ToString(e.KeyValue - 48), ref _nn);

                    // Цифры Num.
                    if ((e.KeyValue > 95) && (e.KeyValue < 104))
                        Charadd(Convert.ToString(e.KeyValue - 96), ref _nn);
                }

                // Если система счисления 2.
                if (_syop == SystemOp.Bin)
                {
                    // Цифры сверху клавиатуры.
                    if ((e.KeyValue > 47) && (e.KeyValue < 50))
                        Charadd(Convert.ToString(e.KeyValue - 48), ref _nn);

                    // Цифры Num.
                    if ((e.KeyValue > 95) && (e.KeyValue < 98))
                        Charadd(Convert.ToString(e.KeyValue - 96), ref _nn);
                }

                // Плюс.
                if (e.KeyValue == 107)
                    btnop_Click(btnpl, e);

                // Минус.
                if (e.KeyValue == 109)
                    btnop_Click(btnmin, e);

                // Запятая.
                if ((e.KeyValue == 110) || (e.KeyValue == 191))
                    btn_Click(btndot, e);

                // Умножить.
                if (e.KeyValue == 106)
                    btnop_Click(btnmul, e);

                // Делить.
                if (e.KeyValue == 111)
                    btnop_Click(btndiv, e);

                // Удаление одного.
                if (e.KeyValue == 8)
                    Bs();

                // Очистка поля.
                if (e.KeyValue == 46)
                {
                    Del();
                    lblvvod.Text = "";
                }

                // Равно.
                if (e.KeyValue == 13)
                {
                    Resultat();
                }
            }
        }

        #endregion

        #region Обработка ошибок

        /// <summary>
        /// Обработчик ошибок.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        static void ErrorEvent(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region Память
        /// <summary>
        /// Разбор нажатых кнопок памяти.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnmem_Click(object sender, EventArgs e)
        {
            var conv = tb1.Text;
            var btn = (Button)sender;

            switch (btn.Text)
            {
                case "MC":
                    _mem = MemActions.MC;
                    Mem(Calc, conv, "0", _mem, ref _resn, _syop);
                    lbl_mem.Visible = false;
                    break;
                case "MR":
                    _mem = MemActions.MR;
                    Mem(Calc, conv, "0", _mem, ref _resn, _syop);
                    tb1.Text = Convert.ToString(_resn);
                    break;
                case "MS":
                    _mem = MemActions.MS;
                    Mem(Calc, conv, "0", _mem, ref _resn, _syop);
                    lbl_mem.Visible = true;
                    break;
                case "M+":
                    _mem = MemActions.MP;
                    Mem(Calc, conv, "0", _mem, ref _resn, _syop);
                    lbl_mem.Visible = true;
                    break;
                case "M-":
                    _mem = MemActions.MM;
                    Mem(Calc, conv, "0", _mem, ref _resn, _syop);
                    lbl_mem.Visible = true;
                    break;
            }
        }


        /// <summary>
        /// Работа с памятью.
        /// </summary>
        /// <param name="calc">Наш класс.</param>
        /// <param name="res">Результат предыдущей операции.</param>
        /// <param name="str">Номер ячейки памяти.</param>
        /// <param name="mm">Операции с мапятью.</param>
        /// <param name="resn">Извлекаемое из памяти значение.</param>
        /// <param name="syop">Текущая система счисления.</param>
        private static void Mem(Xryo calc, Object res, String str, MemActions mm, ref object resn, SystemOp syop)
        {
           
            try
            {
                var n = Convert.ToByte(str);
                // При работе с памятью переводим все к 10-ой системе счисления.
                resn = calc.SSConvert(res, syop, SystemOp.Dec);
// ReSharper disable once InconsistentNaming
                var _res = Convert.ToDouble(resn);
                try
                {
                    calc.Memory(mm, ref _res, n);
                }
                // Если операция была М+ или М-, а память пуста. 
                catch
                {
                    switch (mm)
                    {
                        case MemActions.MP:
                            calc.Memory(MemActions.MS, ref _res, n);
                            break;
                        case MemActions.MM:
                            _res = -_res;
                            calc.Memory(MemActions.MS, ref _res, n);
                            break;
                        default:
                            ErrorEvent("Ошибка памяти!");
                            break;
                    }
                }
                 // Если необходимо извлечь значение из памяти.
                        if (mm == MemActions.MR)
                            resn = calc.SSConvert(_res, SystemOp.Dec, syop);
                
            }
            catch { ErrorEvent("Ошибка памяти!"); }
        }

        #endregion

        #region Справка

        /// <summary>
        /// Вывод окна справки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuHelp_Click(object sender, EventArgs e)
        {
            // Форма справки. 
            var helpForm = new FHelp();
            // Открываем диалогом, чтобы не допустить повторного вызова этой формы.
            helpForm.ShowDialog();

        }
        #endregion

    }
}