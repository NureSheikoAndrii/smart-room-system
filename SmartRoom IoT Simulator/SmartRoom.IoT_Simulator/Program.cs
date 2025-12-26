using SmartRoom.IoT_Simulator;
using System.Net.Http.Json;

//CONFIG
const string SERVER_URL = "https://localhost:7185";// /api/sensor
const int SEND_INTERVAL_MS = 30000; // 30 секунд

try
{
    Console.Title = "IoT Simulator.exe";
    Console.WriteLine("*** IoT Smart Room Simulator запущено ***");
    Console.WriteLine("Натиснiть Ctrl+C для завершення\n");

    using var cts = new CancellationTokenSource();
    Console.CancelKeyPress += (s, e) =>
    {
        Console.WriteLine("\nЗупинка IoT клiєнта...");
        e.Cancel = true;
        cts.Cancel();
    };

    //HTTPS handler (dev certificate bypass)
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    var httpClient = new HttpClient(handler)
    {
        BaseAddress = new Uri(SERVER_URL),
        Timeout = TimeSpan.FromSeconds(10)
    };

    // сенсор з дрейфом
    var sensor = new SensorEmulator();

    await RunIoTAsync(httpClient, sensor, cts.Token);

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("IoT клiєнт зупинено");
}
catch(Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"{DateTime.Now}> {ex.Message}");
    Console.ResetColor();
}

//METHODS

static async Task RunIoTAsync(HttpClient client, SensorEmulator sensor, CancellationToken token)
{
    while (!token.IsCancellationRequested)
    {
        var data = sensor.Read();

        var payload = new
        {
            temperature = data.temperature,
            humidity = data.humidity,
            lightLevel = data.lightLevel
        };

        Random rand = new Random();
        var rNum = rand.Next(1, 9);
        bool success = await SendWithRetryAsync(client, payload, token, 3);

        Console.ForegroundColor = GetColor(rNum);
        Console.WriteLine(
            $"[{DateTime.Now:HH:mm:ss}] " +
            $"{(success ? "Надiслано" : "Не вдалося")} | " +
            $"T={payload.temperature}°C, " +
            $"H={payload.humidity}%, " +
            $"L={payload.lightLevel} lux"
        );
        Console.ResetColor();

        await Task.Delay(SEND_INTERVAL_MS, token);
    }
}

//RETRY

static async Task<bool> SendWithRetryAsync(HttpClient client, object payload, CancellationToken token, int maxRetries = 3)
{
    for (int attempt = 1; attempt <= maxRetries; attempt++)
    {
        try
        {
            var response = await client.PostAsJsonAsync("/api/sensors", payload, token);

            if (response.IsSuccessStatusCode)
                return true;

            Console.ForegroundColor = GetColor(4);
            Console.WriteLine($"HTTP {response.StatusCode}");
            Console.ResetColor();           
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = GetColor(9);
            Console.WriteLine($"Спроба {attempt}: {ex.Message}");
            Console.ResetColor();
        }

        await Task.Delay(2000, token);
    }

    return false;
}


static ConsoleColor GetColor(int rNum)
{
    switch (rNum)
    {
        case 1:
            return ConsoleColor.Cyan;
        case 2:
            return ConsoleColor.DarkCyan;
        case 3:
            return ConsoleColor.Blue;
        case 4:
            return ConsoleColor.Yellow;
        case 5:
            return ConsoleColor.Green;
        case 6:
            return ConsoleColor.Magenta;
        case 7:
            return ConsoleColor.DarkGray;
        case 8:
            return ConsoleColor.Gray;
        case 9:return ConsoleColor.Red;
        default: return ConsoleColor.White;
    }
}