using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ma.Terminal.SelfService.Utils
{
    public class RSAUtils
    {
        //string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArVaf3Pgt+B7uzo8jh+mt16YbIcd0GFPBCbDnby+IdvGSGYxachx87uCmblzICpKfFSCH6rekDu6L/yM1hO/yQdheMS1fmobBClqD5l4K8pnPHdmNbT7hUnMJNGvJnBEzYAGIiViIQoLMI0UeHImkNi0JFUfNGP1vrlp5vKOUf5sVs9jNT3NA7bX6Xv/kwReDr56tbTkl2/c0ypOzMTXEZ1Xwj9QvXzgt3hUnh6hwvlrzwjS4nDfGDnSutxFMeD9thQir9Ew5LpybY3mlGSB74V7380BiTtDaC/ihwlIux9go8EwOYWu6jwTeGoSmX/wEnutEuUtjW8T1gUAh4Hnb1wIDAQAB";

        //string privateKey = "MIIEvwIBADANBgkqhkiG9w0BAQEFAASCBKkwggSlAgEAAoIBAQCtVp/c+C34Hu7OjyOH6a3Xphshx3QYU8EJsOdvL4h28ZIZjFpyHHzu4KZuXMgKkp8VIIfqt6QO7ov/IzWE7/JB2F4xLV+ahsEKWoPmXgrymc8d2Y1tPuFScwk0a8mcETNgAYiJWIhCgswjRR4ciaQ2LQkVR80Y/W+uWnm8o5R/mxWz2M1Pc0Dttfpe/+TBF4Ovnq1tOSXb9zTKk7MxNcRnVfCP1C9fOC3eFSeHqHC+WvPCNLicN8YOdK63EUx4P22FCKv0TDkunJtjeaUZIHvhXvfzQGJO0NoL+KHCUi7H2CjwTA5ha7qPBN4ahKZf/ASe60S5S2NbxPWBQCHgedvXAgMBAAECggEBAKbh11duuRNA9Ll5pcOcfvo3ubdzx5oESL2Dy82H/eJGAVsHfayPMNjrAFEQkqdbMj2s7C5WT2Tw1Wf2BfjO6nXqUgUWogyr3/6P1p1bvT6ERpt+cGLVPymaByqo+5l+FfBAiatxyP3/33m1eaAQBEEEatJKJnQAzB0YjkvHUZjnpe+KRZlLqAiPe7LZ8Ydxwwo9R396721w+D3/R0kRI6f+uvrTzScw2801o3hD57gWIZxRD27OANr9LZcOo973OPYA6Gl+mIgfZJrrThNjbAO3WBcTgShxrs4dG5da/4sfkeHEioocLW3boAfMI+HcA/k0fyilcgFBMccGh/SwboECgYEA7k9onhr0uxDMlNpNs0Ji4KVidBP+na7eGDhG1QMtCqZUyr/nNQ/fQHzboh881VN5SHlpL+ITViIw5qA3l3kMXTmxDW0126QR015eFbGgEGt7S+Bf7jgFHBSdsU3e61GLEKvyXfmBHC2c4GXFzSFb9GdyNVAQWFSeW2J92EaqMj8CgYEAujSPpfTF7RijI55AHdHJNJmaYQak97wC2r/4pjADJ/IjDgOKJEyQYvwUB2FQgRwlXt3+7t/eloMTKA1CDJRdtuHo2EtFOiqo+yH2ru1Bxv81IKN29mTQhDkyliMlwdShC2hzWKTIBRIjajNp/LrYIL/UPA2U6OtadVgDsgoEwGkCgYEA1hxegH7zlwb26F5jJUXmFLRDCsvUHdQ5E0WszkG2PDVJRYi5sMD78rK7mqO6QmhnNahvy2exu9eoW+1jRSKq6y+kVc3jb92vblsA6TjX+Si4dGm5hwyp+prDO8QdHwv6iBYVAj3jtG9+3VZTK4RnW+V9hUzUAqi0RqLtMl37GH0CgYA7HsUdJAJTrSbfADfLP/hqQvrJI5rtLTyax6ji2wulezO2F1mc/NI7G14gmb09wPn8jO+MWHgLwcIrTUpTRCgdEM0lH4DzXugYFEiGcb4YuJ7dpgj3YjryQFbXZIFwcVpQjPFSi78WHRQxe/GC1LAadc2k44sMCO3HpBJITPYFQQKBgQC1asQpTOcYxzZH7xAw5yrDExUV+B+Z9OlEV5xy7ezAA+Uhqnuyo84S+UThc3idL5bn9GcK2cLTwxSZ1epIAkpfOf4SoeFEM7xh/RfjrMLeh/z/M1hiGy4pfhASaUAP9xvt6hdeISdSPT5CfYUCNLNbMoQfm7vtgu+FvAZXOTByog==";

        //static RSACryptoServiceProvider rsa;

        static RsaPrivateCrtKeyParameters privateKeyParam;

        public static void Init(string privateKey)
        {
            var bytes = Convert.FromBase64String(privateKey);
            privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));

        }

        public static byte[] EncryptData(byte[] data)
        {
            //return rsa.Encrypt(data, false);

            IAsymmetricBlockCipher engine = new Pkcs1Encoding(new RsaEngine());


            try
            {
                engine.Init(true, privateKeyParam);
                return engine.ProcessBlock(data, 0, data.Length);
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public static string GetSign(byte[] data, string privateKey)
        {
            var rsa = new RSACryptoServiceProvider();
            //rsa.ImportParameters(new RSAParameters()
            //{
            //    Modulus = privateKeyParam.Modulus.ToByteArrayUnsigned(),
            //    D = privateKeyParam.PublicExponent.ToByteArrayUnsigned(),
            //    P = privateKeyParam.P.ToByteArrayUnsigned(),
            //    Q = privateKeyParam.Q.ToByteArrayUnsigned(),
            //    DP = privateKeyParam.DP.ToByteArrayUnsigned(),
            //    DQ = privateKeyParam.DQ.ToByteArrayUnsigned(),
            //    InverseQ = privateKeyParam.QInv.ToByteArrayUnsigned(),
            //    Exponent = privateKeyParam.Exponent.ToByteArrayUnsigned()
            //});
            rsa.FromXmlString(RSAPrivateKeyPem2Xml());
            var rsaClear = new RSACryptoServiceProvider();
            var paras = rsa.ExportParameters(true);
            rsaClear.ImportParameters(paras);
            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                var signData = rsa.SignData(data, sha256);
                return Convert.ToBase64String(signData);
            }
        }

        private static string RSAPrivateKeyPem2Xml()
        {
            //RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKey));

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned()),
                Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned()));
        }
    }
}
