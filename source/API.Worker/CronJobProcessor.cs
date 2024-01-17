using API.Model.Enums;
using DotNetCore.Results;
using Quartz;
using System.Net;
using System.Text;

public class CronJobProcessor : IJob
{

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            // Lógica para notificar o serviço externo
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            Enum.TryParse(dataMap.GetString("HttpMethod") ?? "", out HttpMethodEnum httpMethod);
            var body = dataMap.GetString("Body") ?? "";

            // lógica de envio HTTP aqui
            using (var httpClient = new HttpClient())
            {
                var response = await SendAsync(httpClient, new StringContent(body, Encoding.UTF8, "application/json"), dataMap.GetString("Uri") ?? "", httpMethod);

                //Ok
                if (response.Status == HttpStatusCode.OK)
                {
                    Console.WriteLine(DateTime.Now.ToString(), await response.Value.Content.ReadAsStringAsync());
                    return;
                }

                //Tratamento de Erros
                Console.WriteLine("ERROR: " + DateTime.Now.ToString() + response.Status.ToString() + response.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        
    }

    public async Task<Result<HttpResponseMessage>> SendAsync(HttpClient httpClient, StringContent content, string uri, HttpMethodEnum httpMethod)
    {
        try
        {
            if (httpMethod == HttpMethodEnum.POST)
                return new Result<HttpResponseMessage>(HttpStatusCode.OK, await httpClient.PostAsync(uri, content));

            if (httpMethod == HttpMethodEnum.GET)
                return new Result<HttpResponseMessage>(HttpStatusCode.OK, await httpClient.GetAsync(uri));

            if (httpMethod == HttpMethodEnum.DELETE)
                return new Result<HttpResponseMessage>(HttpStatusCode.OK, await httpClient.DeleteAsync(uri));

            if (httpMethod == HttpMethodEnum.PUT)
                return new Result<HttpResponseMessage>(HttpStatusCode.OK, await httpClient.PutAsync(uri, content));
        }
        catch (Exception ex)
        {
            return new Result<HttpResponseMessage>(HttpStatusCode.BadRequest, ex.Message);
        }

        return new Result<HttpResponseMessage>(HttpStatusCode.BadRequest, "Invalid HTTP METHOD");
    }
}
