using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GradientDescentAlgorithm
{
    class Program
    {
        static List<List<Double>> genfromtxt(string csvFile, char delimeter)
        {
            var pts = File.ReadAllLines(csvFile).Select(a => a.Split(delimeter).ToList()).ToList();
            var points = new List<List<Double>>();

            foreach (var pt in pts)
            {
                var point = new List<Double>();
                foreach (var a in pt)
                {
                    Double axis;
                    if (Double.TryParse(a, out axis))
                        point.Add(axis);
                    else
                        point.Add(0.0);
                }
                points.Add(point);

            }
            return points;
        }

        static Double compute_error_for_line_given_points(Double b, Double m, List<List<Double>> points)
        {
            var totalError = 0.0;
            foreach(var point in points)
            {
                var x = point.ElementAt(0);
                var y = point.ElementAt(1);
                totalError += Math.Pow((y - (m * x + b)), 2);
            }

            return totalError / points.Count;

        }

        static Tuple<Double, Double> step_gradient(Double current_b, Double current_m, List<List<Double>> points, Double learning_rate)
        {
            var b_gradient = 0.0;
            var m_gradient = 0.0;
            var N = points.Count();
            foreach(var point in points)
            {
                var x = point.ElementAt(0);
                var y = point.ElementAt(1);
                b_gradient += -((double)2 / N) * (y - ((current_m * x) + current_b));
                m_gradient += -((double)2 / N) * x * (y - ((current_m * x) + current_b));
            }
            var new_b = current_b - (learning_rate * b_gradient);
            var new_m = current_m - (learning_rate * m_gradient);
            
            return Tuple.Create(new_b, new_m);
        }

        static Tuple<Double, Double> gradient_descent_runner(Double initial_b, Double initial_m, List<List<Double>> points, int num_iterations, Double learning_rate)
        {
            var b = initial_b;
            var m = initial_m;
            for(var i = 1; i <= num_iterations; i++)
            {
                var result = step_gradient(b, m, points, learning_rate);
                b = result.Item1;
                m = result.Item2;
                var currentError = compute_error_for_line_given_points(b, m, points);
                Thread.Sleep(125);
                Console.WriteLine(String.Format("After {0} iterations b = {1}, m = {2}, error = {3}", i, b, m, currentError));
            }
    
            return Tuple.Create(b, m);
        }

        static void Main(string[] args)
        {

            var points = genfromtxt(@"C:\Users\Farrukh.Saeed\Documents\Visual Studio 2017\Projects\GradientDescentAlgorithm\GradientDescentAlgorithm\data.csv", ',');
            var learning_rate = 0.0001;
            var initial_b = 0.0; // initial y-intercept
            var initial_m = 0.0; //initial slope guess
            var num_iterations = 1000;
            Console.WriteLine(String.Format("Starting gradient descent at b = {0}, m = {1}, error = {2}", initial_b, initial_m, compute_error_for_line_given_points(initial_b, initial_m, points)));
            Console.WriteLine("Running...");

            var finalResult = gradient_descent_runner(initial_b, initial_m, points, num_iterations, learning_rate);
            var final_b = finalResult.Item1;
            var final_m = finalResult.Item2;
            var finalError = compute_error_for_line_given_points(final_b, final_m, points);

            Console.WriteLine(String.Format("After {0} iterations b = {1}, m = {2}, error = {3}", num_iterations, final_b, final_m, finalError));
        }
    }
}
