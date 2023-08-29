using ImageMagick;
using System.CommandLine;

Argument<string[]> imagesArgument = new Argument<string[]>("images", "Image paths");

Command command = new Command("convert", "Convert images");
command.AddArgument(imagesArgument);

command.SetHandler(async (context) =>
{
	string[] images = context.ParseResult.GetValueForArgument(imagesArgument);

	foreach (string image in images)
	{
		MagickImage magickImage = new MagickImage(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, image));

		magickImage.Crop(new MagickGeometry()
		{
			// A4, 150dpi
			Width = 1240,
			Height = 1754,
			IgnoreAspectRatio = true
		});
		magickImage.RePage();

		string outputPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "output", image);
		Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

		await magickImage.WriteAsync(outputPath);
	}
});

await command.InvokeAsync(args);