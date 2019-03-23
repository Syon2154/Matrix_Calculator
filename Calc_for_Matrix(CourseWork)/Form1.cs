using System;
using System.Data;
using System.Windows.Forms;

namespace Calc_for_Matrix
{
    public partial class Form1 : Form
    {
        int n;
        bool invalid_op;     // перевірка на некоректні вхідні дані 
        DataTable mytable;
        Matrix RMS = new Matrix();
        Matrix OMS = new Matrix();
        Matrix x = new Matrix();
        Matrix B = new Matrix();
        public Form1()
        {            
            InitializeComponent();
            dataGridView1.Text = dataGridView1.Text.Replace(",", ".");
        }

        private void Can_Click(bool test)
        {
            button2.Enabled = test;
            button3.Enabled = test;
            button4.Enabled = test;
        }
        
        private void inic1() // створення поля матриці
        {
            string str;
            invalid_op = false;

            if (numericUpDown1.Value > 1)
            {
                n = (int)numericUpDown1.Value;
                dataGridView1.DataSource = mytable;

                for (int i = 1; i < n + 1; i++)
                {
                    str = "x" + i.ToString();
                    mytable.Columns.Add(new DataColumn(str));
                    mytable.Rows.Add();
                }
                mytable.Columns.Add(new DataColumn("b"));
                RMS = new Matrix(n, n + 1);
                OMS = new Matrix(n, n);
                B = new Matrix(n, 1);
                x = new Matrix(n, 1);
            }
        }

        private void inic2() // заповнення матиці та її перевірка на корректність
        {
            try
            {               
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n + 1; j++)
                        {
                            RMS[i, j] = float.Parse(mytable.Rows[i][j].ToString());

                            if (j < n)
                            {
                                OMS[i, j] = RMS[i, j];
                            }

                            if (j == n)
                            {
                                B[i, 0] = RMS[i, n];
                            }                            
                        }
                    }
                invalid_op = false;
            }
            catch
            {
                invalid_op = true;
                MessageBox.Show("Помилка у введенні данних","Помилка !", MessageBoxButtons.OK);
              
            }
        }

        private void answ()
        {
            if(invalid_op == false)
            {
                for (int i = 0; i < n; i++)
                {
                    richTextBox1.Text += "x" + (i + 1).ToString() + " = " + x[i, 0].ToString() + "\n";
                }
            }
                
        }

        //Метод Крамера
        private void KramerMethod()
        {
            Matrix ddet = new Matrix(n, 1);
            Matrix MO = new Matrix(n, n);
            Matrix kOsm = new Matrix(n, n);
            float gdet; 
            
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    kOsm[i, j] = OMS[i, j];
                }
            }                                    
            gdet = kOsm.det(kOsm);

            //визначники невідомих
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        MO[i, j] = OMS[i, j];
                    }                       
                }
                    
                for (int i = 0; i < n; i++)
                {
                    MO[i, k] = B[i, 0];
                }                    
                ddet[k, 0] = MO.det(MO);
            }

            //знаходження невідомих
            x = x.Mult_namb(1 / gdet, ddet);

            if(invalid_op == false)
            {
                richTextBox1.Text += "\nЗа методом Крамера одержано:\n";
            }                                  
        }

        //Метод Гауса
        private void GaussMethod()
        {
            Matrix hM = new Matrix(n, n + 1);
            float dil = 1; float s = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n + 1; j++)
                {
                    hM[i, j] = RMS[i, j];
                }
            }
            
            //прямий хід
            hM = hM.D_M(hM);
            for (int i = 0; i < n; i++)
            {
                dil = 1 / hM[i, i];

                for (int j = 0; j < n + 1; j++)
                {
                    hM[i, j] *= dil;
                } 
            }
            //зворотний хід
            for (int i = n - 1; i >= 0; i--)
            {
                s = 0;
                for (int j = i + 1; j < n; j++)
                {
                    s += hM[i, j] * x[j, 0];
                }
               
                x[i, 0] = hM[i, n] - s;
            }

            if(invalid_op == false)
            {
                richTextBox1.Text += "\nЗа методом Гауса одержано:\n";
            }                                        
        }

        //Матричний метод
        private void MatrixMathod()
        {           
            Matrix hM = new Matrix(n, n);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    hM[i, j] = OMS[i, j];
                }
            }
            
            
            x = x.Multiply(x.Obmatr(hM), B);

            if(invalid_op == false)
            {
                richTextBox1.Text += "\nЗа матричним методом   одержано:\n";
            }
                                  
        }
        
        private void button1_Click_1(object sender, EventArgs e)
        {            
            inic1();           

            if (numericUpDown1.Value >= 2)
            {
                Can_Click(true);
                button1.Enabled = false;
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            mytable.Columns.Clear();
            mytable.Rows.Clear();
            Can_Click(false);
            button1.Enabled = true;
            richTextBox1.Text = "";          
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            inic2();
            KramerMethod();
            answ();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            inic2();
            GaussMethod();
            answ();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            inic2();
            MatrixMathod();
            answ();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {           
            mytable = new DataTable();
            Can_Click(false);
        }

        private void dataGridView1_ColumnAdded_1(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.Width = 60;
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;           
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dataGridView1.RowTemplate.Height = 40;
        }
    }

    //Клас для роботи з матрицями
    public class Matrix
    {
        private float[,] _table;
        private int _r, _c;
        public int Rows
        {
            get { return _r; }
            set { _r = value; }
        }
        public int Colls
        {
            get { return _c; }
            set { _c = value; }
        }
        public float this[int i, int j]
        {
            get
            {
                if (i >= 0 & i < _r & j >= 0 & j < _c)
                {
                    return _table[i, j];
                }
                else
                {
                    return 0;
                }                   
            }

            set
            {
                if (i >= 0 & i < _r & j >= 0 & j < _c) _table[i, j] = value;
            }

        }
        //Порожня матриця без розмірів
        public Matrix() { }

        //Створення матриці на базі іншої матриці такого самого розміру
        public Matrix(Matrix array)
        {
            _table = new float[array._r, array._c];

            for (int i = 0; i < array._r; i++)
            {
                for (int j = 0; j < array._c; j++)
                {
                    _table[i, j] = array[i, j];
                }
            }
                
                   
        }

        //Створення порожньої матриці вказаного розміру
        public Matrix(int m, int n)
        {
            this._r = m; this._c = n; this._table = new float[m, n];
        }
        public Matrix D_M(Matrix A)
        {
            Matrix X = new Matrix(A._r, A._c);
            int l, num; float R; float dm;
            
            for (int i = 0; i < X._r; i++)
            {
                for (int j = 0; j < X._c; j++)
                {
                    X[i, j] = A[i, j];
                }
            }
                 
            for (int k = 0; k < X._c - 1; k++)
            {                
                l = k;
                num = -1;

                while (l < X._r)
                {
                    if (X[l, k] != 0)
                    {
                        num = l; l = X._r;
                    }
                    else
                    {
                        l++;
                    } 
                }

                if (num == -1)
                {
                    dm = 0;
                } 
                else
                {  
                    //перестановка стрічок k і з номером num
                    for (int i = k; i < X._c; i++)
                    {
                        R = X[k, i];
                        X[k, i] = X[num, i];
                        X[num, i] = R;
                    }
                    //нулі в "першому"( k-ому) стовпчику
                    for (int i = k + 1; i < X._r; i++)
                    {
                        dm = X[i, k];

                        for (int j = k; j < X._c; j++)
                        {
                            X[i, j] = -dm / X[k, k] * X[k, j] + X[i, j];
                        }
                            
                    }
                }
            }
            return X;
        }

        //Множення матриці на число
        public Matrix Mult_namb(float x, Matrix y)
        {
            Matrix rez = new Matrix(y._r, y._c);

            for (int i = 0; i < y._r; i++)
            {
                for (int j = 0; j < y._c; j++)
                {
                    rez._table[i, j] = x * y._table[i, j];
                }
            }                                    
            return rez;
        }

        //Множення матриць
        public Matrix Multiply(Matrix A, Matrix B)
        {
            float s = 0;
            Matrix rez = new Matrix(A._r, B._c);
            for (int i = 0; i < A._r; i++)
            {
                for (int j = 0; j < B._c; j++)
                {
                    s = 0;
                    for (int k = 0; k < A._c; k++)
                        s += A._table[i, k] * B._table[k, j];
                    rez._table[i, j] = s;
                }
            }
            return rez;
        }

        //Визначник матриці
        public float det(Matrix A)
        {
            Matrix X = new Matrix(A._r, A._c);
            int l; float R, max; float dm;
            
            for (int i = 0; i < X._r; i++)
            {
                for (int j = 0; j < X._r; j++)
                {
                    X[i, j] = A[i, j];
                }                
            }
                
            for (int k = 0; k < X._r - 1; k++)
            {                
                l = k; max = Math.Abs(X[k, k]);

                for (int i = k; i < X._r; i++)
                {
                    if (Math.Abs(X[i, k]) > max)
                    {
                        max = X[i, k]; l = i;
                    }
                }
                                    
                if (l != k)
                {
                    for (int j = 0; j < X._r; j++)
                    {
                        R = X[k, j];
                        X[k, j] = X[l, j];
                        X[l, j] = -R;
                    }
                }
                                    
                for (int i = k + 1; i < X._r; i++)
                {
                    dm = X[i, k];
                    for (int j = k; j < X._r; j++)
                    {
                        X[i, j] = -dm / X[k, k] * X[k, j] + X[i, j];
                    }                       
                }
            }

            dm = 1;
            for (int i = 0; i < X._r; i++)
            {
                dm *= X[i, i];
            }            
            return dm;
        }

        // Транспортована матриця
        public Matrix Tr_Matr(Matrix A)
        {
            Matrix rez = new Matrix(A._c, A._r);

            for (int i = 0; i < A._c; i++)
            {
                for (int j = 0; j < A._r; j++)
                {
                    rez[i, j] = A[j, i];
                }
            }                                   
            return rez;
        }

        // Знаходження оберненої матриці
        public Matrix Obmatr(Matrix A)
        {
            int m = A._r; float dt = 0;
            Matrix trezult = new Matrix(m, m);
            Matrix rez = new Matrix(m, m);
            Matrix op1 = new Matrix(m - 1, m - 1);
            
                        
            Matrix prmat = new Matrix(m, m);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    for (int k = 0; k < m; k++)
                    {
                        for (int l = 0; l < m; l++)
                        {
                            prmat[k, l] = A[k, l];
                        }
                    }
                                                                                
                    //зсув вліво на 1 позицію. починаючи з j+1 стовпця
                    for (int k = j + 1; k < m; k++)
                    {
                        for (int l = 0; l < m; l++)
                        {                          
                            prmat[l, k - 1] = prmat[l, k];
                        }
                    }
                                                   
                    //зсув вгору на 1 позицію, починаючи з i+1 стрічки
                    for (int k = i + 1; k < m; k++)
                    {
                        for (int l = 0; l < m; l++)
                        {
                            prmat[k - 1, l] = prmat[k, l];
                        }
                    }
                                                   
                    //знаходження  детермінанту клона без останнього рядка і останнього стовпця
                    for (int k = 0; k < m - 1; k++)
                    {
                        for (int l = 0; l < m - 1; l++)
                        {
                            op1[k, l] = prmat[k, l];
                        }
                    }
                                                  
                    dt = det(op1);

                    if ((i + j) % 2 == 0)
                    {
                        trezult[i, j] = dt;
                    }
                    else
                    {
                        trezult[i, j] = -dt;
                    }              
                }
            }

            if (det(A) != 0)
            {
                rez = Mult_namb(1 / det(A), Tr_Matr(trezult));
            }                         
            return rez;
        }
    }
}
