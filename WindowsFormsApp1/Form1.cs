using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace WindowsFormsApp1
{
    
    public partial class Form1 : Form
    {

        public int[,] graph = new int[,]
        {
            { -1, -1, -1, 1, -1, -1, 1, -1, -1, -1, -1 }, //1
            { -1, -1, 1, -1, -1, 1, -1, -1, -1, -1, 1 },//2
            { -1, -1, -1, -1, 1, 1, -1, -1, 1, -1, -1 },//3
            { 1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1 },//4
            { -1, -1, 1, -1, -1, -1, -1, 1, -1, 1, -1 },//5
            { -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1 },//6
            { 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },//7
            { -1, -1, -1, -1, 1, -1, -1, -1, -1, 1, -1 },//8
            { -1, -1, -1, 1, 1, -1, -1, -1, -1, -1, -1 },//9
            { -1, -1, -1, -1, 1, -1, -1, 1, -1, -1, -1 },//10
            { -1, 1, -1, -1, -1, 1, -1, -1, -1, -1, -1 }//11
        };

        //public int[,] graph = new int[,]
        //{
        //    { -1, -1, -1, 1, -1, -1, 1, -1, -1, -1, -1 }, //1
        //    { -1, -1, 1, -1, -1, 1, -1, -1, -1, -1, 1 },//2
        //    { -1, -1, -1, -1, -1, 1, -1, -1, 1, -1, -1 },//3
        //    { 1, -1, -1, -1, -1, -1, -1, 1, 1, -1, -1 },//4
        //    { -1, -1, 1, -1, -1, -1, -1, 1, -1, 1, -1 },//5
        //    { -1, 1, 1, -1, -1, -1, -1, -1, -1, -1, -1 },//6
        //    { 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },//7
        //    { -1, -1, -1, -1, 1, -1, -1, -1, -1, -1, -1 },//8
        //    { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },//9
        //    { -1, -1, -1, -1, 1, -1, -1, 1, -1, -1, -1 },//10
        //    { -1, 1, -1, -1, -1, 1, -1, -1, -1, -1, -1 }//11
        //};

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            string message = "";

            if (radioButton1 != null && radioButton1.Checked)
            {
                                                
                // Створення графу
                Tarjana g = new Tarjana(11);

                // Додавання ребер до графу з використанням матриці суміжності
                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 11; j++)
                    {
                        if (graph[i, j] == 1)
                        {
                            g.AddEdge(i, j);
                        }
                    }
                }

                // Виклик алгоритму Тарьяна та виведення результатів
                Console.WriteLine("Компоненти сильної зв'язності:");
                stopwatch.Start();
                var resT = g.Tarjan();
                stopwatch.Stop();
                for (int i = 0; i < resT.Count; i++)
                {
                    message += $"Компонента {i + 1}: \n";
                    foreach (var vertex in resT[i])
                    {
                        message += $"{vertex + 1} ";
                    }
                    message += "\n";
                }

                message += "\n" + stopwatch.Elapsed.ToString();

                MessageBox.Show(message, "Знаходження компонентів сильної зв'язності за допомогою алгоритму Тар'яна", MessageBoxButtons.OK);
            }
            else if (radioButton2 != null && radioButton2.Checked) 
            {

                Kosaraju g = new Kosaraju(11);

                for (int i = 0; i < 11; i++)
                {
                    for (int j = 0; j < 11; j++)
                    {
                        if (graph[i, j] == 1)
                        {
                            g.AddEdge(i, j);
                        }
                    }
                }
                
                stopwatch.Start();
                List<List<int>> resK = g.GetresK();
                stopwatch.Stop();

                // Виведення знайдених компонент
                Console.WriteLine("Сильно зв'язні компоненти:");
                for (int i = 0; i < resK.Count; i++)
                {
                    message += $"Компонента {i + 1}: \n";
                    foreach (int vertex in resK[i])
                    {
                        message += vertex+1 + " ";
                    }
                    message += "\n";
                }

                message += "\n" + stopwatch.Elapsed.ToString();

                MessageBox.Show(message, "Знаходження оптимального маршруту за допомогою алгоритму Флойда-Уоршела", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Виберіть якийсь із алгоритмів", "Помилка", MessageBoxButtons.OK);
            }

            

            

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

    
}

class Tarjana
{
    private int V; // Кількість вершин
    private List<int>[] sumList; // Список суміжностей

    public Tarjana(int v)
    {
        V = v;
        sumList = new List<int>[v];
        for (int i = 0; i < v; ++i)
        {
            sumList[i] = new List<int>();
        }
    }

    public void AddEdge(int v, int w)
    {
        sumList[v].Add(w);
    }

    // Реалізація алгоритму Тарьяна
    public List<List<int>> Tarjan()
    {
        int index = 0;
        var stack = new Stack<int>();
        var resTList = new List<List<int>>();
        var indexes = new int[V];
        for (int i = 0; i < V; i++)
            indexes[i] = -1;
        var lowlinks = new int[V];
        var visited = new bool[V];

        // Функція для обходу графу DFS
        void StrongConnect(int v)
        {
            indexes[v] = index;
            lowlinks[v] = index;
            index++;
            stack.Push(v);
            visited[v] = true;

            foreach (var w in sumList[v])
            {
                if (indexes[w] < 0)
                {
                    StrongConnect(w);
                    lowlinks[v] = Math.Min(lowlinks[v], lowlinks[w]);
                }
                else if (visited[w])
                {
                    lowlinks[v] = Math.Min(lowlinks[v], indexes[w]);
                }
            }

            if (lowlinks[v] == indexes[v])
            {
                var resT = new List<int>();
                int w;
                do
                {
                    w = stack.Pop();
                    visited[w] = false;
                    resT.Add(w);
                } while (w != v);
                resTList.Add(resT);
            }
        }

        for (int v = 0; v < V; v++)
        {
            if (indexes[v] < 0)
            {
                StrongConnect(v);
            }
        }

        return resTList;
    }
}

class Kosaraju
{
    private int V; // Кількість вершин
    private List<int>[] sum; // Список суміжностей

    public Kosaraju(int v)
    {
        V = v;
        sum = new List<int>[v];
        for (int i = 0; i < v; ++i)
            sum[i] = new List<int>();
    }

    // Додати ребро до графа
    public void AddEdge(int v, int w)
    {
        sum[v].Add(w);
    }

    // Вивести всі сильно зв'язні компоненти
    public List<List<int>> GetresK()
    {
        Stack<int> stack = new Stack<int>();
        bool[] visited = new bool[V];

        // Пушуємо всі вершини на стек у порядку збільшення їхніх індексів
        for (int i = 0; i < V; i++)
            if (!visited[i])
                FillOrder(i, visited, stack);

        // Створюємо транспонований граф
        Kosaraju transposedKosaraju = GetTranspose();

        // Позначаємо всі вершини як неперевірені
        for (int i = 0; i < V; i++)
            visited[i] = false;

        List<List<int>> resK = new List<List<int>>();

        // Проходження по транспонованому графу та збереження сильно зв'язних компонент
        while (stack.Count != 0)
        {
            int v = stack.Pop();
            if (!visited[v])
            {
                List<int> component = new List<int>();
                transposedKosaraju.DFS(v, visited, component);
                resK.Add(component);
            }
        }

        return resK;
    }

    // Допоміжний метод для заповнення стеку в порядку збільшення індексів
    private void FillOrder(int v, bool[] visited, Stack<int> stack)
    {
        visited[v] = true;

        foreach (var neighbor in sum[v])
            if (!visited[neighbor])
                FillOrder(neighbor, visited, stack);

        stack.Push(v);
    }

    // Допоміжний метод для обходу графа в глибину
    private void DFS(int v, bool[] visited, List<int> component)
    {
        visited[v] = true;
        component.Add(v);

        foreach (var neighbor in sum[v])
            if (!visited[neighbor])
                DFS(neighbor, visited, component);
    }

    // Отримати транспонований граф
    private Kosaraju GetTranspose()
    {
        Kosaraju transposedKosaraju = new Kosaraju(V);
        for (int v = 0; v < V; v++)
        {
            foreach (var neighbor in sum[v])
                transposedKosaraju.AddEdge(neighbor, v);
        }
        return transposedKosaraju;
    }
}

