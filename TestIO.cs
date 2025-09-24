using System.Globalization;
using System.Text.RegularExpressions;

class TestIO
{
    private const string InputFormat = @"\((-?\d+(?:\.\d+)?),\s*(-?\d+(?:\.\d+)?)\)";

    static void Main()
    {
        string inputFilePath = "system_input_file.1630412935.txt";
        string outputFilePath = "system_output_file.1630412935.txt";
        string resultingFilePath = "test_report.txt";
        try
        {
            var inputRectangle = ExtractRectangle("Rectangle", inputFilePath);
            var inputPoints = ExtractPoints("Points", inputFilePath);
            var outputStrings = SplitFileToLines(outputFilePath);
            var streamWriter = new StreamWriter(resultingFilePath, false);
            streamWriter.WriteLine($"Expected visited points \t Actual visited points \t Test result (PASS / FAIL)");

            for (int i = 0; i < inputPoints.Count; i++)
            {
                if (IsPointInsideRectangle(inputPoints[i].x, inputPoints[i].y, inputRectangle))
                {
                    //positive test
                    (double x, double y) resultingPoint;
                    if (ParsePoint(outputStrings[i], out resultingPoint))
                    {
                        if (inputPoints[i] == resultingPoint)
                            streamWriter.WriteLine($"{inputPoints[i]} \t {resultingPoint} \t PASS");
                        else
                        {
                            streamWriter.WriteLine($"{inputPoints[i]} \t {resultingPoint} \t FAIL");
                            Console.WriteLine($"Test failed for point {inputPoints[i]}. " +
                                $"Expected: {inputPoints[i]}, " +
                                $"Got: {resultingPoint}; " +
                                $"points were not equal");
                        }
                    }
                    else
                    {
                        streamWriter.WriteLine($"{inputPoints[i]} \t {outputStrings[i]} \t FAIL");
                        Console.WriteLine($"Test failed for point {inputPoints[i]}. " +
                            $"Expected: {inputPoints[i]}, " +
                            $"Got: {outputStrings[i]}; " +
                            $"could not parse resulting point");
                    }
                }
                else
                {
                    //negative test
                    if (outputStrings[i] == "error")
                        streamWriter.WriteLine($"{inputPoints[i]} \t {outputStrings[i]} \t PASS");
                    else
                    {
                        streamWriter.WriteLine($"{inputPoints[i]} \t {outputStrings[i]} \t FAIL");
                        Console.WriteLine($"Test failed for point {inputPoints[i]}. " +
                            $"Expected: error, " +
                            $"Got: {outputStrings[i]}; " +
                            $"if the point is outside of rectangle, error message expected");
                    }
                }
            }
            streamWriter.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}\nInput data is incorrect.");
            return;
        }
    }

    private static List<string> SplitFileToLines(string filePath)
    {
        return new List<string>(File.ReadAllLines(filePath));
    }

    private static bool ParsePoint(string input, out (double x, double y) result)
    {
        result = (0, 0);
        var regex = new Regex(InputFormat);
        var match = regex.Match(input);
        if (!match.Success)
            return false;

        result.x = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        result.y = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
        return true;
    }

    private static bool IsPointInsideRectangle(double px, double py, List<(double x, double y)> rectangle)
    {
        if (rectangle == null || rectangle.Count != 4)
            throw new ArgumentException("Incorrect input rectangle");

        double minX = double.MaxValue, maxX = double.MinValue;
        double minY = double.MaxValue, maxY = double.MinValue;

        foreach (var pt in rectangle)
        {
            if (pt.x < minX) minX = pt.x;
            if (pt.x > maxX) maxX = pt.x;
            if (pt.y < minY) minY = pt.y;
            if (pt.y > maxY) maxY = pt.y;
        }

        return px >= minX && px <= maxX && py >= minY && py <= maxY;
    }

    private static List<(double x, double y)> ExtractRectangle(string startingRow, string inputFilePath)
    {
        var resultPoints = new List<(double x, double y)>();
        var regex = new Regex(InputFormat);
        bool rectangleSection = false;

        foreach (var line in File.ReadLines(inputFilePath))
        {
            if (rectangleSection)
            {
                var matches = regex.Matches(line);
                foreach (Match match in matches)
                {
                    double x = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    double y = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    resultPoints.Add((x, y));
                }
                break; 
            }
            if (line.Trim() == startingRow)
            {
                rectangleSection = true;
            }
        }

        return resultPoints;
    }

    private static List<(double x, double y)> ExtractPoints(string startingRow, string inputFilePath)
    {
        var regex = new Regex(InputFormat);
        bool pointsSection = false;
        int pointIndex = 1;
        var resultPoints = new List<(double x, double y)>();

        foreach (var line in File.ReadLines(inputFilePath))
        {
            if (line.Trim() == startingRow)
            {
                pointsSection = true;
                continue;
            }
            if (pointsSection)
            {
                var match = regex.Match(line);
                if (match.Success)
                {
                    double x = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                    double y = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                    resultPoints.Add((x, y));
                    pointIndex++;
                }
            }
        }
        return resultPoints;
    }
}