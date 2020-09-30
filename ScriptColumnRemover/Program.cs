using System;
using System.IO;
using System.Text;
using System.Threading;

namespace ScriptColumnRemover
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Endereço do Arquivo:");
            var path = Console.ReadLine();

            Console.WriteLine("Coluna desejada:");
            var nomeColuna = Console.ReadLine();

            Console.WriteLine("Valor a ser substituido (vazio para substituir todos):");
            var valorTroca = Console.ReadLine();

            Console.WriteLine("Novo valor:");
            var novoValor = Console.ReadLine();

            int i = 0;

            try
            {
                using (var sr = new StreamReader(path))
                {
                    var file = sr.ReadToEnd();

                    using (var sReader = new StringReader(file))
                    {
                        var builder = new StringBuilder();
                        string line = string.Empty;

                        while ((line = sReader.ReadLine()) != null)
                        {
                            if (!line.StartsWith("insert", StringComparison.OrdinalIgnoreCase))
                                continue;

                            var index1 = GetNthIndex(line, ',', int.Parse(nomeColuna) - 1) + 1;
                            var index2 = GetNthIndex(line, ',', int.Parse(nomeColuna)) - 1;
                            var newLine = line;

                            var valorAtual = line.Substring(index1, (index2 - index1) + 1).Trim();

                            if (String.IsNullOrEmpty(valorTroca) || valorAtual.Equals(valorTroca?.Trim()))
                            {
                                newLine = line.Remove(index1, (index2 - index1) + 1).Insert(index1, novoValor);
                                i++;
                            }                                

                            builder.AppendLine(newLine);
                        }

                        using (var sw = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "NovoScript.txt")))
                        {
                            sw.Write(builder);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Arquivo criado com sucesso!");
            Console.WriteLine($"{i} substituições realizadas.");
            Thread.Sleep(3000);
        }

        public static int GetNthIndex(string s, char t, int n)
        {

            var index = s.IndexOf("values", StringComparison.OrdinalIgnoreCase);
            var novaS = s.Substring(index);

            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (novaS[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i + index;
                    }
                }
            }
            return -1;
        }
    }    
}

