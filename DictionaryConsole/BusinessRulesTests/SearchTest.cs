using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Way2.BusinessRules;
using System.Net.Http;
using System.Net;

namespace Way2.BusinessRulesTests
{
    [TestClass]
    public class SearchTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            //arrange
            Way2Dictionary objDictionary ;
            int ExpectedIntCurrentPosition = int.Parse(Math.Pow(2, 17).ToString());
            // act
            objDictionary = new Way2Dictionary();

            // assert
            Assert.AreEqual(0, objDictionary.NumeroGatinhosMortos);
            Assert.AreEqual(ExpectedIntCurrentPosition, objDictionary.IntCurrentPosition);
            Assert.AreEqual(String.Empty, objDictionary.StrMessage);
            Assert.AreEqual(false, objDictionary.WordFound);
            

        }

        [TestMethod]
        public void ValidateNullWord()
        {
            //arrange
            Way2Dictionary objDictionary;
            String[] args = new String [0];
            objDictionary = new Way2Dictionary();

            // act
            Boolean response = objDictionary.ValidateWord(args);

            // assert
            Assert.AreEqual(false, response);
            Assert.AreEqual(objDictionary.StrMessage, "Erro: Parâmetro inicial não foi enviado");

        }


        [TestMethod]
        public void ValidateEmptyWord()
        {
            //arrange
            Way2Dictionary objDictionary;
            String[] args = new String[1];
            args[0] = String.Empty;
            objDictionary = new Way2Dictionary();

            // act
            Boolean response = objDictionary.ValidateWord(args);

            // assert
            Assert.AreEqual(false, response);
            Assert.AreEqual(objDictionary.StrMessage, "Erro: O parâmetro não deve estar vazio");

        }

        [TestMethod]
        public void ValidateWord()
        {
            //arrange
            Way2Dictionary objDictionary;
            String[] args = new String[1];
            args[0] = "ACHO";

            objDictionary = new Way2Dictionary();

            // act
            Boolean response = objDictionary.ValidateWord(args);

            // assert
            Assert.AreEqual(true, response);
            
        }

        [TestMethod]
        public void TestConnectiontoWebService()
        {
            //arrange
            Way2Dictionary objDictionary;
            String strURLWebService = "http://teste.way2.com.br/dic/api/words/";
            

            objDictionary = new Way2Dictionary();

            // act
            HttpResponseMessage response = objDictionary.GetResponseFromDictionary(1,strURLWebService);

            // assert
            Assert.AreEqual(response.IsSuccessStatusCode, true);
           

        }

        [TestMethod]
        public void TestDataFromWebService()
        {
            //arrange
            Way2Dictionary objDictionary;
            
            objDictionary = new Way2Dictionary();

            // act
            String response = objDictionary.GetDataFromWebService(1);


            // assert
            Assert.AreEqual(response, "ABA");


        }

        [TestMethod]
        public void TestBadRequest()
        {
            //arrange
            Way2Dictionary objDictionary;
            String strURLWebService = "http://teste.way2.com.br/dic/api/words/";


            objDictionary = new Way2Dictionary();

            // act
            HttpResponseMessage response = objDictionary.GetResponseFromDictionary(10000000, strURLWebService);

            // assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);


        }

    
    }
}
