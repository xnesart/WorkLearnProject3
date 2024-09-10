using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace WorkLearnProject3.Controllers
{
    [Route("api/books")]
    public class BooksController : Controller
    {
        private readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "books.txt");

        [HttpGet]
        [Route("get")]
        public string GetAllBooks()
        {
            return GetFromFile(Path.Combine(_path));
        }

        [HttpPost]
        [Route("post")]
        public ActionResult<bool> PostBook([FromQuery] string text)
        {
            bool result = WriteToFile(_path, text, false);

            if (result)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPut]
        [Route("put")]
        public ActionResult<bool> PutBook([FromQuery] string text)
        {
            bool result = WriteToFile(_path, text, true);

            if (result)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPatch]
        [Route("patch")]
        public ActionResult<bool> PatchBook([FromQuery] string oldText, [FromQuery] string newText)
        {
            bool result = WriteToFile(_path, oldText, newText);

            if (result)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpDelete]
        [Route("delete")]
        public ActionResult<bool> DeleteBook([FromQuery] string text)
        {
            bool result = DeleteFromFile(_path, text);

            if (result)
                return Ok(result);
            else
                return BadRequest(result);
        }

        private string GetFromFile(string filePath)
        {
            if (System.IO.File.Exists(filePath))
                return string.Join(Environment.NewLine, System.IO.File.ReadAllLines(filePath));

            return "books are not found";
        }

        private bool WriteToFile(string filePath, string text, bool rewrite)
        {
            bool result = false;
            if (!System.IO.File.Exists(filePath)) System.IO.File.AppendAllText(_path, "");

            try
            {
                if (rewrite) System.IO.File.WriteAllLines(filePath, new string[0]);
                System.IO.File.AppendAllText(filePath, text + Environment.NewLine);
                Console.WriteLine("text has added correctly");
                result = true;
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ArgumentException("something went wrong");
            }
        }

        private bool WriteToFile(string filePath, string oldText, string newText)
        {
            bool result = false;

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    string[] fileContents = System.IO.File.ReadAllLines(filePath);

                    for (int i = 0; i < fileContents.Length; i++)
                    {
                        if (fileContents[i].Contains(oldText))
                        {
                            fileContents[i] = fileContents[i].Replace(oldText, newText);
                            System.IO.File.WriteAllLines(filePath, fileContents);
                            Console.WriteLine("Text has been updated correctly.");
                            result = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new ArgumentException("Something went wrong during file update.");
                }
            }

            return result;
        }

        private bool DeleteFromFile(string filePath, string text)
        {
            bool result = false;

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    string[] fileContents = System.IO.File.ReadAllLines(filePath);

                    for (int i = 0; i < fileContents.Length; i++)
                    {
                        if (fileContents[i].Contains(text))
                        {
                            fileContents[i] = fileContents[i].Replace(text, "");
                            System.IO.File.WriteAllLines(filePath, fileContents);
                            Console.WriteLine("Text has been updated correctly.");
                            result = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw new ArgumentException("Something went wrong during file update.");
                }
            }

            return result;
        }
    }
}