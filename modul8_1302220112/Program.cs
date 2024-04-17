using System;
using System.IO;
using System.Text.Json;

public class BankTransferConfig
{
    public String lang { get; set; }
    public class Transfer
    {
        public int threshold { get; set; }
        public int low_fee { get; set; }
        public int high_fee { get; set; }
    }
    public Transfer transfer { get; set; }
    public String methods { get; set; }
    public class Confirmation
    {
        public String en { get; set; }
        public String id { get; set; }
    }
    public Confirmation confirmation { get; set; }
    public BankTransferConfig()
    {
        lang = "en";
        transfer.threshold = 25000000;
        transfer.low_fee = 6500;
        transfer.high_fee = 15000;
        methods = "RTO SKN RTGS BIFAST";
        confirmation.en = "yes";
        confirmation.id = "ya";
    }

    public BankTransferConfig (string lang, Transfer transfer, string methods, Confirmation confirmation)
    {
        this.lang = lang;
        this.transfer = transfer;
        this.methods = methods;
        this.confirmation = confirmation;
    }

}
public class UIConfig
{
    public BankTransferConfig config;
    public const String filePath = @"config.json";
    public UIConfig()
    {
        try
        {
            ReadConfiguration();
        }
        catch (Exception)
        {
            setDefault();
            WriteNewConfigFile();
        }
    }
    public void setDefault()
    {
        config = new BankTransferConfig();
    }
    public BankTransferConfig ReadConfiguration()
    {
        String configFile = File.ReadAllText(filePath);
        config = JsonSerializer.Deserialize<BankTransferConfig>(configFile);
        return config;
    }
    public void WriteNewConfigFile()
    {
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        String jsonString = JsonSerializer.Serialize(config, options);
        File.WriteAllText(filePath, jsonString);
    }
}

public class Program
{
    static void Main(string[] args)
    {
        UIConfig uiConfig = new UIConfig();
        String language = uiConfig.config.lang;
        if (language == "en")
        {
            Console.WriteLine("Please insert the amount of money to transfer");
        } else if (language == "id")
        {
            Console.WriteLine("Masukkan jumlah uang yang akan ditransfer");
        }
        String input = Console.ReadLine();
        int uangTransfer = int.Parse(input);
        int totalBiaya = uangTransfer;
        int fee;
        if (uiConfig.config.transfer.threshold <= uangTransfer)
        {
            fee = uiConfig.config.transfer.low_fee;
            totalBiaya += fee;
        } else
        {
            fee = uiConfig.config.transfer.high_fee;
            totalBiaya += fee;
        }
        if (language == "en")
        {
            Console.WriteLine("Transfer fee = " + fee);
            Console.WriteLine("Total amount = " + totalBiaya);
        } else if (language == "id")
        {
            Console.WriteLine("Biaya Transfer = " + fee);
            Console.WriteLine("Total biaya = " + totalBiaya);
        }
        if (language == "en")
        {
            Console.WriteLine("Select transfer method: ");
        } else if (language == "id")
        {
            Console.WriteLine("Pilih metode transfer: ");
        }
    }
}