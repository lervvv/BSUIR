using System.Net;
using System.Net.Sockets;
using System.Text;
class ChatServer
{
    static Dictionary<TcpClient, string> clientNames = new Dictionary<TcpClient, string>();

    static void Main()
    {
        Console.WriteLine("Выберите режим: Сервер - 1, Клиент - 2");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            StartServer();
        }
        else if (choice == "2")
        {
            StartClient();
        }
        else
        {
            Console.WriteLine("Неверный ввод.");
        }
    }

    static void StartServer()
    {
        Console.Write("Введите IP-адрес сервера: ");
        string ipAddress = Console.ReadLine();

        Console.Write("Введите порт: ");
        if (!int.TryParse(Console.ReadLine(), out int port)) //port - выходной параметр
        {
            Console.WriteLine("Некорректный порт.");
            return;
        }
        if (!IsPortAvailable(port))
        {
            Console.WriteLine($"Порт {port} уже используется. Выберите другой.");
            return;
        }

        try
        {
            //создание нового сервера
            TcpListener server = new TcpListener(IPAddress.Parse(ipAddress), port);
            //запуск прослушивания
            server.Start();
            Console.WriteLine($"Сервер запущен на {ipAddress}:{port}");
            //асинхронная задача для ввода сообщений сервером
            Task.Run(() => ServerMessageLoop());

            while (true)
            {
                //ожидание подключения клиента
                TcpClient client = server.AcceptTcpClient();
                //запуск обработки клиента
                Task.Run(() => HandleClient(client)); 
            }
        }
        catch
        {
            Console.WriteLine($"Ошибка при запуске сервера.");
        }
    }

    static void ServerMessageLoop()
    {
        while (true)
        {
            string message = Console.ReadLine();
            if (message == "0") break;
            BroadcastMessage($"Сервер: {message}", null);
        }
    }

    static void BroadcastMessage(string message, TcpClient sender)
    {
        lock (clientNames)
        {
            foreach (var client in clientNames.Keys.Where(c => c != sender))
            {
                try
                {
                    //конвертирует текст в массив байтов
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    client.GetStream().Write(buffer, 0, buffer.Length);
                }
                catch { }
            }
        }
    }

    static bool IsPortAvailable(int port)
    {
        try
        {
            //серверный сокет слушает соединения на указанном порту
            using (TcpListener listener = new TcpListener(IPAddress.Any, port))
            {
                //если порт свободен, будет успешный запуск
                listener.Start();
                listener.Stop();
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    static void HandleClient(TcpClient client)
    {
        //сетевой поток
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            //имя клиента
            bytesRead = stream.Read(buffer, 0, buffer.Length);
            string clientName = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

            lock (clientNames)
            {
                clientNames[client] = clientName; //связываем клиента и имя
            }

            Console.WriteLine($"Клиент {clientName} подключился.");
            //читает сообщения от клиента
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Получено сообщение от {clientName}: {message}");
                BroadcastMessage($"{clientName}: {message}", client);
            }
        }
        catch
        {
            //если клиент отключился
            lock (clientNames)
            {
                if (clientNames.TryGetValue(client, out string clientName))
                {
                    Console.WriteLine($"Клиент {clientName} отключился.");
                    clientNames.Remove(client);
                }
            }
        }
        //клиент полностью закрывается, ресурсы очищаются
        client.Close();
    }

    static void StartClient()
    {
        Console.Write("Введите IP-адрес сервера: ");
        string ipAddress = Console.ReadLine();

        Console.Write("Введите порт: ");
        if (!int.TryParse(Console.ReadLine(), out int port))
        {
            Console.WriteLine("Некорректный порт.");
            return;
        }
        Console.Write("Введите ваше имя: ");
        string clientName = Console.ReadLine();

        //создание TCP клиента и подключение к серверу
        using TcpClient client = new TcpClient(ipAddress, port);
        //сетевой поток для обмена данными
        NetworkStream stream = client.GetStream();

        //отправка имени клиента после подключения
        byte[] nameBuffer = Encoding.UTF8.GetBytes(clientName);
        stream.Write(nameBuffer, 0, nameBuffer.Length);

        Task.Run(() => ReceiveMessages(stream));

        Console.WriteLine("Введите сообщение (для выхода напишите `exit`):");
        while (true)
        {
            string input = Console.ReadLine();
            if (input.ToLower() == "exit") break;

            string message = clientName + ": " + input;
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
        }
    }

    static void ReceiveMessages(NetworkStream stream)
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"{message}");
        }
    }
}
