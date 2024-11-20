using System;

class Program
{
    static void Main()
    {
        Func<double, double> f = x => 2 * Math.Pow(x, 7) + 3 * Math.Pow(x, 6) + 3 * Math.Pow(x, 4) - 3;

        int[] nodes = { 3, 4, 5, 6, 7 };
        double a = 3, b = 7;

        double[] weights = { 14.0 / 45, 64.0 / 45, 8.0 / 15, 64.0 / 45, 14.0 / 45 }; 

        double exactIntegral = ExactIntegral(a, b);

        double quadratureApproximation = 0;
        for (int i = 0; i < nodes.Length; i++)
        {
            quadratureApproximation += weights[i] * f(nodes[i]);
        }

        double trapezoidalApprox = (b - a) / 2 * (f(a) + f(b));

        double mid = (a + b) / 2;
        double simpsonsApprox = (b - a) / 6 * (f(a) + 4 * f(mid) + f(b));

        int algebraicPrecision = DetermineAlgebraicPrecision(weights, nodes, a, b);

        Console.WriteLine($"Точний інтеграл: {exactIntegral}");
        Console.WriteLine($"Квадратурна формула: {quadratureApproximation}");
        Console.WriteLine($"Формула трапецій: {trapezoidalApprox}");
        Console.WriteLine($"Формула Сімпсона: {simpsonsApprox}");
        Console.WriteLine($"Алгебраїчний степінь точності: {algebraicPrecision}");
    }

    static double ExactIntegral(double a, double b)
    {
        Func<double, double> F = x => (Math.Pow(x, 8) / 4) + (3 * Math.Pow(x, 7) / 7) + (3 * Math.Pow(x, 5) / 5) - 3 * x;
        return F(b) - F(a);
    }

    static int DetermineAlgebraicPrecision(double[] weights, int[] nodes, double a, double b)
    {
        int maxPrecision = nodes.Length - 1; 

        for (int degree = 0; degree <= maxPrecision; degree++)
        {
            double exactIntegral = ExactPolynomialIntegral(degree, a, b);
            double quadratureResult = 0;

            for (int i = 0; i < nodes.Length; i++)
            {
                quadratureResult += weights[i] * Math.Pow(nodes[i], degree);
            }

            Console.WriteLine($"Degree: {degree}, Exact: {exactIntegral}, Quadrature: {quadratureResult}, Difference: {Math.Abs(quadratureResult - exactIntegral)}");

            if (Math.Abs(quadratureResult - exactIntegral) > 1e-10)
            {
                return degree - 1;
            }
        }

        return maxPrecision; 
    }

    static double ExactPolynomialIntegral(int degree, double a, double b)
    {
        return (Math.Pow(b, degree + 1) - Math.Pow(a, degree + 1)) / (degree + 1);
    }
}
