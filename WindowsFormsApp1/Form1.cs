using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        const int SIZE = 11;

        // Функція для знаходження вершини з найменшою відстанню, яка ще не була відвідана
        static int FindMinDistanceVertex(double[] dist, bool[] visited)
        {
            double minDist = double.MaxValue;
            int minIndex = -1;

            for (int i = 0; i < SIZE; ++i)
            {
                if (!visited[i] && dist[i] < minDist)
                {
                    minDist = dist[i];
                    minIndex = i;
                }
            }

            return minIndex;
        }


        // Функція для виведення найкоротших шляхів до всіх вершин з використанням алгоритму Дейкстри
        static void Dijkstra(double[,] graph, int startVertex, ref string message)
        {
            

            double[] dist = new double[SIZE]; // Масив для зберігання найкоротших відстаней до вершин
            bool[] visited = new bool[SIZE]; // Масив, що вказує, чи була відвідана вершина
            List<int>[] path = new List<int>[SIZE]; // Масив для зберігання шляхів до вершин

            // Ініціалізуємо масиви
            for (int i = 0; i < SIZE; ++i)
            {
                dist[i] = double.MaxValue; // Встановлюємо відстань до всіх вершин на початку як нескінченність
                visited[i] = false; // Жодна вершина ще не відвідана
                path[i] = new List<int>(); // Ініціалізуємо список для шляху до вершини
            }

            // Відстань до стартової вершини завжди 0
            dist[startVertex] = 0;
            path[startVertex].Add(startVertex); // Шлях до початкової вершини

            // Знаходимо найкоротший шлях для кожної вершини
            for (int count = 0; count < SIZE - 1; ++count)
            {
                int u = FindMinDistanceVertex(dist, visited); // Знаходимо вершину з найменшою відстанню
                visited[u] = true; // Позначаємо вершину як відвідану

                // Оновлюємо відстані до сусідніх вершин, якщо вони ще не відвідані і відстань до них через поточну вершину коротша
                for (int v = 0; v < SIZE; ++v)
                {
                    if (!visited[v] && graph[u, v] != -1 && dist[u] != double.MaxValue && dist[u] + graph[u, v] < dist[v])
                    {
                        dist[v] = dist[u] + graph[u, v];
                        path[v] = new List<int>(path[u]); // Копіюємо шлях до вершини u
                        path[v].Add(v); // Додаємо вершину v до шляху
                    }
                }
            }

            // Виводимо результати
            message += $"Distances and paths to all vertices from the vertex {startVertex + 1}: \n";
            for (int i = 0; i < SIZE; ++i)
            {
                message += $"Top {i + 1}: ";
                if (dist[i] != double.MaxValue)
                {

                    message += $"\ncost - {dist[i]};\t Way - ";
                    for (int j = 0; j < path[i].Count; ++j)
                    {
                        //string tmp_vench = ((path[i][j] + 1) != 6 ? "Маршрутка" : "Фунiкулер");
                        message += path[i][j] + 1; // Додати 1 до індексів для нумерації з 1
                        if (j < path[i].Count - 1)
                        {
                            //Console.Write($"-> {tmp_vench} -> ");
                            message += $" -> ";
                        }
                    }
                    message += "\n";
                }
                else
                {
                    message += "impossible to achieve\n";
                }
            }
        }

        static void FloydWarshall(List<List<double>> graph, List<List<int>> next)
        {
            for (int k = 0; k < SIZE; k++)
            {
                for (int i = 0; i < SIZE; i++)
                {
                    for (int j = 0; j < SIZE; j++)
                    {
                        if (graph[i][k] != -1 && graph[k][j] != -1 &&
                            (graph[i][j] == -1 || graph[i][k] + graph[k][j] < graph[i][j]))
                        {
                            graph[i][j] = graph[i][k] + graph[k][j];
                            next[i][j] = k;
                        }
                    }
                }
            }
        }

        static void PrintPath(int i, int j, List<List<int>> next, ref string message)
        {
            if (next[i][j] == -1)
            {
                message += (j + 1);
                return;
            }

            message += next[i][j] + 1 + " -> ";
            PrintPath(next[i][j], j, next, ref message);
        }

        static void OutputResult(int startVertex, List<List<double>> graph, List<List<int>> next, ref string message)
        {
            message += $"\nМiнiмальний шлях для обходу всiх вершин, починаючи з {startVertex + 1} вершини:\n";
            for (int j = 0; j < SIZE; j++)
            {
                if (j != startVertex)
                {
                    message += $"Початок (КПI): {startVertex + 1} -> {j + 1}: ";
                    if (graph[startVertex][j] != -1)
                    {
                        message += $"{startVertex + 1} ->";
                        PrintPath(startVertex, j, next, ref message);
                    }
                    else
                    {
                        message += "Немає шляху";
                    }
                    message += $"\t\t || Вартiсть: {graph[startVertex][j]} UAH \t\n";
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = "";

            if (radioButton1 != null && radioButton1.Checked)
            {
                    double[,] graph = {
                { -1, -1, -1, 15, -1, -1, 15, -1, -1, -1, -1 },
                { -1, -1, 15, -1, -1, 23, -1, -1, -1, -1, 23 },
                { -1, 15, -1, -1, 15, 23, -1, -1, 15, -1, -1 },
                { 15, -1, -1, -1, -1, -1, -1, 15, 15, -1, -1 },
                { -1, -1, 15, -1, -1, -1, -1, 15, 15, 15, -1 },
                { -1, 23, 23, -1, -1, -1, -1, -1, -1, -1, 23 },
                { 15, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
                { -1, -1, -1, 15, 15, -1, -1, -1, -1, 15, -1 },
                { -1, -1, 15, 15, 15, -1, -1, -1, -1, -1, -1 },
                { -1, -1, -1, -1, 15, -1, -1, 15, -1, -1, -1 },
                { -1, 15, -1, -1, -1, 23, -1, -1, -1, -1, -1 }
                };

                // Застосовуємо алгоритм Дейкстри для знаходження найкоротших шляхів від вершини 1
                Dijkstra(graph, 6, ref message);

                MessageBox.Show(message, "Знаходження оптимального маршруту за допомогою алгоритму Деїкстри", MessageBoxButtons.OK);
            }
            else if (radioButton2 != null && radioButton2.Checked) 
            {

                // iнiцiалiзацiя графу з вагами ребер
                List<List<double>> graph = new List<List<double>>
                {
                    new List<double>{ -1, -1, -1, 15, -1, -1, 15, -1, -1, -1, -1 },
                    new List<double>{ -1, -1, 15, -1, -1, 23, -1, -1, -1, -1, 23 },
                    new List<double>{ -1, 15, -1, -1, 15, 23, -1, -1, 15, -1, -1 },
                    new List<double>{ 15, -1, -1, -1, -1, -1, -1, 15, 15, -1, -1 },
                    new List<double>{ -1, -1, 15, -1, -1, -1, -1, 15, 15, 15, -1 },
                    new List<double>{ -1, 23, 23, -1, -1, -1, -1, -1, -1, -1, 23 },
                    new List<double>{ 15, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
                    new List<double>{ -1, -1, -1, 15, 15, -1, -1, -1, -1, 15, -1 },
                    new List<double>{ -1, -1, 15, 15, 15, -1, -1, -1, -1, -1, -1 },
                    new List<double>{ -1, -1, -1, -1, 15, -1, -1, 15, -1, -1, -1 },
                    new List<double>{ -1, 15, -1, -1, -1, 23, -1, -1, -1, -1, -1 }
                };

                // iнiцiалiзацiя матрицi next для вiдстеження шляху
                List<List<int>> next = new List<List<int>>();
                for (int i = 0; i < SIZE; i++)
                {
                    next.Add(new List<int>());
                    for (int j = 0; j < SIZE; j++)
                    {
                        next[i].Add(-1);
                    }
                }

                FloydWarshall(graph, next);



                int startVertex = 6; // Вершина, з якої починається обхiд


                OutputResult(startVertex, graph, next, ref message );

                MessageBox.Show(message, "Знаходження оптимального маршруту за допомогою алгоритму Флойда-Уоршела", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Виберіть якийсь із алгоритмів", "Помилка", MessageBoxButtons.OK);
            }

            

            

        }
    }
}
