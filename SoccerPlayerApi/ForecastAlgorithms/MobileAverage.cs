namespace ForecastAlgorithms;

public class MobileAverage
{
    public int SizeWindow { get; set; } = 4;

    public List<double> GetMovingAverage(List<double> figures)
    {
        int n = figures.Count;
        List<double> resultats = new();

        for (int i = 0; i < figures.Count - SizeWindow; i++)
        {
            IEnumerable<double> window = figures.Skip(i).Take(SizeWindow + 1);
            double temp = 0.5 * window.First() + window.Skip(1).Take(SizeWindow - 1).Sum() + 0.5 * window.Last();
            resultats.Add(temp / SizeWindow);
        }

        return resultats;
    }

    public List<double> GetSeasonalCoefficients(List<double> figures)
    {
        int n = figures.Count;
        List<double> resultats = new();

        for (int i = 0; i < figures.Count - SizeWindow; i++)
        {
            IEnumerable<double> window = figures.Skip(i).Take(SizeWindow + 1);
            double temp = 0.5 * window.First() + window.Skip(1).Take(SizeWindow - 1).Sum() + 0.5 * window.Last();
            double mobileAverage = temp / SizeWindow;
            double current = window.Skip(SizeWindow / 2).First();
            resultats.Add(current / mobileAverage);
        }

        return resultats;
    }
}