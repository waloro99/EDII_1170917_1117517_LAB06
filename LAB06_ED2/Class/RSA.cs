using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace LAB06_ED2.Class
{
    public class RSA
    {
        //N Exception 
        [Serializable]
        internal class INVALID_P_Q_Exception : Exception
        {
            public INVALID_P_Q_Exception() : base(String.Format("N must be bigger than 255"))
            {

            }
        }//End N Exception

        //Prime Exception 
        [Serializable]
        internal class PrimeException : Exception
        {
            public PrimeException(int n) : base(String.Format("{0} its not a prime number", n))
            {

            }
        }//End Prime Exception 

        //E D Exception 
        [Serializable]
        internal class INVALID_E_D_Exception : Exception
        {
            public INVALID_E_D_Exception() : base(String.Format("Wasnt able to find E and D value"))
            {

            }
        }//End Ivalid E D Exception


        // Method builder
        public RSA()
        {

        }

        //Method builder
        public RSA(int P, int Q, string PublicKeyPath, string PrivateKeyPath)
        {
            if (P * Q < 255) throw new INVALID_P_Q_Exception(); //Values are too small
            IsNumberPrime(P);                                         //throws new exception if p es not prime 
            IsNumberPrime(Q);                                         //throws new exception if q es not prime

            int N = P * Q;
            int PHI = (P - 1) * (Q - 1); //Φ
            int E = GetE(PHI, N);
            int D = GetD(PHI, E, PHI, 1, PHI);

            using (StreamWriter sw = new StreamWriter(new FileStream(PublicKeyPath, FileMode.Create)))
            {
                sw.Write(E + "," + N);
            }

            using (StreamWriter sw = new StreamWriter(new FileStream(PrivateKeyPath, FileMode.Create)))
            {
                sw.Write(D + "," + N);
            }
        }



        //------------------------------------ PUBLIC FUNCTIONS -------------------------------------



        //------------------------------------ END PUBLIC FUNCTIONS ---------------------------------


        //------------------------------------ PRIVATE FUNCTIONS -------------------------------------

        //Method private for if number is prime or not ?
        private bool IsNumberPrime(int n)
        {
            if (n == 2) return true;
            for (int i = 3; i < (n / 2 + 1); i++)
                if (n % i == 0) throw new PrimeException(n); //
            return true;
        }//End Method for Is Number Prime

        //Method private for get the number E
        private int GetE(int PHI, int N)
        {
            for (int i = (PHI - 2); 0 < i; i--)
                if (AreNumbersCoprime(i, N) && AreNumbersCoprime(i, PHI))
                    return i;
            throw new INVALID_E_D_Exception();
        }//End method get E

        //Method private for are numbers comprime ?
        private bool AreNumbersCoprime(int n1, int n2)
        {
            List<int> Div_n1 = GetDivs(n1);
            List<int> Div_n2 = GetDivs(n2);

            foreach (var item in Div_n1)
                if (Div_n2.Contains(item) && item != 1)
                    return false;
            //Are comprime
            return true;
        }// End method for numbers are comprime?

        //Method private for Get divisors
        private List<int> GetDivs(int n)
        {
            List<int> ListR = new List<int>();
            for (int i = 1; i <= n; i++)
                if (n % i == 0)
                    ListR.Add(i);
            return ListR;
        }//End method for get divisors

        //Method privte for get the number D
        private int GetD(int L1, int L2, int R1, int R2, int PHI)
        {
            if (L2 == 1)
                return R2;
            int X = L1 / L2;
            int newL2 = L1 - X * L2;
            int newR2 = R1 - X * R2;
            if (newR2 < 0)
                newR2 = PHI - ((newR2 * -1) % PHI);
            //Recursive
            return GetD(L2, newL2, R2, newR2, PHI);
        }

        //------------------------------------ END PRIVATE FUNCTIONS ---------------------------------




    }
}
