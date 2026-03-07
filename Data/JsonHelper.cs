using System.Text.Json;

namespace ManagmentSystem.Data
{

    public sealed class JsonHelper<T>
    {
        private readonly string _filePath;

        public JsonHelper(string fileName)
        {
            string currentDir = AppContext.BaseDirectory;
            DirectoryInfo? directory = new DirectoryInfo(currentDir);

            while (directory != null &&
                  (directory.Name == "bin" ||
                   directory.Name == "Debug" ||
                   directory.Name == "Release" ||
                   directory.Name.StartsWith("net")))
            {
                directory = directory.Parent;
            }

            string projectRoot = directory?.FullName ?? currentDir;
            string folderPath = Path.Combine(projectRoot, "DataFiles");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            _filePath = Path.Combine(folderPath, fileName);
        }

        public List<T> Load()
        {
            if (!File.Exists(_filePath))
                return new List<T>();

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        public void Save(List<T> data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(_filePath, json);
        }
    }
}
/*
 * JsonStorage<T> is a generic helper class used to save and load data
 * from JSON files. It works with any model type (Employee, Product, etc.)
 * because it uses a generic type parameter <T>.
 *
 * How it works:
 * 1. It finds the root folder of the project (not the bin/Debug folder).
 * 2. It creates a "DataFiles" folder in the project root if it does not exist.
 * 3. It builds the full file path for the JSON file you want to use.
 *
 * Load():
 * - Reads the JSON file.
 * - Converts the JSON text into List<T>.
 * - If the file does not exist, it returns an empty list.
 *
 * Save():
 * - Converts List<T> into JSON text.
 * - Saves the JSON text into the file with pretty formatting.
 *
 * Why this class is useful:
 * - You don't repeat file-handling code in every service.
 * - You can store any type of data easily.
 * - The storage location is always correct and consistent.
 * - It keeps your project clean and organized.
 */
