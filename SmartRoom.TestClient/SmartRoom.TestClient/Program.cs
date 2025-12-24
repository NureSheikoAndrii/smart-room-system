using SmartRoom.TestClient.Models;
using System.Net.Http.Json;

var client = new HttpClient
{
    BaseAddress = new Uri("https://localhost:7185")
};

Console.Title = "Smart Room - Admin Test Console";

while (true)
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("\n=== Smart Room | Тестування серверної частини ===");
    Console.ResetColor();

    Console.WriteLine("1️ > Надiслати сенсорнi данi");
    Console.WriteLine("2️ > Переглянути останнi сенсорнi данi");
    Console.WriteLine("3️ > Переглянути логи адмiнiстратора");
    Console.WriteLine("4️ > Оновити налаштування автоматизацiї");
    Console.WriteLine("0️ > Вийти");
    Console.Write("Оберiть дiю: ");

    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                Console.Clear();
                await SendSensorData();Pause();
                break;

            case "2":
                Console.Clear();
                await GetLatestSensorData(); Pause();
                break;

            case "3":
                Console.Clear();
                await GetAdminLogs(); Pause();
                break;

            case "4":
                Console.Clear();
                await UpdateAutomationSettings(); Pause();
                break;

            case "0":
                Console.Clear();
                Console.WriteLine("👋 Завершення роботи"); Pause();
                return;

            default:
                Console.Clear();
                Console.WriteLine("❌ Невiрний вибiр");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"❌ Помилка: {ex.Message}");
        Console.ResetColor();
    }
}

// ================= METHODS =================
async Task SendSensorData()
{
    try
    {
        Console.Write("> Температура: ");
        double temperature = double.Parse(Console.ReadLine()!, System.Globalization.CultureInfo.InvariantCulture);

        Console.Write("> Вологiсть: ");
        double humidity = double.Parse(Console.ReadLine()!, System.Globalization.CultureInfo.InvariantCulture);

        Console.Write("> Освiтленiсть: ");
        int lightLevel = int.Parse(Console.ReadLine()!);

        var sensorData = new
        {
            temperature,
            humidity,
            lightLevel
        };

        var response = await client.PostAsJsonAsync("/api/sensors", sensorData);
        Console.WriteLine($"POST /api/sensors → {response.StatusCode}");
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Pause();
    }
}

async Task GetLatestSensorData()
{
    var data = await client.GetFromJsonAsync<object>("/api/sensors/latest");
    Console.WriteLine("Останнi данi:");
    Console.WriteLine(data);
}

async Task GetAdminLogs()
{
    var logs = await client.GetFromJsonAsync<List<AdminLog>>("/api/admin/logs");

    if (logs == null || logs.Count == 0)
    {
        Console.WriteLine("Логи відсутні");
        return;
    }

    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\nЛоги адміністратора:");
    Console.ResetColor();

    foreach (var log in logs.OrderByDescending(l => l.createdAt).Take(20))
    {
        Console.WriteLine(
            $"[{log.createdAt:dd.MM.yyyy HH:mm:ss}] {log.message}"
        );
    }
}

async Task UpdateAutomationSettings()
{
    Console.Write("Макс. температура: ");
    var maxTemp = double.Parse(Console.ReadLine()!, System.Globalization.CultureInfo.InvariantCulture);

    Console.Write("Мiн. освiтленiсть: ");
    var minLight = int.Parse(Console.ReadLine()!);

    var settings = new
    {
        maxTemperature = maxTemp,
        minLightLevel = minLight
    };

    var response = await client.PostAsJsonAsync("/api/admin/settings", settings);  
    Console.Write($"POST /api/admin/settings → ");
     Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(response.StatusCode);
    Console.ResetColor(); 
}

void Pause()
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write("Натиснiть будь-яку клавiшу, щоб продовжити...");
    Console.ResetColor();
    Console.ReadKey(true);
}