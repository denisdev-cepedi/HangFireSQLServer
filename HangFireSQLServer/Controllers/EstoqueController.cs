using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangFireSQLServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstoqueController : ControllerBase
    {
        [HttpGet]
        [Route("login")]
        public String Login()
        {
            //Fire-and-Forget - job executado somente uma vez
            var jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Bem-Vindo ao estoque virtual!"));
            return $"Job ID: {jobId}. Email de boas vindas envidado ao funcion�rio!";
        }

        [HttpGet]
        [Route("estoquecheckout")]
        public String CheckoutEstoque()
        {
            // Delayed Job - este job � executado somente uma vez mas n�o
            // imediatamente ap�s algum tempo
            var jobId = BackgroundJob.Schedule(() =>
           Console.WriteLine("Seu produto foi inclu�do no checkout !"), TimeSpan.FromSeconds(30));

            return $"Job ID: {jobId}. Produto adicionado ao seu checkout com sucesso!";
        }

        [HttpGet]
        [Route("produtobaixa")]
        public String ProdutoBaixa()
        {
            //Fire and Forget Job - este job � executado apenas uma vez
            var parentjobId = BackgroundJob.Enqueue(() =>
                  Console.WriteLine("A baixa do estoque foi realizada com sucesso!"));

            //Continuations Job - Este job � executado quando o job pai � executado
            BackgroundJob.ContinueJobWith(parentjobId, () =>
                   Console.WriteLine("Baixa do produto enviado!"));

            return "Voc� concluiu a baixa do produto e o recibo foi enviado para o email!";
        }

        [HttpGet]
        [Route("analiseestoque")]
        public String OfertasDiarias()
        {
            //Recurring Job - este job � executado muitas vezes em um cronograma especificado
            RecurringJob.AddOrUpdate(() =>
                Console.WriteLine("Envio de produtos que precisam sair do esque com sugest�o de promo��o")
                , Cron.Daily);

            return "## Sugest�o enviada! ##";
        }
    }
}
