using NerdStore.Pagamentos.Business;

namespace NerdStore.Pagamentos.AntiCorruption;

public class PagamentoCartaoCreditoFacade : IPagamentoCartaoCreditoFacade
{
    private readonly IPayPalGateway _payPalGateway;
    private readonly IConfigurationManager _configManager;

    public PagamentoCartaoCreditoFacade(IPayPalGateway payPalGateway, IConfigurationManager configManager)
    {
        _payPalGateway = payPalGateway;
        _configManager = configManager;
    }

    public Transacao RealizarPagamento(Pedido pedido, Pagamento pagamento)
    {
        var apiKey = _configManager.GetValue("apiKey");
        var encriptionKey = _configManager.GetValue("encriptionKey");

        var serviceKey = _payPalGateway.GetPayPalServiceKey(apiKey, encriptionKey);
        var cardHashKey = _payPalGateway.GetCardHashKey(serviceKey, pagamento.NumeroCartao);

        var pagamentoResult = _payPalGateway.CommitTransaction(cardHashKey, pedido.Id.ToString(), pagamento.Valor);

        // TODO: O gateway de pagamentos que deve retornar o objeto transação
        var transacao = new Transacao
        {
            PedidoId = pedido.Id,
            Total = pedido.Valor,
            PagamentoId = pagamento.Id
        };

        if (pagamentoResult)
        {
            transacao.StatusTransacao = StatusTransacao.Pago;
            return transacao;
        }

        transacao.StatusTransacao = StatusTransacao.Recusado;
        return transacao;
    }
}