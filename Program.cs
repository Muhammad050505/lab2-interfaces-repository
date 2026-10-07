using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab2
{
    // ===================== ЗАДАНИЕ 1 =====================

    public interface IMovable
    {
        void Move(int x, int y);
    }

    public class Point : IMovable
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Point(int x, int y) { X = x; Y = y; }

        public void Move(int x, int y)
        {
            X += x;
            Y += y;
            Console.WriteLine($"Точка перемещена в ({X}; {Y})");
        }
    }

    public interface IDrawable
    {
        void Draw();
    }

    public class Circle : IDrawable
    {
        public double Radius { get; set; }
        public Circle(double r) => Radius = r;
        public void Draw() => Console.WriteLine($"Рисуется круг R={Radius}");
    }

    public class Rectangle : IDrawable
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public Rectangle(double w, double h) { Width = w; Height = h; }
        public void Draw() => Console.WriteLine($"Рисуется прямоугольник {Width}x{Height}");
    }

    public static class DrawingService
    {
        public static void DrawAll(List<IDrawable> shapes)
        {
            foreach (var s in shapes) s.Draw();
        }
    }

    // ===================== ЗАДАНИЕ 2 =====================

    public interface IShare
    {
        double GetArea();
        double GetPerimeter();
    }

    public interface I3DShare : IShare
    {
        double GetVolume();
    }

    public class CircleShare : IShare
    {
        public double Radius { get; set; }
        public CircleShare(double r) => Radius = r;
        public double GetArea() => Math.PI * Radius * Radius;
        public double GetPerimeter() => 2 * Math.PI * Radius;
    }

    public class RectangleShare : IShare
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public RectangleShare(double w, double h) { Width = w; Height = h; }
        public double GetArea() => Width * Height;
        public double GetPerimeter() => 2 * (Width + Height);
    }

    public class Cube : I3DShare
    {
        public double Side { get; set; }
        public Cube(double s) => Side = s;
        public double GetArea() => 6 * Side * Side;
        public double GetPerimeter() => 12 * Side;
        public double GetVolume() => Side * Side * Side;
    }

    public static class ShapeInfo
    {
        public static void PrintShapeInfo(IShare shape)
        {
            Console.WriteLine($"Площадь: {shape.GetArea():F2}");
            Console.WriteLine($"Периметр: {shape.GetPerimeter():F2}");
        }
    }

    // ===================== ЗАДАНИЕ 3 (ISP) =====================

    // «Толстый» интерфейс (антипример)
    public interface IDevice
    {
        void Print();
        void Scan();
        void Fax();
    }

    public class PrinterBad : IDevice
    {
        public void Print() => Console.WriteLine("Печать...");
        public void Scan() { }
        public void Fax() { }
    }

    // Разделённые интерфейсы
    public interface IPrinter { void Print(); }
    public interface IScanner { void Scan(); }
    public interface IFax { void Fax(); }

    public class PrinterOnly : IPrinter
    {
        public void Print() => Console.WriteLine("Принтер печатает");
    }

    public class ScannerOnly : IScanner
    {
        public void Scan() => Console.WriteLine("Сканер сканирует");
    }

    public class MultifunctionDevice : IPrinter, IScanner, IFax
    {
        public void Print() => Console.WriteLine("МФУ печатает");
        public void Scan() => Console.WriteLine("МФУ сканирует");
        public void Fax() => Console.WriteLine("МФУ отправляет факс");
    }

    // ===================== ЗАДАНИЕ 4 =====================

    public interface IPayable
    {
        void Pay(decimal amount);
    }

    public class CreditCard : IPayable
    {
        public string Number { get; set; }
        public CreditCard(string n) => Number = n;
        public void Pay(decimal amount) => Console.WriteLine($"Оплата {amount:C} картой {Number}");
    }

    public class Cash : IPayable
    {
        public void Pay(decimal amount) => Console.WriteLine($"Оплата {amount:C} наличными");
    }

    public static class PaymentProcessor
    {
        public static void ProcessPayment(IPayable method, decimal amount) => method.Pay(amount);
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine($"[Console] {message}");
    }

    public class FileLogger : ILogger
    {
        private readonly string _path;
        public FileLogger(string path) => _path = path;
        public void Log(string message) =>
            File.AppendAllText(_path, $"[File] {DateTime.Now}: {message}\n");
    }

    public static class Worker
    {
        public static void DoWork(ILogger logger)
        {
            logger.Log("Работа начата");
            logger.Log("Работа завершена");
        }
    }

    // ===================== MAIN =====================

    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Задание 1 ===");
            var p = new Point(0, 0);
            p.Move(5, 3);
            DrawingService.DrawAll(new List<IDrawable> { new Circle(5), new Rectangle(4, 6) });

            Console.WriteLine("\n=== Задание 2 ===");
            ShapeInfo.PrintShapeInfo(new CircleShare(5));
            var cube = new Cube(3);
            Console.WriteLine($"Объём куба: {cube.GetVolume()}");

            Console.WriteLine("\n=== Задание 3 ===");
            new PrinterOnly().Print();
            new ScannerOnly().Scan();
            new MultifunctionDevice().Fax();

            Console.WriteLine("\n=== Задание 4 ===");
            PaymentProcessor.ProcessPayment(new CreditCard("1234"), 100m);
            PaymentProcessor.ProcessPayment(new Cash(), 50m);
            Worker.DoWork(new ConsoleLogger());
            Worker.DoWork(new FileLogger("log.txt"));

            Console.WriteLine("\nГотово!");
        }
    }
}
