using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

class NetworkScanner
{
    static async Task Main()
    {
        var tasks = NetworkInterface.GetAllNetworkInterfaces() //массив доступных сетевых интерфейсов
        .Where(nic => nic.OperationalStatus ==  OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback) //фильтруем активные интерфейсы и исключаем виртуальный интерфейс loopback 
        .SelectMany (nic => nic.GetIPProperties().UnicastAddresses //получаем все IP-адреса, связанные с интерфейсом
                .Where(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork) //оставляем только IPv4-адреса
                .Select(ip => ScanNetworkAsync(nic, ip.Address, ip.IPv4Mask))); //запускаем асинхронную задачу ScanNetworkAsync() для каждого IP
        await Task.WhenAll(tasks);
    }

    static async Task ScanNetworkAsync(NetworkInterface networkInterface, IPAddress ipAddress, IPAddress subnetMask)
    {
        var networkIPs = GetNetworkIPs(ipAddress.ToString(), subnetMask.ToString()); //список IP-адресов в сети

        string macString = networkInterface.GetPhysicalAddress().ToString(); //строка мак-адреса
        string macAddress = string.Join(":", Enumerable.Range(0, macString.Length / 2) //форматируем мак-адрес
            .Select(i => macString.Substring(i * 2, 2)));

        Console.WriteLine($"Собственный компьютер ({networkInterface.Name})\nIP-адрес: {ipAddress.ToString()}\nMAC-адрес: {macAddress}\n");

        foreach (string ip in networkIPs)
        {
            if (await PingHostAsync(ip, 500)) // Тайм-аут 500 мс 
            {
                macAddress = await Task.Run(() => GetMacAddress(ip));
                if (!string.IsNullOrEmpty(macAddress))
                {
                    Console.WriteLine($"IP-адрес: {ip}, MAC-адрес: {macAddress}, Интерфейс: {networkInterface.Name}");
                }
            }
        }
    }

    static async Task<bool> PingHostAsync(string ipAddress, int timeout)
    {
        using (Ping ping = new Ping())
        {
            try
            {
                PingReply reply = await ping.SendPingAsync(ipAddress, timeout);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }

    static List<string> GetNetworkIPs(string ipAddress, string subnetMask)
    {
        var ipParts = ipAddress.Split('.').Select(int.Parse).ToArray();
        var maskParts = subnetMask.Split('.').Select(int.Parse).ToArray();

        int[] networkAddress = ipParts.Zip(maskParts, (ip, mask) => ip & mask).ToArray(); //метод зип объединяет два массива (ipParts и maskParts)
        int[] broadcastAddress = networkAddress.Zip(maskParts, (net, mask) => net | (mask ^ 255)).ToArray();

        return Enumerable.Range(networkAddress[3] + 1, broadcastAddress[3] - networkAddress[3] - 1)
    .Select(i => $"{networkAddress[0]}.{networkAddress[1]}.{networkAddress[2]}.{i}")
    .ToList();
    }

    static string GetMacAddress(string ipAddress)
    {
        byte[] macAddr = new byte[6];
        int length = macAddr.Length;
        int result = SendARP((int)IPAddress.Parse(ipAddress).Address, 0, macAddr, ref length); //отправка запроса ARP и получения MAC-адреса

        return result == 0 ? string.Join(":", macAddr.Select(b => b.ToString("X2"))) : null; //объединяем в строку используя :
    }

    [DllImport("iphlpapi.dll", ExactSpelling = true)] //чтобы взять функцию SendARP из файла 
    private static extern int SendARP(int destIP, int srcIP, byte[] macAddr, ref int phyAddrLen); //функция для получения MAC-адреса для данного IP-адреса
}