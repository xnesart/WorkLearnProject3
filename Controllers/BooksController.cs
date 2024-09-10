using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace WorkLearnProject3.Controllers
{
    [Route("api/books")]
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "books.txt");
        
        public BooksController(ILogger<BooksController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("get")]
        public string GetAllBooks()
        {
            HttpContext.Items["LogMessage"] = "Custom log message from GetAllBooks controller";

            _logger.LogInformation("GetAllBooks method called");
            return GetFromFile(_path);
        }

        [HttpPost]
        [Route("post")]
        public ActionResult<bool> PostBook([FromQuery] string text)
        {
            _logger.LogInformation("PostBook method called with text: {Text}", text);
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
            _logger.LogInformation("PutBook method called with text: {Text}", text);
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
            _logger.LogInformation("PatchBook method called with oldText: {OldText} and newText: {NewText}", oldText, newText);
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
            _logger.LogInformation("DeleteBook method called with text: {Text}", text);
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
            if (!System.IO.File.Exists(filePath)) System.IO.File.AppendAllText(filePath, "");

            try
            {
                if (rewrite) System.IO.File.WriteAllLines(filePath, new string[0]);
                System.IO.File.AppendAllText(filePath, text + Environment.NewLine);
                _logger.LogInformation("Text added correctly: {Text}", text);
                result = true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while writing to file");
                throw new ArgumentException("Something went wrong");
            }

            return result;
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
                            _logger.LogInformation("Text updated correctly from '{OldText}' to '{NewText}'", oldText, newText);
                            result = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while updating file");
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
                            _logger.LogInformation("Text deleted correctly: {Text}", text);
                            result = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while deleting from file");
                    throw new ArgumentException("Something went wrong during file update.");
                }
            }

            return result;
        }
    }
}
