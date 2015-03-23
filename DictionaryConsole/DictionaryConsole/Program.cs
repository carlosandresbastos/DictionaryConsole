using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Way2.BusinessRules;

namespace Way2.DictionaryConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new parameter of type objDictionary
            Way2Dictionary objDictionary = new Way2Dictionary();

            Console.WriteLine(" Bem-vindo  - Sistema de Pesquisa do Palavras  ");
            Console.WriteLine(" ===============================================");

            Console.WriteLine("Validação de parâmetros...........");
            // Validate the parameter
            if (objDictionary.ValidateWord(args) == false)
            {

                Console.WriteLine(objDictionary.StrMessage);
                Console.WriteLine("\nPressione Enter para continuar...........................");
                Console.ReadLine();
                return;
            }
            else {
                Console.WriteLine("Validação com êxito!!");
            }

            // O argumento é a palavra para pesquisa
            String strWord = args[0];
            Console.WriteLine("\nProcurando a palavra, por favor aguarde...........");
            

            objDictionary.Search(strWord);
            Console.WriteLine("\n");
            Console.WriteLine("Os resultados da pesquisa:");
            Console.WriteLine("***************************");
            Console.WriteLine(String.Format("\nPalavra para procurar:{0}", strWord));

            // If word was found
            if (objDictionary.WordFound)
            {
                Console.WriteLine(String.Format("Palabra foi encontrada na posição:{0}", objDictionary.IntCurrentPosition.ToString()));
                Console.WriteLine(String.Format("Lamento informar que matou {0} gatinhos :-(", objDictionary.NumeroGatinhosMortos.ToString()));
            
            }
                // Else, word was not found
            else { 
                Console.WriteLine("Palabra nao foi encontrada ");
                Console.WriteLine(String.Format("Lamento informar que matou {0} gatinhos :-(", objDictionary.NumeroGatinhosMortos.ToString()));
            
                // If was found any error, show in console
                if (objDictionary.StrMessage != String.Empty)
                {
                    Console.WriteLine(String.Format("Mensagem: {0}", objDictionary.StrMessage));
                }
            }


            
            Console.WriteLine("\n\nPressione Enter para continuar");
            Console.ReadLine();
            return;


        }
    }
}
