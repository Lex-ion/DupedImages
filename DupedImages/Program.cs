using System.Drawing;
using System.Drawing.Imaging;
namespace DupedImages
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Image> images = new List<Image>();



            Console.WriteLine("Hello, World!");
            Console.WriteLine("gib directory");
            string[] files = Directory.GetFiles(Console.ReadLine());
            foreach (var file in files)
            {
                if (!file.Contains(".png") && !file.Contains(".jpg"))
                    continue;

                images.Add(new Image(file));

                Console.WriteLine(file);
            }

            List<string> pathToDelete = GetDupes(images);

            Console.WriteLine("Press 'Y' to delete dupede images");
            while (Console.ReadKey().KeyChar != 'y') { }

            foreach (string path in pathToDelete)
            {
                File.Delete(path);
            }

        }


        public static List<string> GetDupes(List<Image> images)
        {
            List<string> pathToDelete = new List<string>();

            for (int i = 0; i < images.Count; i++)
            {


                for (int j = i; j < images.Count; j++)
                {


                    if (j <= i || pathToDelete.Contains(images[j].Path))
                        continue;



                    double RGB_diff = 0;
                    bool flagged = true;



                    for (int k = 0; k < 3 && flagged; k++)
                    {
                        for (int l = 0; l < 256; l++)
                        {
                            RGB_diff += Math.Abs(images[i].rgbHistogram[k, l] - images[j].rgbHistogram[k, l]);

                        }
                    }

                    //Console.WriteLine(RGB_diff);
                    if (RGB_diff <= 0.0001f)
                        pathToDelete.Add(images[j].Path);

                    Console.WriteLine(i + " " + j + "  " + RGB_diff);
                }
            }

            return pathToDelete;
        }
    }


    public class Image
    {
        public double[,] rgbHistogram = new double[3, 256];
        public string Path { get; set; }
        public Image(string path)
        {


            Path = path;
            Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(path);
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    rgbHistogram[0, bitmap.GetPixel(i, j).R]++;
                    rgbHistogram[1, bitmap.GetPixel(i, j).G]++;
                    rgbHistogram[2, bitmap.GetPixel(i, j).B]++;
                }
            }

            for (int i = 0; i < rgbHistogram.GetLength(0); i++)
            {
                for (int j = 0; j < rgbHistogram.GetLength(1); j++)
                {
                    rgbHistogram[i, j] /= (bitmap.Size.Height * bitmap.Size.Width);
                }
            }
            bitmap.Dispose();

        }
    }
}