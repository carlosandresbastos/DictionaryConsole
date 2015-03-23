using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Configuration;

namespace Way2.BusinessRules
{
    public class Way2Dictionary
    {
        #region Properties definition

        /// <summary>
        /// Variable to store if the algorithm has found a top of the dictionary
        /// </summary>
        Boolean _InitialFlag;


        private Boolean InitialFlag
        {
            get { return _InitialFlag; }
            set { _InitialFlag = value; }
        }



        int _NumeroGatinhosMortos;

        /// <summary>
        /// Number of cats that died :-( - executions of the web api
        /// </summary>
        public int NumeroGatinhosMortos
        {
            get { return _NumeroGatinhosMortos; }
            set { _NumeroGatinhosMortos = value; }
        }

        int _intCurrentPosition;

        /// <summary>
        /// Current position in the search method
        /// </summary>
        public int IntCurrentPosition
        {
            get { return _intCurrentPosition; }
            set { _intCurrentPosition = value; }
        }

        int _Range;
        /// <summary>
        /// Range of the search
        /// </summary>
        private int Range
        {
            get { return _Range; }
            set { _Range = value; }
        }

        /// <summary>
        /// Initial potential - Used for binary search
        /// </summary>
        int _InitPow;

        private int InitPow
        {
            get { return _InitPow; }
            set { _InitPow = value; }
        }

        int NumberOfAttempts;

        String _strMessage;

        /// <summary>
        /// Stores any message
        /// </summary>
        public String StrMessage
        {
            get { return _strMessage; }
            set { _strMessage = value; }
        }

        ///
        Boolean _WordFound;
        /// <summary>
        /// Stores true if the word was found, false in other case
        /// </summary>
        public Boolean WordFound
        {
            get { return _WordFound; }
            set { _WordFound = value; }
        }

        #endregion

        #region Methods definition

        /// <summary>
        /// Constructor
        /// </summary>
        public Way2Dictionary()
        {
            InitialFlag = true;
            NumeroGatinhosMortos = 0;
            InitPow = 17;
            // Initial position to find
            IntCurrentPosition = int.Parse(Math.Pow(2, InitPow).ToString());
            Range = IntCurrentPosition;
            NumberOfAttempts = 0;
            WordFound = false;
        }


        /// <summary>
        /// Main method to search a word in the dictionary
        /// </summary>
        /// <param name="strWordtoSearch">Word to Search</param>
        public void Search(String strWordtoSearch)
        {
            try
            {
                String strResponse = GetDataFromWebService(IntCurrentPosition);

                // Return 0 if the two words are equal, < 0 if strPalavra is less than strResposta, > 0 if strPalavra is greather than strResponse 
                int resultCompare = String.Compare(strWordtoSearch, strResponse, true);


                // The word was finded!
                if (resultCompare == 0)
                {
                    StrMessage = String.Empty;
                    WordFound = true;
                    return;
                }

                // Didn't found word
                if (Range == 0)
                {
                    StrMessage = String.Empty;
                    WordFound = false;
                    return;
                }

                // The word is greather than the word searched in dictionary
                if (resultCompare > 0)
                {
                    Range = Range / 2;
                    IntCurrentPosition = IntCurrentPosition + Range;
                    // Performs a binary recursive search
                    Search(strWordtoSearch);
                }


                // The word is less than the word searched in dictionary
                if (resultCompare < 0)
                {
                    Range = Range / 2;
                    IntCurrentPosition = IntCurrentPosition - Range;
                    // Performs a binary recursive search
                    Search(strWordtoSearch);
                }
            }
            catch (Exception ex)
            {
                WordFound = false;
                StrMessage = ex.Message;
            }

        }

        public HttpResponseMessage GetResponseFromDictionary(int intPosition, String strUrlWebService)
        {
            try
            {

                // Update current position of the search
                IntCurrentPosition = intPosition;

                // Attach Position to search to the URL
                strUrlWebService = String.Concat(strUrlWebService, intPosition.ToString());

                // Creates a Http Client to get data from WAY2 API
                var client = new HttpClient();

                client.BaseAddress = new Uri(strUrlWebService);
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response = client.GetAsync(strUrlWebService).Result;
                // Other kitten has dead :-(
                NumeroGatinhosMortos++;

                // Release unmanaged resources
                client.Dispose();

                return response;
            }
            catch (HttpRequestException ex)
            {
                WordFound = false;
                StrMessage = ex.ToString();
                return null;
            }
            catch (Exception ex)
            {
                WordFound = false;
                StrMessage = ex.ToString();
                return null;
            }
        }


        public String GetDataFromWebService(int intPosition)
        {
            // Get URL Web Service and Methods from Application configuration keys
            String strUrlWebService = ConfigurationManager.AppSettings["DictionaryWebServiceURL"];
            String strWordfromDictionary = String.Empty;

            IntCurrentPosition = intPosition;
            HttpResponseMessage objResponseMessage = GetResponseFromDictionary(intPosition, strUrlWebService);

            if (objResponseMessage == null)
            {
                throw new Exception("O sistema não se pode conectar ao serviço web");
            }

            //The call was with success
            if (objResponseMessage.IsSuccessStatusCode)
            {

                // Get the top value of the dictionary
                if (InitialFlag)
                {
                    InitPow++;
                    Range = (int)Math.Pow(2, InitPow);
                    intPosition = Range;
                    GetDataFromWebService(intPosition);

                }
                strWordfromDictionary = objResponseMessage.Content.ReadAsStringAsync().Result;
            }
                // Error calling the web service
            else
            {
                // Is out of the limit of dictionary
                if (objResponseMessage.StatusCode == HttpStatusCode.BadRequest)
                {

                    // Call recursive function while could found a Word
                    InitialFlag = false;

                    if (intPosition > Range)
                    {
                        Range = intPosition;
                    }
                    Range = Range / 2;
                    intPosition = Range;
                    strWordfromDictionary = GetDataFromWebService(intPosition);

                }
                else
                    // If was other type of error, try for 3 times connect to web service:
                {
                    NumberOfAttempts++;
                    if (NumberOfAttempts < 3)
                    {
                        GetDataFromWebService(intPosition);
                    }
                    else {
                        throw new Exception("O sistema não se pode conectar ao serviço web");
                    }
                }





            }

            // Release unmanaged resources
            objResponseMessage.Dispose();
            
            return strWordfromDictionary;

        }

        /// <summary>
        /// Validate parameters of the console application
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Boolean ValidateWord(String[] args)
        {
            if (args.Count() == 0)
            {
                StrMessage = "Erro: Parâmetro inicial não foi enviado";
                return false;
            }

            if (args[0].Trim() == String.Empty)
            {
                StrMessage = "Erro: O parâmetro não deve estar vazio";
                return false;
            }

            return true;
        }


        #endregion

    }



}