using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LAB06_ED2.Class;
using System.IO;

namespace LAB06_ED2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        // -http://localhost:4689/weatherforecast/cipher/GetPublicKey/?m=37,7
        [HttpGet("cipher/GetPublicKey", Name = "GetPublicKey")]
        public IEnumerable<PublicKey> Get(string m) //<-- p and q 
        {
            //Get two data p and q for cipher RSA
            string[] N = m.Split(',');
            int p = Convert.ToInt32(N[0]);
            int q = Convert.ToInt32(N[1]);
            string publicKey = @"publickey.txt";//save publickey
            string privatekey = @"privatekey.txt";
            RSA rsa = new RSA(p, q, publicKey, privatekey);

            List<PublicKey> keys = new List<PublicKey>();
            PublicKey key = new PublicKey();
            key.method = "RSA";
            key.publickeyN = rsa.n;
            key.publickeyE = rsa.e;
            keys.Add(key);

            return keys;
        }

        // -http://localhost:4689/weatherforecast/cipher/Caesar2/?word=soycesar,259,211
        [HttpGet("cipher/Caesar2", Name = "Caesar2")]
        public IEnumerable<PublicKey> Get(string word,int a) //word < -- word, n and e
        {
            //Get two data p and q for cipher RSA
            string[] N = word.Split(',');
            int n = Convert.ToInt32(N[1]);
            int e = Convert.ToInt32(N[2]);
            string cesar = N[0];
            string publicKey = @"publickey.txt";//save publickey
            string cipher = @"cipher.txt";
            string cesarPath = @"caesar.txt";
            using (StreamWriter writer = new StreamWriter(cesarPath))
            {
                writer.WriteLine(cesar);
            }

            RSA rsa = new RSA();
            rsa.Encode(cesarPath,cipher,publicKey);
            return null;
        }


    }
}
