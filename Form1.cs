using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Method_of_calculation_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const double pi = 3.14159265359;

        void V_d(double[] d, double h, int n)
        {
            d[0] = 0;
            d[n] = 0;
            for (int i = 1; i < n; i++)
            {
                d[i] = (Math.Sin(i * h + h) - Math.Sin(i * h)) / h - (Math.Sin(i * h) - Math.Sin(i * h - h)) / h;
            }
        }

        void V_lambda(double[] lambda, double ai, double bi, double ci, int n)
        {
            lambda[0] = 0;
            lambda[n] = 0;
            for (int i = 1; i < n; i++)
            {
                lambda[i] = -bi / (ai + ci * lambda[i - 1]);
            }
        }

        void V_nu(double[] nu, double[] d, double[] lambda, double ai, double ci, int n)
        {
            nu[0] = 0;
            nu[n] = 0;
            for (int i = 1; i < n; i++)
            {
                nu[i] = (d[i] - ci * nu[i - 1]) / (ai + ci * lambda[i - 1]);
            }
        }

        void bw_run(double[] m, double[] nu, double[] lambda, int n)
        {
            m[n] = nu[n];
            for (int i = n - 1; i >= 0; i--)
            {
                m[i] = m[i + 1] * lambda[i] + nu[i];
            }
        }

        double delta_max(double[] m, double h, int n)
        {
            double dmax = -1;
            for (int i = 1; i <= n; i++)
            {
                double x = (i - 1) * h + h / 2;
                double s3 = (((Math.Pow(i * h - x, 3) - h * h * (i * h - x)) * m[i - 1]) / (6 * h)) +
                    (((Math.Pow(x - (i - 1) * h, 3) - h * h * (x - (i - 1) * h)) * m[i]) / (6 * h)) +
                    ((i * h - x) * Math.Sin((i - 1) * h)) / h + ((x - (i - 1) * h) * Math.Sin(i * h)) / h;
                dmax = Math.Max(dmax, Math.Abs(s3 - Math.Sin(x)));
            }
            return dmax;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            double prev_dmax = 0;
            for (int n = 5; n <= 10240; n *= 2)
            {
                double h = pi / n; //шаг, расстояние м/у точками
                double ai = 2 * h / 3, bi = h / 6, ci = h / 6;
                double[] m = new double[n + 1];
                double[] lambda = new double[n + 1];
                double[] nu = new double[n + 1];
                double[] d = new double[n + 1];
                V_d(d, h, n);
                V_lambda(lambda, ai, bi, ci, n);
                V_nu(nu, d, lambda, ai, ci, n);
                bw_run(m, nu, lambda, n);
                double dmax = delta_max(m, h, n);
                if (n == 5)
                {
                    prev_dmax = dmax;
                    dataGridView.Rows.Add(n, dmax, "-", "-");
                }
                else
                {
                    double k = prev_dmax / dmax;
                    double deva = prev_dmax / 16;
                    dataGridView.Rows.Add(n, dmax, deva, k);
                    prev_dmax = dmax;
                }
            }
        }

        Form2 info_Form;
        private void button_Click(object sender, EventArgs e)
        {
                if (info_Form == null || info_Form.IsDisposed)
                {
                    info_Form = new Form2();
                    info_Form.Show();
                }
        }
    }
}
