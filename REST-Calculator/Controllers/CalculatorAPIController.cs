using Newtonsoft.Json;
using REST_Calculator.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace REST_Calculator.Controllers
{
    [RoutePrefix("v1")]
    public class CalculatorAPIController : ApiController
    {
        SoapCalc.CalculatorSoapClient client = new SoapCalc.CalculatorSoapClient();
        Logger logger = Logger.GetInstance();

        [Route("test")]
        public void Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var start = new ThreadStart(call);
                var t = new Thread(start);
                t.Start();
            }
        }

        void call()
        {
            MultiplyAsync(12345679, 8);
        }

        [Route("api/add")]
        [HttpGet]
        public string Add(int a, int b)
        {
            int id = logger.RequestToJson();
            logger.RequestToSoap(id);
            int result = client.Add(a, b);
            logger.ResponseFromSoap(id);
            string response = JsonConvert.SerializeObject(result);
            return response;
        }


        [Route("api/addasync")]
        [HttpGet]
        public async Task<string> AddAsync(int a, int b)
        {
            int id = logger.RequestToJson();
            logger.RequestToSoap(id);
            int result = await client.AddAsync(a, b);
            logger.ResponseFromSoap(id);
            string response = JsonConvert.SerializeObject(result);
            return response;
        }

        [Route("api/subtract")]
        [HttpGet]
        public string Subtract(int a, int b)
        {
            int id = logger.RequestToJson();
            logger.RequestToSoap(id);
            int result = client.Subtract(a, b);
            logger.ResponseFromSoap(id);
            string response = JsonConvert.SerializeObject(result);
            return response;
        }


        [Route("api/subtractasync")]
        [HttpGet]
        public async Task<string> SubtractAsync(int a, int b)
        {
            int id = logger.RequestToJson();
            logger.RequestToSoap(id);
            int result = await client.SubtractAsync(a, b);
            logger.ResponseFromSoap(id);
            string response = JsonConvert.SerializeObject(result);
            return response;
        }

        [Route("api/multiply")]
        [HttpGet]
        public string Multiply(int a, int b)
        {
            int id = logger.RequestToJson();
            logger.RequestToSoap(id);
            int result = client.Multiply(a, b);
            logger.ResponseFromSoap(id);
            string response = JsonConvert.SerializeObject(result);
            return response;
        }


        [Route("api/multiplyasync")]
        [HttpGet]
        public async Task<string> MultiplyAsync(int a, int b)
        {
            int id = logger.RequestToJson();
            logger.RequestToSoap(id);
            int result = await client.MultiplyAsync(a, b);
            logger.ResponseFromSoap(id);
            string response = JsonConvert.SerializeObject(result);
            return response;
        }


        [Route("api/divide")]
        [HttpGet]
        public string Divide(int a, int b)
        {
            int id = logger.RequestToJson();
            logger.RequestToSoap(id);
            int result = client.Divide(a, b);
            logger.ResponseFromSoap(id);
            string response = JsonConvert.SerializeObject(result);
            return response;
        }


        [Route("api/divideasync")]
        [HttpGet]
        public async Task<string> DivideAsync(int a, int b)
        {
            int id = logger.RequestToJson();
            logger.RequestToSoap(id);
            int result = await client.DivideAsync(a, b);
            logger.ResponseFromSoap(id);
            string response = JsonConvert.SerializeObject(result);
            return response;
        }

    }
}
