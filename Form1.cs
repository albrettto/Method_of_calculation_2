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

        //    const double start = 0, end = Math.PI;
        //    const int N = 10241;
        //    double[] x = new double[N];
        //    double[] a = new double[N];
        //    double[] b = new double[N];
        //    double[] c = new double[N];
        //    double[] m = new double[N];
        //    double[] d = new double[N];
        //    double[] l = new double[N];
        //    double[] mu = new double[N];
        //    double[] S = new double[N];
        //    double h, X, deltamax, deltaoc, K, maxpred;
        //    double func(double x)
        //    {
        //        return Math.Sin(x);
        //    }
        //    private void btn_Click(object sender, EventArgs e)
        //    {
        //        /*dataGridView.Columns.Add("1", "n");
        //        dataGridView.Columns.Add("2", "deltamax");
        //        dataGridView.Columns.Add("3", "deltaoc");
        //        dataGridView.Columns.Add("4", "K");*/

        //        for (int n = 5; n <= 10240; n *= 2)
        //        {

        //            x[0] = start;
        //            x[n] = end;

        //            h = (end - start) / n;
        //            for (int i = 1; i < n; i++)
        //            {
        //                x[i] = start + i * h;
        //            }

        //            double f_2proizv_a = -Math.Sin(start);
        //            double f_2proizv_b = -Math.Sin(end);

        //            c[0] = 0;
        //            a[0] = 1;
        //            b[0] = 0;
        //            d[0] = f_2proizv_a;
        //            l[0] = -b[0] / a[0];
        //            mu[0] = d[0] / a[0];
        //            b[n] = 0;
        //            a[n] = 1;
        //            c[n] = 0;
        //            d[n] = f_2proizv_b;
        //            for (int i = 1; i <= n - 1; i++)
        //            {
        //                a[i] = 2 * h / 3;
        //                b[i] = h / 6;
        //                c[i] = h / 6;
        //                d[i] = (func(x[i + 1]) - 2 * func(x[i]) + func(x[i - 1])) / h;
        //                l[i] = -b[i] / (a[i] + c[i] * l[i - 1]);
        //                mu[i] = (d[i] - c[i] * mu[i - 1]) / (a[i] + c[i] * l[i - 1]);
        //            }
        //            l[n] = -b[n] / (a[n] + c[n] * l[n - 1]);
        //            mu[n] = (d[n] - c[n] * mu[n - 1]) / (a[n] + c[n] * l[n - 1]);
        //            m[n] = mu[n];
        //            for (int i = n - 1; i >= 0; i--)
        //            {
        //                m[i] = l[i] * m[i + 1] + mu[i];
        //            }
        //            deltamax = Math.Abs(S[1] - func(x[0] + h / 2));

        //            for (int i = 1; i <= n; i++)
        //            {
        //                X = x[i - 1] + h / 2;
        //                if (Math.Abs(S[i] - func(X)) > deltamax)
        //                    deltamax = Math.Abs(S[i] - func(X));
        //            }
        //            if (n > 5)
        //            {
        //                deltaoc = maxpred / 16;
        //                K = maxpred / deltamax;
        //                dataGridView.Rows.Add(n, deltamax, deltaoc, K);
        //            }
        //            else dataGridView.Rows.Add(n, deltamax, "-", "-");

        //            maxpred = deltamax;
        //        }

        //    }
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
                double h = pi / n;
                double ai = 2 * h / 3, bi = h / 6, ci = h / 6;
                //double a0 = 1, b0 = 0, cn = 0, an = 1;
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
    }
}
