using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.IO;

namespace PAA_lab2_stringovi
{
    internal class Program
    {
        static uint StrLength(string sadrzajFajla)
        {
            char[] delimiteri = { ' ', ',', '.', '!', '?', ';', ':' };
            string[] podstringovi = sadrzajFajla.Split(delimiteri, StringSplitOptions.RemoveEmptyEntries);
            return (uint)podstringovi.Length;
        }
        static string Levenstein(string str, string podstring, uint duzinaStr,string tipKodiranja)//mozda in/out argument za vreme
        {
            string s=null;//= new string[duzinaStr];
            StringBuilder sb = new StringBuilder();
            //char[,] matrica = new char[50,50];
            Stopwatch sw = new Stopwatch();
            sw.Start();
            char[] delimiteri = { ' ', ',', '.', '!', '?', ';', ':' };
            string[] podstringovi = str.Split(delimiteri, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < podstringovi.Length-1 ; i++)
            {
                LevensteinDistance(podstring, podstringovi[i],sb);
                //podstring je zadati pattern, podstringovi[i] je sadrzaj fajla podeljen u podstringove   
            }
            sw.Stop();
            UpisUFajl(s,podstring ,tipKodiranja, podstringovi.Length, sw);
            return s;
        }
        static string LevensteinDistance(string str1, string str2,StringBuilder s)
        {
            int n = str1.Length;
            int m = str2.Length;
            
            int[,] mat = new int[n , m];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (i == 0)
                        mat[i, j] = j;
                    else if (j == 0)
                        mat[i, j] = i;
                    else if (str1[i - 1] == str2[j - 1])
                        mat[i, j] = mat[i - 1, j - 1];
                    else
                        mat[i, j] = 1 + Math.Min(Math.Min(mat[i, j - 1], mat[i - 1, j]), mat[i - 1, j - 1]);
                }
            }
            int distance = mat[n - 1, m - 1];
            //StampanjeMatrice(mat, n , m );
            if (distance<=3)
            {
                //s += str1 += str2;
                s.Append(str1).Append(str2).Append(" ");

            }

            return s.ToString();
        }
        
        static string ConvertHexUAscii(string heksadecimalniPodaci)
        {
            // Ukloni bilo koje razmake ili druge nepotrebne znakove iz heksadecimalnih podataka
            heksadecimalniPodaci = heksadecimalniPodaci.Replace(" ", "").Replace("\n", "").Replace("\r", "");

            // Niz bajtova
            byte[] bajtovi = new byte[heksadecimalniPodaci.Length / 2];

            // Konvertuj svaki par heksadecimalnih karaktera u bajt
            for (int i = 0; i < bajtovi.Length; i++)
            {
                bajtovi[i] = Convert.ToByte(heksadecimalniPodaci.Substring(i * 2, 2), 16);
            }

            // Konvertuj bajtove u string
            string s = System.Text.Encoding.ASCII.GetString(bajtovi);

            return s;
        }
        static void Direktorijum(string pattern,string putanjaDoDirektorijuma, string tipKodiranja)//ascii ili hexadecimalni
        {
            if (Directory.Exists(putanjaDoDirektorijuma))
            {
                string[] txtFajlovi = Directory.GetFiles(putanjaDoDirektorijuma, "*.txt");
                foreach (string txtfajl in txtFajlovi)
                {
                    Console.WriteLine(Path.GetFileName(txtfajl));
                    string sadrzajTxtFajla = "";
                    try
                    {
                       if(tipKodiranja=="ASCII")
                         sadrzajTxtFajla = File.ReadAllText(txtfajl);//txtFajl
                       else
                        {
                            sadrzajTxtFajla = ConvertHexUAscii(File.ReadAllText(txtfajl));
                        }
                        
                        uint strLength = StrLength(sadrzajTxtFajla);
                        Levenstein(sadrzajTxtFajla, pattern, strLength,tipKodiranja);
                    }
                    catch (IOException ioe)
                    {
                        Console.WriteLine("Greska pri citanju fajla " + txtfajl + ioe.Message);
                    }
                }
                //Console.WriteLine("Direktorijum postoji");
            }
            else
                Console.WriteLine("Nepostojeci direktorijum");
            Console.ReadKey();
        }
        static void GenerisiFajl(string nazivFajla, string tekst)
        {
            using (StreamWriter writer = new StreamWriter(nazivFajla))
            {
                writer.Write(tekst);
            }
        }
        static void UpisUFajl(string s,string podstring,string tipKodiranja,int brEl,Stopwatch vreme)
        {//s=null??
            string nazivFajla = $"Poklapanja podstringa {podstring.Length} sa tekstom{brEl} {tipKodiranja}.txt";
            using (StreamWriter sw=new StreamWriter(nazivFajla))
            {
                sw.WriteLine($"Vreme izvrsavanja:{vreme.ElapsedMilliseconds} ms");
                sw.WriteLine(s);
            }
        }
        static string GenerisiHexString(int brojReci)
        {
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < brojReci; i++)
            {
                int duzinaReci = random.Next(1, 50); // duzina svake reci
                for (int j = 0; j < duzinaReci; j++)
                {
                    char hexChar = Convert.ToChar(random.Next(0, 16) < 10 ? random.Next(48, 58) : random.Next(65, 71));
                    sb.Append(hexChar);
                }
                sb.Append(" ");
            }

            return sb.ToString();
        }
        static void Main(string[] args)
        {
            
            /*
            GenerisiFajl("hex 100.txt", GenerisiHexString(100));
            //GenerateFile("ascii100.txt", GenerateAsciiString(100));

            GenerisiFajl("hex 1000.txt", GenerisiHexString(1000));
            //GenerateFile("ascii1000.txt", GenerateAsciiString(1000));

            GenerisiFajl("hex 10000.txt", GenerisiHexString(10000));
            //GenerateFile("ascii10000.txt", GenerateAsciiString(10000));

            GenerisiFajl("hex 100000.txt", GenerisiHexString(100000));
            //GenerateFile("ascii100000.txt", GenerateAsciiString(100000));
            */
            
            Console.WriteLine("Unesi putanju do direktorijuma sa ASCII: ");
            string putanjaDoDirektorijuma = Console.ReadLine();
            Direktorijum("every", putanjaDoDirektorijuma, "ASCII");//5

            Console.WriteLine("Unesi putanju do direktorijuma sa ASCII: ");
            putanjaDoDirektorijuma = Console.ReadLine();
            Direktorijum("discovered", putanjaDoDirektorijuma, "ASCII");//10

            Console.WriteLine("Unesi putanju do direktorijuma sa ASCII: ");
            putanjaDoDirektorijuma = Console.ReadLine();
            Direktorijum("thatthehusbandwascar", putanjaDoDirektorijuma, "ASCII");//20

            Console.WriteLine("Unesi putanju do direktorijuma sa ASCII: ");
            putanjaDoDirektorijuma = Console.ReadLine();
            Direktorijum("thatthehusbandwascarryingonanintriguewithaFrenchgirl", putanjaDoDirektorijuma, "ASCII");//50

            //////////////////////////////////////////////////////////
            /*
            Console.WriteLine("Unesi putanju do direktorijuma sa hex: ");
            putanjaDoDirektorijuma = Console.ReadLine();
            Direktorijum("every", putanjaDoDirektorijuma, "Hex");//5

            Console.WriteLine("Unesi putanju do direktorijuma sa hex: ");
            putanjaDoDirektorijuma = Console.ReadLine();
            Direktorijum("discovered", putanjaDoDirektorijuma, "Hex");//10

            Console.WriteLine("Unesi putanju do direktorijuma sa hex: ");
            putanjaDoDirektorijuma = Console.ReadLine();
            Direktorijum("thatthehusbandwascar", putanjaDoDirektorijuma, "Hex");//20

            Console.WriteLine("Unesi putanju do direktorijuma sa hex: ");
            putanjaDoDirektorijuma = Console.ReadLine();
            Direktorijum("thatthehusbandwascarryingonanintriguewithaFrenchgirl", putanjaDoDirektorijuma, "Hex");//50
            */
            Console.ReadKey();
        }
    }

}

/*
       static string GenerateAsciiString(int brojReci)
       {
           StringBuilder sb = new StringBuilder();
           Random random = new Random();

           for (int i = 0; i < brojReci; i++)
           {
               int length = random.Next(1, 50); // Random length between 1 and 10 characters
               for (int j = 0; j < length; j++)
               {
                   char asciiChar = Convert.ToChar(random.Next(0, 255)); // ASCII codes for printable characters
                   sb.Append(asciiChar);
               }
               sb.Append(" ");
           }

           return sb.ToString();
       }*/
/*
        static void StampanjeMatrice(int[,] mat, int n, int m)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Console.Write(mat[i, j] + " ");
                }
                Console.WriteLine();
            }
           
        }*/