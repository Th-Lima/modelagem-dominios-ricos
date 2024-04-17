namespace NerdStore.Pagamentos.AntiCorruption;

public interface IPagamentosConfigurationManager
{
    string GetValue(string node);
}