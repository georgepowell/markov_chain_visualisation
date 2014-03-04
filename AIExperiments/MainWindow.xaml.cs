using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AIExperiments
{
    public partial class MainWindow : Window
    {
        const int CircleRadius = 200;
        const int Interval = 50;
        const int NumberOfCircles = 26;
        const int CircleSize = 30;

        const int IndexRandom = 0;
        const int IndexSequential = 1;
        const int IndexBible = 2;
        const int IndexShakespeare = 3;
        const int IndexHTML = 4;

        string textSoFar = "";

        bool correct;

        int predictedActive;
        int active;
        int previouslyActive;

        DispatcherTimer timer = new DispatcherTimer();
        Random r;
        StreamReader bibleReader;
        StreamReader shakespearReader;
        StreamReader htmlReader;

        Dictionary<int, Dictionary<int, int>> counters;
        List<int> knownSymbols;

        int correctCount;

        int incorrectCount;

        int count;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromMilliseconds(Interval);
            timer.Tick += timer_Tick;

            Reset();
        }

        private void Reset()
        {
            r = new Random();
            bibleReader = new StreamReader("bible.txt");
            shakespearReader = new StreamReader("shakespeare.txt");
            htmlReader = new StreamReader("html.txt");

            counters = new Dictionary<int, Dictionary<int, int>>();
            knownSymbols = new List<int>();

            previouslyActive = 0;
            count = 0;

            correctCount = 0;
            incorrectCount = 0;

            textSoFar = "";
        }

        void timer_Tick(object sender, EventArgs e)
        {
            active = getNextSymbol();

            correct = active == predictedActive;

            if (correct)
                correctCount++;
            else
                incorrectCount++;

            if (!knownSymbols.Contains(active))
                knownSymbols.Add(active);

            knownSymbols = knownSymbols.OrderBy(n => n).ToList();

            IncrementCounters();

            predictedActive = getPrediction();

            textSoFar += symbolAsString(active).Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " "); ;

            Redraw();

            previouslyActive = active;
        }

        private int getPrediction()
        {
            int maxCount = -1;
            int prediction = active; // default to predicting that the same symbol gets fired again

            if (counters.ContainsKey(active))
                foreach (int nextSymbol in counters[active].Keys)
                    if (counters[active][nextSymbol] > maxCount)
                    {
                        maxCount = counters[active][nextSymbol];
                        prediction = nextSymbol;
                    }

            return prediction;
        }

        private int getNextSymbol()
        {
            if (patternCombo.SelectedIndex == IndexRandom)
                return r.Next() % NumberOfCircles;
            else if (patternCombo.SelectedIndex == IndexSequential)
                return count++ % NumberOfCircles;
            else if (patternCombo.SelectedIndex == IndexBible)
                return bibleReader.Read();
            else if (patternCombo.SelectedIndex == IndexShakespeare)
                return shakespearReader.Read();
            else if (patternCombo.SelectedIndex == IndexHTML)
                return htmlReader.Read();

            else return 0;
        }

        private void Redraw()
        {
            canvas.Children.Clear();

            DrawConnections(previouslyActive, active);
            DrawNodes(knownSymbols.Count);
            DrawStatistics();
        }

        private void DrawStatistics()
        {
            int total = (correctCount + incorrectCount);
            double correctRatio = total == 0 ? 0 : 1.0 * correctCount / total;
            correctRatio = (int)(1000 * correctRatio) / 1000.0;
            double incorrectRatio = 1 - correctRatio;

            double xMidpoint = total == 0 ? 620 : 10 + 610 * incorrectRatio;

            Line redLine = new Line()
            {
                X1 = 10,
                Y1 = 10,
                X2 = xMidpoint,
                Y2 = 10,
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 5
            };

            Line greenLine = new Line()
            {
                X1 = xMidpoint,
                Y1 = 10,
                X2 = 620,
                Y2 = 10,
                Stroke = new SolidColorBrush(Colors.Green),
                StrokeThickness = 5
            };

            double percentage = 100.0 * correctRatio;

            TextBlock txtPercentage = new TextBlock()
            {
                Text = percentage.ToString() + "%",
                Margin = new Thickness(xMidpoint, 15, 0, 0)
            };

            canvas.Children.Add(redLine);
            canvas.Children.Add(greenLine);
            canvas.Children.Add(txtPercentage);

            if (txtSoFar != null)
            txtSoFar.Text = textSoFar;
        }

        private void DrawConnections(int latestFrom, int latestTo)
        {
            Line redLine = null;

            foreach (int from in counters.Keys)
            {
                Line thickestLine = null;
                int thickestWidth = 0;

                foreach (int to in counters[from].Keys)
                {
                    bool isRed = from == latestFrom && to == latestTo;
                    Color color = isRed ? correct ? Colors.Green : Colors.Red : Colors.Gray;
                    Line line = getLine(from, to, ref color);

                    if (TrimLines && counters[from][to] > thickestWidth)
                    {
                        thickestLine = line;
                        thickestWidth = counters[from][to];
                    }

                    if (!isRed)
                    {
                        if (!TrimLines)
                            canvas.Children.Add(line);
                    }
                    else
                        redLine = line;
                }

                if (TrimLines)
                    canvas.Children.Add(thickestLine);
            }

            if (redLine != null && !canvas.Children.Contains(redLine))
                canvas.Children.Add(redLine);
        }

        private Line getLine(int from, int to, ref Color color)
        {
            var fromCoordinate = getCircleCoordinate(knownSymbols.Count, knownSymbols.IndexOf(from));
            var toCoordinate = getCircleCoordinate(knownSymbols.Count, knownSymbols.IndexOf(to));
            Line line = new Line()
            {
                X1 = fromCoordinate.Item1,
                Y1 = fromCoordinate.Item2,
                X2 = toCoordinate.Item1,
                Y2 = toCoordinate.Item2,
                Stroke = new SolidColorBrush(color),
                StrokeThickness = counters[from][to] > CircleSize ? CircleSize : counters[from][to]
            };
            return line;
        }

        void DrawNodes(int totalCircles)
        {
            for (int i = 0; i < totalCircles; i++)
            {
                var coordinate = getCircleCoordinate(totalCircles, i);
                int thickness = 1;

                int symbolInt = knownSymbols[i];

                if (counters.ContainsKey(symbolInt) && counters[symbolInt].ContainsKey(symbolInt))
                    thickness = counters[symbolInt][symbolInt];

                Ellipse circle = new Ellipse()
                {
                    Width = CircleSize,
                    Height = CircleSize,
                    Margin = new Thickness(coordinate.Item1 - 15, coordinate.Item2 - 15, 0, 0),
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = thickness,
                    Fill = symbolInt == active ? new SolidColorBrush(Colors.Blue) : new SolidColorBrush(Colors.LightGray)
                };
                canvas.Children.Add(circle);

                string text = symbolAsString(symbolInt);

                TextBlock symbol = new TextBlock()
                {
                    Margin = new Thickness(coordinate.Item1 - 4, coordinate.Item2 - 8, 0, 0),
                    Text = text,
                    FontWeight = FontWeights.Bold
                };
                canvas.Children.Add(symbol);
            }
        }

        private string symbolAsString(int symbol)
        {
            if (patternCombo.SelectedIndex == IndexBible || patternCombo.SelectedIndex == IndexShakespeare || patternCombo.SelectedIndex == IndexHTML)
                return ((char)symbol).ToString();
            else
                return symbol.ToString();
        }

        Tuple<double, double> getCircleCoordinate(int totalCircles, int circleNumber)
        {
            double xCenter = 320;
            double yCenter = 260;
            double x = xCenter + CircleRadius * Math.Cos(circleNumber * Math.PI * 2 / totalCircles);
            double y = yCenter + CircleRadius * Math.Sin(circleNumber * Math.PI * 2 / totalCircles);
            return new Tuple<double, double>(x, y);
        }

        private void IncrementCounters()
        {
            if (!counters.ContainsKey(previouslyActive))
                counters.Add(previouslyActive, new Dictionary<int, int>());

            if (!counters[previouslyActive].ContainsKey(active))
                counters[previouslyActive].Add(active, 0);

            counters[previouslyActive][active]++;
        }

        private void patternCombo_SelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            Reset();
            Redraw();
        }

        public bool TrimLines { get { return checkboxTrim.IsChecked == true; } }

        private void btnStartStop_Click(object sender, RoutedEventArgs e)
        {
            timer.IsEnabled = !timer.IsEnabled;

            btnStartStop.Content = timer.IsEnabled ? "Stop" : "Start";
        }

        private void btnStep_Click(object sender, RoutedEventArgs e)
        {
            timer_Tick(null, null);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            Redraw();
        }

        private void checkboxTrim_Unchecked(object sender, RoutedEventArgs e)
        {
            Redraw();
        }
    }
}
